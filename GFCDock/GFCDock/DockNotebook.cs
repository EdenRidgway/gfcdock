//
//  DockNotebook.cs
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
using System.Collections.Generic;

namespace GFCDock
{
    internal class DockNotebook : Gtk.Notebook
    {
        #region Public Properties
        public DockingLocation DockingLocation {
            get { return this.dockingLocation; }
        }

        public DockPanel DockPanel {
            get { return this.dockPanel; }
        }
        #endregion

        #region Constructor/Destructor
        public DockNotebook (DockPanel dp, DockingLocation dl)
        {
            this.dockPanel = dp;
            this.dockingLocation = dl;
            if (dl == DockingLocation.Document) {
                
                this.TabPos = Gtk.PositionType.Top;
                this.ShowTabs = true;
            } else {
                
                this.TabPos = Gtk.PositionType.Bottom;
                this.ShowTabs = false;
            }
            this.Scrollable = true;
            this.EnablePopup = true;
        }
        #endregion

        #region Public functions
        public void Dock (IDockableWidget dw)
        {
            dw.DockPanel = this.dockPanel;
            dw.DockingLocation = this.dockingLocation;
            dw.DockingState = DockingState.Docked;
            if (this.dockingLocation == DockingLocation.Document) {

                this.Page = this.AppendPageMenu ((Gtk.Widget)dw, new DockCaption (dw, 0, false, true), new Gtk.Label (dw.Title));
            } else {
                
                DockPane dp = new DockPane (dw);
                this.Page = this.AppendPageMenu (dp, new DockCaption (dw, -1, false, false), new Gtk.Label (dw.Title));
            }
        }

        public IDockableWidget GetDockedWidget (int index)
        {
            if (index >= this.NPages)
                throw new DockPanelException ("Trying to get a widget: invalid page index.");
            
            Gtk.Widget w = this.GetNthPage (index);
            if (this.dockingLocation == DockingLocation.Document) {
                
                if (!(w is IDockableWidget))
                    throw new DockPanelException ("Trying to get a widget: unknown type.");
                
                return (IDockableWidget)w;
            } else {
                
                if (!(w is DockPane))
                    throw new DockPanelException ("Trying to remove a widget: unknown type.");
                
                return ((DockPane)w).DockedWidget;
            }
        }

        public void Undock (IDockableWidget dw)
        {
            if (this.NPages == 0)
                throw new DockPanelException ("Trying to remove a widget: not exist on the notebook.");
            
            if (this.dockingLocation == DockingLocation.Document) {
                
                int pn = this.PageNum ((Gtk.Widget)dw);
                if (pn == -1)
                    throw new DockPanelException ("Trying to remove a widget: not exist on the notebook.");
                
                this.RemovePage (pn);
            } else {
                
                for (int idx = 0; idx < this.NPages; idx++) {
                    
                    Gtk.Widget w = this.GetNthPage (idx);
                    if (!(w is DockPane))
                        throw new DockPanelException ("Trying to remove a widget: unknown page type.");
                    
                    if (((DockPane)w).DockedWidget == dw) {
                        
                        this.RemovePage (idx);
                        return;
                    }
                }
                throw new DockPanelException ("Trying to remove a widget: not exist on the notebook.");
            }
        }

        public DockNotebookSerializable GetSerializableObject ()
        {
            DockNotebookSerializable o = new DockNotebookSerializable ();
            o.DockingLocation = this.DockingLocation;
            if (this.Parent is Gtk.Paned) {

                Gtk.Paned pn = (Gtk.Paned)this.Parent;
                o.PanePosition = pn.Position;
            }
            o.DockedWidgets = new List<DockableWidgetSerializable> ();
            for (int i = 0; i < this.NPages; i++) {

                IDockableWidget dw = this.GetDockedWidget (i);
                DockableWidgetSerializable ds = new DockableWidgetSerializable ();
                ds.Title = dw.Title;
                ds.DefaultDockingLocation = dw.DefaultDockingLocation;
                ds.DockableLocation = dw.DockableLocation;
                ds.DockingCapability = dw.DockingCapability;
                ds.Type = dw.GetType ().ToString ();
                ds.Parameter = dw.Parameter;
                o.DockedWidgets.Add (ds);
            }

            return o;
        }
        #endregion

        #region Overriden functions
        // Update ShowTabs property when a page is added/removed
        protected override void OnPageAdded (Gtk.Widget p0, uint p1)
        {
            base.OnPageAdded (p0, p1);
            this.UpdateShowTabs ();
        }

        protected override void OnPageRemoved (Gtk.Widget p0, uint p1)
        {
            base.OnPageRemoved (p0, p1);
            this.UpdateShowTabs ();
        }
        #endregion

        #region Private
        private DockPanel dockPanel;
        private DockingLocation dockingLocation;

        private void UpdateShowTabs ()
        {
            /*
             * only show tabs on the following conditions:
             *    we are on document location
             *  or
             *    we have more than one page
             */            
            if ((this.dockingLocation != DockingLocation.Document) && (this.ShowTabs != (this.NPages > 1))) {
                
                this.ShowTabs = (this.NPages > 1);
                this.QueueDraw ();
            }
        }
        #endregion
    }
}

