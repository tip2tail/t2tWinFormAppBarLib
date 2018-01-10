using System;
using System.Windows.Forms;
using tip2tail.WinFormAppBarLib;

namespace LibTestProgram
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            AppBarHelper.SetAppBar(this, AppBarEdge.Top);
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            AppBarHelper.AppBarMessage = "TestAppBarApplication";
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            AppBarHelper.SetAppBar(this, AppBarEdge.Left);
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            AppBarHelper.SetAppBar(this, AppBarEdge.None);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            AppBarHelper.SetAppBar(this, AppBarEdge.Right);
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            AppBarHelper.SetAppBar(this, AppBarEdge.Bottom);
        }
    }
}
