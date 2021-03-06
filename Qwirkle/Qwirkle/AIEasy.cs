﻿using System;
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
            List<Tuple<Block, int, int>> convertedPlay = null;
           
                int play = rnd.Next(0, plays.Count);
                List<Tuple<string, string, int, int>> thePlay = plays[play];
                convertedPlay = new List<Tuple<Block, int, int>>();
              
                    Block blockFromHand;
                    Tuple<string, string, int, int> hold = thePlay[0];
                    for (int k = 0; k < Hand.Length; k++)
                    {
                        if ((Hand[k].Shape.ToString() == hold.Item1) && (Hand[k].Color.ToString() == hold.Item2))
                        {
                            blockFromHand = Hand[k];
                            convertedPlay.Add(new Tuple<Block, int, int>(blockFromHand, hold.Item3, hold.Item4));
                        }
                    }
                
                plays.Remove(thePlay);
            
            return convertedPlay;
        }

        public override List<Tuple<Block, int, int>> PlayOnGap(List<Tuple<int, int>> gaps)
        {
            int block = rnd.Next(0, Hand.Length);
            int gap = rnd.Next(0, gaps.Count);
            List<Tuple<Block, int, int>> play = new List<Tuple<Block, int, int>>();
            Tuple<int, int> theGap = gaps[gap];
            play.Add(new Tuple<Block, int, int>(Hand[block], theGap.Item1, theGap.Item2));
            return play;
        }

    }
}
