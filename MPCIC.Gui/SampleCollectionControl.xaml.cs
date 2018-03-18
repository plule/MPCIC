using Avalonia.Controls;
using Avalonia.Markup.Xaml;
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
            
            _sampleStack.Children.Clear();
            foreach(Sample sample in collection.Samples)
            {
                _sampleStack.Children.Add(new SampleControl(sample));
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _name = this.FindControl<TextBox>("name");

            _sampleStack = this.FindControl<StackPanel>("sampleStack");

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
    }
}
