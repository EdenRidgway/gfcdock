
// This file has been generated by the GUI designer. Do not modify.
namespace GFCDock
{
	public partial class DockDragWindow
	{
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget GFCDock.DockDragWindow
			this.Sensitive = false;
			this.Name = "GFCDock.DockDragWindow";
			this.Title = global::Mono.Unix.Catalog.GetString ("DockDragWindow");
			this.TypeHint = ((global::Gdk.WindowTypeHint)(4));
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			this.Resizable = false;
			this.AllowGrow = false;
			this.AcceptFocus = false;
			this.Decorated = false;
			this.FocusOnMap = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 500;
			this.DefaultHeight = 438;
			this.Show ();
		}
	}
}
