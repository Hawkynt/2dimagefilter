#region (c)2008-2013 Hawkynt
/*
 *  cImage 
 *  Image filtering library 
    Copyright (C) 2010-2013 Hawkynt

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

namespace Classes {
  internal class ScriptSerializerException : Exception {
    private readonly string _filename;
    public string Filename { get { return (this._filename); } }

    private readonly int _lineNumber;
    public int LineNumber { get { return (this._lineNumber); } }

    private readonly CLIExitCode _errorType;
    public CLIExitCode ErrorType { get { return (this._errorType); } }

    public ScriptSerializerException(string filename, int lineNumber, CLIExitCode errorType) {
      this._filename = filename;
      this._lineNumber = lineNumber;
      this._errorType = errorType;
    }
  }
}
