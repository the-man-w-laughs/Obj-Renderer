using Drawer;

namespace Renderer
{
    public partial class MainForm : Form
    {
        private Rectangle _screenBounds;
        private Bitmap _bitMap;        

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _screenBounds = Screen.PrimaryScreen.Bounds;
            _bitMap = new Bitmap(_screenBounds.Width, _screenBounds.Height);            

            pictureBox.Invalidate();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            _bitMap.DrawLine(Color.Black, 0, 0, 100, 100);
            e.Graphics.DrawImage(_bitMap, 0, 0);
        }
    }
}