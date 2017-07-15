using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rascii.Constants;
using Rascii.Util;
using System.Collections.Generic;

namespace Rascii.Screen
{
    public class Messages : Content
    {

        private List<string> messages = new List<string>();
        private int maxMessageView = 4;
        private int maxMessageHistory = 50;
        private int x, y;
        private int currentStart = 0;
        private KeyboardState ls;

        public Messages(int x, int y, int height)
        {
            this.x = x;
            this.y = y;
            this.maxMessageView = height / (int)ContentChest.Instance.gamefont.MeasureString("XXXXXX").Y - 2;
        }

        public override void Update()
        {
            base.Update();
            CheckInput();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            int drawStart = 0;
            if (messages.Count > 0)
            {
                for (int i = currentStart; i < currentStart + maxMessageView; i++)
                {
                    if (i < messages.Count)
                    {
                        spriteBatch.DrawString(ContentChest.Instance.gamefont, messages[i], new Vector2(x + Project.messageWidth - ContentChest.Instance.gamefont.MeasureString(messages[i]).X - Project.panePadding, y + Project.panePadding + (drawStart * ContentChest.Instance.gamefont.MeasureString(messages[i]).Y)), Color.White);
                        drawStart++;
                    }
                }
            }

        }

        private void CheckInput()
        {
            KeyboardState ks = Keyboard.GetState();

            if(ks.IsKeyDown(KeyBindings.DOWNMESSAGE) && ls.IsKeyUp(KeyBindings.DOWNMESSAGE))
            {
                if (currentStart < messages.Count)
                {
                    currentStart++;
                }
            } else if (ks.IsKeyDown(KeyBindings.UPMESSAGE) && ls.IsKeyUp(KeyBindings.UPMESSAGE))
            {
                if(currentStart > 0)
                {
                    currentStart--;
                }
            }
            ls = ks;
        }

        public void AddMessage(string message)
        {
            messages.Insert(0, message);
            if(messages.Count > maxMessageHistory)
            {
                messages.RemoveAt(messages.Count);
            }
        }
    }
}
