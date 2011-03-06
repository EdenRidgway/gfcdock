//
//  MainWindow.cs
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
using Gtk;
using GFCDock;

namespace GFCDockSample
{
    public partial class MainWindow : Window
    {
        #region Public Properties
        #endregion

        #region Constructor/Destructor
        public MainWindow () : base(WindowType.Toplevel)
        {
            this.Build ();
            this.dockperspective.DockPanel = this.dockPanel;
            this.dockperspective.PerspectivePath = Directory.GetCurrentDirectory ();
            this.dockperspective.Current = "default";
        }
        #endregion

        #region Public functions
        #endregion

        #region Overriden functions
        #endregion

        #region Event handlers
        protected virtual void MainWindow_DeleteEvent (object o, Gtk.DeleteEventArgs args)
        {
            this.dockPanel.Serialize (this.dockperspective.Current);
            // exit
            Application.Quit ();
            args.RetVal = true;
        }

        protected virtual void Left_Clicked (object sender, System.EventArgs e)
        {
            NewContent (DockingLocation.Left, DockingCapability.AutoHideable, DockingLocation.Right);
        }

        protected virtual void Right_Clicked (object sender, System.EventArgs e)
        {
            NewContent (DockingLocation.Right, DockingCapability.All, DockingLocation.All & ~(DockingLocation.Document));
        }

        protected virtual void Top_Clicked (object sender, System.EventArgs e)
        {
            NewContent (DockingLocation.Top, DockingCapability.AutoHideable, DockingLocation.Bottom);
        }

        protected virtual void Bottom_Clicked (object sender, System.EventArgs e)
        {
            NewContent (DockingLocation.Bottom, DockingCapability.All, DockingLocation.All);
        }

        protected virtual void Document_Clicked (object sender, System.EventArgs e)
        {
            NewContent (DockingLocation.Document, DockingCapability.All, DockingLocation.All);
        }

        protected virtual void Clear_Activated (object sender, System.EventArgs e)
        {
            dockPanel.Clear ();
        }

        protected virtual void Serialize_Activated (object sender, System.EventArgs e)
        {
            dockPanel.Serialize (this.dockperspective.Current);
        }

        protected virtual void ThemeHelper_Activated (object sender, System.EventArgs e)
        {
            dockPanel.Dock (new ThemeHelper ());
        }
        #endregion

        #region Private
        private void NewContent (DockingLocation dl, DockingCapability dca, DockingLocation dal)
        {
            IDockableWidget dw;
            if (dl == DockingLocation.Document) {
                dw = new NoteView ();
                dw.Title += " long title";
            } else {
                dw = new TreePanel ();
            }
            dw.DockableLocation = dal | dl;
            dw.DefaultDockingLocation = dl;
            dw.DockingCapability = dca;
            dockPanel.Dock (dw);
        }
        #endregion
    }
}

