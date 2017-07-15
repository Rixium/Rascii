using Microsoft.Xna.Framework;
using Rascii.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rascii.Screen.MapClasses
{
    public class Cell
    {

        String value;
        String oldValue;
        int x, xCord;
        int y, yCord;
        Vector2 pos;
        Color color, oldColor;
        Color backColor, oldBackColor;
        bool visible = false;
        bool beenVisible = false;
        bool walkable;

        public Cell(int x, int y, string value, Color color, Color backcolor, int xCord, int yCord)
        {
            this.x = x;
            this.y = y;
            this.value = value;
            this.pos = new Vector2(x, y);
            this.color = color;
            this.backColor = backcolor;
            this.xCord = xCord;
            this.yCord = yCord;
        }

        public string GetValue()
        {
            return value;
        }

        public Vector2 GetPosition()
        {
            return pos;
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, Project.tileSize, Project.tileSize);
        }

        public Color GetColor()
        {
            return color;
        }
        
        public Color GetBackColor()
        {
            return backColor;
        }

        public Vector2 GetCoordinates()
        {
            return new Vector2(xCord, yCord);
        }

        public void SetValue(string value)
        {
            this.oldValue = this.value;
            this.value = value;

            if(this.value == ".")
            {
                walkable = true;
            }
        }

        public void SetVisible(bool visible)
        {
            this.visible = visible;
            if(visible)
            {
                beenVisible = true;
            }
        }

        public bool GetBeenVisible()
        {
            return beenVisible;
        }

        public void RemoveEntity()
        {
            this.value = oldValue;
            this.color = oldColor;
        }

        public void SetColor(Color color)
        {
            this.oldColor = this.color;
            this.color = color;
        }

        public void SetBackColor(Color color)
        {
            this.oldBackColor = this.backColor;
            this.backColor = color;
        }

        public bool GetVisible()
        {
            return visible;
        }

        public bool GetWalkable()
        {
            return walkable;
        }

    }
}
