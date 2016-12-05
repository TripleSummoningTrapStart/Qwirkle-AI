using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qwirkle
{
    public abstract class Player
    {
        public abstract Block[] Hand { get; protected set; }
        public abstract int Score { get; protected set; }
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
        public abstract void RemoveBlocksFromHand(List<Tuple<Block, int, int>> play);
        public void UpdateScore(int score)
        {
            this.Score += score;
        }
        public abstract List<Tuple<Block, int, int>> DeterminePlay(string[] Plays);
    }
}
