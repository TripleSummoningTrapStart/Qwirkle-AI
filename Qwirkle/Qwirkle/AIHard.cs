using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qwirkle
{
    public class AIHard : Player
    {
        public override Block[] Hand { get; protected set; }
        public override int Score { get; protected set; }
        public AIHard(Block[] startHand) : base(startHand)
        {
        }

        public override List<Tuple<Block, int, int>> DeterminePlay(string[] Plays)
        {
            throw new NotImplementedException();
        }

        public override void RemoveBlocksFromHand(List<Tuple<Block, int, int>> play)
        {
            throw new NotImplementedException();
        }
    }
}
