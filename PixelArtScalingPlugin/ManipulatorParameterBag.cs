#region (c)2008-2026 Hawkynt
/*
 *  Image filtering library
    Copyright (C) 2008-2026 Hawkynt

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
using System.ComponentModel;
using System.Globalization;
using System.Linq;

using Hawkynt.ColorProcessing;

namespace PixelArtScaling {
  /// <summary>
  /// Plugin-side twin of the WinForms exe's <c>Classes.ManipulatorParameterBag</c> (PG1).
  /// Surfaces a <see cref="ManipulatorEntry"/>'s <see cref="ParameterDescriptor"/> set as a
  /// set of typed properties to a <see cref="System.Windows.Forms.PropertyGrid"/>. Backed by
  /// a <see cref="Dictionary{TKey,TValue}"/> keyed on parameter name; each property's
  /// <see cref="PropertyDescriptor"/> reads/writes that dictionary.
  /// </summary>
  /// <remarks>
  /// <para>
  /// We deliberately avoid <see cref="System.Reflection.Emit"/>: a custom
  /// <see cref="ICustomTypeDescriptor"/> + per-parameter <see cref="PropertyDescriptor"/>
  /// subclass is sufficient and works on net48.
  /// </para>
  /// <para>
  /// Numeric ranges (Min/Max) are clamped on set so the grid can't push a value outside
  /// what the descriptor allows. Choice / enum descriptors expose a
  /// <see cref="StandardValuesCollection"/> via a private converter so PropertyGrid renders
  /// a drop-down.
  /// </para>
  /// </remarks>
  internal sealed class ManipulatorParameterBag : ICustomTypeDescriptor {

    private readonly IReadOnlyList<ParameterDescriptor> _descriptors;
    private readonly Dictionary<string, object> _values;
    private readonly PropertyDescriptorCollection _properties;

    private ManipulatorParameterBag(IReadOnlyList<ParameterDescriptor> descriptors, IReadOnlyDictionary<string, object> initialValues) {
      this._descriptors = descriptors;
      this._values = new Dictionary<string, object>(StringComparer.Ordinal);
      var props = new PropertyDescriptor[descriptors.Count];
      for (var i = 0; i < descriptors.Count; ++i) {
        var d = descriptors[i];
        var seed = d.DefaultValue;
        if (initialValues != null && initialValues.TryGetValue(d.Name, out var preset))
          seed = preset;
        this._values[d.Name] = seed;
        props[i] = new ParameterPropertyDescriptor(d);
      }
      this._properties = new PropertyDescriptorCollection(props, readOnly: true);
    }

    /// <summary>
    /// Builds a fresh bag whose properties mirror <paramref name="descriptors"/>; every
    /// entry is initialised to its <see cref="ParameterDescriptor.DefaultValue"/>.
    /// </summary>
    public static ManipulatorParameterBag CreateFor(IReadOnlyList<ParameterDescriptor> descriptors) {
      if (descriptors == null)
        throw new ArgumentNullException(nameof(descriptors));
      return new ManipulatorParameterBag(descriptors, null);
    }

    /// <summary>
    /// Builds a bag whose properties mirror <paramref name="descriptors"/>; for each descriptor
    /// the seed value is taken from <paramref name="initialValues"/> when present, falling back
    /// to the descriptor's <see cref="ParameterDescriptor.DefaultValue"/>. Lets the dialog
    /// rehydrate from a saved token without the user re-editing every field.
    /// </summary>
    public static ManipulatorParameterBag CreateFor(IReadOnlyList<ParameterDescriptor> descriptors, IReadOnlyDictionary<string, object> initialValues) {
      if (descriptors == null)
        throw new ArgumentNullException(nameof(descriptors));
      return new ManipulatorParameterBag(descriptors, initialValues);
    }

    /// <summary>
    /// Returns the current parameter values keyed by descriptor name. The returned view is
    /// read-only — re-render the bag for fresh defaults rather than mutating in place.
    /// </summary>
    public IReadOnlyDictionary<string, object> ToValues()
      => new System.Collections.ObjectModel.ReadOnlyDictionary<string, object>(
        new Dictionary<string, object>(this._values, StringComparer.Ordinal));

    /// <summary>True when at least one parameter has been changed away from its default.</summary>
    public bool HasOverrides {
      get {
        foreach (var d in this._descriptors)
          if (!Equals(this._values[d.Name], d.DefaultValue))
            return true;
        return false;
      }
    }

    #region ICustomTypeDescriptor — every "give me properties" path goes through _properties.

    AttributeCollection ICustomTypeDescriptor.GetAttributes() => AttributeCollection.Empty;
    string ICustomTypeDescriptor.GetClassName() => nameof(ManipulatorParameterBag);
    string ICustomTypeDescriptor.GetComponentName() => null;
    TypeConverter ICustomTypeDescriptor.GetConverter() => TypeDescriptor.GetConverter(typeof(object));
    EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() => null;
    PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() => null;
    object ICustomTypeDescriptor.GetEditor(Type editorBaseType) => null;
    EventDescriptorCollection ICustomTypeDescriptor.GetEvents() => EventDescriptorCollection.Empty;
    EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes) => EventDescriptorCollection.Empty;
    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() => this._properties;
    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes) => this._properties;
    object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd) => this;

    #endregion

    /// <summary>
    /// Per-parameter <see cref="PropertyDescriptor"/> — reads/writes the parent bag's
    /// dictionary, exposes the descriptor's display-name and description, and (for
    /// constrained parameters) returns a <see cref="StandardValuesCollection"/> via a
    /// nested converter so the grid renders a drop-down.
    /// </summary>
    private sealed class ParameterPropertyDescriptor : PropertyDescriptor {
      private readonly ParameterDescriptor _descriptor;
      private readonly TypeConverter _converter;

      public ParameterPropertyDescriptor(ParameterDescriptor descriptor)
        : base(descriptor.Name, BuildAttributes(descriptor)) {
        this._descriptor = descriptor;
        this._converter = descriptor.AllowedValues != null && descriptor.AllowedValues.Count > 0
          ? (TypeConverter)new AllowedValuesConverter(descriptor)
          : descriptor.Type.IsEnum
            ? new EnumConverter(descriptor.Type)
            : TypeDescriptor.GetConverter(descriptor.Type);
      }

      private static Attribute[] BuildAttributes(ParameterDescriptor descriptor) {
        var attributes = new List<Attribute>(3) { new CategoryAttribute("Parameters") };
        if (!string.IsNullOrEmpty(descriptor.DisplayName) && descriptor.DisplayName != descriptor.Name)
          attributes.Add(new DisplayNameAttribute(descriptor.DisplayName));
        if (!string.IsNullOrEmpty(descriptor.Description))
          attributes.Add(new DescriptionAttribute(descriptor.Description));
        return attributes.ToArray();
      }

      public override Type ComponentType => typeof(ManipulatorParameterBag);
      public override Type PropertyType => this._descriptor.Type;
      public override bool IsReadOnly => false;
      public override TypeConverter Converter => this._converter;
      public override bool CanResetValue(object component) => component is ManipulatorParameterBag bag
        && bag._values.TryGetValue(this._descriptor.Name, out var v)
        && !Equals(v, this._descriptor.DefaultValue);

      public override void ResetValue(object component) {
        if (component is ManipulatorParameterBag bag)
          bag._values[this._descriptor.Name] = this._descriptor.DefaultValue;
      }

      public override bool ShouldSerializeValue(object component) => this.CanResetValue(component);

      public override object GetValue(object component) {
        if (component is ManipulatorParameterBag bag && bag._values.TryGetValue(this._descriptor.Name, out var v))
          return v;
        return this._descriptor.DefaultValue;
      }

      public override void SetValue(object component, object value) {
        if (!(component is ManipulatorParameterBag bag))
          return;
        bag._values[this._descriptor.Name] = _Coerce(value, this._descriptor);
      }

      /// <summary>
      /// Coerces user-supplied input into the descriptor's declared type and clamps it
      /// into <c>[Min, Max]</c> when both bounds are set. Falls back to the default value
      /// on any conversion error so a malformed value never poisons the bag.
      /// </summary>
      private static object _Coerce(object raw, ParameterDescriptor descriptor) {
        if (raw == null)
          return descriptor.DefaultValue;
        var target = descriptor.Type;
        object converted;
        try {
          converted = target.IsInstanceOfType(raw)
            ? raw
            : Convert.ChangeType(raw, target, CultureInfo.InvariantCulture);
        } catch {
          return descriptor.DefaultValue;
        }

        // Clamp numeric values into [Min, Max] when bounds are present. Strings/enums fall through.
        if (descriptor.MinValue is IComparable min && converted is IComparable cmpMin && min.GetType() == converted.GetType() && cmpMin.CompareTo(min) < 0)
          converted = min;
        if (descriptor.MaxValue is IComparable max && converted is IComparable cmpMax && max.GetType() == converted.GetType() && cmpMax.CompareTo(max) > 0)
          converted = max;
        return converted;
      }
    }

    /// <summary>
    /// Type converter that forwards every standard-value query to
    /// <see cref="ParameterDescriptor.AllowedValues"/>. Picked when the descriptor
    /// declares a discrete-choice surface — PropertyGrid renders a closed-list drop-down.
    /// </summary>
    private sealed class AllowedValuesConverter : TypeConverter {
      private readonly ParameterDescriptor _descriptor;
      private readonly StandardValuesCollection _values;
      public AllowedValuesConverter(ParameterDescriptor descriptor) {
        this._descriptor = descriptor;
        this._values = new StandardValuesCollection(descriptor.AllowedValues.ToList());
      }
      public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
      public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) => this._values;
      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
      public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
        if (value is string s) {
          // Match the printed form of any allowed value first — keeps int "1" and the like consistent.
          foreach (var allowed in this._descriptor.AllowedValues)
            if (string.Equals(s, allowed?.ToString(), StringComparison.Ordinal))
              return allowed;
          // Fall back to the descriptor's underlying type conversion so typed entries still parse.
          try {
            return Convert.ChangeType(s, this._descriptor.Type, culture);
          } catch {
            return this._descriptor.DefaultValue;
          }
        }
        return base.ConvertFrom(context, culture, value);
      }
    }
  }
}
