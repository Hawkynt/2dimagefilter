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
      this.lDetails = new System.Windows.Forms.Label();
      this.pnImage = new System.Windows.Forms.Panel();
      this.pbImage = new System.Windows.Forms.PictureBox();
      tlpLayout = new System.Windows.Forms.TableLayoutPanel();
      tlpLayout.SuspendLayout();
      this.pnImage.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
      this.SuspendLayout();
      // 
      // tlpLayout
      // 
      tlpLayout.ColumnCount = 1;
      tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      tlpLayout.Controls.Add(this.lDetails, 0, 1);
      tlpLayout.Controls.Add(this.pnImage, 0, 0);
      tlpLayout.Dock = System.Windows.Forms.DockStyle.Fill;
      tlpLayout.Location = new System.Drawing.Point(0, 0);
      tlpLayout.Name = "tlpLayout";
      tlpLayout.RowCount = 2;
      tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
      tlpLayout.Size = new System.Drawing.Size(452, 513);
      tlpLayout.TabIndex = 0;
      // 
      // lDetails
      // 
      this.lDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.lDetails.Location = new System.Drawing.Point(3, 490);
      this.lDetails.Name = "lDetails";
      this.lDetails.Size = new System.Drawing.Size(446, 23);
      this.lDetails.TabIndex = 1;
      this.lDetails.Text = "Details";
      this.lDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.lDetails.Click += new System.EventHandler(this._EventWrapper);
      // 
      // pnImage
      // 
      this.pnImage.AutoScroll = true;
      this.pnImage.Controls.Add(this.pbImage);
      this.pnImage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnImage.Location = new System.Drawing.Point(0, 0);
      this.pnImage.Margin = new System.Windows.Forms.Padding(0);
      this.pnImage.Name = "pnImage";
      this.pnImage.Size = new System.Drawing.Size(452, 490);
      this.pnImage.TabIndex = 2;
      this.pnImage.SizeChanged += new System.EventHandler(this.pnImage_SizeChanged);
      // 
      // pbImage
      // 
      this.pbImage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pbImage.Location = new System.Drawing.Point(0, 0);
      this.pbImage.Name = "pbImage";
      this.pbImage.Size = new System.Drawing.Size(452, 490);
      this.pbImage.TabIndex = 1;
      this.pbImage.TabStop = false;
      this.pbImage.Click += new System.EventHandler(this._EventWrapper);
      // 
      // ImageWithDetails
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(tlpLayout);
      this.Name = "ImageWithDetails";
      this.Size = new System.Drawing.Size(452, 513);
      tlpLayout.ResumeLayout(false);
      this.pnImage.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lDetails;
    private System.Windows.Forms.Panel pnImage;
    private System.Windows.Forms.PictureBox pbImage;
  }
}
