using System.Windows;

namespace MyPicturesFolderOrganizer
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
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            txtSelectedFolder.Text = dialog.SelectedPath;
        }

        private void Organize_Click(object sender, RoutedEventArgs e)
        {
            PicturesOrganizer.Organize(txtSelectedFolder.Text);
            MessageBox.Show("Organizing pictures done. Duplicates are moved to duplicates folder, Other files are moved to OtherFiles folder, Empty folders are deleted.");
        }
    }
}
