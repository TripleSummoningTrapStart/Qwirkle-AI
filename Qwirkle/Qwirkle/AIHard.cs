using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qwirkle
{
    public class AIHard : Player
    {
        public AIHard(Block[] startHand) : base(startHand)
        {
        }

        public List<Tuple<Block, int, int>> DeterminePlay(string[] plays)
        {
            throw new NotImplementedException();
        }
    }
}
