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

        private ScorePlayDelegate _score;
        public AIHard(Block[] startHand, ScorePlayDelegate mp):base(startHand)
        {
            this.Hand = startHand;
            this._score = mp;
        }

        public override List<Tuple<Block, int, int>> DeterminePlay(List<List<Tuple<string, string, int, int>>> plays)
        {
            List<Tuple<Block, int, int>> bestPlay = new List<Tuple<Block, int, int>>();
            int bestPlayScore = int.MinValue;
            for (int i = 0; i < plays.Count; i++)
            {
                List<Tuple<Block, int, int>> convertedPlay = new List<Tuple<Block, int, int>>();
                for (int k = 0; k < plays[i].Count; k++)
                {
                    Block blockFromHand;
                    Tuple<string, string, int, int> hold = plays[i][k];
                    for (int m = 0; m < Hand.Length; m++)
                    {
                        if ((Hand[m].Shape.ToString() == hold.Item1) && (Hand[m].Color.ToString() == hold.Item2))
                        {
                            blockFromHand = Hand[k];
                            convertedPlay.Add(new Tuple<Block, int, int>(blockFromHand, hold.Item3, hold.Item4));
                        }
                    }
                }
                int testScore = _score(convertedPlay);
                if(testScore > bestPlayScore)
                {
                    bestPlay = convertedPlay;
                    bestPlayScore = testScore;
                }
                
            }
            
            return bestPlay;
        }
    }
}
