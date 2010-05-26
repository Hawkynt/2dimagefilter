using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using nImager;

namespace ImageResizer {
  /// <summary>
  /// Interaktionslogik für "App.xaml"
  /// </summary>
  public partial class App : Application {
    public App() {
      // TODO: tests here
      sPixel.AllowThresholds = true;
    }
  }
}
