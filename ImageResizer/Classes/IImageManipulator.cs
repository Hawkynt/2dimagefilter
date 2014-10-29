#region (c)2008-2015 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2008-2015 Hawkynt

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
#endregion
namespace Classes {
  internal interface IImageManipulator {
    bool SupportsWidth { get; }
    bool SupportsHeight { get; }
    bool SupportsRepetitionCount { get; }
    bool SupportsGridCentering { get; }
    bool SupportsThresholds { get; }
    bool SupportsRadius { get; }
    bool ChangesResolution { get; }
    string Description { get; }
  }
}
