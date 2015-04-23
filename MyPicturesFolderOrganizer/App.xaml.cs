using System.Windows;
using System.Windows.Threading;
using log4net;
using log4net.Config;

namespace PicturesVideosOrganizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(App));

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            XmlConfigurator.Configure();
            _logger.Error(e.Exception);

            MessageBox.Show(e.Exception.Message, "Something is not right.", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        } 
    }
}
