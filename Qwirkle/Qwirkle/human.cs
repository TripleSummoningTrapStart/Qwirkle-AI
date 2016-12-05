using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qwirkle
{
    public class Human: Player
    {
        public override Block[] Hand{ get; protected set;}

        public override int Score{ get; protected set; }

        public Human(Block[] startHand):base(startHand)
        {

        }

        public override void RemoveBlocksFromHand(List<Tuple<Block, int, int>> play)
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

        public override List<Tuple<Block, int, int>> DeterminePlay(string[] Plays)
        {
            throw new NotImplementedException();
        }

    }
}
