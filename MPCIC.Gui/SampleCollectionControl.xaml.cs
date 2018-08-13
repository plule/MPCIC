using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Data;
using Avalonia.Media;
using MPCIC.Core;

namespace MPCIC.Gui
{
    public class SampleCollectionControl : UserControl
    {

        public SampleCollection SampleCollection { get; private set; }

        public SampleCollectionControl()
        {
            this.InitializeComponent();
        }

        public void LoadCollection(SampleCollection collection)
        {
            SampleCollection = collection;
            DataContext = collection;

            _sampleGrid.Children.RemoveRange(_sampleGrid.ColumnDefinitions.Count, _sampleGrid.Children.Count - _sampleGrid.ColumnDefinitions.Count);
            _sampleGrid.RowDefinitions.RemoveRange(1, _sampleGrid.RowDefinitions.Count - 1);
            int row = 1;
            foreach(Sample sample in collection.Samples)
            {
                _sampleGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                _sampleGrid.Children.Add(new TextBlock(){
                    DataContext = sample,
                    [!TextBlock.TextProperty] = new Binding("FileName"),
                    [Grid.RowProperty] = row,
                    [Grid.ColumnProperty] = 0,
                    Margin = new Avalonia.Thickness(3)
                });

                _sampleGrid.Children.Add(new TextBlock(){
                    DataContext = sample,
                    [!TextBlock.TextProperty] = new Binding("LowNote"),
                    [Grid.RowProperty] = row,
                    [Grid.ColumnProperty] = 1,
                    Margin = new Avalonia.Thickness(3),
                    TextAlignment = TextAlignment.Center
                });

                _sampleGrid.Children.Add(new TextBlock(){
                    DataContext = sample,
                    [!TextBlock.TextProperty] = new Binding("RootNote"),
                    FontWeight = FontWeight.Bold,
                    [Grid.RowProperty] = row,
                    [Grid.ColumnProperty] = 2,
                    Margin = new Avalonia.Thickness(3),
                    TextAlignment = TextAlignment.Center
                });

                _sampleGrid.Children.Add(new TextBlock(){
                    DataContext = sample,
                    [!TextBlock.TextProperty] = new Binding("HighNote"),
                    [Grid.RowProperty] = row,
                    [Grid.ColumnProperty] = 3,
                    Margin = new Avalonia.Thickness(3),
                    TextAlignment = TextAlignment.Center
                });

                row++;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _name = this.FindControl<TextBox>("name");

            _sampleStack = this.FindControl<StackPanel>("sampleStack");
            _sampleGrid = this.FindControl<Grid>("sampleGrid");

            SampleCollection collection = new SampleCollection();
            if(Design.IsDesignMode)
            {
                collection.Samples.Add(new Sample("Sample", new Note(Key.C, 1), "C1.wav"));
                collection.Samples.Add(new Sample("Sample", new Note(Key.C, 2), "C2.wav"));
                collection.Samples.Add(new Sample("Sample", new Note(Key.C, 3), "C3.wav"));
                collection.Samples.Add(new Sample("Sample", new Note(Key.C, 4), "C4.wav"));
            }

            LoadCollection(collection);
        }

        private TextBox _name;
        private StackPanel _sampleStack;
        private Grid _sampleGrid;
    }
}
