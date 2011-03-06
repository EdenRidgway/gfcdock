//
//  DockPanel.cs
//  
//  Author:
//       Gfab Corner's <gfab.corners@free.fr>
// 
//  Copyright (c) 2011 Gfab Corner's
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using Gtk;

namespace GFCDock
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class DockPanel : Gtk.Bin
    {
        #region Public Properties
        public int DefaultLeftSize {
            get { return defaultLeftSize; }
            set { defaultLeftSize = value; }
        }

        public int DefaultBottomSize {
            get { return this.defaultBottomSize; }
            set { defaultBottomSize = value; }
        }

        public int DefaultRightSize {
            get { return this.defaultRightSize; }
            set { defaultRightSize = value; }
        }

        public int DefaultTopSize {
            get { return this.defaultTopSize; }
            set { defaultTopSize = value; }
        }

        public Gdk.Size SizeWithoutAutoHide {
            get {
                
                Gdk.Size s = this.Allocation.Size;
                foreach (KeyValuePair<DockingLocation, DockAutoHide> pair in this.dockAutoHides) {
                    
                    switch (pair.Key) {
                    
                    case DockingLocation.Top:
                        s.Height -= pair.Value.HeightRequest;
                        break;
                    case DockingLocation.Bottom:
                        s.Height -= pair.Value.HeightRequest;
                        break;
                    case DockingLocation.Left:
                        s.Width -= pair.Value.WidthRequest;
                        break;
                    case DockingLocation.Right:
                        s.Width -= pair.Value.WidthRequest;
                        break;
                    }
                }
                return s;
            }
        }

        internal event DockDocumentMaximizedEventHandler DockDocumentMaximizedEvent;
        #endregion

        #region Constructor/Destructor
        public DockPanel ()
        {
            this.Build ();
        }
        #endregion

        #region Public functions
        public void Dock (IDockableWidget dw)
        {
            if (!(dw is Gtk.Widget))
                throw new DockPanelException ("Dock a widget: not a widget.");
            
            if (dw.DockingLocation == DockingLocation.None) {
                
                this.Dock (dw, this.ComputeDockingLocation (dw));
            } else {
                
                this.Dock (dw, dw.DockingLocation);
            }
        }

        public void Dock (IDockableWidget dw, DockingLocation dl)
        {
            if (!(dw is Gtk.Widget))
                throw new DockPanelException ("Dock a widget: not a widget.");
            
            if (dl == DockingLocation.None)
                throw new DockPanelException ("Dock a widget: no dockable location.");
            
            if (dw.DockingState == DockingState.AutoHide)
                this.UnAutoHide (dw);
            
            DockNotebook dn = this.GetDockNotebook (dl, true);
            if (dn == null)
                throw new DockPanelException ("Dock a widget: unknown location.");
            
            dn.Dock (dw);
            this.dockedWidgets.Add (dw);
            ShowAll ();
        }

        public void UnDock (IDockableWidget dw)
        {
            if (!(dw is Gtk.Widget))
                throw new DockPanelException ("Undock a widget: not a widget.");
            
            if (!dockedWidgets.Contains (dw))
                throw new DockPanelException ("Undock a widget: not docked.");
            
            DockingLocation dl = dw.DockingLocation;
            DockNotebook dn = this.GetDockNotebook (dl);
            if (dn == null)
                throw new DockPanelException ("Undock a widget: Notebook not exist for that location.");
            
            dn.Undock (dw);
            this.dockedWidgets.Remove (dw);
            dw.DockingState = DockingState.None;
            if ((dl != DockingLocation.Document) && (dn.Children.Length == 0)) {
                
                this.RemoveDockNotebook (dn);
            }
            ShowAll ();
        }

        public void AutoHide (IDockableWidget dw)
        {
            if (dw.DockingState != DockingState.AutoHide) {
                
                if (dw.DockingLocation == DockingLocation.None) {
                    this.AutoHide (dw, this.ComputeDockingLocation (dw));
                } else {
                    this.AutoHide (dw, dw.DockingLocation);
                }
            }
        }

        public void AutoHide (IDockableWidget dw, DockingLocation dl)
        {
            Gdk.Size sz = new Gdk.Size (0, 0);
            if (dl == DockingLocation.Top) {
                
                sz.Height = this.defaultTopSize;
            } else if (dl == DockingLocation.Bottom) {
                
                sz.Height = this.defaultBottomSize;
            } else if (dl == DockingLocation.Left) {
                
                sz.Width = this.defaultLeftSize;
            } else if (dl == DockingLocation.Right) {
                
                sz.Width = this.defaultRightSize;
            }
            
            this.AutoHide (dw, dl, sz);
        }

        public void AutoHide (IDockableWidget dw, DockingLocation dl, Gdk.Size sz)
        {
            if (!(dw is Gtk.Widget))
                throw new DockPanelException ("Autohide a widget: not a widget.");
            
            if (dl == DockingLocation.None)
                throw new DockPanelException ("Autohide a widget: no dockable location.");
            
            if (dw.DockingState == DockingState.Docked)
                this.UnDock (dw);
            
            DockAutoHide da = this.GetDockAutoHide (dl, true);
            if (da == null)
                throw new DockPanelException ("AutoHide a widget: unknown location.");
            
            da.Add (dw, sz);
            this.dockedWidgets.Add (dw);
            ShowAll ();
        }

        public void UnAutoHide (IDockableWidget dw)
        {
            if (!(dw is Gtk.Widget))
                throw new DockPanelException ("UnAutoHide a widget: not a widget.");
            
            if (!dockedWidgets.Contains (dw))
                throw new DockPanelException ("UnAutoHide a widget: not docked.");
            
            DockingLocation dl = dw.DockingLocation;
            DockAutoHide da = this.GetDockAutoHide (dl, false);
            if (da == null)
                throw new DockPanelException ("UnAutoHide a widget: AutoHide not exist for that location.");
            
            da.Remove (dw);
            this.dockedWidgets.Remove (dw);
            dw.DockingState = DockingState.None;
            if (da.IsEmpty) {
                
                this.RemoveDockAutoHide (da);
            }
            ShowAll ();
        }

        internal void DragMove (IDockableWidget dw, Gdk.Point posRoot)
        {
            DragCheck oldDragLocation = this.dragLocation;
            Gdk.Point pos = posRoot;
            int X, Y;
            this.GdkWindow.GetOrigin (out X, out Y);
            pos.X -= X;
            pos.Y -= Y;
            
            if (dragChecks.Count == 0)
                this.ComputeDragChecks (dw);
            
            this.dragLocation = this.GetDragLocation (pos);
            
            if (oldDragLocation.dockingLocation != this.dragLocation.dockingLocation) {
                
                if (this.dragWindow != null) {
                    
                    this.dragWindow.Destroy ();
                    this.dragWindow = null;
                }
                
                if (this.dragLocation.dockingLocation != DockingLocation.None) {
                    
                    Gdk.Rectangle rect = this.dragLocation.rectangle;
                    rect.X += X;
                    rect.Y += Y;
                    this.dragWindow = new DockDragWindow (rect);
                    this.dragWindow.Show ();
                }
            }
        }

        internal void DragStop (IDockableWidget dw, Gdk.Point pos)
        {
            if (this.dragWindow != null) {
                
                this.dragWindow.Destroy ();
                this.dragWindow = null;
            }
            
            this.dragChecks.Clear ();
            
            if (this.dragLocation.dockingLocation == DockingLocation.None)
                return;
            
            if (dw.DockingLocation == this.dragLocation.dockingLocation)
                return;
            
            if (dw.DockingState == DockingState.Docked)
                this.UnDock (dw); else if (dw.DockingState == DockingState.AutoHide)
                this.UnAutoHide (dw);
            
            this.Dock (dw, this.dragLocation.dockingLocation);
            this.dragLocation = new DragCheck ();
        }

        public void Clear ()
        {
            IDockableWidget[] dws = new IDockableWidget[this.dockedWidgets.Count];
            this.dockedWidgets.CopyTo (dws);
            foreach (IDockableWidget dw in dws) {
                
                if (dw.DockingState == DockingState.Docked)
                    this.UnDock (dw); else if (dw.DockingState == DockingState.AutoHide)
                    this.UnAutoHide (dw);
            }
            this.dockedWidgets.Clear ();
        }

        public void Serialize (string filename)
        {
            if (this.documentMaximized)
                return;

            XmlSerializer xs = new XmlSerializer (typeof(DockPanelSerializable));
            using (StreamWriter sw = new StreamWriter (filename)) {
                xs.Serialize (sw, this.GetSerializableObject ());
                sw.Close ();
            }
        }

        public void Deserialize (string filename)
        {
            if (this.documentMaximized)
                return;

             DockPanelSerializable o;
            XmlSerializer xs = new XmlSerializer (typeof(DockPanelSerializable));
            using (StreamReader sr = new StreamReader (filename)) {
                o = (DockPanelSerializable)xs.Deserialize (sr);
                sr.Close ();
            }
            
            this.Clear ();
            
            this.DefaultBottomSize = o.DefaultBottomSize;
            this.DefaultLeftSize = o.DefaultLeftSize;
            this.DefaultRightSize = o.DefaultRightSize;
            this.DefaultTopSize = o.DefaultTopSize;
            
            System.Reflection.Assembly asb = System.Reflection.Assembly.GetEntryAssembly ();
            foreach (DockAutoHideSerializable dns in o.DockAutoHides) {

                foreach (DockableWidgetSerializable dws in dns.DockedWidgets) {
                    
                    IDockableWidget dw = (IDockableWidget)asb.CreateInstance (dws.Type);
                    dw.DefaultDockingLocation = dws.DefaultDockingLocation;
                    dw.DockableLocation = dws.DockableLocation;
                    dw.DockingCapability = dws.DockingCapability;
                    dw.Title = dws.Title;
                    dw.Parameter = dws.Parameter;
                    this.AutoHide (dw, dns.DockingLocation, dws.Size);
                }
            }
            
            foreach (DockNotebookSerializable dns in o.DockNotebooks) {
                
                if (!this.dockNotebooks.ContainsKey (dns.DockingLocation)) {
                    
                    this.CreateDockNotebook (dns.DockingLocation, dns.PanePosition);
                }
                foreach (DockableWidgetSerializable dws in dns.DockedWidgets) {
                    
                    IDockableWidget dw = (IDockableWidget)asb.CreateInstance (dws.Type);
                    dw.DefaultDockingLocation = dws.DefaultDockingLocation;
                    dw.DockableLocation = dws.DockableLocation;
                    dw.DockingCapability = dws.DockingCapability;
                    dw.Title = dws.Title;
                    dw.Parameter = dws.Parameter;
                    this.Dock (dw, dns.DockingLocation);
                }
            }
        }

        internal DockPanelSerializable GetSerializableObject ()
        {
            DockPanelSerializable o = new DockPanelSerializable ();
            o.DefaultBottomSize = this.DefaultBottomSize;
            o.DefaultLeftSize = this.DefaultLeftSize;
            o.DefaultRightSize = this.DefaultRightSize;
            o.DefaultTopSize = this.DefaultTopSize;
            
            o.DockAutoHides = new List<DockAutoHideSerializable> ();
            foreach (KeyValuePair<DockingLocation, DockAutoHide> pair in this.dockAutoHides) {
                o.DockAutoHides.Add (pair.Value.GetSerializableObject ());
            }

            o.DockNotebooks = new List<DockNotebookSerializable> ();
            LinkedList<DockNotebookSerializable> dns = new LinkedList<DockNotebookSerializable> ();
            Widget w = this.dockRoot;
            while (w is Paned) {

                Paned p = (Paned)w;
                if (p.Child1 is Paned) {

                    w = p.Child1;
                    if (p.Child2 is DockNotebook) {

                        DockNotebook d = (DockNotebook)p.Child2;
                        dns.AddFirst (d.GetSerializableObject ());
                    }
                }
                else {

                    w = p.Child2;
                    if (p.Child1 is DockNotebook) {

                        DockNotebook d = (DockNotebook)p.Child1;
                        dns.AddFirst (d.GetSerializableObject ());
                    }
                }
            }
            if (w is DockNotebook) {

                DockNotebook d = (DockNotebook)w;
                dns.AddFirst (d.GetSerializableObject ());
            }
            foreach (DockNotebookSerializable d in dns) {

                o.DockNotebooks.Add (d);
            }
            return o;
        }

        public void ToggleMinimizeMaximizeDocument ()
        {
            System.Reflection.Assembly asb = System.Reflection.Assembly.GetEntryAssembly ();
            string path = System.IO.Path.GetDirectoryName (asb.Location);
            if (this.documentMaximized) {

                this.documentMaximized = false;
                if (this.DockDocumentMaximizedEvent != null)
                    this.DockDocumentMaximizedEvent (this, new DockDocumentMaximizedEventArgs (false));
                this.Deserialize (path + "/perspective.tmp");
            }
            else {

                this.Serialize (path + "/perspective.tmp");
                this.documentMaximized = true;
                if (this.DockDocumentMaximizedEvent != null)
                    this.DockDocumentMaximizedEvent (this, new DockDocumentMaximizedEventArgs (true));
                IDockableWidget[] dwc = new IDockableWidget[this.dockedWidgets.Count];
                this.dockedWidgets.CopyTo (dwc);
                foreach (IDockableWidget dw in dwc) {

                    if ((dw.DockingLocation != DockingLocation.Document) &&
                        (dw.DockingState == DockingState.Docked)) {

                        this.AutoHide (dw);
                    }
                }
            }
        }
        #endregion

        #region Overriden functions
        #endregion

        #region Event handlers
        #endregion

        #region Private
        private List<IDockableWidget> dockedWidgets = new List<IDockableWidget> ();

        private Gtk.Widget dockRoot;

        private bool documentMaximized = false;

        private Dictionary<DockingLocation, DockNotebook> dockNotebooks = new Dictionary<DockingLocation, DockNotebook> ();
        private Dictionary<DockingLocation, DockAutoHide> dockAutoHides = new Dictionary<DockingLocation, DockAutoHide> ();

        private int defaultLeftSize;
        private int defaultRightSize;
        private int defaultTopSize;
        private int defaultBottomSize;

        private DockDragWindow dragWindow = null;
        private DragCheck dragLocation = new DragCheck ();
        private struct DragCheck
        {
            public DockingLocation dockingLocation;
            public Gdk.Rectangle rectangle;

            public DragCheck (DockingLocation dl, Gdk.Rectangle r)
            {
                this.dockingLocation = dl;
                this.rectangle = r;
            }
        }
        private List<DragCheck> dragChecks = new List<DragCheck> ();

        private void ComputeDragChecks (IDockableWidget dw)
        {
            if ((dw.DockableLocation & DockingLocation.Left) != 0) {
                
                DragCheck dc;
                dc.dockingLocation = DockingLocation.Left;
                DockNotebook dn = this.GetDockNotebook (DockingLocation.Left);
                if (dn != null) {
                    dc.rectangle = dn.Allocation;
                } else {
                    dc.rectangle = new Gdk.Rectangle (this.Allocation.X, this.Allocation.Y, 30, this.Allocation.Height);
                    foreach (KeyValuePair<DockingLocation, DockAutoHide> pair in this.dockAutoHides) {
                        
                        switch (pair.Key) {
                        
                        case DockingLocation.Top:
                            dc.rectangle.Y += pair.Value.HeightRequest;
                            dc.rectangle.Height -= pair.Value.HeightRequest;
                            break;
                        case DockingLocation.Bottom:
                            dc.rectangle.Height -= pair.Value.HeightRequest;
                            break;
                        case DockingLocation.Left:
                            dc.rectangle.X += pair.Value.WidthRequest;
                            break;
                        case DockingLocation.Right:
                            break;
                        }
                    }
                }
                dragChecks.Add (dc);
            }
            
            if ((dw.DockableLocation & DockingLocation.Top) != 0) {
                
                DragCheck dc;
                dc.dockingLocation = DockingLocation.Top;
                DockNotebook dn = this.GetDockNotebook (DockingLocation.Top);
                if (dn != null) {
                    dc.rectangle = dn.Allocation;
                } else {
                    dc.rectangle = new Gdk.Rectangle (this.Allocation.X, this.Allocation.Y, this.Allocation.Width, 30);
                    foreach (KeyValuePair<DockingLocation, DockAutoHide> pair in this.dockAutoHides) {
                        
                        switch (pair.Key) {
                        
                        case DockingLocation.Top:
                            dc.rectangle.Y += pair.Value.HeightRequest;
                            break;
                        case DockingLocation.Bottom:
                            break;
                        case DockingLocation.Left:
                            dc.rectangle.X += pair.Value.WidthRequest;
                            dc.rectangle.Width -= pair.Value.WidthRequest;
                            break;
                        case DockingLocation.Right:
                            dc.rectangle.Width -= pair.Value.WidthRequest;
                            break;
                        }
                    }
                }
                dragChecks.Add (dc);
            }
            
            if ((dw.DockableLocation & DockingLocation.Right) != 0) {
                
                DragCheck dc;
                dc.dockingLocation = DockingLocation.Right;
                DockNotebook dn = this.GetDockNotebook (DockingLocation.Right);
                if (dn != null) {
                    dc.rectangle = dn.Allocation;
                } else {
                    dc.rectangle = new Gdk.Rectangle (this.Allocation.X + this.Allocation.Width - 30, this.Allocation.Y, 30, this.Allocation.Height);
                    foreach (KeyValuePair<DockingLocation, DockAutoHide> pair in this.dockAutoHides) {
                        
                        switch (pair.Key) {
                        
                        case DockingLocation.Top:
                            dc.rectangle.Y += pair.Value.HeightRequest;
                            dc.rectangle.Height -= pair.Value.HeightRequest;
                            break;
                        case DockingLocation.Bottom:
                            dc.rectangle.Height -= pair.Value.HeightRequest;
                            break;
                        case DockingLocation.Left:
                            break;
                        case DockingLocation.Right:
                            dc.rectangle.X -= pair.Value.WidthRequest;
                            break;
                        }
                    }
                }
                dragChecks.Add (dc);
            }
            
            if ((dw.DockableLocation & DockingLocation.Bottom) != 0) {
                
                DragCheck dc;
                dc.dockingLocation = DockingLocation.Bottom;
                DockNotebook dn = this.GetDockNotebook (DockingLocation.Bottom);
                if (dn != null) {
                    dc.rectangle = dn.Allocation;
                } else {
                    dc.rectangle = new Gdk.Rectangle (this.Allocation.X, this.Allocation.Y + this.Allocation.Height - 30, this.Allocation.Width, 30);
                    foreach (KeyValuePair<DockingLocation, DockAutoHide> pair in this.dockAutoHides) {
                        
                        switch (pair.Key) {
                        
                        case DockingLocation.Top:
                            break;
                        case DockingLocation.Bottom:
                            dc.rectangle.Y -= pair.Value.HeightRequest;
                            break;
                        case DockingLocation.Left:
                            dc.rectangle.X += pair.Value.WidthRequest;
                            dc.rectangle.Width -= pair.Value.WidthRequest;
                            break;
                        case DockingLocation.Right:
                            dc.rectangle.Width -= pair.Value.WidthRequest;
                            break;
                        }
                    }
                }
                dragChecks.Add (dc);
            }
            
            if ((dw.DockableLocation & DockingLocation.Document) != 0) {
                
                DockNotebook dn = this.GetDockNotebook (DockingLocation.Document);
                if (dn != null) {
                    DragCheck dc;
                    dc.dockingLocation = DockingLocation.Document;
                    dc.rectangle = dn.Allocation;
                    dragChecks.Add (dc);
                }
            }
        }

        private DockingLocation ComputeDockingLocation (IDockableWidget dw)
        {
            DockingLocation dl = dw.DefaultDockingLocation;
            if ((dw.DockableLocation & dl) == DockingLocation.None) {
                
                if ((dw.DockableLocation & DockingLocation.Left) != DockingLocation.None) {
                    dl = DockingLocation.Left;
                } else if ((dw.DockableLocation & DockingLocation.Right) != DockingLocation.None) {
                    dl = DockingLocation.Right;
                } else if ((dw.DockableLocation & DockingLocation.Bottom) != DockingLocation.None) {
                    dl = DockingLocation.Bottom;
                } else if ((dw.DockableLocation & DockingLocation.Top) != DockingLocation.None) {
                    dl = DockingLocation.Top;
                } else if ((dw.DockableLocation & DockingLocation.Document) != DockingLocation.None) {
                    dl = DockingLocation.Document;
                } else {
                    dl = DockingLocation.None;
                }
            }
            return dl;
        }

        private DockAutoHide CreateDockAutoHide (DockingLocation dl)
        {
            if (dl == DockingLocation.None)
                throw new DockPanelException ("Create autohide: no docking location.");
            
            if (dl == DockingLocation.Document)
                throw new DockPanelException ("Create autohide: forbidden on document location.");
            
            DockAutoHide da = new DockAutoHide (this, dl);
            this.dockAutoHides.Add (dl, da);
            if (dl == DockingLocation.Top) {
                
                this.vbox.PackStart (da, false, false, 0);
                this.vbox.ReorderChild (da, 0);
            } else if (dl == DockingLocation.Bottom) {
                
                this.vbox.PackEnd (da, false, false, 0);
                this.vbox.ReorderChild (da, 2);
            } else if (dl == DockingLocation.Left) {
                
                this.hbox.PackStart (da, false, false, 0);
                this.hbox.ReorderChild (da, 0);
            } else if (dl == DockingLocation.Right) {
                
                this.hbox.PackEnd (da, false, false, 0);
                this.hbox.ReorderChild (da, 2);
            }
            
            return da;
        }

        private DockAutoHide GetDockAutoHide (DockingLocation dl, bool createIfNull)
        {
            if (this.dockAutoHides.ContainsKey (dl))
                return this.dockAutoHides[dl];
            
            if (!createIfNull)
                return null;
            
            return this.CreateDockAutoHide (dl);
        }

        private void RemoveDockAutoHide (DockAutoHide da)
        {
            if (da.Parent == vbox) {
                
                vbox.Remove (da);
            } else if (da.Parent == hbox) {
                
                hbox.Remove (da);
            } else
                throw new DockPanelException ("Remove autoHide: unknown autoHide.");
            
            this.dockAutoHides.Remove (da.DockingLocation);
        }

        private DockNotebook CreateDockNotebook (DockingLocation dl)
        {
            int pp = 0;
            if (dl == DockingLocation.Top) {
                
                pp = this.DefaultTopSize;
            } else if (dl == DockingLocation.Bottom) {

                pp = this.SizeWithoutAutoHide.Height - this.DefaultBottomSize;
            } else if (dl == DockingLocation.Left) {

                pp = this.DefaultLeftSize;
            } else if (dl == DockingLocation.Right) {

                pp = this.SizeWithoutAutoHide.Width - this.DefaultRightSize;
            }
            return CreateDockNotebook (dl, pp);
        }

        private DockNotebook CreateDockNotebook (DockingLocation dl, int pp)
        {
            if (dl == DockingLocation.None)
                throw new DockPanelException ("Create notebook: no docking location.");
            
            DockNotebook dn = new DockNotebook (this, dl);
            this.dockNotebooks.Add (dl, dn);
            if (dl == DockingLocation.Document) {
                
                this.dockRoot = dn;
                this.hbox.PackStart (dn, true, true, 0);
                this.hbox.ReorderChild (dn, 1);
            } else {
                
                Paned pane;
                if ((dl == DockingLocation.Top) || (dl == DockingLocation.Bottom))
                    pane = new VPaned ();
                else
                    pane = new HPaned ();
                
                if ((dl == DockingLocation.Top) || (dl == DockingLocation.Left)) {
                    
                    pane.Pack1 (dn, false, false);
                    if (this.dockRoot != null) {
                        Widget w = this.dockRoot;
                        this.hbox.Remove (w);
                        pane.Pack2 (w, false, false);
                    } else {
                        if (!this.dockNotebooks.ContainsKey (DockingLocation.Document)) {
                            DockNotebook dnd = new DockNotebook (this, DockingLocation.Document);
                            this.dockNotebooks.Add (DockingLocation.Document, dnd);
                            pane.Pack2 (dnd, true, false);
                        }
                    }
                    this.dockRoot = pane;
                    this.hbox.PackStart (pane, true, true, 0);
                    this.hbox.ReorderChild (pane, 1);
                } else {
                    
                    pane.Pack2 (dn, false, false);
                    if (this.dockRoot != null) {
                        Widget w = this.dockRoot;
                        this.hbox.Remove (w);
                        pane.Pack1 (w, false, false);
                    } else {
                        if (!this.dockNotebooks.ContainsKey (DockingLocation.Document)) {
                            DockNotebook dnd = new DockNotebook (this, DockingLocation.Document);
                            this.dockNotebooks.Add (DockingLocation.Document, dnd);
                            pane.Pack1 (dnd, true, false);
                        }
                    }
                    this.dockRoot = pane;
                    this.hbox.PackStart (pane, true, true, 0);
                    this.hbox.ReorderChild (pane, 1);
                }

                pane.Position = pp;
            }
            
            return dn;
        }

        private DockNotebook GetDockNotebook (DockingLocation dl)
        {
            return GetDockNotebook (dl, false);
        }

        private DockNotebook GetDockNotebook (DockingLocation dl, bool createIfNull)
        {
            if (this.dockNotebooks.ContainsKey (dl))
                return this.dockNotebooks[dl];
            
            if (!createIfNull)
                return null;
            
            return this.CreateDockNotebook (dl);
        }

        private void RemoveDockNotebook (DockNotebook dn)
        {
            if (dn.Parent is Paned) {

                Paned pn = (Paned)dn.Parent;
                Widget w;
                if (pn.Child1 == dn)
                    w = pn.Child2;
                else
                    w = pn.Child1;
                bool isDockNotebookDocument = ((w is DockNotebook) && (((DockNotebook)w).DockingLocation == DockingLocation.Document));
                pn.Remove (pn.Child1);
                pn.Remove (pn.Child2);
                if (pn == this.dockRoot) {
                    
                    this.hbox.Remove (pn);
                    this.dockRoot = w;
                    this.hbox.PackStart (w, true, true, 0);
                    this.hbox.ReorderChild (w, 1);
                } else if (pn.Parent is Paned) {
                    
                    Paned ppn = (Paned)pn.Parent;
                    if (ppn.Child1 == pn) {
                        
                        ppn.Remove (pn);
                        ppn.Pack1 (w, isDockNotebookDocument, false);
                    } else {
                        
                        ppn.Remove (pn);
                        ppn.Pack2 (w, isDockNotebookDocument, false);
                    }
                } else
                    throw new DockPanelException ("Remove notebook: unknown notebook.");
            }
            this.dockNotebooks.Remove (dn.DockingLocation);
        }

        private DragCheck GetDragLocation (Gdk.Point p)
        {
            foreach (DragCheck dc in this.dragChecks) {
                
                if (dc.rectangle.Contains (p))
                    return dc;
            }
            
            return new DragCheck ();
        }
        #endregion
    }
}

