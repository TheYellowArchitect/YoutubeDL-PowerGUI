using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    
    public class GridDownloadItem : INotifyPropertyChanged
    {
        private string title;
        public string Title
        {
            get { return this.title; }
            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    this.NotifyPropertyChanged("Title");
                }
            }
        }

        private string uniformResourceLocator;
        public string URL
        {
            get { return this.uniformResourceLocator; }
            set
            {
                if (this.uniformResourceLocator != value)
                {
                    this.uniformResourceLocator = value;
                    this.NotifyPropertyChanged("URL");
                }
            }
        }

        private string progress;
        public string Progress { 
            get { return this.progress; } 
            set
            {
                if (this.progress != value)
                {
                    this.progress = value;
                    this.NotifyPropertyChanged("Progress");
                }
            } 
        }

        private string size;
        public string Size
        {
            get { return this.size; }
            set
            {
                if (this.size != value)
                {
                    this.size = value;
                    this.NotifyPropertyChanged("Size");
                }
            }
        }

        private string timeRemaining;
        public string TimeRemaining
        {
            get { return this.timeRemaining; }
            set
            {
                if (this.timeRemaining != value)
                {
                    this.timeRemaining = value;
                    this.NotifyPropertyChanged("TimeRemaining");
                }
            }
        }

        private string downloadSpeed;
        public string DownloadSpeed
        {
            get { return this.downloadSpeed; }
            set
            {
                if (this.downloadSpeed != value)
                {
                    this.downloadSpeed = value;
                    this.NotifyPropertyChanged("DownloadSpeed");
                }
            }
        }

        //public Date VideoUploadDate



        public event PropertyChangedEventHandler PropertyChanged;

        public const string defaultTitle = "UnknownTitle";
        public const string defaultURL = "Unknown.com";
        public const string defaultProgress = "Unknown";
        public const string defaultSize = "Unknown";
        public const string defaultTimeRemaining = "Unknown";
        public const string defaultDownloadSpeed = "Unknown";

        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public GridDownloadItem (string initialURL = defaultURL, string initialTitle = defaultTitle, string initialProgress = defaultProgress, string initialSize = defaultSize, string initialTimeRemaining = defaultTimeRemaining)
        {
            Title = initialTitle;
            URL = initialURL;
            Progress = initialProgress;
            Size = initialSize;
            TimeRemaining = initialTimeRemaining;
            DownloadSpeed = defaultDownloadSpeed;
        }

        /// <summary>
        /// It has no more default values, it is ready to be visualized!
        /// This is how we check if it really is true, if it really has no more default values!
        /// Note download speed is not included but doesn't matter!
        /// </summary>
        /// <returns></returns>
        public bool IsFilled()
        {
            if (URL != defaultURL && Title != defaultTitle && Progress != defaultProgress && Size != defaultSize && TimeRemaining != defaultTimeRemaining)
                return true;
            else
                return false;
        }

        //Make all its strings ""
        public void FinishClear()
        {
            Progress = "";
            TimeRemaining = "";
            DownloadSpeed = "";
        }
    }
}
