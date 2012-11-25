using System.Windows.Forms;

namespace UI.CustomControls
{
    public partial class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            InitializeComponent();

            DoubleBuffered = true;
        }
    }
}
