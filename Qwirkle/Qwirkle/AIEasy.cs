using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qwirkle
{
    public class AIEasy : Player
    {
        public override Block[] Hand { get; protected set; }
        public override int Score { get; protected set; }
        static Random rnd = new Random();
        public AIEasy(Block[] startHand) : base(startHand)
        {
        }

        public override List<Tuple<Block, int, int>> DeterminePlay(List<List<Tuple<string, string, int, int>>> plays)
        {
            int play = rnd.Next(0, plays.Count);
            List<Tuple<string, string, int, int>> thePlay = plays[play];
            List<Tuple<Block, int, int>> convertedPlay = new List<Tuple<Block, int, int>>();
            for (int i = 0; i < thePlay.Count; i++)
            {
                Block blockFromHand;
                Tuple<string, string, int, int> hold = thePlay[i];
                for(int k = 0; k < Hand.Length; k++)
                {
                    if((Hand[k].Shape.ToString() == hold.Item1) && (Hand[k].Color.ToString() == hold.Item2))
                    {
                        blockFromHand = Hand[k];
                        convertedPlay.Add(new Tuple<Block, int, int>(blockFromHand, hold.Item3, hold.Item4));
                    }
                }
            }
            return convertedPlay;
        }

        public override void RemoveBlocksFromHand(List<Tuple<Block, int, int>> play)
        {
            throw new NotImplementedException();
        }
    }
}
