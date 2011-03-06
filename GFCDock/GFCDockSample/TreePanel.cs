//
//  TreePanel.cs
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
using System.IO;
using System.Xml;

namespace GFCDockSample
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class TreePanel : Gtk.Bin, IDockableWidget
    {
        public TreePanel ()
        {
            this.Build ();
            this.Title = "Tree Panel";

            Gtk.TreeStore store = new Gtk.TreeStore (typeof(string), typeof(string));
            
            for (int i = 0; i < 5; i++) {
                /*Gtk.TreeIter iter =*/ store.AppendValues ("Demo " + i, "Data " + i);
            }
            
            this.treeview1.Model = store;
            this.treeview1.HeadersVisible = true;
            
            this.treeview1.AppendColumn ("Demo", new Gtk.CellRendererText (), "text", 0);
            this.treeview1.AppendColumn ("Data", new Gtk.CellRendererText (), "text", 1);
        }

        public string Title { get; set; }
        public DockingLocation DockableLocation { get; set; }
        public DockingLocation DefaultDockingLocation { get; set; }
        public DockingCapability DockingCapability { get; set; }
        public DockPanel DockPanel { get; set; }
        public DockingLocation DockingLocation { get; set; }
        public DockingState DockingState { get; set; }
        public string Parameter {
            get {
                StringWriter sw = new StringWriter ();
                XmlWriter xw = new XmlTextWriter (sw);
                xw.WriteElementString ("CheckBox", this.checkbutton.Active.ToString ());
                xw.Close ();
                return sw.ToString ();
            }
            set {
                StringReader sr = new StringReader (value);
                XmlReader xr = new XmlTextReader (sr);
                this.checkbutton.Active = bool.Parse(xr.ReadElementString ("CheckBox"));
            }
        }

    }
}

