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
        public Board()
        {
            GameArea = new Block[9, 9];
            _blockBag = MakeBlockBag();
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
            StringBuilder hold = new StringBuilder();
            for(int i = 0; i < GameArea.GetLength(0); i++)
            {
                hold.Append("[");
                for(int k = 0; k < GameArea.GetLength(0); i++)
                {

                }
                hold.Append("]");
                if(hold.Length > 2)
                {

                }
                
            }

            returnString.Append("]");
            return returnString.ToString();
        }
    }
}
