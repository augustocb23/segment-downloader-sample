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
		private StatusDownload Downloads = StatusDownload.waiting;
		private readonly PersistedListExtension _persistedList;

		private static ManualResetEventSlim _manualResetEventSlim;


		public Downloader()
		{
			InitializeComponent();
			RegisterProtocols();
			_persistedList = new PersistedListExtension();
			_manualResetEventSlim = new ManualResetEventSlim();

			// register the listener
			DownloadManager.Instance.DownloadEnded += DownloadEnded;

			// if there is unfinished downloads
			if (DownloadManager.Instance.Downloads.Any())
				ResumeDownloads();
		}

		private void BtnStart(object sender, EventArgs e)
		{
			switch (Downloads)
			{
				case StatusDownload.downloading:
					PauseDownloads();
					break;
				case StatusDownload.waiting:
					StartDownloads();
					break;
				case StatusDownload.paused:
					ResumeDownloads();
					break;
				case StatusDownload.finished:
					Application.Exit();
					break;
			}
		}

		private void ResumeDownloads()
		{
			var downloader = DownloadManager.Instance.Downloads.FirstOrDefault();
			downloader.Start();
			status.Text = "Downloading...";
			start.Text = "Pause";
			Downloads = StatusDownload.downloading;
			Thread t = new Thread(UpdateProgress);
			t.Start();
		}

		private void PauseDownloads()
		{
			status.Text = "Saving...";
			//pause all and force update
			_persistedList.Dispose();
			status.Text = "Paused";
			start.Text = "Resume";
			Downloads = StatusDownload.paused;
		}

		private void StartDownloads()
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
			status.Text = "Downloading...";
			start.Text = "Pause";
			Downloads = StatusDownload.downloading;

			Thread t = new Thread(UpdateProgress);
			t.Start();
		}

		private void DownloadEnded(object sender, DownloaderEventArgs e)
		{
			Console.WriteLine(e.Downloader.State == DownloaderState.EndedWithError
				? $"Download Ended With Error '{e.Downloader.LastError.Message}'"
				: "Download Ended");

			// validate file
			SetControlPropertyValue(status, "Text", "Validating file...");
			if (FileValidator.Validate(e.Downloader.LocalFile, Hash))
				SetControlPropertyValue(status, "Text", "Download finished");
			else
				SetControlPropertyValue(status, "Text", "Invalid file");

			// update window
			SetControlPropertyValue(start, "Text", "Close");
			SetControlPropertyValue(progress, "Value", 100);
			Downloads = StatusDownload.finished;

			// force to update the persisted list
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

		private void UpdateProgress()
		{
			var downloader = DownloadManager.Instance.Downloads.FirstOrDefault();
			while (downloader.IsWorking())
			{
				SetControlPropertyValue(progress, "Value", (int)downloader.Progress);
				SetControlPropertyValue(status, "Text", "Downloading... " + Convert.ToInt32(downloader.Rate / 1024) + " KB/s");
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

		private void CloseApp(object sender, FormClosedEventArgs e)
		{
			if (Downloads == StatusDownload.downloading)
			{
				SetControlPropertyValue(status, "Text", "Saving...");
				PauseDownloads();
			}
			Application.Exit();
		}
	}

	enum StatusDownload
	{
		waiting,
		downloading,
		paused,
		finished
	}

}
