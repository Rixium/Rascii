using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rascii.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rascii.Screen
{
    public class Pane
    {

        Rectangle rectangle;
        Color color;
        string name;
        Content content;

        public Pane(int width, int height, int x, int y, Color color, string name, Content content)
        {
            this.rectangle = new Rectangle(x, y, width, height);
            this.color = color;
            this.name = name;
            this.content = content;
        }

        public void Update()
        {
            if(content != null)
            {
                content.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentChest.Instance.pixel, rectangle, color);
            //spriteBatch.DrawString(ContentChest.Instance.gamefont, name, new Vector2(rectangle.X + 10, rectangle.Y + 10), Color.White);
            if(content != null)
            {
                content.Draw(spriteBatch);
            }
        }

        public Content GetContent()
        {
            return this.content;
        }

    }
}
