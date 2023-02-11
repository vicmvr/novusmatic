using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Novusmatic
{
    public partial class frmNovusmatic : Form
    {
        public frmNovusmatic()
        {
            InitializeComponent();
        }

        private void catalogoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCatalogo catalogo = new frmCatalogo();
            catalogo.ShowDialog();
            catalogo.Location=new Point(0,0);
            //catalogo.StartPosition = FormStartPosition.CenterScreen;
            //catalogo.WindowState = FormWindowState.Maximized;
        }
    }
}
