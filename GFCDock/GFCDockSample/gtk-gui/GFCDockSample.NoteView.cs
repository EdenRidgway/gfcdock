
// This file has been generated by the GUI designer. Do not modify.
namespace GFCDockSample
{
	public partial class NoteView
	{
		private global::Gtk.Alignment alignment1;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gtk.TextView textview3;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget GFCDockSample.NoteView
			global::Stetic.BinContainer.Attach (this);
			this.Name = "GFCDockSample.NoteView";
			// Container child GFCDockSample.NoteView.Gtk.Container+ContainerChild
			this.alignment1 = new global::Gtk.Alignment (0.5f, 0.5f, 1f, 1f);
			this.alignment1.Name = "alignment1";
			this.alignment1.BorderWidth = ((uint)(5));
			// Container child alignment1.Gtk.Container+ContainerChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.textview3 = new global::Gtk.TextView ();
			this.textview3.CanFocus = true;
			this.textview3.Name = "textview3";
			this.GtkScrolledWindow.Add (this.textview3);
			this.alignment1.Add (this.GtkScrolledWindow);
			this.Add (this.alignment1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}