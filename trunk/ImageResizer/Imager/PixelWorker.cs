
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Imager.Interface;

namespace Imager {
  /// <summary>
  /// This class gets us fast access to a small window of pixels in the source and target images.
  /// </summary>
  internal class PixelWorker<TColorStorage> {
    private readonly IList<TColorStorage> _sourceImage;
    private int _sourceX;
    private readonly int _sourceY;
    private int _sourceOffset;
    
    private readonly int _sourceWidth;
    private readonly int _sourceHeight;
    private readonly OutOfBoundsUtils.OutOfBoundsHandler _sourceXWrapper;
    private readonly OutOfBoundsUtils.OutOfBoundsHandler _sourceYWrapper;

    private readonly IList<TColorStorage> _targetImage;
    private int _targetOffset;

    #region offset calculators for source image
    // these are lazy calculated offsets, so once used, the method pointer gets replaced by a function that returns the calculated constant
    private Func<int> _sourceOffsetM2X;
    private int _sourceOffsetM2XValue;
    private Func<int> _sourceOffsetM1X;
    private int _sourceOffsetM1XValue;
    private Func<int> _sourceOffsetP1X;
    private int _sourceOffsetP1XValue;
    private Func<int> _sourceOffsetP2X;
    private int _sourceOffsetP2XValue;
    private Func<int> _sourceOffsetP3X;
    private int _sourceOffsetP3XValue;
    private Func<int> _sourceOffsetM2Y;
    private int _sourceOffsetM2YValue;
    private Func<int> _sourceOffsetM1Y;
    private int _sourceOffsetM1YValue;
    private Func<int> _sourceOffsetP1Y;
    private int _sourceOffsetP1YValue;
    private Func<int> _sourceOffsetP2Y;
    private int _sourceOffsetP2YValue;
    private Func<int> _sourceOffsetP3Y;
    private int _sourceOffsetP3YValue;
    #endregion

    #region offsets for target image
    private const int _targetOffsetP1X = 1;
    private const int _targetOffsetP2X = 2;
    private const int _targetOffsetP3X = 3;
    private const int _targetOffsetP4X = 4;
    private readonly int _targetOffsetM1Y;
    private readonly int _targetOffsetP1Y;
    private readonly int _targetOffsetP2Y;
    private readonly int _targetOffsetP3Y;
    private readonly int _targetOffsetP4Y;
    #endregion

    public PixelWorker(IList<TColorStorage> sourceImage, int sourceX, int sourceY, int sourceWidth, int sourceHeight, OutOfBoundsUtils.OutOfBoundsHandler sourceXWrapper, OutOfBoundsUtils.OutOfBoundsHandler sourceYWrapper, IList<TColorStorage> targetImage, int targetX, int targetY, int targetWidth) {
      Contract.Requires(sourceX >= 0 && sourceX < sourceWidth && sourceY >= 0 && sourceY < sourceHeight);
      this._sourceImage = sourceImage;
      this._sourceX = sourceX;
      this._sourceY = sourceY;
      this._sourceWidth = sourceWidth;
      this._sourceHeight = sourceHeight;

      // we can safely calculate this offset, because we assume that the central source pixel is never out of bounds
      this._sourceOffset = sourceWidth * sourceY + sourceX;

      // we only check pixels in a row or column once for over-/underflow, to avoid calling the wrappers over and over again
      this._sourceXWrapper = sourceXWrapper;
      this._sourceYWrapper = sourceYWrapper;

      // we calculate a delta offset for pixels around the center and store these independent X from Y
      this._sourceOffsetM2X = this._CalculateOffsetM2X;
      this._sourceOffsetM1X = this._CalculateOffsetM1X;
      this._sourceOffsetP1X = this._CalculateOffsetP1X;
      this._sourceOffsetP2X = this._CalculateOffsetP2X;
      this._sourceOffsetP3X = this._CalculateOffsetP3X;
      this._sourceOffsetM2Y = this._CalculateOffsetM2Y;
      this._sourceOffsetM1Y = this._CalculateOffsetM1Y;
      this._sourceOffsetP1Y = this._CalculateOffsetP1Y;
      this._sourceOffsetP2Y = this._CalculateOffsetP2Y;
      this._sourceOffsetP3Y = this._CalculateOffsetP3Y;

      this._targetImage = targetImage;
      this._targetOffset = targetWidth * targetY + targetX;

      // pre-calculating the row offset for target image, because they surely get used
      this._targetOffsetM1Y = targetWidth *  -1;          // for nx0 filters
      this._targetOffsetP1Y = targetWidth     ;          // for nx2 filters
      this._targetOffsetP2Y = targetWidth << 1;          // for nx3 filters
      this._targetOffsetP3Y = targetWidth *  3;          // for nx4 filters
      this._targetOffsetP4Y = targetWidth << 2;          // for nx5 filters
    }

