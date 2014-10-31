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
// This file is only generated to unroll the main image working loop upon compile time
using System;
using System.Drawing;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Imager {
  partial class cImage {
    private cImage _RunLoop(Rectangle? filterRegion, byte scaleX, byte scaleY, Action<PixelWorker<sPixel>> scaler) {
      var startX = filterRegion == null ? 0 : Math.Max(0, filterRegion.Value.Left);
      var startY = filterRegion == null ? 0 : Math.Max(0, filterRegion.Value.Top);

      var endX = filterRegion == null ? this.Width : Math.Min(this.Width, filterRegion.Value.Right);
      var endY = filterRegion == null ? this.Height : Math.Min(this.Height, filterRegion.Value.Bottom);

      var width = endX - startX;

      var result = new cImage(width * scaleX, (endY - startY) * scaleY);
          
      Parallel.ForEach(
        Partitioner.Create(startY, endY),
        () => 0,
        (range, _, threadStorage) => {
          var threadSrcMinY = range.Item1;
          var threadSrcMaxY = range.Item2;
          
          var targetY = (threadSrcMinY - startY) * scaleY;
          for (var sourceY = threadSrcMinY; sourceY < threadSrcMaxY;++sourceY) {
            var worker=new PixelWorker<sPixel>(
              this.GetImageData(), 
              startX,
              sourceY, 
              this._width, 
              this._height, 
              this._horizontalOutOfBoundsHandler,
              this._verticalOutOfBoundsHandler, 
              result.GetImageData(), 
              0, 
              targetY, 
              result._width
            );
            var xRange=width;
            while(xRange>=64){
              xRange-=64;
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
              scaler(worker);
              worker.IncrementX(scaleX);
            }
            for (; xRange>0;--xRange) {
              scaler(worker);
              worker.IncrementX(scaleX);
            }
            
            targetY += scaleY;
          }
          return (threadStorage);
        },
        _ => { }
      );

      return(result);
    }
  }
}