using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                d(_human.Hand, _human.Score, 0, _board.GameArea);
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
                    int totalCount = 1;
                    int count;
                    List<Tuple<Block, int, int>> playList2 = play.ToList();
                    for (int i = 0; i < play.Count; i++)
                    {

                        totalCount = 1;
                        humanScore += _board.ScorePlay(play[i], 0, 1, play, out count); //up
                        totalCount += count;
                        humanScore += _board.ScorePlay(play[i], 0, -1, play, out count); //down
                        totalCount += count;
                        if (totalCount > 1)
                        {
                            humanScore += 1; //for current peice
                        }
                        if (totalCount == 6)
                        {
                            humanScore += 6;
                        }
                    }
                    for (int i = 0; i < playList2.Count; i++)
                    {
                        totalCount = 1;
                        humanScore += _board.ScorePlay(playList2[i], 1, 0, playList2, out count); // left
                        totalCount += count;
                        humanScore += _board.ScorePlay(playList2[i], -1, 0, playList2, out count); //right
                        totalCount += count;
                        if(totalCount > 1)
                        {
                            humanScore += 1; //for current peice
                        }
                        if (totalCount == 6)
                        {
                            humanScore += 6;
                        }

                    }
                    _human.UpdateScore(humanScore);
                    FireObserver();
                    return true;
                }
            }
            return false;
        }

        private int ScorePlay(List<Tuple<Block, int, int>> play, int yDirection, int xDirection)
        {
            return 0;
        }
    }
}
