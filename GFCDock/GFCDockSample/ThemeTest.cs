//
//  ColorTest.cs
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
using Gtk;
using Cairo;
using Pango;

namespace GFCDockSample
{
    [System.ComponentModel.ToolboxItem(true)]
    public class ThemeTest : Gtk.DrawingArea
    {
        public ThemeTest ()
        {
        }

        protected override bool OnExposeEvent (Gdk.EventExpose ev)
        {
            base.OnExposeEvent (ev);

            Gdk.Window win = ev.Window;
            Gdk.Rectangle area = ev.Area;

            int x = 10;
            int y = 10;
            foreach (StateType item in Enum.GetValues (typeof(StateType)))
            {
                win.DrawRectangle (this.Style.LightGC (item), true, x, y, 20, 20);
                win.DrawRectangle (this.Style.MidGC (item), true, x, y + 21, 20, 20);
                win.DrawRectangle (this.Style.DarkGC (item), true, x, y + 42, 20, 20);
                win.DrawRectangle (this.Style.BaseGC (item), true, x, y + 63, 20, 20);
                win.DrawRectangle (this.Style.BackgroundGC (item), true, x, y + 84, 20, 20);
                win.DrawRectangle (this.Style.ForegroundGC (item), true, x, y + 105, 20, 20);
                x += 21;
            }
            x += 5;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("LightGC"));
            y += 21;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("MidGC"));
            y += 21;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("DarkGC"));
            y += 21;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("BaseGC"));
            y += 21;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("BackgroundGC"));
            y += 21;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("ForegroundGC"));
            y += 31;
            x = 11;
            foreach (StateType item in Enum.GetValues (typeof(StateType)))
            {
                win.DrawRectangle (this.Style.BackgroundGC (item), true, x, y, 200, 20);
                Pango.Layout layout = this.CreatePangoLayout (item.ToString () + " (" + ((int)item).ToString () + ")");
                Gtk.Style.PaintLayout (this.Style, win, item, false, area, this, "", x, y, layout);
                y += 21;
                x += 21;
            }
            y += 10;
            x = 10;
            int ys = y;
            foreach (ShadowType item in Enum.GetValues (typeof(ShadowType)))
            {
                y = ys;
                Gtk.Style.PaintBox (this.Style, win, this.State, item, area, this, "PaintBox", x, y, 20,
                20);
                y += 25;
                Gtk.Style.PaintBoxGap (this.Style, win, this.State, item, area, this, "PaintBoxGap", x, y, 20,
                20, PositionType.Left | PositionType.Right | PositionType.Bottom | PositionType.Top, 0, 1);
                y += 25;
                Gtk.Style.PaintCheck (this.Style, win, this.State, item, area, this, "PaintCheck", x, y, 20,
                20);
                y += 25;
                Gtk.Style.PaintDiamond (this.Style, win, this.State, item, area, this, "PaintDiamond", x, y, 20,
                20);
                y += 25;
                Gtk.Style.PaintExtension (this.Style, win, this.State, item, area, this, "PaintExtension", x, y, 20,
                20, PositionType.Left);
                y += 25;
                Gtk.Style.PaintFlatBox (this.Style, win, StateType.Active, item, area, this, "PaintFlatBox", x, y, 20,
                20);
                y += 25;
                Gtk.Style.PaintFocus (this.Style, win, this.State, area, this, "PaintFocus", x, y, 20,
                20);
                y += 25;
                Gtk.Style.PaintHandle (this.Style, win, this.State, item, area, this, "PaintHandle", x, y, 20,
                7, (x < 60) ? Orientation.Horizontal : Orientation.Vertical);
                y += 25;
                Gtk.Style.PaintOption (this.Style, win, this.State, item, area, this, "PaintOption", x, y, 20,
                20);
                y += 25;
                Gtk.Style.PaintSlider (this.Style, win, this.State, item, area, this, "PaintSlider", x, y, 20,
                20, (x < 60) ? Orientation.Horizontal : Orientation.Vertical);
                y += 25;
                Gtk.Style.PaintTab (this.Style, win, this.State, item, area, this, "PaintTab", x, y, 20,
                20);
                y += 25;
                x += 21;
            }
            int xs = x;
            x = 10;
            foreach (ExpanderStyle item in Enum.GetValues (typeof(ExpanderStyle)))
            {
                Gtk.Style.PaintExpander (this.Style, win, this.State, area, this, "PaintExpander", x+10, y+10, item);
                x += 21;
            }
            y = ys;
            x = xs + 5;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("PaintBox"));
            y += 25;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("PaintBoxGap"));
            y += 25;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("PaintCheck"));
            y += 25;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("PaintDiamond"));
            y += 25;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("PaintExtension"));
            y += 25;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("PaintFlatBox"));
            y += 25;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("PaintFocus"));
            y += 25;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("PaintHandle"));
            y += 25;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("PaintOption"));
            y += 25;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("PaintSlider"));
            y += 25;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("PaintTab"));
            y += 25;
            Gtk.Style.PaintLayout (this.Style, win, StateType.Normal, false, area, this, "", x, y, this.CreatePangoLayout ("PaintExpander"));
            y += 25;
            return true;
        }
        protected override void OnSizeAllocated (Gdk.Rectangle allocation)
        {
            base.OnSizeAllocated (allocation);
            // Insert layout code here.
            
        }
        protected override void OnSizeRequested (ref Requisition requisition)
        {
            // Calculate desired size here.
            requisition.Width = 300;
            requisition.Height = 600;
        }
    }
}

