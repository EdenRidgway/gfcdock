//  
//  DockDocumentMaximizedEventArgs.cs
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
    public class DockDocumentMaximizedEventArgs : EventArgs
    {
        #region Public Properties
        public bool Maximized {
            get { return this.maximized; }
        }
        #endregion

        #region Constructor/Destructor
        public DockDocumentMaximizedEventArgs (bool maximized)
        {
            this.maximized = maximized;
        }
        #endregion

        #region Public functions
        #endregion

        #region Overriden functions
        #endregion

        #region Event handlers
        #endregion

        #region Private
        private bool maximized;
        #endregion
    }

    internal delegate void DockDocumentMaximizedEventHandler (object sender, DockDocumentMaximizedEventArgs e);
}

