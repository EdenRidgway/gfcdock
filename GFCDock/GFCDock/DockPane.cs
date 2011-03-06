//
//  DockPane.cs
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
using System.Collections.Generic;
using Gtk;

namespace GFCDock
{
    [System.ComponentModel.ToolboxItem(true)]
    internal partial class DockPane : Gtk.Bin
    {
        #region Public Properties
        public event DockDragStateEventHandler DockDragStateEvent;

        public IDockableWidget DockedWidget {
            get { return this.dockedWidget; }
        }
        #endregion

        #region Constructor/Destructor
        public DockPane (IDockableWidget dw)
        {
            this.Build ();
            this.dockedWidget = dw;
            this.caption = new DockCaption (dw, -2, true, true);
            this.caption.DockDragStateEvent += new DockDragStateEventHandler (Caption_DockDragStateEvent);
            this.hbox = new HBox (false, 0);
            this.hbox.PackStart (this.caption, true, true, 0);
            this.vbox.PackStart (this.hbox, false, false, 0);
            this.vbox.PackEnd ((Gtk.Widget)dw, true, true, 0);
            this.ShowAll ();
        }
        #endregion

        #region Public functions
        #endregion

        #region Overriden functions
        protected override void OnParentSet (Widget previous_parent)
        {
            base.OnParentSet (previous_parent);
            if (this.Parent == null) {

                this.hbox.Remove (this.caption);
                this.vbox.Remove (this.hbox);
                this.vbox.Remove ((Gtk.Widget)this.dockedWidget);
                this.dockedWidget = null;
            }
        }
        #endregion

        #region Private
        private IDockableWidget dockedWidget;
        private HBox hbox;
        private DockCaption caption;

        private void Caption_DockDragStateEvent (object o, DockDragStateEventArgs arg)
        {
            if (this.DockDragStateEvent != null)
                this.DockDragStateEvent(this, arg);

        }
        #endregion
    }
}

