namespace Imager.Interface {
  /// <summary>
  /// This tells us how to handle read requests to pixels which are out of image bounds.
  /// </summary>
  public enum OutOfBoundsMode {
    /// <summary>
    /// aaa abcde eee
    /// </summary>
    ConstantExtension = 0,
    /// <summary>
    /// cba abcde edc
    /// </summary>
    HalfSampleSymmetric,
    /// <summary>
    /// dcb abcde dcb
    /// </summary>
    WholeSampleSymmetric,
    /// <summary>
    /// cde abcde abc
    /// </summary>
    WrapAround
  }
}
