
// This file has been generated by the GUI designer. Do not modify.
namespace GFCDockSample
{
	public partial class TreePanel
	{
		private global::Gtk.VBox vbox1;

		private global::Gtk.CheckButton checkbutton;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gtk.TreeView treeview1;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget GFCDockSample.TreePanel
			global::Stetic.BinContainer.Attach (this);
			this.Name = "GFCDockSample.TreePanel";
			// Container child GFCDockSample.TreePanel.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.checkbutton = new global::Gtk.CheckButton ();
			this.checkbutton.CanFocus = true;
			this.checkbutton.Name = "checkbutton";
			this.checkbutton.Label = global::Mono.Unix.Catalog.GetString ("Saved Parameter");
			this.checkbutton.DrawIndicator = true;
			this.vbox1.Add (this.checkbutton);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.checkbutton]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.treeview1 = new global::Gtk.TreeView ();
			this.treeview1.CanFocus = true;
			this.treeview1.Name = "treeview1";
			this.GtkScrolledWindow.Add (this.treeview1);
			this.vbox1.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.GtkScrolledWindow]));
			w3.Position = 1;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}
