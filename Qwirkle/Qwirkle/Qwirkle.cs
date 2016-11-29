using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qwirkle
{
    public partial class Qwirkle : Form
    {
        private PrologCommunicator _prolog;
        private Board _board;
        public Qwirkle()
        {
            InitializeComponent();
            _prolog = new PrologCommunicator();
            _board = new Board();

        }

        private void ux_TestBtn_Click(object sender, EventArgs e)
        {
            //var res = _prolog.Test();
            //MessageBox.Show(res);
            Block b = _board.GetBlock();
            pictureBox1.Image = b.Image;
        }

    }
}