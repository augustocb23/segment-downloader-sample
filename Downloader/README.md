# Downloader

Usage example for [SegmentDownloader Core](https://www.nuget.org/packages/SegmentDownloader.Core), a updated version from the tool created by @guilhermelabigalini, maintained by @golavr.

## Persisted List Extension

The extension was already implemented on the [original project](https://github.com/guilhermelabigalini/mydownloader), but it was messed with the UI. I just copied the necessary files and made small changes to remove unused code.

When open the application, instantiate the PersistedList and save it on a private property. Then, just look for paused downloads and start it...

```csharp
_persistedList = new PersistedListExtension();
if (DownloadManager.Instance.Downloads.Any())
    foreach (var downloader in DownloadManager.Instance.Downloads)
        downloader.Start();
```

Call the `dispose()` method to save all downloads to a XML file. This method can also be called to pause all downloads. The file is updated using a `TimerCallback` that saves the list every 120 seconds (you can change this time with the `SaveListIntervalInSeconds` constant.

## File Validator

When the download is finished, the file is validated using a SHA256 Hashing Algorithm. You can obtaing the hash using the `Get-FileHash` function from Windows PowerShell. [See the docs](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/get-filehash) for details.

Call the static method `Validate(path, hash)` to check the file. Using the `DownloadEnded` event...

```csharp
String hash = "...";
if (FileValidator.Validate(e.Downloader.LocalFile, hash))
    //file is valid
else
    //file is invalid
```