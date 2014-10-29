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
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

using Classes;

using ImageResizer.Properties;

namespace ImageResizer.Windows {
  public partial class AboutWindow : Form {
    public AboutWindow() {
      InitializeComponent();

      var link = new LinkLabel.Link { LinkData = Resources.urlProject };
      this.llaHomepage.Links.Add(link);

      this.lblInfo.Text = this.lblInfo.Text
        .Replace("{appname}", ReflectionUtils.GetEntryAssemblyAttribute<AssemblyProductAttribute>(p => p.Product).ToString())
        .Replace("{version}", ReflectionUtils.GetEntryAssemblyAttribute<AssemblyFileVersionAttribute>(v => v.Version).ToString())
        .Replace("{copyright}", ReflectionUtils.GetEntryAssemblyAttribute<AssemblyCopyrightAttribute>(c => c.Copyright).ToString())
      ;
    }

    private void butOK_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void llaHomepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      Process.Start(e.Link.LinkData as string);
    }
  }
}
