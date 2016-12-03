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


        private Controller _controller;
        private MakePlayDelegate _makePlay;
        private PictureBox _originClicked;
        private Stack<Tuple<PictureBox, PictureBox>> _undoStack;
        private List<Tuple<Block, int, int>> _holdPlay;
        public Qwirkle(MakePlayDelegate d, Controller c)
        {
            InitializeComponent();

            _controller = c;

            _makePlay = d;
            _undoStack = new Stack<Tuple<PictureBox, PictureBox>>();
            _holdPlay = new List<Tuple<Block, int, int>>();

        }

        private void ux_TestBtn_Click(object sender, EventArgs e)
        {
            //var res = _prolog.Test();
            //MessageBox.Show(res);
            bool validPlay = _makePlay(_holdPlay);
            if (validPlay)
            {
                _undoStack.Clear();

            }
            else
            {
                MessageBox.Show("Your play was invalid");
                CompletelyUndo();
            }
            _holdPlay.Clear();

        }
        public void UpdateForm(Block[] playerHand, int playerScore, int computerScore, Block[,] board)
        {
            Playerhand1.Image = playerHand[0].Image;
            Playerhand2.Image = playerHand[1].Image;
            Playerhand3.Image = playerHand[2].Image;
            Playerhand4.Image = playerHand[3].Image;
            Playerhand5.Image = playerHand[4].Image;
            Playerhand6.Image = playerHand[5].Image;
            Playerhand1.Tag = playerHand[0];
            Playerhand2.Tag = playerHand[1];
            Playerhand3.Tag = playerHand[2];
            Playerhand4.Tag = playerHand[3];
            Playerhand5.Tag = playerHand[4];
            Playerhand6.Tag = playerHand[5];

            PlayerScore.Text = playerScore.ToString();
            AIScore.Text = computerScore.ToString();

          
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox destinationClicked = (PictureBox)sender;
            if (_originClicked == null && (destinationClicked.Name.Contains("hand") || destinationClicked.Image == null))
            {
                _originClicked = destinationClicked;
                //Highlights clicked object
                //_originClicked.BorderStyle = BorderStyle.Fixed3D;
                _originClicked.BackColor = Color.Yellow;
                Padding p = new Padding(3);
                _originClicked.Padding = p;
            }
            else if (_originClicked != null)
            {
                if(_originClicked.Name.Contains("hand") && destinationClicked.Name.Contains("hand"))
                {
                    object tagHold = _originClicked.Tag;
                    _originClicked.Tag = destinationClicked.Tag;
                    destinationClicked.Tag = tagHold;
                }
                if ((_originClicked.Name.Contains("board") || _originClicked.Name.Contains("picture")) && destinationClicked.Name.Contains("hand"))
                {
                    PictureBox hold = _originClicked;
                    _originClicked = destinationClicked;
                    destinationClicked = hold;
                }
                if (_originClicked != null && _originClicked != destinationClicked && !((_originClicked.Name.Contains("board") || _originClicked.Name.Contains("picture")) && (destinationClicked.Name.Contains("board") || destinationClicked.Name.Contains("picture"))) && (destinationClicked.Image == null || destinationClicked.Name.Contains("hand")))
                {
                    Image hold = _originClicked.Image;
                    _originClicked.Image = destinationClicked.Image;
                    destinationClicked.Image = hold;

                    //Removes Highlighting
                    _originClicked.BackColor = Color.White;
                    Padding p = new Padding(0);
                    _originClicked.Padding = p;
                    destinationClicked.BackColor = Color.White;
                    destinationClicked.Padding = p;


                    _undoStack.Push(new Tuple<PictureBox, PictureBox>(_originClicked, destinationClicked));
                    if (_originClicked.Name.Contains("hand") && (destinationClicked.Name.Contains("board") || destinationClicked.Name.Contains("picture")))
                    {
                        string location = (string)destinationClicked.Tag;
                        string[] locationCoord = location.Split(',');
                        int playLocationY = Convert.ToInt32(locationCoord[0]);
                        int playLocationX = Convert.ToInt32(locationCoord[1]);
                        _holdPlay.Add(new Tuple<Block, int, int>((Block)_originClicked.Tag, playLocationY, playLocationX));
                    }
                    _originClicked = null;
                }
                else
                {
                    _originClicked.BackColor = Color.White;
                    Padding p = new Padding(0);
                    _originClicked.Padding = p;
                    destinationClicked.BackColor = Color.White;
                    destinationClicked.Padding = p;
                    _originClicked = null;
                }
            }

        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            if (_undoStack.Count > 0)
            {
                Tuple<PictureBox, PictureBox> undo = _undoStack.Pop();
                Image hold = undo.Item1.Image;
                undo.Item1.Image = undo.Item2.Image;
                undo.Item2.Image = hold;
            }
            if (_holdPlay.Count > 0)
            {
                _holdPlay.RemoveAt(_holdPlay.Count - 1);
            }
        }

        private void Qwirkle_Shown(object sender, EventArgs e)
        {
            _makePlay(null);
        }
        private void CompletelyUndo()
        {
            while (_undoStack.Count > 0)
            {
                Tuple<PictureBox, PictureBox> undo = _undoStack.Pop();
                Image hold = undo.Item1.Image;
                undo.Item1.Image = undo.Item2.Image;
                undo.Item2.Image = hold;
            }
            _holdPlay.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _controller.WriteBoard();
        }
    }
}