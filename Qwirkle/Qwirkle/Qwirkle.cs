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
        private Controller _controller;
        private MakePlayDelegate _makePlay;
        private PictureBox _originClicked;
        public Qwirkle(MakePlayDelegate d, Controller c, PrologCommunicator p)
        {
            InitializeComponent();

            _controller = c;
            _prolog = p;
            _makePlay = d;
           

        }

        private void ux_TestBtn_Click(object sender, EventArgs e)
        {
            //var res = _prolog.Test();
            //MessageBox.Show(res);
            _makePlay(new Tuple<Block, int, int>(null, 0, 0));
        }
        public void UpdateForm(Block[] playerHand, int PlayerScore, int ComputerScore, Block[,] Board)
        {
            Playerhand1.Image = playerHand[0].Image;
            Playerhand2.Image = playerHand[1].Image;
            Playerhand3.Image = playerHand[2].Image;
            Playerhand4.Image = playerHand[3].Image;
            Playerhand5.Image = playerHand[4].Image;
            Playerhand6.Image = playerHand[5].Image;
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox desinationClicked = (PictureBox)sender;
            if (_originClicked == null && (desinationClicked.Name.Contains("hand") || desinationClicked.Image == null))
            {
                _originClicked = desinationClicked;
            }
            else if(_originClicked != null && (_originClicked!= desinationClicked) && ((_originClicked.Name.Contains("hand") && desinationClicked.Name.Contains("hand") || (_originClicked.Image == null || desinationClicked.Image == null))))
            {
                Image hold = _originClicked.Image;
                _originClicked.Image = desinationClicked.Image;
                desinationClicked.Image = hold;
                _originClicked = null;
            }

        }
    }
}