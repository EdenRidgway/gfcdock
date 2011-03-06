//  
//  DockPanelException.cs
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
using System.Runtime.Serialization;

namespace GFCDock
{
	[Serializable]
	public class DockPanelException : Exception
	{
        #region Public Properties
        #endregion

        #region Constructor/Destructor
        public DockPanelException () : base()
        {
        }

        public DockPanelException (string message) : base(message)
        {
        }

        public DockPanelException (string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DockPanelException (SerializationInfo info, StreamingContext context): base(info, context)
        {
        }
        #endregion

        #region Public functions
        #endregion

        #region Overriden functions
        #endregion

        #region Event handlers
        #endregion

        #region Private
        #endregion
	}
}

