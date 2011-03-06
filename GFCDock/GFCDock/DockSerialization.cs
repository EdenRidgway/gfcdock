//  
//  DockSerialization.cs
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
using System.Xml;
using System.Collections.Generic;

namespace GFCDock
{
    public class DockPanelSerializable {

        public int DefaultLeftSize { get; set; }

        public int DefaultBottomSize { get; set; }

        public int DefaultRightSize { get; set; }

        public int DefaultTopSize { get; set; }

        public List<DockNotebookSerializable> DockNotebooks { get; set; }

        public List<DockAutoHideSerializable> DockAutoHides { get; set; }
    }

    public class DockNotebookSerializable {

        public DockingLocation DockingLocation { get; set; }

        public int PanePosition { get; set; }

        public List<DockableWidgetSerializable> DockedWidgets { get; set; }
    }

    public class DockAutoHideSerializable
    {

        public DockingLocation DockingLocation { get; set; }

        public List<DockableWidgetSerializable> DockedWidgets { get; set; }
    }

    public class DockableWidgetSerializable {

        public string Type { get; set; }

        public string Title { get; set; }

        public DockingLocation DockableLocation { get; set; }

        public DockingLocation DefaultDockingLocation { get; set; }

        public DockingCapability DockingCapability { get; set; }

        public Gdk.Size Size { get; set; }

        public string Parameter { get; set; }
    }
}