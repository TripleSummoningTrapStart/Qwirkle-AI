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



        public override List<Tuple<Block, int, int>> DeterminePlay(List<List<Tuple<string, string, int, int>>> plays)
        {
            throw new NotImplementedException();
        }

        public override List<Tuple<Block, int, int>> PlayOnGap(List<Tuple<int, int>> gaps)
        {
            throw new NotImplementedException();
        }
    }
}
