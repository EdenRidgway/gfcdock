//
//  IDockableWidget.cs
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
    public interface IDockableWidget
    {
        /* Configuration properties */
        string Title { get; set; }

        DockingLocation DockableLocation { get; set; }

        DockingLocation DefaultDockingLocation { get; set; }

        DockingCapability DockingCapability { get; set; }

        /* internal used properties */
        DockPanel DockPanel { get; set; }

        DockingLocation DockingLocation { get; set; }

        DockingState DockingState { get; set; }

        /* Serialize/Deserialize parameters
         *
         * This string is get when the widget is about to be serialized,
         * means about to be saved in a perspective, and set when the widget
         * is deserialized, means restored from a perspective
         */
        string Parameter { get; set; }
    }
}

