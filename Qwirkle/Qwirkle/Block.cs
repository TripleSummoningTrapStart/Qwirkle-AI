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
        public BlockColor Color { get; private set; }
        public BlockShape Shape { get; private set; }

        public Image Image { get; private set; }

        //public BlockShape Shape
        //{
        //    get { return _shape; }
        //    set { _shape = value; }
        //}

        //public BlockColor Color
        //{
        //    get { return _color; }
        //    set { _color = value; }
        //}

        public Block(BlockColor color, BlockShape shape, Image image)
        {
            this.Color = color;
            this.Shape = shape;
            this.Image = image;
        }
    }
}