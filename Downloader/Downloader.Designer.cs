namespace Downloader
{
	partial class Downloader
	{
		private System.ComponentModel.IContainer components = null;

				protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código gerado pelo Windows Form Designer
		private void InitializeComponent()
		{
			this.start = new System.Windows.Forms.Button();
			this.progress = new System.Windows.Forms.ProgressBar();
			this.status = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// start
			// 
			this.start.Location = new System.Drawing.Point(432, 141);
			this.start.Name = "start";
			this.start.Size = new System.Drawing.Size(75, 23);
			this.start.TabIndex = 0;
			this.start.Text = "Start";
			this.start.UseVisualStyleBackColor = true;
			this.start.Click += new System.EventHandler(this.BtnStart);
			// 
			// progress
			// 
			this.progress.Location = new System.Drawing.Point(12, 170);
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(495, 24);
			this.progress.Step = 1;
			this.progress.TabIndex = 1;
			// 
			// status
			// 
			this.status.Location = new System.Drawing.Point(12, 141);
			this.status.Name = "status";
			this.status.ReadOnly = true;
			this.status.Size = new System.Drawing.Size(414, 20);
			this.status.TabIndex = 2;
			this.status.Text = "Waiting to begin...";
			// 
			// Downloader
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(515, 208);
			this.Controls.Add(this.status);
			this.Controls.Add(this.progress);
			this.Controls.Add(this.start);
			this.Name = "Downloader";
			this.Text = "Downloader";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CloseApp);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button start;
		private System.Windows.Forms.ProgressBar progress;
		private System.Windows.Forms.TextBox status;
	}
}

