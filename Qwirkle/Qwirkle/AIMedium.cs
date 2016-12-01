using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qwirkle
{
    public class AIMedium : Player
    {
        public AIMedium(Block[] startHand) : base(startHand)
        {
        }

        public List<Tuple<Block, int, int>> DeterminePlay(string[] Plays)
        {
            throw new NotImplementedException();
        }
    }
}
