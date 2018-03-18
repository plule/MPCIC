using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MPCIC.Core;

namespace MPCIC.Gui
{
    public class SampleControl : UserControl
    {
        public Sample Sample { get; }

        public SampleControl() : this(new Sample())
        {
        }

        public SampleControl(Sample sample)
        {
            this.InitializeComponent();
            Sample = sample;
            DataContext = sample;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
