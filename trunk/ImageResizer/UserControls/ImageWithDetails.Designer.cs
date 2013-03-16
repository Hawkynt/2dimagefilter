namespace ImageResizer.UserControls {
  partial class ImageWithDetails {
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.Windows.Forms.TableLayoutPanel tlpLayout;
      this.pbImage = new System.Windows.Forms.PictureBox();
      this.lDetails = new System.Windows.Forms.Label();
      tlpLayout = new System.Windows.Forms.TableLayoutPanel();
      tlpLayout.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
      this.SuspendLayout();
      // 
      // tlpLayout
      // 
      tlpLayout.ColumnCount = 1;
      tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      tlpLayout.Controls.Add(this.pbImage, 0, 0);
      tlpLayout.Controls.Add(this.lDetails, 0, 1);
      tlpLayout.Dock = System.Windows.Forms.DockStyle.Fill;
      tlpLayout.Location = new System.Drawing.Point(0, 0);
      tlpLayout.Name = "tlpLayout";
      tlpLayout.RowCount = 2;
      tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
      tlpLayout.Size = new System.Drawing.Size(144, 142);
      tlpLayout.TabIndex = 0;
      // 
      // pbImage
      // 
      this.pbImage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pbImage.Location = new System.Drawing.Point(3, 3);
      this.pbImage.Name = "pbImage";
      this.pbImage.Size = new System.Drawing.Size(138, 113);
      this.pbImage.TabIndex = 0;
      this.pbImage.TabStop = false;
      this.pbImage.Click += new System.EventHandler(this._EventWrapper);
      // 
      // lDetails
      // 
      this.lDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.lDetails.Location = new System.Drawing.Point(3, 119);
      this.lDetails.Name = "lDetails";
      this.lDetails.Size = new System.Drawing.Size(138, 23);
      this.lDetails.TabIndex = 1;
      this.lDetails.Text = "Details";
      this.lDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.lDetails.Click += new System.EventHandler(this._EventWrapper);
      // 
      // ImageWithDetails
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(tlpLayout);
      this.Name = "ImageWithDetails";
      this.Size = new System.Drawing.Size(144, 142);
      tlpLayout.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox pbImage;
    private System.Windows.Forms.Label lDetails;
  }
}
