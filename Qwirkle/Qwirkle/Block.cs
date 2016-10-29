using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qwirkle
{
    public class Block
    {
        private BlockColor _color;
        private BlockShape _shape;

        public BlockShape Shape
        {
            get { return _shape; }
            set { _shape = value; }
        }

        public BlockColor Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Block(BlockColor color, BlockShape shape)
        {
            _color = color;
            _shape = shape;
        }

        public Image GetImage()
        {
            throw new NotImplementedException();
        }
    }
}