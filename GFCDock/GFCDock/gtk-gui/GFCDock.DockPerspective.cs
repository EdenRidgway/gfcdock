
// This file has been generated by the GUI designer. Do not modify.
namespace GFCDock
{
	public partial class DockPerspective
	{
		private global::Gtk.HBox hbox1;

		private global::Gtk.ComboBox combobox;

		private global::Gtk.Button buttonNew;

		private global::Gtk.Button buttonDelete;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget GFCDock.DockPerspective
			global::Stetic.BinContainer.Attach (this);
			this.Name = "GFCDock.DockPerspective";
			// Container child GFCDock.DockPerspective.Gtk.Container+ContainerChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 5;
			// Container child hbox1.Gtk.Box+BoxChild
			this.combobox = new global::Gtk.ComboBox ();
			this.combobox.TooltipMarkup = "Current perspective";
			this.combobox.Sensitive = false;
			this.combobox.Name = "combobox";
			this.hbox1.Add (this.combobox);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.combobox]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonNew = new global::Gtk.Button ();
			this.buttonNew.TooltipMarkup = "Create a new perspective";
			this.buttonNew.Sensitive = false;
			this.buttonNew.CanFocus = true;
			this.buttonNew.Name = "buttonNew";
			this.buttonNew.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child buttonNew.Gtk.Container+ContainerChild
			global::Gtk.Alignment w2 = new global::Gtk.Alignment (0.5f, 0.5f, 0f, 0f);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w3 = new global::Gtk.HBox ();
			w3.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w4 = new global::Gtk.Image ();
			w4.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Menu);
			w3.Add (w4);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w6 = new global::Gtk.Label ();
			w3.Add (w6);
			w2.Add (w3);
			this.buttonNew.Add (w2);
			this.hbox1.Add (this.buttonNew);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.buttonNew]));
			w10.Position = 1;
			w10.Expand = false;
			w10.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonDelete = new global::Gtk.Button ();
			this.buttonDelete.TooltipMarkup = "Delete current perspective";
			this.buttonDelete.Sensitive = false;
			this.buttonDelete.CanFocus = true;
			this.buttonDelete.Name = "buttonDelete";
			this.buttonDelete.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child buttonDelete.Gtk.Container+ContainerChild
			global::Gtk.Alignment w11 = new global::Gtk.Alignment (0.5f, 0.5f, 0f, 0f);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w12 = new global::Gtk.HBox ();
			w12.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w13 = new global::Gtk.Image ();
			w13.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-remove", global::Gtk.IconSize.Menu);
			w12.Add (w13);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w15 = new global::Gtk.Label ();
			w12.Add (w15);
			w11.Add (w12);
			this.buttonDelete.Add (w11);
			this.hbox1.Add (this.buttonDelete);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.buttonDelete]));
			w19.Position = 2;
			w19.Expand = false;
			w19.Fill = false;
			this.Add (this.hbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.combobox.Changed += new global::System.EventHandler (this.Combobox_Changed);
			this.buttonNew.Clicked += new global::System.EventHandler (this.ButtonNew_Clicked);
			this.buttonDelete.Activated += new global::System.EventHandler (this.ButtonDelete_Clicked);
		}
	}
}
