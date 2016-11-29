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
        private Player _human;
        private List<UpdateDelegate> _observer;
        public Controller(List<UpdateDelegate> observer)
        {
            _observer = observer;
            _board = new Board();
            _human = new Player(MakeNewHand());
        }

        private void FireObserver()
        {
            foreach(UpdateDelegate d in _observer)
            {
                d(_human.Hand, _human.Score, 0, _board.GameArea);
            }
        }
        private Block[] MakeNewHand()
        {
            Block[] hand = new Block[6];
            for(int i = 0; i < hand.Length; i++)
            {
                hand[i] = _board.GetBlock();
            }
            return hand;
        }
        public bool MakePlay(Tuple<Block, int, int> play)
        {

            FireObserver();
            return false;
        }
    }
}
