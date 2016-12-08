using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qwirkle
{
    public class AIMedium : Player
    {


        public override Block[] Hand { get; protected set; }
        public override int Score { get; protected set; }

        public AIMedium(Block[] startHand) : base(startHand)
        {
        }
        public override List<Tuple<Block, int, int>> DeterminePlay(List<List<Tuple<string, string, int, int>>> plays)
        {
            List<Tuple<string, string, int, int>> thePlay = plays.OrderByDescending(t => t.Count).First();
            List<Tuple<Block, int, int>> convertedPlay = new List<Tuple<Block, int, int>>();
            for (int i = 0; i < thePlay.Count; i++)
            {
                Block blockFromHand;
                Tuple<string, string, int, int> hold = thePlay[i];
                for (int k = 0; k < Hand.Length; k++)
                {
                    if ((Hand[k].Shape.ToString() == hold.Item1) && (Hand[k].Color.ToString() == hold.Item2))
                    {
                        blockFromHand = Hand[k];
                        convertedPlay.Add(new Tuple<Block, int, int>(blockFromHand, hold.Item3, hold.Item4));
                    }
                }
            }
            return convertedPlay;
        }
    }
}
