//
//  DockSizer.cs
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
    [System.ComponentModel.ToolboxItem(true)]
    internal partial class DockSizer : Gtk.Bin
    {
        #region Public Properties
        public event DockDragStateEventHandler DockDragStateEvent;
        #endregion

        #region Constructor/Destructor
        internal DockSizer (DockAutoHideWindow da, DockingLocation dl)
        {
            this.Build ();
            this.dockAutoHideWindow = da;
            this.dockingLocation = dl;
            if ((dl & DockingLocation.Horizontal) != DockingLocation.None) {
                
                this.SetSizeRequest (-1, 10);
            } else {
                
                this.SetSizeRequest (10, -1);
            }
        }
        #endregion

        #region Public functions
        #endregion

        #region Overriden functions
        protected override bool OnExposeEvent (Gdk.EventExpose ev)
        {
            base.OnExposeEvent (ev);
            Gdk.Window win = ev.Window;
            Gdk.Rectangle area = this.Allocation;
            if (this.MouseIsHover)
                win.DrawRectangle (this.Style.LightGC (Gtk.StateType.Active), true, area);
            else
                win.DrawRectangle (this.Style.BackgroundGC (Gtk.StateType.Active), true, area);
            if ((this.dockingLocation & DockingLocation.Horizontal) != DockingLocation.None) {

                Gtk.Style.PaintHandle (this.Style, win, Gtk.StateType.Active, Gtk.ShadowType.None, area, this, "", area.X, area.Y, area.Width,
                area.Height, Gtk.Orientation.Horizontal);
                if (this.dockingLocation == DockingLocation.Top) {
                    win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), area.X, area.Y, area.X + area.Width - 1, area.Y);
                } else {
                    win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), area.X, area.Y + area.Height - 1, area.X + area.Width - 1, area.Y + area.Height - 1);
                }
            } else {

                Gtk.Style.PaintHandle (this.Style, win, Gtk.StateType.Active, Gtk.ShadowType.None, area, this, "", area.X, area.Y, area.Width,
                area.Height, Gtk.Orientation.Vertical);
                if (this.dockingLocation == DockingLocation.Left) {
                    win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), area.X, area.Y, area.X, area.Y + area.Height - 1);
                } else {
                    win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), area.X + area.Width - 1, area.Y, area.X + area.Width - 1, area.Y + area.Height - 1);
                }
            }
            return true;
        }
        #endregion

        #region Event handlers
        protected virtual void EventBox_ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
        {
            
            this.canDrag = true;
            pointDragOrigin = new Gdk.Point ((int) args.Event.X, (int) args.Event.Y);
            if (this.GdkWindow != null)
                this.GdkWindow.Cursor = new Gdk.Cursor (Gdk.CursorType.Fleur);
            this.QueueDraw ();
        }

        protected virtual void EventBox_ButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
        {
            if (this.canDrag) {
                
                this.canDrag = false;
                if (this.MouseIsHover) {
                    this.GdkWindow.Cursor = new Gdk.Cursor (Gdk.CursorType.Hand1);
                } else if (this.GdkWindow != null) {
                    this.GdkWindow.Cursor = null;
                }
            }
            if (this.onDrag) {
                
                this.onDrag = false;
                Gdk.Point p = new Gdk.Point ((int)args.Event.X, (int)args.Event.Y);
                p.X -= this.pointDragOrigin.X;
                p.Y -= this.pointDragOrigin.Y;
                if (this.dockAutoHideWindow != null)
                    this.dockAutoHideWindow.SizerMove (p);
                if (this.DockDragStateEvent != null)
                    this.DockDragStateEvent (this, new DockDragStateEventArgs (this.onDrag));
            }
            this.QueueDraw ();
        }

        protected virtual void EventBox_EnterNotifyEvent (object o, Gtk.EnterNotifyEventArgs args)
        {
            this.MouseIsHover = true;
            if (!this.canDrag) {

                if (this.GdkWindow != null)
                    this.GdkWindow.Cursor = new Gdk.Cursor (Gdk.CursorType.Hand1);
            }
            this.QueueDraw ();
        }

        protected virtual void EventBox_LeaveNotifyEvent (object o, Gtk.LeaveNotifyEventArgs args)
        {
            this.MouseIsHover = false;
            if (!this.canDrag) {

                if (this.GdkWindow != null)
                    this.GdkWindow.Cursor = null;
            }
            this.QueueDraw ();
        }

        protected virtual void EventBox_MotionNotifyEvent (object o, Gtk.MotionNotifyEventArgs args)
        {
            if (this.canDrag) {

                this.onDrag = true;
                Gdk.Point p = new Gdk.Point ((int)args.Event.X, (int)args.Event.Y);
                p.X -= this.pointDragOrigin.X;
                p.Y -= this.pointDragOrigin.Y;
                if (this.dockAutoHideWindow != null)
                    this.dockAutoHideWindow.SizerMove (p);
                if (this.DockDragStateEvent != null)
                    this.DockDragStateEvent (this, new DockDragStateEventArgs (this.onDrag));
            }
        }
        #endregion

        #region Private
        private DockAutoHideWindow dockAutoHideWindow;
        private DockingLocation dockingLocation;
        private bool MouseIsHover;
        private bool canDrag;
        private bool onDrag;
        private Gdk.Point pointDragOrigin;
        #endregion
    }
}

