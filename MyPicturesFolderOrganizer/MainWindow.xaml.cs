using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace PicturesVideosOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            txtSelectedFolder.Text = dialog.SelectedPath;
        }

        private void Organize_Click(object sender, RoutedEventArgs e)
        {
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            btnOrganize.IsEnabled = false;
            backgroundWorker.RunWorkerAsync(txtSelectedFolder.Text);
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnOrganize.IsEnabled = true;

            if (e.Error != null)
                throw e.Error;
            
            MessageBox.Show("Organizing pictures done. Duplicates are moved to duplicates folder, Other files are moved to OtherFiles folder, Empty folders are deleted.", "Aha...",MessageBoxButton.OK, MessageBoxImage.Information);
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Organizer.Organize((string)e.Argument);
        }
    }
}
