namespace GenMap.UI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMap = new System.Windows.Forms.Panel();
            this.boxMap = new System.Windows.Forms.PictureBox();
            this.panelMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boxMap)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMap
            // 
            this.panelMap.Controls.Add(this.boxMap);
            this.panelMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMap.Location = new System.Drawing.Point(0, 0);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(621, 419);
            this.panelMap.TabIndex = 1;
            // 
            // boxMap
            // 
            this.boxMap.BackColor = System.Drawing.Color.White;
            this.boxMap.Location = new System.Drawing.Point(201, 38);
            this.boxMap.Name = "boxMap";
            this.boxMap.Size = new System.Drawing.Size(100, 50);
            this.boxMap.TabIndex = 0;
            this.boxMap.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 419);
            this.Controls.Add(this.panelMap);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panelMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.boxMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.PictureBox boxMap;
    }
}

