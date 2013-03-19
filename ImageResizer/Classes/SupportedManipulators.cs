using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;

using Classes.ImageManipulators;
using Classes.ImageManipulators.Scalers;

using Imager;
using Imager.Classes;
using Imager.Interface;

namespace Classes {
  internal static class SupportedManipulators {
    public static readonly KeyValuePair<string, IImageManipulator>[] MANIPULATORS = new KeyValuePair<string, IImageManipulator>[0]

    #region add interpolators
.Concat(
    from p in cImage.INTERPOLATORS
    select new KeyValuePair<string, IImageManipulator>(_GetDescriptionForEnumValue(p) + " <GDI+>", new Interpolator(p))
    )
    #endregion

    #region add resampler
.Concat(
    from p in _GetEnumValues<KernelType>()
    select new KeyValuePair<string, IImageManipulator>(_GetDescriptionForEnumValue(p), new Resampler(p))
    )
    #endregion

    #region add pixel resizer
.Concat(
      from p in _GetEnumValues<PixelScalerType>()
      select new KeyValuePair<string, IImageManipulator>(_GetDescriptionForEnumValue(p), new PixelScaler(p))
    )
    #endregion

    #region add xbr resizer
.Concat(
      from p in _GetEnumValues<XbrScalerType>()
      select new KeyValuePair<string, IImageManipulator>(_GetDescriptionForEnumValue(p) + " <NoBlend>", new XbrScaler(p, false))
)
.Concat(
      from p in _GetEnumValues<XbrScalerType>()
      select new KeyValuePair<string, IImageManipulator>(_GetDescriptionForEnumValue(p), new XbrScaler(p, true))
)
    #endregion

    #region add nq resizer
.Concat(
      from p in _GetEnumValues<NqScalerType>()
      from m in _GetEnumValues<NqMode>()
      select new KeyValuePair<string, IImageManipulator>(_GetDescriptionForEnumValue(p) + (m == NqMode.Normal ? string.Empty : " " + _GetDescriptionForEnumValue(m)), new NqScaler(p, m))
    )
    #endregion

    #region plane extractors
.Concat(
    new[] {
      new KeyValuePair<string, IImageManipulator>("Red",new PlaneExtractor(c=>c.Red)),
      new KeyValuePair<string, IImageManipulator>("Green",new PlaneExtractor(c=>c.Green)),
      new KeyValuePair<string, IImageManipulator>("Blue",new PlaneExtractor(c=>c.Blue)),
      new KeyValuePair<string, IImageManipulator>("Alpha",new PlaneExtractor(c=>c.Alpha)),
      new KeyValuePair<string, IImageManipulator>("Luminance",new PlaneExtractor(c=>c.Luminance)),
      new KeyValuePair<string, IImageManipulator>("ChrominanceU",new PlaneExtractor(c=>c.ChrominanceU)),
      new KeyValuePair<string, IImageManipulator>("ChrominanceV",new PlaneExtractor(c=>c.ChrominanceV)),
      new KeyValuePair<string, IImageManipulator>("u",new PlaneExtractor(c=>c.u)),
      new KeyValuePair<string, IImageManipulator>("v",new PlaneExtractor(c=>c.v)),
      new KeyValuePair<string, IImageManipulator>("Hue",new PlaneExtractor(c=>c.Hue)),
      new KeyValuePair<string, IImageManipulator>("Hue Colored",new PlaneExtractor(c=>c.HueColored)),
      new KeyValuePair<string, IImageManipulator>("Brightness",new PlaneExtractor(c=>c.Brightness)),
      new KeyValuePair<string, IImageManipulator>("Min",new PlaneExtractor(c=>c.Min)),
      new KeyValuePair<string, IImageManipulator>("Max",new PlaneExtractor(c=>c.Max)),
      new KeyValuePair<string, IImageManipulator>("ExtractColors",new PlaneExtractor(c=>c.ExtractColors)),
      new KeyValuePair<string, IImageManipulator>("ExtractDeltas",new PlaneExtractor(c=>c.ExtractDeltas)),
    }
    )
    #endregion

.ToArray();

    #region enum utils
    /// <summary>
    /// Gets a typed enumeration of all values of a given enumeration.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <returns>All values.</returns>
    private static IEnumerable<T> _GetEnumValues<T>() where T : struct {
      Contract.Requires(typeof(T).IsEnum, "Only enums supported");
      return (from T i in Enum.GetValues(typeof(T))
              select i);
    }

    /// <summary>
    /// Gets a descriptive name for a given enum value by trying to find a description attribute first or using the name for the given value.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>A descriptive text.</returns>
    private static string _GetDescriptionForEnumValue<T>(T value) where T : struct {
      Contract.Requires(typeof(T).IsEnum, "Only enum supported");
      var valueName = Enum.GetName(typeof(T), value);
      var fieldInfo = typeof(T).GetMember(valueName)[0];
      var descriptionAttribute = (DescriptionAttribute)fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
      return (descriptionAttribute == null ? valueName : descriptionAttribute.Description);
    }
    #endregion

  }
}
