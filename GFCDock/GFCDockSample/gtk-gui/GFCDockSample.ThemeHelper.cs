
// This file has been generated by the GUI designer. Do not modify.
namespace GFCDockSample
{
	public partial class ThemeHelper
	{
		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::GFCDockSample.ThemeTest themetest1;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget GFCDockSample.ThemeHelper
			global::Stetic.BinContainer.Attach (this);
			this.Name = "GFCDockSample.ThemeHelper";
			// Container child GFCDockSample.ThemeHelper.Gtk.Container+ContainerChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			global::Gtk.Viewport w1 = new global::Gtk.Viewport ();
			w1.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport.Gtk.Container+ContainerChild
			this.themetest1 = new global::GFCDockSample.ThemeTest ();
			this.themetest1.Name = "themetest1";
			w1.Add (this.themetest1);
			this.GtkScrolledWindow.Add (w1);
			this.Add (this.GtkScrolledWindow);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}