using Avalonia;
using Avalonia.Markup.Xaml;

namespace MPCIC.Gui
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
