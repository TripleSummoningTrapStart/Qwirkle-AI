using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qwirkle
{
    public class Player
    {
        public Block[] Hand { get; private set; }
        public int Score { get; private set; }
        public Player(Block[] startHand)
        {
            Hand = startHand;
        }
        public void FillHand(Block[] newBlocks)
        {
            for(int i = 0; i < newBlocks.Length; i++)
            {
                Hand[Hand.Length - newBlocks.Length + i] = newBlocks[i];
            }
        }
    }
}
