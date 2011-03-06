//  
//  DockDragWindow.cs
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
    public partial class DockDragWindow : Gtk.Window
    {
        #region Public Properties
        #endregion

        #region Constructor/Destructor
        public DockDragWindow (Gdk.Rectangle rect) : base(Gtk.WindowType.Toplevel)
        {
            this.Build ();
            if (this.Screen.IsComposited)
                this.Opacity = 0.6;
            this.HeightRequest = rect.Height;
            this.WidthRequest = rect.Width;
            this.Move (rect.X, rect.Y);
            this.KeepAbove = true;
        }
        #endregion

        #region Public functions
        #endregion

        #region Overriden functions
        protected override bool OnExposeEvent (Gdk.EventExpose evnt)
        {
            base.OnExposeEvent (evnt);
            Gdk.Window win = evnt.Window;
            win.DrawRectangle (this.Style.BaseGC (Gtk.StateType.Selected), true, this.Allocation);
            return true;
        }
        #endregion
        
        #region Event handlers
        #endregion
        
        #region Private
        #endregion
    }
}

