using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Qwirkle.Properties;

namespace Qwirkle
{
    public class Board
    {
        public Block[,] GameArea { get; private set; }

        private List<Block> _blockBag = new List<Block>();
        static Random rnd = new Random();
        private bool _expanded;
        public Board()
        {
            GameArea = new Block[9, 9];
            _blockBag = MakeBlockBag();
            _expanded = false;
        }

        private List<Block> MakeBlockBag()
        {
            List<Block> bag = new List<Block>();
            foreach (BlockColor c in Enum.GetValues(typeof(BlockColor)))
            {
                foreach (BlockShape s in Enum.GetValues(typeof(BlockShape)))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        bag.Add(new Block(c, s, (Image)Resources.ResourceManager.GetObject(s.ToString() + '-' + c.ToString())));
                    }
                }
            }
            return bag;
        }

        public Block GetBlock()
        {
            int r = rnd.Next(_blockBag.Count);
            Block newBlock = _blockBag[r];
            _blockBag.RemoveAt(r);
            return newBlock;
        }

        public void updateBoard(List<Tuple<Block, int, int>> play)
        {
            foreach (Tuple<Block, int, int> p in play)
            {
                GameArea[p.Item2, p.Item3] = p.Item1;
            }
        }
        public int ScorePlay(int yOrigin, int xOrigin, int yDirection, int xDirection, List<Tuple<Block, int, int>> play)
        {
            int score = 0;
            int xCheck = xOrigin - xDirection;
            int yCheck = yOrigin - yDirection;
            while ((yCheck >= 0 && xCheck >= 0) && (yCheck < GameArea.GetLength(1) && xCheck < GameArea.GetLength(1)) && GameArea[yCheck, xCheck] != null)
            {
                
                Block check = GameArea[yCheck, xCheck];
                for (int i = 0; i < play.Count; i++)
                {
                    if (play[i].Item1 == check)
                    {
                        play.Remove(play[i]);
                    }
                }

                score += 1;
                xCheck -= xDirection;
                yCheck -= yDirection;

            }
            return score;
        }
        public string ConvertBoardToStringArray()
        {
            StringBuilder returnString = new StringBuilder("[");
            
            for(int i = 0; i < GameArea.GetLength(0); i++)
            {
                StringBuilder hold = new StringBuilder("[");
                for(int k = 0; k < GameArea.GetLength(0); k++)
                {
                    if(GameArea[i,k] != null)
                    {
                        Block b = GameArea[i, k];
                        hold.Append(string.Format("play({0},{1}, tile({2},{3}),", i, k, b.Shape, b.Color));
                    }
                }
                hold.Remove(hold.Length - 1, 1);
                hold.Append("],");
                if(hold.Length > 2)
                {
                    returnString.Append(hold.ToString());
                }
            }
            returnString.Remove(returnString.Length - 1, 1);
            returnString.Append("]");
            return returnString.ToString();
        }
        public bool WasExpanded()
        {
            if(_expanded)
            {
                _expanded = false;
                return true;
            }
            return false;
        }
    }
}
