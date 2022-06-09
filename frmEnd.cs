using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vidyakali
{
    public partial class frmEnd : Form
    {
        public frmEnd(Image img)
        {
            InitializeComponent();
            this.BackgroundImage = img;
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;

        }

        private void frmEnd_Load(object sender, EventArgs e)
        {

        }

    }
}
