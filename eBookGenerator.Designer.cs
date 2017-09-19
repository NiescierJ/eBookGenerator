namespace eBookGenerator
{
    partial class eBookGenerator
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
            this.cboCDName = new System.Windows.Forms.ComboBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.lstLogger = new System.Windows.Forms.ListBox();
            this.lblDone = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboCDName
            // 
            this.cboCDName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboCDName.FormattingEnabled = true;
            this.cboCDName.Location = new System.Drawing.Point(9, 12);
            this.cboCDName.Name = "cboCDName";
            this.cboCDName.Size = new System.Drawing.Size(318, 21);
            this.cboCDName.TabIndex = 0;
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(349, 10);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // lstLogger
            // 
            this.lstLogger.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstLogger.FormattingEnabled = true;
            this.lstLogger.Location = new System.Drawing.Point(9, 68);
            this.lstLogger.Name = "lstLogger";
            this.lstLogger.Size = new System.Drawing.Size(419, 459);
            this.lstLogger.TabIndex = 2;
            // 
            // lblDone
            // 
            this.lblDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDone.ForeColor = System.Drawing.Color.Red;
            this.lblDone.Location = new System.Drawing.Point(349, 43);
            this.lblDone.Name = "lblDone";
            this.lblDone.Size = new System.Drawing.Size(75, 22);
            this.lblDone.TabIndex = 3;
            this.lblDone.Text = "DONE!";
            this.lblDone.Visible = false;
            // 
            // eBookGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 541);
            this.Controls.Add(this.lblDone);
            this.Controls.Add(this.lstLogger);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.cboCDName);
            this.Name = "eBookGenerator";
            this.Text = "ALS eBook Generator";
            this.Load += new System.EventHandler(this.eBookGenerator_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboCDName;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.ListBox lstLogger;
        private System.Windows.Forms.Label lblDone;
    }
}

