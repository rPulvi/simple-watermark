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
        #endregion
        
        public string LogoName {get;set;}
        public string OutputFolder { get; set; }
        
        public ObservableCollection<string> SourceFiles { get; set; }

        private WMarker _wMarker;
        private string[] _sourceFiles;        

        public MainViewModel() 
        {
            OpenFileSelection = new DelegateCommand(c => this.DoOpenFileSelection(), null);
            QuitApplication = new DelegateCommand(c => this.DoQuitApplication(), null);
            ApplyWatermark = new DelegateCommand(c => this.DoApplyWatermark(), null);
            OpenSettingsWindow = new DelegateCommand(c => this.DoOpenSettingsWindow(), null);
            OpenLogoWin = new DelegateCommand(c => this.DoOpenLogoWin(), null);

            var defaultLogo = Properties.Settings.Default.LogoPath;
            var defaultOutPath = Properties.Settings.Default.OutputFolder;

            if (defaultLogo != null && File.Exists(defaultLogo))
                LogoName = defaultLogo;
            else
                LogoName = @"C:\cA.png";

            if (defaultOutPath != null && File.Exists(defaultOutPath))
                OutputFolder = Properties.Settings.Default.OutputFolder;
            else
                OutputFolder = @"C:\";

            _wMarker = new WMarker(LogoName);//Get logo path from settings

            SourceFiles = new ObservableCollection<string>();
            
            OnPropertyChanged("LogoName");
            OnPropertyChanged("OutputFolder");
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
                _sourceFiles = ofd.FileNames;
                SourceFiles = new ObservableCollection<string>(_sourceFiles);
                OnPropertyChanged("SourceFiles");
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
            if (_sourceFiles != null && _sourceFiles.Count() > 0)
            {
                foreach (var f in _sourceFiles)
                {                                        
                    _wMarker.ApplyWatermark(f, LogoName, Path.Combine(OutputFolder, Path.GetFileName(f)));
                }

                MessageBox.Show("You're done");
            }
            else
                MessageBox.Show("No images selected");
        }
    }
}
