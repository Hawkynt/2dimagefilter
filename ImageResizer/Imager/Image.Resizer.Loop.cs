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
    private cImage _RunLoop(Rectangle? filterRegion, byte scaleX, byte scaleY, Action<cImage, int, int, cImage, int, int> scaler) {
      var startX = filterRegion == null ? 0 : Math.Max(0, filterRegion.Value.Left);
      var startY = filterRegion == null ? 0 : Math.Max(0, filterRegion.Value.Top);

      var endX = filterRegion == null ? this.Width : Math.Min(this.Width, filterRegion.Value.Right);
      var endY = filterRegion == null ? this.Height : Math.Min(this.Height, filterRegion.Value.Bottom);

      var result = new cImage((endX - startX) * scaleX, (endY - startY) * scaleY);
      var threadTgtMaxX = (endX - startX) * scaleX;
          
      Parallel.ForEach(
        Partitioner.Create(startY, endY),
        () => 0,
        (range, _, threadStorage) => {
          var threadSrcMinY = range.Item1;
          var threadSrcMaxY = range.Item2;
          var targetY = (threadSrcMaxY - startY) * scaleY;
          for (var sourceY = threadSrcMaxY; sourceY > threadSrcMinY;) {
            --sourceY;
            targetY -= scaleY;
            var targetX = threadTgtMaxX;
            var sourceX=endX;
            while((sourceX-startX)>=64){
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
            }
            for (; sourceX > startX;) {
              --sourceX;
              targetX -= scaleX;
              scaler(this,sourceX,sourceY,result,targetX,targetY);
            }
          }
          return (threadStorage);
        },
        _ => { }
      );

      return(result);
    }
  }
}