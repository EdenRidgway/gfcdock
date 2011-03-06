//
//  DockEnum.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using Gtk;
using System;

namespace GFCDock
{
    public enum DockingLocation
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Horizontal = 3,
        Left = 4,
        TopLeft = 5,
        BottomLeft = 6,
        HorizontalLeft = 7,
        Right = 8,
        TopRight = 9,
        BottomRight = 10,
        HorizontalRight = 11,
        Vertical = 12,
        TopVertical = 13,
        BottomVertical = 14,
        HorizontalVertical = 15,
        Document = 16,
        DocumentTop = 17,
        DocumentBottom = 18,
        DocumentHorizontal = 19,
        DocumentLeft = 20,
        DocumentTopLeft = 21,
        DocumentBottomLeft = 22,
        DocumentHorizontalLeft = 23,
        DocumentRight = 24,
        DocumentTopRight = 25,
        DocumentBottomRight = 26,
        DocumentHorizontalRight = 27,
        DocumentVertical = 28,
        DocumentTopVertical = 29,
        DocumentBottomVertical = 30,
        All = 31,
    }

    public enum DockingState
    {
        None,
        Docked,
        AutoHide
    }

    public enum DockButtonType
    {
        None,
        Close,
        AutoHide_Pined,
        AutoHide_Unpined
    }

    public enum DockingCapability
    {
        None = 0,
        Closeable = 1,
        AutoHideable = 2,
        All = 3
    }
}

