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
            this.Hand = startHand;
        }
        public void FillHand(Block[] newBlocks)
        {
            for(int i = 0; i < newBlocks.Length; i++)
            {
                this.Hand[Hand.Length - newBlocks.Length + i] = newBlocks[i];
            }
        }
        public void RemoveBlocksFromHand(List<Tuple<Block, int, int>> play)
        {

            List<Block> HandList = Hand.ToList<Block>();
            foreach (Tuple<Block, int, int> p in play)
            {
                HandList.Remove(p.Item1);
            }
            while (HandList.Count < 6)
            {
                HandList.Add(null);
            }
            Hand = HandList.ToArray();

        }
        public void UpdateScore(int score)
        {
            this.Score += score;
        }

        public string HandString()
        {
            StringBuilder hold = new StringBuilder("[");
            for (int k = 0; k < Hand.Length; k++)
            {
                if (Hand[k] != null)
                {
                    Block b = Hand[k];
                    hold.Append(string.Format("tile({0},{1}),", b.Shape, b.Color));
                }
            }
            hold.Remove(hold.Length - 1, 1);
            hold.Append("]");
            return hold.ToString();
        }
        public abstract List<Tuple<Block, int, int>> DeterminePlay(List<List<Tuple<string, string, int, int>>> plays, ScorePlayDelegate scorePlay);
        public abstract List<Tuple<Block, int, int>> PlayOnGap(List<Tuple<int, int>> gaps);
    }
}
