using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyWatermark.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Commands
        public DelegateCommand OpenFileSelection { get; private set; }
        public DelegateCommand QuitApplication { get; private set; }
        public DelegateCommand ApplyWatermark { get; private set; }
        public DelegateCommand OpenSettingsWindow { get; private set; }
        public DelegateCommand OpenLogoWin { get; private set; }
        public DelegateCommand SelectOutputFolder { get; private set; }
        #endregion

        public string LogoName {get;set;}
        public string OutputFolder { get; set; }
                
        public ObservableCollection<PhotoItem> SourceImages { get; set; }

        private WMarker _wMarker;

        public MainViewModel() 
        {
            OpenFileSelection = new DelegateCommand(c => this.DoOpenFileSelection(), null);
            QuitApplication = new DelegateCommand(c => this.DoQuitApplication(), null);
            ApplyWatermark = new DelegateCommand(c => this.DoApplyWatermark(), null);
            OpenSettingsWindow = new DelegateCommand(c => this.DoOpenSettingsWindow(), null);
            OpenLogoWin = new DelegateCommand(c => this.DoOpenLogoWin(), null);
            SelectOutputFolder = new DelegateCommand(c => this.DoSelectOutputFolder(), null);

            var defaultLogo = Properties.Settings.Default.LogoPath;
            var defaultOutPath = Properties.Settings.Default.OutputFolder;

            if (defaultLogo != null && File.Exists(defaultLogo))
                LogoName = defaultLogo;
            else
                LogoName = "";

            if (defaultOutPath != null && File.Exists(defaultOutPath))
                OutputFolder = Properties.Settings.Default.OutputFolder;
            else
                OutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (LogoName != null && LogoName.Length > 0)
                _wMarker = new WMarker(LogoName);
            else
                _wMarker = new WMarker();

            SourceImages = new ObservableCollection<PhotoItem>();
            
            OnPropertyChanged("LogoName");
            OnPropertyChanged("OutputFolder");
        }

        private void DoSelectOutputFolder()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                OutputFolder = dialog.SelectedPath;
                OnPropertyChanged("OutputFolder");
            }

        }

        private void DoQuitApplication()
        {
            Environment.Exit(0);
        }

        private void DoOpenFileSelection()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Images (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == true)
            {
                foreach (var f in ofd.FileNames)
                {
                    PhotoItem item = new PhotoItem(f);
                    if(item!=null)
                        SourceImages.Add(item);
                }

                OnPropertyChanged("SourceImages");
                //string[] safeFilePath = ofd.SafeFileNames; //Stores only the file name, wo the path.
            }
        }

        private void DoOpenSettingsWindow()
        { 
            //Open settings window here...
            //Get logo path 
            //Get output path
        }

        private void DoOpenLogoWin()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Images (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == true)
            {
                LogoName = ofd.FileName;
                OnPropertyChanged("LogoName");

                Properties.Settings.Default.LogoPath = LogoName;
                Properties.Settings.Default.Save();
            }
        }

        private void DoApplyWatermark()
        {
            if(SourceImages!= null && SourceImages.Count > 0)
            {
                foreach (PhotoItem pi in SourceImages)
                {
                    if (LogoName != null && LogoName.Length > 0)
                        _wMarker.ApplyWatermark(pi.FullPath, LogoName, Path.Combine(OutputFolder, Path.GetFileName(pi.FullPath)));
                }

                MessageBox.Show("You're done");
            }
            else
                MessageBox.Show("No images selected");
        }
    }
}
