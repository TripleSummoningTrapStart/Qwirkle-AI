using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Qwirkle
{
    public class Controller
    {
        private Board _board;
        private PrologCommunicator _prolog;
        private Player _human;
        private Player _AI;
        private List<UpdateDelegate> _observer;
        public Controller(List<UpdateDelegate> observer, PrologCommunicator p, int AIDifficulty)
        {
            _observer = observer;
            _board = new Board();
            _human = new Player(MakeNewHand());
            _prolog = p;
            switch (AIDifficulty)
            {
                case 0:
                    _AI = new AIEasy(MakeNewHand());
                    break;
                case 1:
                    _AI = new AIMedium(MakeNewHand());
                    break;
                case 2:
                    _AI = new AIHard(MakeNewHand());
                    break;
            }
        }

        private void FireObserver()
        {
            foreach (UpdateDelegate d in _observer)
            {
                d(_human.Hand, _human.Score, 0, _board.GameArea, true); //_board.WasExpanded());
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
                FireObserver();
            }
            else
            {
                bool valid = true;
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
                    int humanScore = 0;
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
                        humanScore += holdScore;
                    }
                    for (int i = 0; i < playList2.Count; i++)
                    {
                        Tuple<Block, int, int> currentPlay = playList2[i];
                        int holdScore = 0;
                        holdScore += ScorePlay(playList2, 0, 1, currentPlay); //left
                        holdScore += ScorePlay(playList2, 0, -1, currentPlay); //right
                        if(holdScore > 0)
                        {
                            holdScore += 1; // current piece;
                        }
                        if (holdScore == 6)
                        {
                            holdScore += 6;
                        }
                        humanScore += holdScore;
                    }
                    _human.UpdateScore(humanScore);
                    FireObserver();
                    return true;
                }
            }
            return false;
        }

        private int ScorePlay(List<Tuple<Block, int, int>> play, int yDirection, int xDirection, Tuple<Block, int, int> currentPlay)
        {

            return _board.ScorePlay(currentPlay.Item2, currentPlay.Item3, yDirection, xDirection, play); ;
        }
        public void WriteBoard()
        {
            string board = _board.ConvertBoardToStringArray();
            File.WriteAllText(@"U:\ai\testBoard.txt", board);
        }
    }
}
