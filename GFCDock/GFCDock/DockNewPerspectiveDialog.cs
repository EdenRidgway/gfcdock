//  
//  DockNewPerspectiveDialog.cs
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
    internal partial class DockNewPerspectiveDialog : Gtk.Dialog
    {
        #region Public Properties
        public string PerspectiveName;
        #endregion

        #region Constructor/Destructor
        public DockNewPerspectiveDialog (DockPerspective dp)
        {
            this.TransientFor = (Gtk.Window)dp.Toplevel;
            this.Build ();
            this.dockPerspective = dp;
        }
        #endregion

        #region Public functions
        #endregion

        #region Overriden functions
        protected override void OnResponse (Gtk.ResponseType response_id)
        {
            if (response_id == Gtk.ResponseType.Ok) {

                PerspectiveName = entry.Text;
            }
            base.OnResponse (response_id);
        }
        #endregion

        #region Event handlers
        protected virtual void Entry_Changed (object sender, System.EventArgs e)
        {
            this.buttonOk.Sensitive = !this.dockPerspective.PerspectiveExist (entry.Text);
        }
        #endregion

        #region Private
        private DockPerspective dockPerspective;
        #endregion
    }
}

