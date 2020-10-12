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
  internal enum CLIExitCode {
    RestartingInGuiMode = -1,
    OK = 0,
    UnknownParameter=1,
    TooLessArguments=2,
    JpegNotSupportedOnThisPlatform=3,
    NothingToSave=4,
    FilenameMustNotBeNull=5,
    InvalidTargetDimensions=6,
    CouldNotParseDimensionsAsWord=7,
    NothingToResize=8,
    UnknownFilter=9,
    ExceptionDuringImageLoad=10,
    ExceptionDuringImageWrite=11,
    TargetFileCouldNotBeSaved=17,
    InvalidFilterDescription=12,
    CouldNotParseParameterAsFloat=13,
    CouldNotParseParameterAsByte=14,
    InvalidOutOfBoundsMode=15,

    RuntimeError=16,
  }
}
