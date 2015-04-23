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
            DialogResult result = dialog.ShowDialog();
            txtSelectedFolder.Text = dialog.SelectedPath;
        }

        private void Organize_Click(object sender, RoutedEventArgs e)
        {
            Organizer.Organize(txtSelectedFolder.Text);
            MessageBox.Show("Organizing pictures done. Duplicates are moved to duplicates folder, Other files are moved to OtherFiles folder, Empty folders are deleted.");
        }
    }
}
