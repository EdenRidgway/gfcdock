//  
//  DockAutoHideContent.cs
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
    internal class DockAutoHideContent
    {
        #region Public Properties
        public IDockableWidget DockedWidget;
        public Pango.Layout Layout {
            get { return this.layout; }
            set {
                this.layout = value;
                if (this.layout != null) {
                    int W, H;
                    this.layout.GetPixelSize (out W, out H);
                    this.Size = new Gdk.Size (W, H);
                    this.layout.Width = (int)(this.width * Pango.Scale.PangoScale);
                }
            }
        }
        public Gdk.Size Size;
        public Gdk.Rectangle Rectangle;
        public bool MousIsHover;
        public DockAutoHideWindow AHWindow;
        public Gdk.Rectangle rectangleAHWindow;
        public int Width {
            get { return this.width; }
            set {
                this.width = value;
                if (this.layout != null)
                    this.layout.Width = (int)(this.width * Pango.Scale.PangoScale);
            }
        }
        #endregion

        #region Constructor/Destructor
        public DockAutoHideContent (IDockableWidget dw, Gdk.Size sz, Pango.Layout l)
        {
            this.DockedWidget = dw;
            this.layout = l;
            int W, H;
            this.layout.GetPixelSize (out W, out H);
            this.Size = new Gdk.Size (W, H);
            this.width = W;
            this.MousIsHover = false;
            this.AHWindow = null;
            this.rectangleAHWindow = new Gdk.Rectangle (0, 0, 0, 0);
            this.rectangleAHWindow.Size = sz;
        }
        #endregion

        #region Public functions
        #endregion

        #region Overriden functions
        #endregion

        #region Event handlers
        #endregion

        #region Private
        private int width;
        private Pango.Layout layout;
        #endregion
    }
}

