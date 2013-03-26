using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Classes {
  /// <summary>
  /// This class stores the different configuration options of the application and handles config save/load
  /// </summary>
  internal static class Config {
    #region consts
    private const string _ROOT_NODE_NAME = "Configuration";
    private const string _VALUE_ATTRIBUTE_NAME = "value";
    private const string _LAST_SAVE_DIRECTORY_NODE_NAME = "LastSaveDirectory";
    private const string _LAST_LOAD_DIRECTORY_NODE_NAME = "LastLoadDirectory";
    private const string _SOURCE_SIZE_MODE_NODE_NAME = "SourceSizeMode";
    private const string _TARGET_SIZE_MODE_NODE_NAME = "TargetSizeMode";
    #endregion

    #region props
    public static string LastSaveDirectory { get; set; }
    public static string LastLoadDirectory { get; set; }
    public static PictureBoxSizeMode? SourceSizeMode { get; set; }
    public static PictureBoxSizeMode? TargetSizeMode { get; set; }
    #endregion

    /// <summary>
    /// Loads the configuration from the specified file.
    /// </summary>
    /// <param name="configurationFile">The configuration file.</param>
    public static void Load(string configurationFile) {
      if (string.IsNullOrWhiteSpace(configurationFile) || !File.Exists(configurationFile))
        return;

      var root = XElement.Load(configurationFile);

      // if root node is different, skip loading
      if (!string.Equals(root.Name.LocalName, _ROOT_NODE_NAME, StringComparison.CurrentCultureIgnoreCase))
        return;

      PictureBoxSizeMode temp;
      foreach (XElement node in root.Nodes()) {

        // skip nodes without value attribute
        var valueAttribute = node.Attribute(_VALUE_ATTRIBUTE_NAME);
        if (valueAttribute == null)
          continue;

        // skip nodes with empty value attribute
        var value = valueAttribute.Value;
        if (string.IsNullOrWhiteSpace(value))
          continue;

        var nodeName = node.Name.LocalName;

        // set value depending on node and validity
        if (string.Equals(nodeName, _LAST_LOAD_DIRECTORY_NODE_NAME, StringComparison.CurrentCultureIgnoreCase))
          LastLoadDirectory = value;
        else if (string.Equals(nodeName, _LAST_SAVE_DIRECTORY_NODE_NAME, StringComparison.CurrentCultureIgnoreCase))
          LastSaveDirectory = value;
        else if (string.Equals(nodeName, _SOURCE_SIZE_MODE_NODE_NAME, StringComparison.CurrentCultureIgnoreCase) && Enum.TryParse(value, true, out temp))
          SourceSizeMode = temp;
        else if (string.Equals(nodeName, _TARGET_SIZE_MODE_NODE_NAME, StringComparison.CurrentCultureIgnoreCase) && Enum.TryParse(value, true, out temp))
          TargetSizeMode = temp;
      }

    }

    /// <summary>
    /// Saves the configuration to the specified file.
    /// </summary>
    /// <param name="configurationFile">The configuration file.</param>
    public static void Save(string configurationFile) {

      var root = new XElement(_ROOT_NODE_NAME);
      root.Add(new XElement(_LAST_SAVE_DIRECTORY_NODE_NAME, new XAttribute(_VALUE_ATTRIBUTE_NAME, LastSaveDirectory ?? string.Empty)));
      root.Add(new XElement(_LAST_LOAD_DIRECTORY_NODE_NAME, new XAttribute(_VALUE_ATTRIBUTE_NAME, LastLoadDirectory ?? string.Empty)));
      root.Add(new XElement(_SOURCE_SIZE_MODE_NODE_NAME, new XAttribute(_VALUE_ATTRIBUTE_NAME, SourceSizeMode == null ? string.Empty : SourceSizeMode.Value.ToString())));
      root.Add(new XElement(_TARGET_SIZE_MODE_NODE_NAME, new XAttribute(_VALUE_ATTRIBUTE_NAME, TargetSizeMode == null ? string.Empty : TargetSizeMode.Value.ToString())));
      root.Save(configurationFile);

    }
  }
}
