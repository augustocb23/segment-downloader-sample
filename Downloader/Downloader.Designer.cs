namespace Downloader
{
	partial class Downloader
	{
		/// <summary>
		/// Variável de designer necessária.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Limpar os recursos que estão sendo usados.
		/// </summary>
		/// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código gerado pelo Windows Form Designer

		/// <summary>
		/// Método necessário para suporte ao Designer - não modifique 
		/// o conteúdo deste método com o editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.iniciar = new System.Windows.Forms.Button();
			this.progress = new System.Windows.Forms.ProgressBar();
			this.status = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// iniciar
			// 
			this.iniciar.Location = new System.Drawing.Point(432, 141);
			this.iniciar.Name = "iniciar";
			this.iniciar.Size = new System.Drawing.Size(75, 23);
			this.iniciar.TabIndex = 0;
			this.iniciar.Text = "Iniciar";
			this.iniciar.UseVisualStyleBackColor = true;
			this.iniciar.Click += new System.EventHandler(this.BtnIniciar);
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
			this.status.Text = "Aguardando início";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(515, 208);
			this.Controls.Add(this.status);
			this.Controls.Add(this.progress);
			this.Controls.Add(this.iniciar);
			this.Name = "Form1";
			this.Text = "Downloader";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FecharPrograma);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button iniciar;
		private System.Windows.Forms.ProgressBar progress;
		private System.Windows.Forms.TextBox status;
	}
}

