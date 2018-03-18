using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MPCIC.Core;
using System;

namespace MPCIC.Gui
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _chooseSamplesButton = this.FindControl<Button>("chooseSamples");
            _chooseSamplesButton.Click += OnChooseSamplesClick;

            _doItButton = this.FindControl<Button>("doIt");
            _doItButton.Click += OnDoItClick;

            _sampleCollection = this.FindControl<SampleCollectionControl>("sampleCollection");
        }

        private async void OnChooseSamplesClick(object sender, RoutedEventArgs eventArgs)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog(); 
                ofd.Title = "Choose samples"; 
                ofd.AllowMultiple = true; 
 
                string[] files = await ofd.ShowAsync(VisualRoot as Window); 
 
                if(files != null) 
                { 
                    SampleCollection collection = SampleCollection.FromFilesInDirectory(files);
 
                    _sampleCollection.LoadCollection(collection);
                }
            }
            catch(Exception e)
            {
                showErrorDialog(e);
            }
        }

        private void OnDoItClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _sampleCollection.SampleCollection.DoIt();
            }
            catch(Exception ex)
            {
                showErrorDialog(ex);
            }
        }

        private void showErrorDialog(Exception e)
        {
            Window error = new Window();
            TextBox txt = new TextBox();
            txt.IsReadOnly = true;
            txt.Text = string.Format("{0}\n\n\n{1}", e.Message, e.StackTrace);
            txt.TextWrapping = Avalonia.Media.TextWrapping.Wrap;
            error.Content = txt;
            error.Width = 500;
            error.Height = 300;

            error.ShowDialog();
        }

        private Button _chooseSamplesButton;
        private Button _doItButton;
        private SampleCollectionControl _sampleCollection;
    }
}
