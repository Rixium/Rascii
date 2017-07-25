using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rascii.Screen.MapClasses
{
    class Room
    {

        Rectangle rect;
        public int x, y, width, height;

        public Room(Rectangle rect, int x, int y, int width, int height)
        {
            this.rect = rect;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public Rectangle GetBounds()
        {
            return this.rect;
        }

        public Vector2 Center()
        {
            return new Vector2(x + width / 2, y + height / 2);
        }

        public int Left()
        {
            return x;
        }

        public int Right()
        {
            return x + width - 1;
        }

        public int Top()
        {
            return y;
        }

        public int Bottom()
        {
            return y + height - 1;
        }

        public bool Contains(Cell cell)
        {
            if(cell.GetCoordinates().X > Left() && cell.GetCoordinates().X < Right())
            {
                if(cell.GetCoordinates().Y > Top() && cell.GetCoordinates().Y < Bottom())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
