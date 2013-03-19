namespace Classes {
  internal interface IImageManipulator {
    bool SupportsWidth { get; }
    bool SupportsHeight { get; }
    bool SupportsRepetitionCount { get; }
    bool SupportsGridCentering { get; }
    bool SupportsThresholds { get; }
    bool ChangesResolution { get; }
  }
}
