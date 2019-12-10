namespace gdirender
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._renderPanel = new gdirender.DoubleBufferedPanel();
            this.SuspendLayout();
            //
            // renderPanel
            //
            this._renderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._renderPanel.Location = new System.Drawing.Point(0, 0);
            this._renderPanel.Name = "renderPanel";
            this._renderPanel.Size = new System.Drawing.Size(1024, 512);
            this._renderPanel.TabIndex = 0;
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 512);
            this.Controls.Add(this._renderPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "elbpsx";
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel _renderPanel;
    }
}

