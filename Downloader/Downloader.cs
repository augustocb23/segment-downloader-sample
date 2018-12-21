using SegmentDownloader.Core;
using SegmentDownloader.Extension.PersistedList;
using SegmentDownloader.Protocol;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Downloader
{
	public partial class Downloader : Form
	{
		private const string Url = "ftp://speedtest.tele2.net/50MB.zip";
		private const string LocalFolder = @"c:\temp";
		private const string Hash = "8565A714DCA840F8652C5BAE9249AB05F5FB5A4F9F13FBE23304B10F68252DA2";
		private StatusDownload Downloads = StatusDownload.aguardando;
		private readonly PersistedListExtension _persistedList;

		private static ManualResetEventSlim _manualResetEventSlim;


		public Downloader()
		{
			InitializeComponent();
			RegisterProtocols();
			_persistedList = new PersistedListExtension();
			_manualResetEventSlim = new ManualResetEventSlim();

			// registra o evento para Downloads concluídos
			DownloadManager.Instance.DownloadEnded += DownloadEnded;

			//se há downloads não finalizados
			if (DownloadManager.Instance.Downloads.Any())
				ContinuarDownloads();
		}

		private void BtnIniciar(object sender, EventArgs e)
		{
			switch (Downloads)
			{
				case StatusDownload.baixando:
					PausarDownload();
					break;
				case StatusDownload.aguardando:
					IniciarDownload();
					break;
				case StatusDownload.emPausa:
					ContinuarDownloads();
					break;
				case StatusDownload.concluido:
					Application.Exit();
					break;
			}
		}

		private void ContinuarDownloads()
		{
			var downloader = DownloadManager.Instance.Downloads.FirstOrDefault();
			downloader.Start();
			status.Text = "Baixando...";
			iniciar.Text = "Pausar";
			Downloads = StatusDownload.baixando;
			Thread t = new Thread(AtualizarProgresso);
			t.Start();
		}

		private void PausarDownload()
		{
			status.Text = "Salvando...";
			_persistedList.Dispose();
			status.Text = "Em pausa";
			iniciar.Text = "Continuar";
			Downloads = StatusDownload.emPausa;
		}

		private void IniciarDownload()
		{
			var resourceLocation = SegmentDownloader.Core.ResourceLocation.FromURL(Url);

			// local file path
			var uri = new Uri(Url);
			var fileName = uri.Segments.Last();
			var localFilePath = Path.Combine(LocalFolder, fileName);

			// create downloader with 8 segments
			var downloader = DownloadManager.Instance.Add(resourceLocation, null, localFilePath, 8, false);

			// start download
			downloader.Start();
			status.Text = "Baixando...";
			iniciar.Text = "Pausar";
			Downloads = StatusDownload.baixando;

			Thread t = new Thread(AtualizarProgresso);
			t.Start();
		}

		private void DownloadEnded(object sender, DownloaderEventArgs e)
		{
			Console.WriteLine(e.Downloader.State == DownloaderState.EndedWithError
				? $"Download Ended With Error '{e.Downloader.LastError.Message}'"
				: "Download Ended");

			SetControlPropertyValue(status, "Text", "Validando arquivo...");
			if (FileValidator.Validate(e.Downloader.LocalFile, Hash))
				SetControlPropertyValue(status, "Text", "Download concluído");
			else
				SetControlPropertyValue(status, "Text", "Arquivo inválido");

			SetControlPropertyValue(iniciar, "Text", "Fechar");
			SetControlPropertyValue(progress, "Value", 100);
			Downloads = StatusDownload.concluido;

			_persistedList.Dispose();
			_manualResetEventSlim.Set();
		}
		private static void RegisterProtocols()
		{
			// register protocols
			ProtocolProviderFactory.RegisterProtocolHandler("http", typeof(HttpProtocolProvider));
			ProtocolProviderFactory.RegisterProtocolHandler("https", typeof(HttpProtocolProvider));
			ProtocolProviderFactory.RegisterProtocolHandler("ftp", typeof(FtpProtocolProvider));
		}

		private void AtualizarProgresso()
		{
			var downloader = DownloadManager.Instance.Downloads.FirstOrDefault();
			while (downloader.IsWorking())
			{
				SetControlPropertyValue(progress, "Value", (int)downloader.Progress);
				SetControlPropertyValue(status, "Text", "Baixando... " + Convert.ToInt32(downloader.Rate / 1024) + " KB/s");
				Thread.Sleep(500);
			}
		}

		delegate void SetControlValueCallback(Control oControl, string propName, object propValue);

		private void SetControlPropertyValue(Control oControl, string propName, object propValue)
		{

			if (oControl.InvokeRequired)
			{
				SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
				oControl.Invoke(d, new object[] { oControl, propName, propValue });
			}
			else
			{
				Type t = oControl.GetType();
				PropertyInfo[] props = t.GetProperties();
				foreach (PropertyInfo p in props)
				{
					if (p.Name.ToUpper() == propName.ToUpper())
					{
						p.SetValue(oControl, propValue, null);
					}
				}
			}
		}

		private void FecharPrograma(object sender, FormClosedEventArgs e)
		{
			if (Downloads == StatusDownload.baixando)
				SetControlPropertyValue(status, "Text", "Salvando...");
			_persistedList.Dispose();
		}
	}

	enum StatusDownload
	{
		aguardando,
		baixando,
		emPausa,
		concluido
	}

}