    public int SourceX(){
      return(this._sourceX);
    }

    public int SourceY(){
      return(this._sourceY);
    }

    public int SourceHeight(){
      return(this._sourceHeight);
    }

    public void IncrementX(int targetXIncrementor){
      this._targetOffset+=targetXIncrementor;
      this._sourceOffset++;
      this._sourceX++;
      this._sourceOffsetM2X = this._CalculateOffsetM2X;
      this._sourceOffsetM1X = this._GetOffsetM1X;
      this._sourceOffsetM1XValue = -1;
      this._sourceOffsetP1X = this._CalculateOffsetP1X;
      this._sourceOffsetP2X = this._CalculateOffsetP2X;
      this._sourceOffsetP3X = this._CalculateOffsetP3X;
    }

    #region access source points
    public TColorStorage SourceM2M2() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM2X() + this._sourceOffsetM2Y()]);
    }
    public TColorStorage SourceM1M2() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM1X() + this._sourceOffsetM2Y()]);
    }
    public TColorStorage SourceP0M2() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM2Y()]);
    }
    public TColorStorage SourceP1M2() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP1X() + this._sourceOffsetM2Y()]);
    }
    public TColorStorage SourceP2M2() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP2X() + this._sourceOffsetM2Y()]);
    }
    public TColorStorage SourceP3M2() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP3X() + this._sourceOffsetM2Y()]);
    }
    public TColorStorage SourceM2M1() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM2X() + this._sourceOffsetM1Y()]);
    }
    public TColorStorage SourceM1M1() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM1X() + this._sourceOffsetM1Y()]);
    }
    public TColorStorage SourceP0M1() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM1Y()]);
    }
    public TColorStorage SourceP1M1() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP1X() + this._sourceOffsetM1Y()]);
    }
    public TColorStorage SourceP2M1() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP2X() + this._sourceOffsetM1Y()]);
    }
    public TColorStorage SourceP3M1() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP3X() + this._sourceOffsetM1Y()]);
    }
    public TColorStorage SourceM2P0() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM2X()]);
    }
    public TColorStorage SourceM1P0() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM1X()]);
    }
    public TColorStorage SourceP0P0() {
      return (this._sourceImage[this._sourceOffset]);
    }
    public TColorStorage SourceP1P0() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP1X()]);
    }
    public TColorStorage SourceP2P0() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP2X()]);
    }
    public TColorStorage SourceP3P0() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP3X()]);
    }
    public TColorStorage SourceM2P1() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM2X() + this._sourceOffsetP1Y()]);
    }
    public TColorStorage SourceM1P1() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM1X() + this._sourceOffsetP1Y()]);
    }
    public TColorStorage SourceP0P1() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP1Y()]);
    }
    public TColorStorage SourceP1P1() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP1X() + this._sourceOffsetP1Y()]);
    }
    public TColorStorage SourceP2P1() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP2X() + this._sourceOffsetP1Y()]);
    }
    public TColorStorage SourceP3P1() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP3X() + this._sourceOffsetP1Y()]);
    }
    public TColorStorage SourceM2P2() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM2X() + this._sourceOffsetP2Y()]);
    }
    public TColorStorage SourceM1P2() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM1X() + this._sourceOffsetP2Y()]);
    }
    public TColorStorage SourceP0P2() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP2Y()]);
    }
    public TColorStorage SourceP1P2() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP1X() + this._sourceOffsetP2Y()]);
    }
    public TColorStorage SourceP2P2() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP2X() + this._sourceOffsetP2Y()]);
    }
    public TColorStorage SourceP3P2() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP3X() + this._sourceOffsetP2Y()]);
    }
    public TColorStorage SourceM2P3() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM2X() + this._sourceOffsetP3Y()]);
    }
    public TColorStorage SourceM1P3() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetM1X() + this._sourceOffsetP3Y()]);
    }
    public TColorStorage SourceP0P3() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP3Y()]);
    }
    public TColorStorage SourceP1P3() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP1X() + this._sourceOffsetP3Y()]);
    }
    public TColorStorage SourceP2P3() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP2X() + this._sourceOffsetP3Y()]);
    }
    public TColorStorage SourceP3P3() {
      return (this._sourceImage[this._sourceOffset + this._sourceOffsetP3X() + this._sourceOffsetP3Y()]);
    }
    #endregion

    #region access target points
    public void TargetP0M1(TColorStorage value) {
      this._targetImage[this._targetOffset + this._targetOffsetM1Y] = value;
    }
    public void TargetP1M1(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP1X + this._targetOffsetM1Y] = value;
    }
    public void TargetP2M1(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP2X + this._targetOffsetM1Y] = value;
    }
    public void TargetP3M1(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP3X + this._targetOffsetM1Y] = value;
    }
    public void TargetP4M1(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP4X + this._targetOffsetM1Y] = value;
    }
    public void TargetP0P0(TColorStorage value) {
      this._targetImage[this._targetOffset] = value;
    }
    public void TargetP1P0(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP1X] = value;
    }
    public void TargetP2P0(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP2X] = value;
    }
    public void TargetP3P0(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP3X] = value;
    }
    public void TargetP4P0(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP4X] = value;
    }
    public void TargetP0P1(TColorStorage value) {
      this._targetImage[this._targetOffset + this._targetOffsetP1Y] = value;
    }
    public void TargetP1P1(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP1X + this._targetOffsetP1Y] = value;
    }
    public void TargetP2P1(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP2X + this._targetOffsetP1Y] = value;
    }
    public void TargetP3P1(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP3X + this._targetOffsetP1Y] = value;
    }
    public void TargetP4P1(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP4X + this._targetOffsetP1Y] = value;
    }
    public void TargetP0P2(TColorStorage value) {
      this._targetImage[this._targetOffset + this._targetOffsetP2Y] = value;
    }
    public void TargetP1P2(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP1X + this._targetOffsetP2Y] = value;
    }
    public void TargetP2P2(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP2X + this._targetOffsetP2Y] = value;
    }
    public void TargetP3P2(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP3X + this._targetOffsetP2Y] = value;
    }
    public void TargetP4P2(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP4X + this._targetOffsetP2Y] = value;
    }
    public void TargetP0P3(TColorStorage value) {
      this._targetImage[this._targetOffset + this._targetOffsetP3Y] = value;
    }
    public void TargetP1P3(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP1X + this._targetOffsetP3Y] = value;
    }
    public void TargetP2P3(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP2X + this._targetOffsetP3Y] = value;
    }
    public void TargetP3P3(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP3X + this._targetOffsetP3Y] = value;
    }
    public void TargetP4P3(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP4X + this._targetOffsetP3Y] = value;
    }
    public void TargetP0P4(TColorStorage value) {
      this._targetImage[this._targetOffset + this._targetOffsetP4Y] = value;
    }
    public void TargetP1P4(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP1X + this._targetOffsetP4Y] = value;
    }
    public void TargetP2P4(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP2X + this._targetOffsetP4Y] = value;
    }
    public void TargetP3P4(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP3X + this._targetOffsetP4Y] = value;
    }
    public void TargetP4P4(TColorStorage value) {
      this._targetImage[this._targetOffset + _targetOffsetP4X + this._targetOffsetP4Y] = value;
    }
    #endregion

    #region calculate source offset deltas

    private int _GetOffsetM2X() { return (this._sourceOffsetM2XValue); }
    private int _CalculateOffsetM2X() {
      var current = this._sourceX;
      var value = current - 2;
      if (value < 0)
        value = this._sourceXWrapper(value, this._sourceWidth, true);

      var result = value - current;
      this._sourceOffsetM2XValue = result;
      this._sourceOffsetM2X = this._GetOffsetM2X;
      return (result);
    }
    private int _GetOffsetM1X() { return (this._sourceOffsetM1XValue); }
    private int _CalculateOffsetM1X() {
      var current = this._sourceX;
      var value = current - 1;
      if (value < 0)
        value = this._sourceXWrapper(value, this._sourceWidth, true);

      var result = value - current;
      this._sourceOffsetM1XValue = result;
      this._sourceOffsetM1X = this._GetOffsetM1X;
      return (result);
    }
    private int _GetOffsetP1X() { return (this._sourceOffsetP1XValue); }
    private int _CalculateOffsetP1X() {
      var current = this._sourceX;
      var value = current + 1;
      if (value >= this._sourceWidth)
        value = this._sourceXWrapper(value, this._sourceWidth, false);

      var result = value - current;
      this._sourceOffsetP1XValue = result;
      this._sourceOffsetP1X = this._GetOffsetP1X;
      return (result);
    }
    private int _GetOffsetP2X() { return (this._sourceOffsetP2XValue); }
    private int _CalculateOffsetP2X() {
      var current = this._sourceX;
      var value = current + 2;
      if (value >= this._sourceWidth)
        value = this._sourceXWrapper(value, this._sourceWidth, false);

      var result = value - current;
      this._sourceOffsetP2XValue = result;
      this._sourceOffsetP2X = this._GetOffsetP2X;
      return (result);
    }
    private int _GetOffsetP3X() { return (this._sourceOffsetP3XValue); }
    private int _CalculateOffsetP3X() {
      var current = this._sourceX;
      var value = current + 3;
      if (value >= this._sourceWidth)
        value = this._sourceXWrapper(value, this._sourceWidth, false);

      var result = value - current;
      this._sourceOffsetP3XValue = result;
      this._sourceOffsetP3X = this._GetOffsetP3X;
      return (result);
    }
    private int _GetOffsetM2Y() { return (this._sourceOffsetM2YValue); }
    private int _CalculateOffsetM2Y() {
      var current = this._sourceY;
      var value = current - 2;
      if (value < 0)
        value = this._sourceYWrapper(value, this._sourceHeight, true);

      var result = (value - current) * this._sourceWidth;
      this._sourceOffsetM2YValue = result;
      this._sourceOffsetM2Y = this._GetOffsetM2Y;
      return (result);
    }
    private int _GetOffsetM1Y() { return (this._sourceOffsetM1YValue); }
    private int _CalculateOffsetM1Y() {
      var current = this._sourceY;
      var value = current - 1;
      if (value < 0)
        value = this._sourceYWrapper(value, this._sourceHeight, true);

      var result = (value - current) * this._sourceWidth;
      this._sourceOffsetM1YValue = result;
      this._sourceOffsetM1Y = this._GetOffsetM1Y;
      return (result);
    }
    private int _GetOffsetP1Y() { return (this._sourceOffsetP1YValue); }
    private int _CalculateOffsetP1Y() {
      var current = this._sourceY;
      var value = current + 1;
      if (value >= this._sourceHeight)
        value = this._sourceYWrapper(value, this._sourceHeight, false);

      var result = (value - current) * this._sourceWidth;
      this._sourceOffsetP1YValue = result;
      this._sourceOffsetP1Y = this._GetOffsetP1Y;
      return (result);
    }
    private int _GetOffsetP2Y() { return (this._sourceOffsetP2YValue); }
    private int _CalculateOffsetP2Y() {
      var current = this._sourceY;
      var value = current + 2;
      if (value >= this._sourceHeight)
        value = this._sourceYWrapper(value, this._sourceHeight, false);

      var result = (value - current) * this._sourceWidth;
      this._sourceOffsetP2YValue = result;
      this._sourceOffsetP2Y = this._GetOffsetP2Y;
      return (result);
    }
    private int _GetOffsetP3Y() { return (this._sourceOffsetP3YValue); }
    private int _CalculateOffsetP3Y() {
      var current = this._sourceY;
      var value = current + 3;
      if (value >= this._sourceHeight)
        value = this._sourceYWrapper(value, this._sourceHeight, false);

      var result = (value - current) * this._sourceWidth;
      this._sourceOffsetP3YValue = result;
      this._sourceOffsetP3Y = this._GetOffsetP3Y;
      return (result);
    }
    #endregion

  }
}