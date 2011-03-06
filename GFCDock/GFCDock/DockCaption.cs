//
//  DockCaption.cs
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
    internal partial class DockCaption : Gtk.Bin
    {
        #region Public Properties
        public event DockDragStateEventHandler DockDragStateEvent;
        #endregion

        #region Constructor/Destructor
        public DockCaption (IDockableWidget dw, int fontResize, bool drawBackground, bool btVisible)
        {
            this.Build ();
            this.dockedWidget = dw;
            this.fontResize = fontResize;
            this.drawBackground = drawBackground;
            this.btVisible = btVisible;
            this.GenerateLayout (dw.Title);
            int w, h;
            this.layout.GetPixelSize (out w, out h);
            if (h < 16)
                h = 16;
            w += 10;
            if ((btVisible) && ((dw.DockingCapability & DockingCapability.AutoHideable) != 0))
                w += 16;
            if ((btVisible) && ((dw.DockingCapability & DockingCapability.Closeable) != 0))
                w += 16;
            this.SetSizeRequest ((drawBackground) ? 0 : w, h);
            this.MouseIsHover = false;
            this.MouseIsHoverBtAutoHide = false;
            this.MouseIsHoverBtClose = false;
            this.btAutoHideIsPressed = false;
            this.btCloseIsPressed = false;
            this.canDrag = false;
            this.onDrag = false;
        }
        #endregion

        #region Public functions
        #endregion

        #region Overriden functions
        protected override bool OnExposeEvent (Gdk.EventExpose evnt)
        {
            base.OnExposeEvent (evnt);
            Gdk.Window win = evnt.Window;
            Gdk.Rectangle area = this.Allocation;
            Gtk.StateType st = (this.drawBackground) ? Gtk.StateType.Active : this.State;
            if (this.drawBackground) {
                
                if (this.MouseIsHover)
                    win.DrawRectangle (this.Style.LightGC (st), true, area);
                else
                    win.DrawRectangle (this.Style.BackgroundGC (st), true, area);
            }
            
            win.DrawLayout (this.Style.ForegroundGC (st), this.ptLayout.X, this.ptLayout.Y, this.layout);

            if (this.dockedWidget.DockingState == DockingState.Docked)
                this.DrawButton (DockButtonType.AutoHide_Pined, win, this.btAutoHideRect.X, this.btAutoHideRect.Y, this.btAutoHideRect.Width, this.btAutoHideRect.Height, this.MouseIsHoverBtAutoHide, this.btAutoHideIsPressed);
            else
                this.DrawButton (DockButtonType.AutoHide_Unpined, win, this.btAutoHideRect.X, this.btAutoHideRect.Y, this.btAutoHideRect.Width, this.btAutoHideRect.Height, this.MouseIsHoverBtAutoHide, this.btAutoHideIsPressed);
            this.DrawButton (DockButtonType.Close, win, this.btCloseRect.X, this.btCloseRect.Y, this.btCloseRect.Width, this.btCloseRect.Height, this.MouseIsHoverBtClose, this.btCloseIsPressed);
            
            return true;
        }

        protected override void OnParentSet (Gtk.Widget previous_parent)
        {
            base.OnParentSet (previous_parent);
            if (this.Parent == null) {

                this.dockedWidget = null;
            }
        }

        protected override void OnStyleSet (Gtk.Style previous_style)
        {
            base.OnStyleSet (previous_style);
            this.GenerateLayout (this.dockedWidget.Title);
            int w, h;
            this.layout.GetPixelSize (out w, out h);
            if (h < 16)
                h = 16;
            this.SetSizeRequest (this.WidthRequest, h);
        }

        protected override void OnSizeAllocated (Gdk.Rectangle allocation)
        {
            base.OnSizeAllocated (allocation);
            this.GenerateLayout (this.dockedWidget.Title);
        }
        #endregion

        #region Event handlers
        protected virtual void EventBox_ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
        {
            if (args.Event.Button == 1) {

                if (args.Event.Type == Gdk.EventType.ButtonPress) {

                    Gdk.Point p = new Gdk.Point (this.Allocation.X + (int)args.Event.X, this.Allocation.Y + (int)args.Event.Y);
                    if (this.btAutoHideRect.Contains (p)) {

                        this.MouseIsHoverBtAutoHide = true;
                        this.MouseIsHoverBtClose = false;
                        this.btAutoHideIsPressed = true;
                    } else if (this.btCloseRect.Contains (p)) {

                        this.MouseIsHoverBtAutoHide = false;
                        this.MouseIsHoverBtClose = true;
                        this.btCloseIsPressed = true;
                    } else {

                        this.canDrag = true;
                        if (this.GdkWindow != null)
                            this.GdkWindow.Cursor = new Gdk.Cursor (Gdk.CursorType.Fleur);
                    }
                    this.QueueDraw ();
                }
                else if (args.Event.Type == Gdk.EventType.TwoButtonPress) {

                    if (this.dockedWidget.DockingLocation == DockingLocation.Document) {

                       
                        if (this.canDrag) {
                            
                            this.canDrag = false;
                            if (this.MouseIsHover) {
                                this.GdkWindow.Cursor = new Gdk.Cursor (Gdk.CursorType.Hand1);
                            } else if (this.GdkWindow != null) {
                                this.GdkWindow.Cursor = null;
                            }
                        }
                        this.btAutoHideIsPressed = false;
                        this.btCloseIsPressed = false;
                        this.onDrag = false;
                        this.dockedWidget.DockPanel.ToggleMinimizeMaximizeDocument ();
                    }
                }
            }
        }

        protected virtual void EventBox_ButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
        {
            if (this.canDrag) {
                
                this.canDrag = false;
                if (this.MouseIsHover) {
                    this.GdkWindow.Cursor = new Gdk.Cursor (Gdk.CursorType.Hand1);
                }
                else if (this.GdkWindow != null) {
                    this.GdkWindow.Cursor = null;
                }
            }
            if (this.btAutoHideIsPressed) {
                
                this.btAutoHideIsPressed = false;
                if (this.MouseIsHoverBtAutoHide) {

                    if (this.dockedWidget.DockingState == DockingState.Docked)
                        this.dockedWidget.DockPanel.AutoHide (this.dockedWidget);
                    else if (this.dockedWidget.DockingState == DockingState.AutoHide)
                        this.dockedWidget.DockPanel.Dock (this.dockedWidget);
                }
            } else if (this.btCloseIsPressed) {
                
                this.btCloseIsPressed = false;
                if (this.MouseIsHoverBtClose) {

                    if (this.dockedWidget.DockingState == DockingState.Docked)
                        this.dockedWidget.DockPanel.UnDock (this.dockedWidget);
                    else if (this.dockedWidget.DockingState == DockingState.AutoHide)
                        this.dockedWidget.DockPanel.UnAutoHide (this.dockedWidget);
                }
            } else if (this.onDrag) {
                
                this.onDrag = false;
                if (this.dockedWidget != null) {
                    if (this.dockedWidget.DockPanel != null) {
                        Gdk.Point pr = new Gdk.Point ((int)args.Event.XRoot, (int)args.Event.YRoot);
                        this.dockedWidget.DockPanel.DragStop (this.dockedWidget, pr);
                    }
                }
                if (this.DockDragStateEvent != null)
                    this.DockDragStateEvent (this, new DockDragStateEventArgs (this.onDrag));

            }
            this.QueueDraw ();
        }

        protected virtual void EventBox_EnterNotifyEvent (object o, Gtk.EnterNotifyEventArgs args)
        {
            this.MouseIsHover = true;
            Gdk.Point p = new Gdk.Point (this.Allocation.X + (int)args.Event.X, this.Allocation.Y + (int)args.Event.Y);
            if (this.btAutoHideRect.Contains (p)) {
                
                this.MouseIsHoverBtAutoHide = true;
            } else if (this.btCloseRect.Contains (p)) {
                
                this.MouseIsHoverBtClose = true;
            } else {
                
                if (this.GdkWindow != null)
                    this.GdkWindow.Cursor = new Gdk.Cursor (Gdk.CursorType.Hand1);
            }
            this.QueueDraw ();
        }

        protected virtual void EventBox_LeaveNotifyEvent (object o, Gtk.LeaveNotifyEventArgs args)
        {
            this.MouseIsHover = false;
            this.MouseIsHoverBtAutoHide = false;
            this.MouseIsHoverBtClose = false;
            if (!this.canDrag) {
                
                if (this.GdkWindow != null)
                    this.GdkWindow.Cursor = null;
            }
            this.QueueDraw ();
        }

        protected virtual void EventBox_MotionNotifyEvent (object o, Gtk.MotionNotifyEventArgs args)
        {
            Gdk.Point p = new Gdk.Point (this.Allocation.X + (int)args.Event.X, this.Allocation.Y + (int)args.Event.Y);
            if (this.canDrag) {

                this.onDrag = true;
                if (this.dockedWidget != null) {
                    if (this.dockedWidget.DockPanel != null) {
                        Gdk.Point pr = new Gdk.Point ((int)args.Event.XRoot, (int)args.Event.YRoot);
                        this.dockedWidget.DockPanel.DragMove (this.dockedWidget, pr);
                    }
                }
                if (this.DockDragStateEvent != null)
                    this.DockDragStateEvent (this, new DockDragStateEventArgs (this.onDrag));
            } else {
                
                this.MouseIsHoverBtAutoHide = false;
                this.MouseIsHoverBtClose = false;
                if (this.btAutoHideRect.Contains (p)) {
                    
                    this.MouseIsHoverBtAutoHide = true;
                    if (this.GdkWindow != null)
                        this.GdkWindow.Cursor = null;
                } else if (this.btCloseRect.Contains (p)) {
                    
                    this.MouseIsHoverBtClose = true;
                    if (this.GdkWindow != null)
                        this.GdkWindow.Cursor = null;
                } else {
                    
                    if (this.GdkWindow != null)
                        this.GdkWindow.Cursor = new Gdk.Cursor (Gdk.CursorType.Hand1);
                }
                this.QueueDraw ();
            }
        }
        #endregion

        #region Private
        private IDockableWidget dockedWidget;
        private int fontResize;
        private bool drawBackground;
        private bool btVisible;
        private Pango.Layout layout;
        private bool MouseIsHover;
        private bool MouseIsHoverBtAutoHide;
        private bool MouseIsHoverBtClose;
        private bool btAutoHideIsPressed;
        private bool btCloseIsPressed;
        private bool canDrag;
        private bool onDrag;
        private Gdk.Point ptLayout;
        private Gdk.Rectangle btAutoHideRect;
        private Gdk.Rectangle btCloseRect;

        private void GenerateLayout (string text)
        {
            int w, h;
            
            this.layout = this.CreatePangoLayout (text);
            this.layout.Ellipsize = Pango.EllipsizeMode.Middle;
            w = this.Allocation.Width;
            if (this.btAutoHideRect.Width > 0)
                w -= 16;
            if (this.btCloseRect.Width > 0)
                w -= 16;
            this.layout.Width = (int)((w - 10) * Pango.Scale.PangoScale);
            if (this.fontResize != 0) {
                
                this.layout.FontDescription = this.layout.Context.FontDescription.Copy ();
                int s = (int)(this.layout.FontDescription.Size / Pango.Scale.PangoScale);
                this.layout.FontDescription.Size = (int)((s + this.fontResize) * Pango.Scale.PangoScale);
            }
            
            this.layout.GetPixelSize (out w, out h);
            this.ptLayout = new Gdk.Point (this.Allocation.X + 5, this.Allocation.Y + (this.Allocation.Height - h) / 2);
            
            w = this.Allocation.Width;
            if ((btVisible) && ((this.dockedWidget.DockingCapability & DockingCapability.Closeable) != 0)) {
                
                this.btCloseRect = new Gdk.Rectangle (this.Allocation.X + w - 16, this.Allocation.Y + 1 + (this.Allocation.Height - 16) / 2, 13, 13);
                w -= 16;
            }
            
            if ((btVisible) && (this.dockedWidget.DockingLocation != DockingLocation.Document) && ((this.dockedWidget.DockingCapability & DockingCapability.AutoHideable) != 0)) {
                
                this.btAutoHideRect = new Gdk.Rectangle (this.Allocation.X + w - 16, this.Allocation.Y + 1 + (this.Allocation.Height - 16) / 2, 13, 13);
            }
            
        }

        private void DrawButton (DockButtonType btType, Gdk.Window win, int x, int y, int width, int height, bool msIsHover, bool btIsPressed)
        {
            Gdk.GC gc;
            int radius = 2;
            int offs = 0;
            
            if ((width == 0) || (height == 0))
                return;
            
            if (msIsHover) {
                
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
                
                if (!btIsPressed) {
                    
                    gc = this.Style.LightGC (Gtk.StateType.Normal);
                    win.DrawLine (gc, x + radius, y + 1, x + width - radius, y + 1);
                    win.DrawLine (gc, x + 1, y + radius, x + radius, y + 1);
                    win.DrawLine (gc, x + 1, y + height - radius, x + 1, y + radius);
                } else {
                    
                    offs = 1;
                    gc = this.Style.LightGC (Gtk.StateType.Active);
                    win.DrawLine (gc, x + width + 1, y + radius + 1, x + width + 1, y + height - radius);
                    win.DrawLine (gc, x + width + 1, y + height - radius, x + width - radius, y + height + 1);
                    win.DrawLine (gc, x + width - radius, y + height + 1, x + radius + 1, y + height + 1);
                }
            }
            
            if (btType == DockButtonType.Close) {
                
                win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), x + 4 + offs, y + 3 + offs, x + 10 + offs, y + 9 + offs);
                win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), x + 3 + offs, y + 3 + offs, x + 10 + offs, y + 10 + offs);
                win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), x + 3 + offs, y + 4 + offs, x + 9 + offs, y + 10 + offs);
                
                win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), x + 4 + offs, y + 10 + offs, x + 10 + offs, y + 4 + offs);
                win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), x + 3 + offs, y + 10 + offs, x + 10 + offs, y + 3 + offs);
                win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), x + 3 + offs, y + 9 + offs, x + 9 + offs, y + 3 + offs);
            } else if (btType == DockButtonType.AutoHide_Pined) {
                
                win.DrawRectangle (this.Style.DarkGC (Gtk.StateType.Active), false, x + 5 + offs, y + 3 + offs, 4, 5);
                win.DrawRectangle (this.Style.DarkGC (Gtk.StateType.Active), false, x + 5 + offs, y + 3 + offs, 3, 5);
                win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), x + 4 + offs, y + 8 + offs, x + 10 + offs, y + 8 + offs);
                win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), x + 7 + offs, y + 8 + offs, x + 7 + offs, y + 11 + offs);
            } else if (btType == DockButtonType.AutoHide_Unpined) {

                win.DrawRectangle (this.Style.DarkGC (Gtk.StateType.Active), false, x + 3 + offs, y + 5 + offs, 5, 4);
                win.DrawRectangle (this.Style.DarkGC (Gtk.StateType.Active), false, x + 3 + offs, y + 5 + offs, 5, 3);
                win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), x + 8 + offs, y + 4 + offs, x + 8 + offs, y + 10 + offs);
                win.DrawLine (this.Style.DarkGC (Gtk.StateType.Active), x + 8 + offs, y + 7 + offs, x + 11 + offs, y + 7 + offs);
            }
        }
        #endregion
    }
}

