//  
//  DockPerspective.cs
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

namespace GFCDock
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class DockPerspective : Gtk.Bin
    {
        #region Public Properties
        public DockPanel DockPanel {
            get { return this.dockPanel; }
            set {
                if (this.documentMaximized)
                    return;

                if (this.dockPanel != null)
                    this.DockPanel.DockDocumentMaximizedEvent -= DockPanel_DockDocumentMaximizedEvent;

                this.dockPanel = value;
                if (this.dockPanel != null) {

                    this.combobox.Sensitive = true;
                    this.DockPanel.DockDocumentMaximizedEvent += DockPanel_DockDocumentMaximizedEvent;
                }
                else {

                    this.combobox.Sensitive = false;
                }
            }
        }

        public string PerspectivePath {
            get { return this.path; }
            set {
                if (this.documentMaximized)
                    return;

                this.path = value;
                if (this.path.Length != 0) {

                    this.buttonNew.Sensitive = true;
                    this.buttonDelete.Sensitive = true;
                    this.listStore.Clear ();
                    bool defaultExist = false;
                    string[] filenames = Directory.GetFiles (this.path, "*_perspective.xml", SearchOption.TopDirectoryOnly);
                    foreach (string filename in filenames) {

                        string fn = this.GetName (filename);
                        if (fn.ToLower () == "default") {

                            defaultExist = true;
                        }
                        this.listStore.AppendValues (fn);
                    }
                    if (!defaultExist) {

                        this.listStore.AppendValues ("default");
                    }
                }
                else {

                    this.buttonNew.Sensitive = false;
                    this.buttonDelete.Sensitive = false;
                }
            }
        }

        public string Current {
            get { return this.current; }
            set {
                Gtk.TreeIter it;
                if (this.listStore.GetIterFirst (out it)) {

                    do {

                        string st = (string)this.listStore.GetValue (it, 0);
                        if (st == value) {

                            this.combobox.SetActiveIter (it);
                            return;
                        }
                    } while (this.listStore.IterNext (ref it));
                }
            }
        }
        #endregion

        #region Constructor/Destructor
        public DockPerspective ()
        {
            this.Build ();
            this.current = "";
            this.path = "";
            this.listStore = new Gtk.ListStore (typeof(string));
            this.listStore.SetSortColumnId (0, Gtk.SortType.Ascending);
            this.combobox.Model = this.listStore;
            Gtk.CellRendererText text = new Gtk.CellRendererText ();
            Pango.Layout l = this.CreatePangoLayout ("Gg");
            Pango.FontDescription fd = l.Context.FontDescription.Copy ();
            int s = (int)(fd.Size / Pango.Scale.PangoScale);
            fd.Size = (int)((s - 2) * Pango.Scale.PangoScale);
            text.FontDesc = fd;
            this.combobox.PackStart (text, false);
            this.combobox.AddAttribute (text, "text", 0);
        }
        #endregion

        #region Public functions
        public bool PerspectiveExist (string name)
        {
            Gtk.TreeIter it;
            if (this.listStore.GetIterFirst (out it)) {

                    do {

                        string st = (string)this.listStore.GetValue (it, 0);
                    if (st == name) {

                        return true;
                    }
                } while (this.listStore.IterNext (ref it));
            }
            return false;
        }
        #endregion

        #region Overriden functions
        #endregion

        #region Event handlers
        protected virtual void Combobox_Changed (object sender, System.EventArgs e)
        {
            if (this.documentMaximized)
                return;

            Gtk.TreeIter it;
            if (this.combobox.GetActiveIter (out it)) {

                string fn = (string)this.listStore.GetValue (it, 0);
                bool isDefault = (fn.ToLower () == "default");
                this.buttonDelete.Sensitive = !isDefault;
                fn = this.GetPath (fn);
                if (File.Exists (fn)) {

                    if (this.current.Length != 0) {

                        this.dockPanel.Serialize (this.current);
                    }
                    this.current = fn;
                    this.dockPanel.Deserialize (fn);
                }
                else if (isDefault) {

                    if (this.current.Length != 0) {

                        this.dockPanel.Serialize (this.current);
                    }
                    this.current = fn;
                }
                else if (this.current.Length != 0) {
                    this.Current = this.GetName (this.current);
                }
                else {
                    this.combobox.Active = -1;
                }
            }
        }

        protected virtual void ButtonNew_Clicked (object sender, System.EventArgs e)
        {
            DockNewPerspectiveDialog dlg = new DockNewPerspectiveDialog (this);
            if (dlg.Run () == (int)Gtk.ResponseType.Ok) {

                this.dockPanel.Serialize (this.GetPath (dlg.PerspectiveName));
                Gtk.TreeIter it = this.listStore.AppendValues (dlg.PerspectiveName);
                this.combobox.SetActiveIter (it);
            }
            dlg.Destroy ();
        }

        protected virtual void ButtonDelete_Clicked (object sender, System.EventArgs e)
        {
            if (this.current.Length > 0) {
                File.Delete (this.current);
            }
            string name = this.GetName (this.current);
            Gtk.TreeIter it;
            if (this.listStore.GetIterFirst (out it)) {
                
                do {
                    
                    string st = (string)this.listStore.GetValue (it, 0);
                    if (st == name) {
                        
                        this.listStore.Remove (ref it);
                        break;
                    }
                } while (this.listStore.IterNext (ref it));
            }
            this.current = "";
            this.combobox.Active = -1;
        }
        #endregion

        

        #region Private
        private DockPanel dockPanel;
        private string path;
        private string current;
        private Gtk.ListStore listStore;
        private bool documentMaximized = false;
        private bool comboboxSensitive;
        private bool buttonNewSensitive;
        private bool buttonDeleteSensitive;

        private string GetName (string path)
        {
            string name = System.IO.Path.GetFileName (path);
            return name.Replace ("_perspective.xml", "");
        }

        private string GetPath (string name)
        {
            return this.path + "/" + name + "_perspective.xml";
        }

        private void DockPanel_DockDocumentMaximizedEvent (object o, DockDocumentMaximizedEventArgs arg)
        {
            if (arg.Maximized) {

                this.comboboxSensitive = this.combobox.Sensitive;
                this.buttonNewSensitive = this.buttonNew.Sensitive;
                this.buttonDeleteSensitive = this.buttonDelete.Sensitive;
                this.combobox.Sensitive = false;
                this.buttonNew.Sensitive = false;
                this.buttonDelete.Sensitive = false;
            }
            else {

                this.combobox.Sensitive = this.comboboxSensitive;
                this.buttonNew.Sensitive = this.buttonNewSensitive;
                this.buttonDelete.Sensitive = this.buttonDeleteSensitive;
            }
            this.documentMaximized = arg.Maximized;
        }
        #endregion
    }
}

