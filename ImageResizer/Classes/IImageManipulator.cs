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
using System;
using System.Collections.Generic;

using Hawkynt.ColorProcessing;

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

    /// <summary>
    /// Tunable parameters this manipulator surfaces to the UI. Empty for fixed-default
    /// algorithms (the historical case); non-empty when the underlying upstream descriptor
    /// has a <see cref="ParameterDescriptor"/> set registered through
    /// <see cref="ParameterMetadata"/>.
    /// </summary>
    IReadOnlyList<ParameterDescriptor> Parameters { get; }

    /// <summary>
    /// Returns a manipulator instance bound to the supplied parameter values. For
    /// non-parametric manipulators (empty <see cref="Parameters"/>) this simply returns
    /// the current instance — callers can chain unconditionally.
    /// </summary>
    IImageManipulator CreateWith(IReadOnlyDictionary<string, object> values);
  }

  /// <summary>
  /// Shared default-empty parameter surface for non-parametric manipulators.
  /// </summary>
  internal static class ImageManipulatorDefaults {
    public static readonly IReadOnlyList<ParameterDescriptor> EmptyParameters
      = Array.Empty<ParameterDescriptor>();
  }
}
