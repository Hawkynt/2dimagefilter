#region (c)2010-2020 Hawkynt
/*
  This file is part of Hawkynt's .NET Framework extensions.

    Hawkynt's .NET Framework extensions are free software: 
    you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Hawkynt's .NET Framework extensions is distributed in the hope that 
    it will be useful, but WITHOUT ANY WARRANTY; without even the implied 
    warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See
    the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Hawkynt's .NET Framework extensions.  
    If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System.Diagnostics.Contracts;

namespace System.Windows.Forms {
  internal static partial class ControlExtensions {

    /// <summary>
    /// Safely invokes the given code on the control's thread.
    /// </summary>
    /// <param name="This">This Control.</param>
    /// <param name="task">The task to perform in its thread.</param>
    /// <param name="async">if set to <c>true</c> [async].</param>
    /// <returns>
    ///   <c>true</c> when no thread switch was needed; otherwise, <c>false</c>.
    /// </returns>
    public static bool SafelyInvoke(this Control This, Action task, bool @async = true) {
      Contract.Requires(This != null);
      Contract.Requires(task != null);
      var result = This.InvokeRequired;
      if (result) {
        if (@async)
          This.BeginInvoke(task);
        else
          This.Invoke(task);
      } else
        task();
      return (!result);
    }

    /// <summary>
    /// Executes a task in a thread that is definitely not the GUI thread.
    /// </summary>
    /// <param name="This">This Control.</param>
    /// <param name="task">The task.</param>
    /// <returns><c>true</c> when a thread switch was needed; otherwise, <c>false</c>.</returns>
    public static bool Async(this Control This, Action task) {
      Contract.Requires(This != null);
      Contract.Requires(task != null);
      if (This.InvokeRequired) {
        task();
        return (false);
      }
      task.BeginInvoke(task.EndInvoke, null);
      return (true);
    }

    /// <summary>
    /// Safelies executes an action and returns the result..
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="This">The this.</param>
    /// <param name="function">The function.</param>
    /// <returns>Whatever the method returned.</returns>
    public static TResult SafelyInvoke<TResult>(this Control This, Func<TResult> function) {
      Contract.Requires(This != null);
      Contract.Requires(function != null);
      if (This.InvokeRequired)
        return ((TResult)This.Invoke(function));
      return (function());
    }

  }
}