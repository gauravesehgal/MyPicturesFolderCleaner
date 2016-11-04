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
            Organize(txtSelectedFolder.Text);
        }

        private async void Organize(string selectedFolderPath)
        {
            btnOrganize.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            await Organizer.Organize(selectedFolderPath);
            progressBar.Visibility = Visibility.Hidden;
            btnOrganize.IsEnabled = true;
            MessageBox.Show("Organizing pictures done. Duplicates are moved to duplicates folder, Other files are moved to OtherFiles folder, Empty folders are deleted.", "Aha...", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
