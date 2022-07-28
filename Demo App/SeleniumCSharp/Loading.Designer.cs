
namespace SeleniumCSharp
{
    partial class Loading
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Loading));
            this.axOPOSRFID1 = new AxOPOSRFIDLib.AxOPOSRFID();
            ((System.ComponentModel.ISupportInitialize)(this.axOPOSRFID1)).BeginInit();
            this.SuspendLayout();
            // 
            // axOPOSRFID1
            // 
            this.axOPOSRFID1.Enabled = true;
            this.axOPOSRFID1.Location = new System.Drawing.Point(382, 74);
            this.axOPOSRFID1.Name = "axOPOSRFID1";
            this.axOPOSRFID1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axOPOSRFID1.OcxState")));
            this.axOPOSRFID1.Size = new System.Drawing.Size(28, 28);
            this.axOPOSRFID1.TabIndex = 0;
            // 
            // Loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 120);
            this.Controls.Add(this.axOPOSRFID1);
            this.Name = "Loading";
            this.Text = "Loading";
            ((System.ComponentModel.ISupportInitialize)(this.axOPOSRFID1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxOPOSRFIDLib.AxOPOSRFID axOPOSRFID1;
    }
}