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
    public partial class StartForm : Form
    {
        public int Difficulty { get; private set; }
        public StartForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Difficulty = 0;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Difficulty = 1;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Difficulty = 2;
            Close();
        }
    }
}
