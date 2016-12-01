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
        public Board ()
        {
            GameArea = new Block[9,9];
            _blockBag = MakeBlockBag();
        }

        private List<Block> MakeBlockBag()
        {
            List<Block> bag = new List<Block>();
            foreach(BlockColor c in Enum.GetValues(typeof(BlockColor)))
            {
                foreach(BlockShape s in Enum.GetValues(typeof(BlockShape)))
                {
                    for(int i = 0; i < 3; i++)
                    {
                        bag.Add(new Block(c, s, (Image) Resources.ResourceManager.GetObject(s.ToString() + '-' + c.ToString())));
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
            foreach(Tuple<Block, int, int> p in play)
            {
                GameArea[p.Item2, p.Item3] = p.Item1;
            }
        }
    }
}
