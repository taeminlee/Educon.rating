using Google.Apis.Download;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDocsCrawler
{
    public class Doc : INotifyPropertyChanged
    {
        private bool isProcessing = false;
        public bool IsProcessing
        {
            get { return isProcessing; }
            set
            {
                isProcessing = value;
                NotifyPropertyChanged();
            }
        }
        private bool isDownloaded = false;
        public bool IsDownloaded
        {
            get { return isDownloaded; }
            set
            {
                isDownloaded = value;
                NotifyPropertyChanged();
            }
        }
        private bool isParsed = false;
        public bool IsParsed
        {
            get { return isParsed; }
            set
            {
                isParsed = value;
                NotifyPropertyChanged();
            }
        }
        private string fileId;
        public string FileId
        {
            get { return fileId; }
            set
            {
                fileId = value;
                if (fileId != "" && fileId != null)
                {
                    Task.Run(new Action(() =>
                    {
                        Download();
                    }));
                }
                NotifyPropertyChanged();
            }
        }
        private string txt;
        public string Txt
        {
            get { return txt; }
            set
            {
                txt = value;
                NotifyPropertyChanged();
            }
        }
        private string json;
        public string Json
        {
            get { return json; }
            set
            {
                json = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async Task Download()
        {
            var enc = new UTF8Encoding();
            var request = App.service.Files.Export(fileId, "text/plain");
            var stream = new System.IO.MemoryStream();

            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged +=
                (IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            {
                                Console.WriteLine(progress.BytesDownloaded);
                                break;
                            }
                        case DownloadStatus.Completed:
                            {
                                Console.WriteLine("Download complete.");
                                IsDownloaded = true;
                                Txt = enc.GetString(stream.ToArray());
                                break;
                            }
                        case DownloadStatus.Failed:
                            {
                                Console.WriteLine("Download failed.");
                                break;
                            }
                    }
                };
            request.Download(stream);
        }
    }
}
