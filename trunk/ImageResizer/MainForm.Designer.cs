namespace ImageResizer {
  partial class MainForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.Windows.Forms.StatusStrip ssBottom;
      System.Windows.Forms.GroupBox gbSourceImage;
      System.Windows.Forms.GroupBox gbTargetImage;
      System.Windows.Forms.FlowLayoutPanel flpActions;
      System.Windows.Forms.GroupBox gbAdvanced;
      System.Windows.Forms.GroupBox gbBorderPixelHandling;
      System.Windows.Forms.Label label7;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      System.Windows.Forms.Label label6;
      System.Windows.Forms.Label label5;
      System.Windows.Forms.Label label4;
      System.Windows.Forms.GroupBox gbTargetResolution;
      System.Windows.Forms.Label label9;
      System.Windows.Forms.Label label8;
      System.Windows.Forms.Label label3;
      System.Windows.Forms.Label label2;
      System.Windows.Forms.Label label1;
      System.Windows.Forms.GroupBox gbMethod;
      this.tssBusy = new System.Windows.Forms.ToolStripStatusLabel();
      this.iwhSourceImage = new ImageResizer.UserControls.ImageWithDetails();
      this.iwhTargetImage = new ImageResizer.UserControls.ImageWithDetails();
      this.btResize = new System.Windows.Forms.Button();
      this.btSwitch = new System.Windows.Forms.Button();
      this.btRepeat = new System.Windows.Forms.Button();
      this.cUseThresholds = new System.Windows.Forms.CheckBox();
      this.cbVerticalBPH = new System.Windows.Forms.ComboBox();
      this.cbHorizontalBPH = new System.Windows.Forms.ComboBox();
      this.nudWidth = new System.Windows.Forms.NumericUpDown();
      this.nudHeight = new System.Windows.Forms.NumericUpDown();
      this.cbResizeMethod = new System.Windows.Forms.ComboBox();
      this.msMain = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tlpMainLayout = new System.Windows.Forms.TableLayoutPanel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.gbActions = new System.Windows.Forms.GroupBox();
      this.ofdOpenFile = new System.Windows.Forms.OpenFileDialog();
      this.sfdSave = new System.Windows.Forms.SaveFileDialog();
      ssBottom = new System.Windows.Forms.StatusStrip();
      gbSourceImage = new System.Windows.Forms.GroupBox();
      gbTargetImage = new System.Windows.Forms.GroupBox();
      flpActions = new System.Windows.Forms.FlowLayoutPanel();
      gbAdvanced = new System.Windows.Forms.GroupBox();
      gbBorderPixelHandling = new System.Windows.Forms.GroupBox();
      label7 = new System.Windows.Forms.Label();
      label6 = new System.Windows.Forms.Label();
      label5 = new System.Windows.Forms.Label();
      label4 = new System.Windows.Forms.Label();
      gbTargetResolution = new System.Windows.Forms.GroupBox();
      label9 = new System.Windows.Forms.Label();
      label8 = new System.Windows.Forms.Label();
      label3 = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      label1 = new System.Windows.Forms.Label();
      gbMethod = new System.Windows.Forms.GroupBox();
      ssBottom.SuspendLayout();
      gbSourceImage.SuspendLayout();
      gbTargetImage.SuspendLayout();
      flpActions.SuspendLayout();
      gbAdvanced.SuspendLayout();
      gbBorderPixelHandling.SuspendLayout();
      gbTargetResolution.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
      gbMethod.SuspendLayout();
      this.msMain.SuspendLayout();
      this.tlpMainLayout.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gbActions.SuspendLayout();
      this.SuspendLayout();
      // 
      // ssBottom
      // 
      ssBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssBusy});
      ssBottom.Location = new System.Drawing.Point(0, 565);
      ssBottom.Name = "ssBottom";
      ssBottom.Size = new System.Drawing.Size(889, 22);
      ssBottom.TabIndex = 1;
      ssBottom.Text = "statusStrip1";
      // 
      // tssBusy
      // 
      this.tssBusy.Image = global::ImageResizer.Properties.Resources.ProgressCircularBlue;
      this.tssBusy.Name = "tssBusy";
      this.tssBusy.Size = new System.Drawing.Size(75, 17);
      this.tssBusy.Text = "Resizing...";
      this.tssBusy.Visible = false;
      // 
      // gbSourceImage
      // 
      gbSourceImage.Controls.Add(this.iwhSourceImage);
      gbSourceImage.Dock = System.Windows.Forms.DockStyle.Fill;
      gbSourceImage.Location = new System.Drawing.Point(3, 3);
      gbSourceImage.Name = "gbSourceImage";
      gbSourceImage.Size = new System.Drawing.Size(288, 535);
      gbSourceImage.TabIndex = 0;
      gbSourceImage.TabStop = false;
      gbSourceImage.Text = "Source Image";
      // 
      // iwhSourceImage
      // 
      this.iwhSourceImage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.iwhSourceImage.Location = new System.Drawing.Point(3, 16);
      this.iwhSourceImage.Name = "iwhSourceImage";
      this.iwhSourceImage.Size = new System.Drawing.Size(282, 516);
      this.iwhSourceImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.iwhSourceImage.TabIndex = 0;
      this.iwhSourceImage.Click += new System.EventHandler(this.iwhSourceImage_Click);
      // 
      // gbTargetImage
      // 
      gbTargetImage.Controls.Add(this.iwhTargetImage);
      gbTargetImage.Dock = System.Windows.Forms.DockStyle.Fill;
      gbTargetImage.Location = new System.Drawing.Point(597, 3);
      gbTargetImage.Name = "gbTargetImage";
      gbTargetImage.Size = new System.Drawing.Size(289, 535);
      gbTargetImage.TabIndex = 1;
      gbTargetImage.TabStop = false;
      gbTargetImage.Text = "Target Image";
      // 
      // iwhTargetImage
      // 
      this.iwhTargetImage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.iwhTargetImage.Location = new System.Drawing.Point(3, 16);
      this.iwhTargetImage.Name = "iwhTargetImage";
      this.iwhTargetImage.Size = new System.Drawing.Size(283, 516);
      this.iwhTargetImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.iwhTargetImage.TabIndex = 0;
      this.iwhTargetImage.Click += new System.EventHandler(this.iwhTargetImage_Click);
      // 
      // flpActions
      // 
      flpActions.AutoSize = true;
      flpActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      flpActions.Controls.Add(this.btResize);
      flpActions.Controls.Add(this.btSwitch);
      flpActions.Controls.Add(this.btRepeat);
      flpActions.Dock = System.Windows.Forms.DockStyle.Top;
      flpActions.Location = new System.Drawing.Point(3, 16);
      flpActions.Name = "flpActions";
      flpActions.Size = new System.Drawing.Size(288, 29);
      flpActions.TabIndex = 0;
      // 
      // btResize
      // 
      this.btResize.Image = global::ImageResizer.Properties.Resources.Resize;
      this.btResize.Location = new System.Drawing.Point(3, 3);
      this.btResize.Name = "btResize";
      this.btResize.Size = new System.Drawing.Size(75, 23);
      this.btResize.TabIndex = 0;
      this.btResize.Text = "Resize";
      this.btResize.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btResize.UseVisualStyleBackColor = true;
      this.btResize.Click += new System.EventHandler(this.btResize_Click);
      // 
      // btSwitch
      // 
      this.btSwitch.Image = global::ImageResizer.Properties.Resources.Switch;
      this.btSwitch.Location = new System.Drawing.Point(84, 3);
      this.btSwitch.Name = "btSwitch";
      this.btSwitch.Size = new System.Drawing.Size(75, 23);
      this.btSwitch.TabIndex = 0;
      this.btSwitch.Text = "Switch";
      this.btSwitch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btSwitch.UseVisualStyleBackColor = true;
      this.btSwitch.Click += new System.EventHandler(this.btSwitch_Click);
      // 
      // btRepeat
      // 
      this.btRepeat.Image = global::ImageResizer.Properties.Resources.Repeat;
      this.btRepeat.Location = new System.Drawing.Point(165, 3);
      this.btRepeat.Name = "btRepeat";
      this.btRepeat.Size = new System.Drawing.Size(75, 23);
      this.btRepeat.TabIndex = 0;
      this.btRepeat.Text = "Repeat";
      this.btRepeat.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btRepeat.UseVisualStyleBackColor = true;
      this.btRepeat.Click += new System.EventHandler(this.btRepeat_Click);
      // 
      // gbAdvanced
      // 
      gbAdvanced.AutoSize = true;
      gbAdvanced.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      gbAdvanced.Controls.Add(this.cUseThresholds);
      gbAdvanced.Dock = System.Windows.Forms.DockStyle.Top;
      gbAdvanced.Location = new System.Drawing.Point(0, 215);
      gbAdvanced.Name = "gbAdvanced";
      gbAdvanced.Size = new System.Drawing.Size(294, 55);
      gbAdvanced.TabIndex = 3;
      gbAdvanced.TabStop = false;
      gbAdvanced.Text = "Advanced";
      // 
      // cUseThresholds
      // 
      this.cUseThresholds.AutoSize = true;
      this.cUseThresholds.Location = new System.Drawing.Point(6, 19);
      this.cUseThresholds.Name = "cUseThresholds";
      this.cUseThresholds.Size = new System.Drawing.Size(100, 17);
      this.cUseThresholds.TabIndex = 0;
      this.cUseThresholds.Text = "Use Thresholds";
      this.cUseThresholds.UseVisualStyleBackColor = true;
      // 
      // gbBorderPixelHandling
      // 
      gbBorderPixelHandling.AutoSize = true;
      gbBorderPixelHandling.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      gbBorderPixelHandling.Controls.Add(label7);
      gbBorderPixelHandling.Controls.Add(label6);
      gbBorderPixelHandling.Controls.Add(this.cbVerticalBPH);
      gbBorderPixelHandling.Controls.Add(this.cbHorizontalBPH);
      gbBorderPixelHandling.Controls.Add(label5);
      gbBorderPixelHandling.Controls.Add(label4);
      gbBorderPixelHandling.Dock = System.Windows.Forms.DockStyle.Top;
      gbBorderPixelHandling.Location = new System.Drawing.Point(0, 129);
      gbBorderPixelHandling.Name = "gbBorderPixelHandling";
      gbBorderPixelHandling.Size = new System.Drawing.Size(294, 86);
      gbBorderPixelHandling.TabIndex = 3;
      gbBorderPixelHandling.TabStop = false;
      gbBorderPixelHandling.Text = "Border pixel handling";
      // 
      // label7
      // 
      label7.Image = ((System.Drawing.Image)(resources.GetObject("label7.Image")));
      label7.Location = new System.Drawing.Point(259, 51);
      label7.Name = "label7";
      label7.Size = new System.Drawing.Size(16, 16);
      label7.TabIndex = 2;
      // 
      // label6
      // 
      label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
      label6.Location = new System.Drawing.Point(259, 22);
      label6.Name = "label6";
      label6.Size = new System.Drawing.Size(16, 16);
      label6.TabIndex = 2;
      // 
      // cbVerticalBPH
      // 
      this.cbVerticalBPH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbVerticalBPH.FormattingEnabled = true;
      this.cbVerticalBPH.Location = new System.Drawing.Point(90, 46);
      this.cbVerticalBPH.Name = "cbVerticalBPH";
      this.cbVerticalBPH.Size = new System.Drawing.Size(161, 21);
      this.cbVerticalBPH.TabIndex = 1;
      // 
      // cbHorizontalBPH
      // 
      this.cbHorizontalBPH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbHorizontalBPH.FormattingEnabled = true;
      this.cbHorizontalBPH.Location = new System.Drawing.Point(90, 19);
      this.cbHorizontalBPH.Name = "cbHorizontalBPH";
      this.cbHorizontalBPH.Size = new System.Drawing.Size(161, 21);
      this.cbHorizontalBPH.TabIndex = 1;
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new System.Drawing.Point(6, 49);
      label5.Name = "label5";
      label5.Size = new System.Drawing.Size(49, 13);
      label5.TabIndex = 0;
      label5.Text = "Vertically";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point(6, 22);
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size(61, 13);
      label4.TabIndex = 0;
      label4.Text = "Horizontally";
      // 
      // gbTargetResolution
      // 
      gbTargetResolution.AutoSize = true;
      gbTargetResolution.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      gbTargetResolution.Controls.Add(label9);
      gbTargetResolution.Controls.Add(label8);
      gbTargetResolution.Controls.Add(this.nudWidth);
      gbTargetResolution.Controls.Add(this.nudHeight);
      gbTargetResolution.Controls.Add(label3);
      gbTargetResolution.Controls.Add(label2);
      gbTargetResolution.Controls.Add(label1);
      gbTargetResolution.Dock = System.Windows.Forms.DockStyle.Top;
      gbTargetResolution.Location = new System.Drawing.Point(0, 45);
      gbTargetResolution.Name = "gbTargetResolution";
      gbTargetResolution.Size = new System.Drawing.Size(294, 84);
      gbTargetResolution.TabIndex = 3;
      gbTargetResolution.TabStop = false;
      gbTargetResolution.Text = "Target Resolution";
      // 
      // label9
      // 
      label9.Image = ((System.Drawing.Image)(resources.GetObject("label9.Image")));
      label9.Location = new System.Drawing.Point(132, 21);
      label9.Name = "label9";
      label9.Size = new System.Drawing.Size(16, 16);
      label9.TabIndex = 2;
      // 
      // label8
      // 
      label8.Image = ((System.Drawing.Image)(resources.GetObject("label8.Image")));
      label8.Location = new System.Drawing.Point(132, 47);
      label8.Name = "label8";
      label8.Size = new System.Drawing.Size(16, 16);
      label8.TabIndex = 2;
      // 
      // nudWidth
      // 
      this.nudWidth.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
      this.nudWidth.Location = new System.Drawing.Point(57, 19);
      this.nudWidth.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
      this.nudWidth.Name = "nudWidth";
      this.nudWidth.Size = new System.Drawing.Size(68, 20);
      this.nudWidth.TabIndex = 1;
      // 
      // nudHeight
      // 
      this.nudHeight.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
      this.nudHeight.Location = new System.Drawing.Point(57, 45);
      this.nudHeight.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
      this.nudHeight.Name = "nudHeight";
      this.nudHeight.Size = new System.Drawing.Size(68, 20);
      this.nudHeight.TabIndex = 1;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point(154, 35);
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size(121, 13);
      label3.TabIndex = 0;
      label3.Text = "0 means Auto-determine";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(6, 47);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(38, 13);
      label2.TabIndex = 0;
      label2.Text = "Height";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point(6, 21);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(35, 13);
      label1.TabIndex = 0;
      label1.Text = "Width";
      // 
      // gbMethod
      // 
      gbMethod.AutoSize = true;
      gbMethod.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      gbMethod.Controls.Add(this.cbResizeMethod);
      gbMethod.Dock = System.Windows.Forms.DockStyle.Top;
      gbMethod.Location = new System.Drawing.Point(0, 0);
      gbMethod.Name = "gbMethod";
      gbMethod.Size = new System.Drawing.Size(294, 45);
      gbMethod.TabIndex = 3;
      gbMethod.TabStop = false;
      gbMethod.Text = "Method";
      // 
      // cbResizeMethod
      // 
      this.cbResizeMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbResizeMethod.FormattingEnabled = true;
      this.cbResizeMethod.Location = new System.Drawing.Point(6, 19);
      this.cbResizeMethod.Name = "cbResizeMethod";
      this.cbResizeMethod.Size = new System.Drawing.Size(282, 21);
      this.cbResizeMethod.TabIndex = 0;
      // 
      // msMain
      // 
      this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.msMain.Location = new System.Drawing.Point(0, 0);
      this.msMain.Name = "msMain";
      this.msMain.Size = new System.Drawing.Size(889, 24);
      this.msMain.TabIndex = 0;
      this.msMain.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "File";
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
      this.openToolStripMenuItem.Text = "Open";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
      this.saveToolStripMenuItem.Text = "Save";
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
      // 
      // saveAsToolStripMenuItem
      // 
      this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
      this.saveAsToolStripMenuItem.Text = "Save As";
      this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
      this.closeToolStripMenuItem.Text = "Close";
      this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(111, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // tlpMainLayout
      // 
      this.tlpMainLayout.ColumnCount = 3;
      this.tlpMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tlpMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tlpMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tlpMainLayout.Controls.Add(gbSourceImage, 0, 0);
      this.tlpMainLayout.Controls.Add(gbTargetImage, 2, 0);
      this.tlpMainLayout.Controls.Add(this.panel1, 1, 0);
      this.tlpMainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tlpMainLayout.Location = new System.Drawing.Point(0, 24);
      this.tlpMainLayout.Name = "tlpMainLayout";
      this.tlpMainLayout.RowCount = 1;
      this.tlpMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tlpMainLayout.Size = new System.Drawing.Size(889, 541);
      this.tlpMainLayout.TabIndex = 2;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.gbActions);
      this.panel1.Controls.Add(gbAdvanced);
      this.panel1.Controls.Add(gbBorderPixelHandling);
      this.panel1.Controls.Add(gbTargetResolution);
      this.panel1.Controls.Add(gbMethod);
      this.panel1.Location = new System.Drawing.Point(297, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(294, 376);
      this.panel1.TabIndex = 2;
      // 
      // gbActions
      // 
      this.gbActions.AutoSize = true;
      this.gbActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.gbActions.Controls.Add(flpActions);
      this.gbActions.Dock = System.Windows.Forms.DockStyle.Top;
      this.gbActions.Location = new System.Drawing.Point(0, 270);
      this.gbActions.Name = "gbActions";
      this.gbActions.Size = new System.Drawing.Size(294, 48);
      this.gbActions.TabIndex = 4;
      this.gbActions.TabStop = false;
      this.gbActions.Text = "Actions";
      // 
      // ofdOpenFile
      // 
      this.ofdOpenFile.FileName = "openFileDialog1";
      this.ofdOpenFile.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
      this.ofdOpenFile.RestoreDirectory = true;
      this.ofdOpenFile.Title = "Select Image to resize";
      // 
      // sfdSave
      // 
      this.sfdSave.DefaultExt = "png";
      this.sfdSave.Filter = "Portable Network Graphics|*.png|Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
      this.sfdSave.RestoreDirectory = true;
      this.sfdSave.Title = "Enter filename";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(889, 587);
      this.Controls.Add(this.tlpMainLayout);
      this.Controls.Add(ssBottom);
      this.Controls.Add(this.msMain);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.msMain;
      this.Name = "MainForm";
      this.Text = "ImageResizer";
      ssBottom.ResumeLayout(false);
      ssBottom.PerformLayout();
      gbSourceImage.ResumeLayout(false);
      gbTargetImage.ResumeLayout(false);
      flpActions.ResumeLayout(false);
      gbAdvanced.ResumeLayout(false);
      gbAdvanced.PerformLayout();
      gbBorderPixelHandling.ResumeLayout(false);
      gbBorderPixelHandling.PerformLayout();
      gbTargetResolution.ResumeLayout(false);
      gbTargetResolution.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
      gbMethod.ResumeLayout(false);
      this.msMain.ResumeLayout(false);
      this.msMain.PerformLayout();
      this.tlpMainLayout.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.gbActions.ResumeLayout(false);
      this.gbActions.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.ComboBox cbResizeMethod;
    private System.Windows.Forms.NumericUpDown nudWidth;
    private System.Windows.Forms.NumericUpDown nudHeight;
    private System.Windows.Forms.ComboBox cbVerticalBPH;
    private System.Windows.Forms.ComboBox cbHorizontalBPH;
    private System.Windows.Forms.Button btResize;
    private System.Windows.Forms.Button btSwitch;
    private System.Windows.Forms.Button btRepeat;
    private System.Windows.Forms.GroupBox gbActions;
    private System.Windows.Forms.CheckBox cUseThresholds;
    private UserControls.ImageWithDetails iwhTargetImage;
    private UserControls.ImageWithDetails iwhSourceImage;
    private System.Windows.Forms.ToolStripStatusLabel tssBusy;
    private System.Windows.Forms.MenuStrip msMain;
    private System.Windows.Forms.TableLayoutPanel tlpMainLayout;
    private System.Windows.Forms.OpenFileDialog ofdOpenFile;
    private System.Windows.Forms.SaveFileDialog sfdSave;

  }
}