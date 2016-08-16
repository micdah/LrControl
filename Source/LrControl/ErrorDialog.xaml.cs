using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using LrControlCore.Util;
using micdah.LrControl.Core;

namespace micdah.LrControl
{
    /// <summary>
    ///     Interaction logic for ErrorDialog.xaml
    /// </summary>
    public partial class ErrorDialog
    {
        private readonly string _exceptionString;

        public ErrorDialog(Exception exception)
        {
            InitializeComponent();

            _exceptionString = new ExceptionStringBuilder(exception).ToString();
            ExceptionDetails.Text = _exceptionString;
        }


        protected override void OnClosed(EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonCopy_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(_exceptionString);
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}