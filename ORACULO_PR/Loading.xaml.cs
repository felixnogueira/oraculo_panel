
using System.Windows.Controls;
using WpfAnimatedGif;

namespace ORACULO_PR
{
    /// <summary>
    /// Interação lógica para Loading.xam
    /// </summary>
    public partial class Loading : UserControl
    {
        public Loading()
        {
            InitializeComponent();
            ImageBehavior.SetAutoStart(gifPlay, true);

        }
    }
}
