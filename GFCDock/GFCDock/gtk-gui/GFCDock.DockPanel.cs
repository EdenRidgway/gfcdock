
// This file has been generated by the GUI designer. Do not modify.
namespace GFCDock
{
	public partial class DockPanel
	{
		private global::Gtk.VBox vbox;

		private global::Gtk.HBox hbox;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget GFCDock.DockPanel
			global::Stetic.BinContainer.Attach (this);
			this.Name = "GFCDock.DockPanel";
			// Container child GFCDock.DockPanel.Gtk.Container+ContainerChild
			this.vbox = new global::Gtk.VBox ();
			this.vbox.Name = "vbox";
			// Container child vbox.Gtk.Box+BoxChild
			this.hbox = new global::Gtk.HBox ();
			this.hbox.Name = "hbox";
			this.vbox.Add (this.hbox);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox[this.hbox]));
			w1.Position = 1;
			this.Add (this.vbox);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Show ();
		}
	}
}