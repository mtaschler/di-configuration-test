namespace DiConfigurationTest
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IWritableOptions<WindowSettings> _settings;

        public MainWindow(IWritableOptions<WindowSettings> settings)
        {
            InitializeComponent();

            _settings = settings;
            WindowState = _settings.Value.IsMaximized ? WindowState.Maximized : WindowState.Normal;
            Left = _settings.Value.Left;
            Top = _settings.Value.Top;
            Width = _settings.Value.Width;
            Height = _settings.Value.Height;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (WindowState == WindowState.Maximized)
            {
                _settings.Update(settings => settings.IsMaximized = true);
            }
            else if (WindowState == WindowState.Normal)
            {
                _settings.Update(settings =>
                {
                    settings.IsMaximized = false;
                    settings.Width = sizeInfo.NewSize.Width;
                    settings.Height = sizeInfo.NewSize.Height;
                });
            }
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            _settings.Update(settings =>
            {
                settings.Left = this.Left;
                settings.Top = this.Top;
            });
        }
    }
}
