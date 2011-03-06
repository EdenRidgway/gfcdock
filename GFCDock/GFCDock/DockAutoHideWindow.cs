//
//  DockAutoHideWindow.cs
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
namespace GFCDock
{
    internal partial class DockAutoHideWindow : Gtk.Window
    {
        #region Public Properties
        #endregion

        #region Constructor/Destructor
        public DockAutoHideWindow (DockAutoHide da, DockAutoHideContent ahc, Gdk.Rectangle rp) : base(Gtk.WindowType.Popup)
        {
            this.Build ();
            this.TransientFor = (Gtk.Window)da.DockPanel.Toplevel;
            this.dockAutoHide = da;
            this.dockAutoHideContent = ahc;
            this.rectangleParent = rp;
            this.dockedWidget = ahc.DockedWidget;
            DockPane dp = new DockPane (this.dockedWidget);
            dp.DockDragStateEvent += new DockDragStateEventHandler (DockPaneSizer_DockDragStateEvent);
            DockSizer ds = new DockSizer (this, this.dockedWidget.DockingLocation);
            ds.DockDragStateEvent += new DockDragStateEventHandler (DockPaneSizer_DockDragStateEvent);
            Gtk.Box box = null;
            if (this.dockedWidget.DockingLocation == DockingLocation.Top) {
                
                box = new Gtk.VBox (false, 0);
                box.PackStart (dp, true, true, 0);
                box.PackEnd (ds, false, false, 0);
            } else if (this.dockedWidget.DockingLocation == DockingLocation.Bottom) {
                
                box = new Gtk.VBox (false, 0);
                box.PackStart (ds, false, false, 0);
                box.PackEnd (dp, true, true, 0);
            } else if (this.dockedWidget.DockingLocation == DockingLocation.Left) {
                
                box = new Gtk.HBox (false, 0);
                box.PackStart (dp, true, true, 0);
                box.PackEnd (ds, false, false, 0);
            } else if (this.dockedWidget.DockingLocation == DockingLocation.Right) {
                
                box = new Gtk.HBox (false, 0);
                box.PackStart (ds, false, false, 0);
                box.PackEnd (dp, true, true, 0);
            }
            if (box != null)
                this.Add (box);
            this.SetSizeRequest (ahc.rectangleAHWindow.Width, ahc.rectangleAHWindow.Height);
            this.Move (ahc.rectangleAHWindow.X, ahc.rectangleAHWindow.Y);
            this.ShowAll ();
        }
        #endregion

        #region Public functions
        public void SizerMove (Gdk.Point p)
        {
            if (this.dockedWidget.DockingLocation == DockingLocation.Top) {
                
                int height = this.dockAutoHideContent.rectangleAHWindow.Height + p.Y;
                if ((height > 40) && (height < this.dockAutoHide.DockPanel.SizeWithoutAutoHide.Height)) {
                    
                    this.dockAutoHideContent.rectangleAHWindow.Height = height;
                    this.SetSizeRequest (this.dockAutoHideContent.rectangleAHWindow.Width, this.dockAutoHideContent.rectangleAHWindow.Height);
                }
            } else if (this.dockedWidget.DockingLocation == DockingLocation.Bottom) {
                
                int height = this.dockAutoHideContent.rectangleAHWindow.Height - p.Y;
                if ((height > 40) && (height < this.dockAutoHide.DockPanel.SizeWithoutAutoHide.Height)) {
                    
                    this.dockAutoHideContent.rectangleAHWindow.Y += p.Y;
                    this.Move (this.dockAutoHideContent.rectangleAHWindow.X, this.dockAutoHideContent.rectangleAHWindow.Y);
                    this.dockAutoHideContent.rectangleAHWindow.Height = height;
                    this.SetSizeRequest (this.dockAutoHideContent.rectangleAHWindow.Width, this.dockAutoHideContent.rectangleAHWindow.Height);
                }
            } else if (this.dockedWidget.DockingLocation == DockingLocation.Left) {
                
                int width = this.dockAutoHideContent.rectangleAHWindow.Width + p.X;
                if ((width > 40) && (width < this.dockAutoHide.DockPanel.SizeWithoutAutoHide.Width)) {
                    
                    this.dockAutoHideContent.rectangleAHWindow.Width = width;
                    this.SetSizeRequest (this.dockAutoHideContent.rectangleAHWindow.Width, this.dockAutoHideContent.rectangleAHWindow.Height);
                }
            } else if (this.dockedWidget.DockingLocation == DockingLocation.Right) {
                
                int width = this.dockAutoHideContent.rectangleAHWindow.Width - p.X;
                if ((width > 40) && (width < this.dockAutoHide.DockPanel.SizeWithoutAutoHide.Width)) {
                    
                    this.dockAutoHideContent.rectangleAHWindow.X += p.X;
                    this.Move (this.dockAutoHideContent.rectangleAHWindow.X, this.dockAutoHideContent.rectangleAHWindow.Y);
                    this.dockAutoHideContent.rectangleAHWindow.Width = width;
                    this.SetSizeRequest (this.dockAutoHideContent.rectangleAHWindow.Width, this.dockAutoHideContent.rectangleAHWindow.Height);
                }
            }
        }
        #endregion

        #region Overriden functions
        protected override bool OnExposeEvent (Gdk.EventExpose evnt)
        {
            base.OnExposeEvent (evnt);
            Gdk.Window win = evnt.Window;
            Gdk.Rectangle area = this.Allocation;
            area.Width -= 1;
            area.Height -= 1;
            win.DrawRectangle (this.Style.DarkGC (Gtk.StateType.Active), false, area);
            return true;
        }

        protected override bool OnLeaveNotifyEvent (Gdk.EventCrossing evnt)
        {
            Gdk.Point p = new Gdk.Point ((int)evnt.XRoot, (int)evnt.YRoot);
            Gdk.Rectangle r = this.dockAutoHideContent.rectangleAHWindow;
            if ((evnt.Detail != Gdk.NotifyType.Inferior) && (!r.Contains (p)))
                this.dockAutoHide.Hide (this.dockedWidget);
            return base.OnLeaveNotifyEvent (evnt);
        }

        protected override bool OnMotionNotifyEvent (Gdk.EventMotion evnt)
        {
            Gdk.Point p = new Gdk.Point ((int)evnt.XRoot, (int)evnt.YRoot);
            Gdk.Rectangle r = this.dockAutoHideContent.rectangleAHWindow;
            if ((!this.onDrag) && (!r.Contains (p)) && (!this.rectangleParent.Contains (p)))
                this.dockAutoHide.Hide (this.dockedWidget);
            return base.OnMotionNotifyEvent (evnt);
        }
        #endregion

        #region Event handlers
        #endregion

        #region Private
        private DockAutoHide dockAutoHide;
        private DockAutoHideContent dockAutoHideContent;
        private IDockableWidget dockedWidget;
        private Gdk.Rectangle rectangleParent;
        private bool onDrag;

        private void DockPaneSizer_DockDragStateEvent (object o, DockDragStateEventArgs arg)
        {
            this.onDrag = arg.OnDrag;
        }
        #endregion
    }
}

