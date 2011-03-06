//
//  DockAutoHide.cs
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
    [System.ComponentModel.ToolboxItem(true)]
    internal partial class DockAutoHide : Gtk.Bin
    {
        #region Public Properties
        public DockingLocation DockingLocation {
            get { return this.dockingLocation; }
        }

        public DockPanel DockPanel {
            get { return this.dockPanel; }
        }

        public bool IsEmpty {
            get { return (this.contents.Count == 0); }
        }
        #endregion

        #region Constructor/Destructor
        public DockAutoHide (DockPanel dp, DockingLocation dl)
        {
            this.Build ();
            this.dockPanel = dp;
            this.dockingLocation = dl;
            Pango.Layout layout = this.GenerateLayout ("Tg");
            int w, h;
            layout.GetPixelSize (out w, out h);
            if (h < 20)
                h = 20;
            if ((this.DockingLocation & DockingLocation.Horizontal) != DockingLocation.None) {
                this.SetSizeRequest (-1, h);
            } else {
                this.SetSizeRequest (h, -1);
            }
        }
        #endregion

        #region Public functions
        public void Add (IDockableWidget dw, Gdk.Size sz)
        {
            if (this.GetContent (dw) == null) {

                dw.DockPanel = this.DockPanel;
                dw.DockingLocation = this.DockingLocation;
                dw.DockingState = DockingState.AutoHide;
                DockAutoHideContent c = new DockAutoHideContent (dw, sz, this.GenerateLayout (dw.Title));
                this.contents.Add (c);
                this.ResizeContents ();
                this.QueueDraw ();
            }
        }

        public void Remove (IDockableWidget dw)
        {
            DockAutoHideContent c = this.GetContent (dw);
            if (c != null) {

                if (c.AHWindow != null) {

                    c.AHWindow.Destroy ();
                    c.AHWindow = null;
                }
                this.contents.Remove (c);
                c.DockedWidget = null;
                c.Layout = null;
                this.ResizeContents ();
                this.QueueDraw ();
            }
        }

        public void Hide (IDockableWidget dw)
        {
            DockAutoHideContent c = this.GetContent (dw);
            if (c != null) {

                if (c.AHWindow != null) {

                    c.AHWindow.Destroy ();
                    c.AHWindow = null;
                    this.QueueDraw ();
                }
            }
        }

        public DockAutoHideSerializable GetSerializableObject ()
        {
            DockAutoHideSerializable o = new DockAutoHideSerializable ();
            o.DockingLocation = this.DockingLocation;
            o.DockedWidgets = new List<DockableWidgetSerializable> ();
            foreach (DockAutoHideContent dc in this.contents) {
                
                DockableWidgetSerializable ds = new DockableWidgetSerializable ();
                ds.Title = dc.DockedWidget.Title;
                ds.DefaultDockingLocation = dc.DockedWidget.DefaultDockingLocation;
                ds.DockableLocation = dc.DockedWidget.DockableLocation;
                ds.DockingCapability = dc.DockedWidget.DockingCapability;
                ds.Type = dc.DockedWidget.GetType ().ToString ();
                ds.Size = dc.rectangleAHWindow.Size;
                ds.Parameter = dc.DockedWidget.Parameter;
                o.DockedWidgets.Add (ds);
            }

            return o;
        }
        #endregion

        #region Overriden functions
        protected override void OnStyleSet (Gtk.Style previous_style)
        {
            base.OnStyleSet (previous_style);
            foreach (DockAutoHideContent c in this.contents) {

                if (c.AHWindow != null) {

                    c.AHWindow.Destroy ();
                    c.AHWindow = null;
                }
                c.Layout = this.GenerateLayout (c.DockedWidget.Title);
            }
            Pango.Layout layout = this.GenerateLayout ("Tg");
            int w, h;
            layout.GetPixelSize (out w, out h);
            if (h < 20)
                h = 20;
            if ((this.DockingLocation & DockingLocation.Horizontal) != DockingLocation.None) {
                this.SetSizeRequest (-1, h);
            } else {
                this.SetSizeRequest (h, -1);
            }
        }

        protected override void OnSizeAllocated (Gdk.Rectangle allocation)
        {
            base.OnSizeAllocated (allocation);
            this.ResizeContents ();
        }

        protected override bool OnExposeEvent (Gdk.EventExpose evnt)
        {
            base.OnExposeEvent (evnt);
            Gdk.Window win = evnt.Window;
            Gdk.Rectangle area = this.Allocation;
            win.DrawRectangle (this.Style.BackgroundGC (Gtk.StateType.Active), true, area);
            if ((this.DockingLocation & DockingLocation.Horizontal) != DockingLocation.None) {

                if (this.dockingLocation == DockingLocation.Top) {
                    win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), area.X, area.Y + area.Height - 1, area.X + area.Width - 1, area.Y + area.Height - 1);
                } else {
                    win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), area.X, area.Y, area.X + area.Width - 1, area.Y);
                }
                foreach (DockAutoHideContent c in this.contents) {

                    if ((c.MousIsHover) || (c.AHWindow != null)) {

                        DrawButton (win, c.Rectangle.X, c.Rectangle.Y, c.Rectangle.Width-1, c.Rectangle.Height-1);
                    }
                    int Y = this.Allocation.Y + ((this.Allocation.Height - c.Size.Height) / 2);
                    win.DrawLayout (this.Style.ForegroundGC (Gtk.StateType.Active), c.Rectangle.X + 5, Y, c.Layout);
                }
            }
            else {

                if (this.dockingLocation == DockingLocation.Left) {
                    win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), area.X + area.Width - 1, area.Y, area.X + area.Width - 1, area.Y + area.Height - 1);
                } else {
                    win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), area.X, area.Y, area.X, area.Y + area.Height - 1);
                }
                foreach (DockAutoHideContent c in this.contents) {

                    if ((c.MousIsHover) || (c.AHWindow != null)) {

                        DrawButton (win, c.Rectangle.X, c.Rectangle.Y, c.Rectangle.Width - 1, c.Rectangle.Height - 1);
                    }
                    int X = this.Allocation.X + ((this.Allocation.Width - c.Size.Height) / 2);
                    Cairo.Context ct = Gdk.CairoHelper.Create (win);
                    if (this.dockingLocation == DockingLocation.Left)
                        ct.Rotate (4.7123889803847);
                    else
                        ct.Rotate (1.5707963267949);
                    Pango.CairoHelper.UpdateLayout (ct, c.Layout);
                    win.DrawLayout (this.Style.ForegroundGC (Gtk.StateType.Active), X, c.Rectangle.Y + 5, c.Layout);
                    ((IDisposable)ct).Dispose ();
                }
            }
            return true;
        }
        #endregion

        #region Event handlers
        protected virtual void EventBox_EnterNotifyEvent (object o, Gtk.EnterNotifyEventArgs args)
        {
            Gdk.Point p = new Gdk.Point (this.Allocation.X + (int)args.Event.X, this.Allocation.Y + (int)args.Event.Y);
            this.CheckMouseIsHover (p);
        }

        protected virtual void EventBox_LeaveNotifyEvent (object o, Gtk.LeaveNotifyEventArgs args)
        {
            foreach (DockAutoHideContent c in this.contents) {

                c.MousIsHover = false;
            }
            this.QueueDraw ();
        }

        protected virtual void EventBox_MotionNotifyEvent (object o, Gtk.MotionNotifyEventArgs args)
        {
            Gdk.Point p = new Gdk.Point (this.Allocation.X + (int)args.Event.X, this.Allocation.Y + (int)args.Event.Y);
            this.CheckMouseIsHover (p);
        }
        #endregion

        #region Private
        private DockPanel dockPanel;
        private DockingLocation dockingLocation;
        private List<DockAutoHideContent> contents = new List<DockAutoHideContent> ();

        private DockAutoHideContent GetContent (IDockableWidget dw)
        {
            foreach (DockAutoHideContent c in contents) {
                if (c.DockedWidget == dw)
                    return c;
            }

            return null;
        }

        private Pango.Layout GenerateLayout (string text)
        {
            Pango.Layout layout = this.CreatePangoLayout (text);
            layout.Ellipsize = Pango.EllipsizeMode.Middle;
            layout.FontDescription = layout.Context.FontDescription.Copy ();
            int s = (int)(layout.FontDescription.Size / Pango.Scale.PangoScale);
            layout.FontDescription.Size = (int)((s - 1) * Pango.Scale.PangoScale);
            return layout;
        }

        private void ResizeContents ()
        {
            int S = 0;
            foreach (DockAutoHideContent c in this.contents) {

                if (c.AHWindow != null) {

                    c.AHWindow.Destroy ();
                    c.AHWindow = null;
                }
                S += c.Size.Width + 10;
            }
            double[] percents = new double[this.contents.Count];
            int idx = 0;
            foreach (DockAutoHideContent c in this.contents) {

                percents[idx++] = (double)(c.Size.Width + 10) / (double)S;
            }

            int A;
            if ((this.DockingLocation & DockingLocation.Horizontal) != DockingLocation.None) {

                A = this.Allocation.Width;
            }
            else {

                A = this.Allocation.Height;
            }
            A -= 10;

            if (S > A) {

                S -= A;
                idx = 0;
                foreach (DockAutoHideContent c in this.contents) {

                    int d = (int)((double)S * percents[idx++]);
                    if (d == 0)
                        d = 1;
                    c.Width = c.Size.Width - d;
                }
            }
            else {

                foreach (DockAutoHideContent c in this.contents) {

                    c.Width = c.Size.Width;
                }
            }
            if ((this.DockingLocation & DockingLocation.Horizontal) != DockingLocation.None) {

                int X = this.Allocation.X + 5;
                foreach (DockAutoHideContent c in this.contents) {

                    c.Rectangle = new Gdk.Rectangle (X, this.Allocation.Y, c.Width + 10, this.HeightRequest);
                    X += c.Width + 10;
                }
            }
            else {

                int Y = this.Allocation.Y + 5;
                foreach (DockAutoHideContent c in this.contents) {

                    c.Rectangle = new Gdk.Rectangle (this.Allocation.X, Y, this.WidthRequest, c.Width + 10);
                    Y += c.Width + 10;
                }
            }
            this.QueueDraw ();
        }

        private void DrawButton (Gdk.Window win, int x, int y, int width, int height)
        {
            Gdk.GC gc;
            int radius = 2;

            if ((width == 0) || (height == 0))
                return;
            
            gc = this.Style.LightGC (Gtk.StateType.Active);
            win.DrawRectangle (gc, true, x + radius, y + radius, width - radius, height - radius);
            gc = this.Style.DarkGC (Gtk.StateType.Active);
            win.DrawLine (gc, x + radius, y, x + width - radius, y);
            win.DrawLine (gc, x + width - radius, y, x + width, y + radius);
            win.DrawLine (gc, x + width, y + radius, x + width, y + height - radius);
            win.DrawLine (gc, x + width, y + height - radius, x + width - radius, y + height);
            win.DrawLine (gc, x + width - radius, y + height, x + radius, y + height);
            win.DrawLine (gc, x + radius, y + height, x, y + height - radius);
            win.DrawLine (gc, x, y + height - radius, x, y + radius);
            win.DrawLine (gc, x, y + radius, x + radius, y);
        }

        private void CheckMouseIsHover (Gdk.Point p)
        {
            bool change = false;
            foreach (DockAutoHideContent c in this.contents) {

                bool m = c.Rectangle.Contains (p);
                if (c.MousIsHover != m) {
                    
                    change = true;
                    c.MousIsHover = m;
                    if ((c.MousIsHover) && (c.AHWindow == null)) {

                        c.rectangleAHWindow.Location = this.Allocation.Location;
                        int X, Y;
                        this.GdkWindow.GetOrigin (out X, out Y);
                        c.rectangleAHWindow.X += X;
                        c.rectangleAHWindow.Y += Y;
                        if (this.dockingLocation == DockingLocation.Top) {

                            c.rectangleAHWindow.Y += this.Allocation.Height;
                            c.rectangleAHWindow.Width = this.Allocation.Width;
                        } else if (this.dockingLocation == DockingLocation.Bottom) {

                            c.rectangleAHWindow.Y -= c.rectangleAHWindow.Height;
                            c.rectangleAHWindow.Width = this.Allocation.Width;
                        } else if (this.dockingLocation == DockingLocation.Left) {

                            c.rectangleAHWindow.X += this.Allocation.Width;
                            c.rectangleAHWindow.Height = this.Allocation.Height;
                        } else if (this.dockingLocation == DockingLocation.Right) {

                            c.rectangleAHWindow.X -= c.rectangleAHWindow.Width;
                            c.rectangleAHWindow.Height = this.Allocation.Height;
                        }
                        Gdk.Rectangle rect = c.Rectangle;
                        rect.Offset (X, Y);
                        c.AHWindow = new DockAutoHideWindow (this, c, rect);
                        c.AHWindow.ShowAll ();
                    }
                }
            }
            if (change)
                this.QueueDraw ();
        }
        #endregion
    }
}

