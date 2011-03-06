//  
//  NoteContent.cs
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
using GFCDock;

namespace GFCDockSample
{
    public partial class NoteView : Gtk.Bin, IDockableWidget
    {
        public NoteView ()
        {
            this.Build ();

            this.Title = "Note View";
        }

        public string Title { get; set; }
        public DockingLocation DockableLocation { get; set; }
        public DockingLocation DefaultDockingLocation { get; set; }
        public DockingCapability DockingCapability { get; set; }
        public DockPanel DockPanel { get; set; }
        public DockingLocation DockingLocation { get; set; }
        public DockingState DockingState { get; set; }
        public string Parameter { get; set; }
    }
}

