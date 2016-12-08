using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Qwirkle.Properties;

namespace Qwirkle
{
    public class Controller
    {
        private Board _board;
        private PrologCommunicator _prolog;
        private Human _human;
        private Player _AI;
        private List<UpdateDelegate> _observer;
        private string test = "[[play(space(0, 1), tile(star, red)), play(space(0, 0), tile(cross, red)), play(space(0, 2), tile(square, red))],[play(space(0, 1), tile(star, red)), play(space(0, 0), tile(cross, red))],[play(space(0, 1), tile(star, red))]]";
        public Controller(List<UpdateDelegate> observer, PrologCommunicator p, int AIDifficulty)
        {
            _observer = observer;
            _board = new Board();
            _human = new Human(MakeNewHand());
            _prolog = p;

            Block[] testHand = new Block[] { new Block (BlockColor.red, BlockShape.cross, (Image)Resources.ResourceManager.GetObject(BlockShape.cross.ToString() + '-' + BlockColor.red.ToString())),
                                                        new Block (BlockColor.red, BlockShape.star, (Image)Resources.ResourceManager.GetObject(BlockShape.star.ToString() + '-' + BlockColor.red.ToString())),
                                                        new Block (BlockColor.red, BlockShape.square, (Image)Resources.ResourceManager.GetObject(BlockShape.square.ToString() + '-' + BlockColor.red.ToString()))};
            switch (AIDifficulty)
            {
                case 0:
                    _AI = new AIEasy(testHand);
                    break;
                case 1:
                    _AI = new AIMedium(testHand);
                    break;
                case 2:
                    _AI = new AIHard(testHand, Score);
                    break;
            }
        }

        private void FireObserver(List<Tuple<Block, int, int>> aiPlay)
        {
            foreach (UpdateDelegate d in _observer)
            {
                d(_human.Hand, _human.Score, 0, aiPlay);
            }
        }
        private Block[] MakeNewHand()
        {
            Block[] hand = new Block[6];
            for (int i = 0; i < hand.Length; i++)
            {
                hand[i] = _board.GetBlock();
            }
            return hand;
        }
        public bool MakePlay(List<Tuple<Block, int, int>> play)
        {
            if (play == null)
            {
                FireObserver(new List<Tuple<Block, int, int>>());
            }
            else
            {
                bool valid = _prolog.TestMove(ConvertPlayToString(play));
                //Check Valid play in prolog
                if (valid)
                {
                    _human.RemoveBlocksFromHand(play);
                    Block[] fillhand = new Block[play.Count];
                    for (int i = 0; i < play.Count; i++)
                    {
                        fillhand[i] = _board.GetBlock();
                    }
                    _human.FillHand(fillhand);
                    _board.updateBoard(play);
                    //parseReturnedGaps(_prolog.GetGaps(_board.ConvertBoardToString()));
                    int humanScore = Score(play);
                    _human.UpdateScore(humanScore);
                    //List<Tuple<Block, int, int>> aiPlay = _AI.DeterminePlay(parseReturnedPlays(test));
                    List<Tuple<Block, int, int>> aiPlay = _AI.PlayOnGap(parseReturnedGaps(_prolog.GetGaps(_board.ConvertBoardToString())));
                    _board.updateBoard(aiPlay);
                    FireObserver(aiPlay);
                    return true;
                }
            }
            return false;
        }
        private int Score(List<Tuple<Block, int, int>> play)
        {
            int score = 0;
            List<Tuple<Block, int, int>> playList2 = play.ToList();
            for (int i = 0; i < play.Count; i++)
            {
                Tuple<Block, int, int> currentPlay = play[i];
                int holdScore = 0;
                holdScore += ScorePlay(play, 1, 0, currentPlay); //up
                holdScore += ScorePlay(play, -1, 0, currentPlay);  //down
                if (holdScore > 0)
                {
                    holdScore += 1; // current piece;
                }
                if (holdScore == 6)
                {
                    holdScore += 6;
                }
                score += holdScore;
            }
            for (int i = 0; i < playList2.Count; i++)
            {
                Tuple<Block, int, int> currentPlay = playList2[i];
                int holdScore = 0;
                holdScore += ScorePlay(playList2, 0, 1, currentPlay); //left
                holdScore += ScorePlay(playList2, 0, -1, currentPlay); //right
                if (holdScore > 0)
                {
                    holdScore += 1; // current piece;
                }
                if (holdScore == 6)
                {
                    holdScore += 6;
                }
                score += holdScore;
            }
            return score;
        }
        private int ScorePlay(List<Tuple<Block, int, int>> play, int yDirection, int xDirection, Tuple<Block, int, int> currentPlay)
        {

            return _board.ScorePlay(currentPlay.Item2, currentPlay.Item3, yDirection, xDirection, play); ;
        }
        public void WriteBoard()
        {
            string board = _board.ConvertBoardToString();
            File.WriteAllText(@"U:\ai\testBoard1.txt", board);
        }
        private List<List<Tuple<string, string, int, int>>> parseReturnedPlays(string s)
        {
            List<List<Tuple<string, string, int, int>>> plays = new List<List<Tuple<string, string, int, int>>>();
            s = s.Substring(1, s.Length - 1).Replace(",", " ").Replace("play", "").Replace("tuple", "");
            string[][] test = s.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Split(new char[] { '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
            foreach (string[] st in test)
            {
                List<Tuple<string, string, int, int>> holdList = new List<Tuple<string, string, int, int>>();
                if (st.Length > 3)
                {
                    continue;
                }
                for (int i = 0; i < st.Length; i++)
                {
                    if (st[i] == " ")
                    {
                        continue;
                    }
                    else
                    {
                        if (st[i] == "space")
                        {
                            //Tuple<string, string, int, int> hold = new Tuple<string, string, int, int>(st[i + 4], st[i + 5], Convert.ToInt32(st[i + 1]), Convert.ToInt32(st[i + 2]));
                            //i += 5;
                            //holdList.Add(hold);
                        }
                    }
                }

                plays.Add(holdList);
            }
            return plays;
        }
        private List<Tuple<int, int>> parseReturnedGaps(string s)
        {
            List<Tuple<int, int>> gaps = new List<Tuple<int, int>>();
            s = s.Substring(1, s.Length - 1).Replace(",", " ").Replace("play", "").Replace("tuple", "");
            string[][] test = s.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Split(new char[] { '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
            foreach (string[] st in test)
            {
                if (st.Length > 3 || st.Length == 0)
                {
                    continue;
                }
                else
                {
                    Tuple<int, int> hold = new Tuple<int, int>(Convert.ToInt32(st[1]), Convert.ToInt32(st[2]));
                    gaps.Add(hold);
                }
            }
            return gaps;
        }
        private string ConvertPlayToString(List<Tuple<Block, int, int>> play)
        {
            StringBuilder returnString = new StringBuilder("[");

            for (int i = 0; i < play.Count; i++)
            {
                Block b = play[i].Item1;
                returnString.Append(string.Format("tile({0},{1}),", b.Shape, b.Color));
            }
            returnString.Remove(returnString.Length - 1, 1);
            returnString.Append("]");
            return returnString.ToString();
        }
    }
}
