using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rascii.Constants;
using Rascii.Screen.MapClasses;
using Rascii.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rascii.Screen
{
    public class Stats : Content
    {

        Player player;
        int x, y;

        public Stats(int x, int y, Player player)
        {
            this.x = x;
            this.y = y;
            this.player = player;

        }

        public override void Update()
        {
            base.Update();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(ContentChest.Instance.gamefont, "Name: " + player.GetStats().name, new Vector2(x + 10, y + 10), Color.White);
            spriteBatch.DrawString(ContentChest.Instance.gamefont, "Level: " + player.GetStats().level, new Vector2(x + 10, y + 30), Color.White);
            spriteBatch.DrawString(ContentChest.Instance.gamefont, "Health: " + player.GetStats().currHealth + "/" + player.GetStats().maxHealth, new Vector2(x + 10, y + 50), Color.White);
            spriteBatch.DrawString(ContentChest.Instance.gamefont, String.Format("Attack: {0} ({1}%)",player.GetStats().attack, player.GetStats().attackChance), new Vector2(x + 10, y + 70), Color.White);
            spriteBatch.DrawString(ContentChest.Instance.gamefont, String.Format("Defence: {0} ({1}%)",player.GetStats().defence, player.GetStats().defenceChance), new Vector2(x + 10, y + 90), Color.White);
            spriteBatch.DrawString(ContentChest.Instance.gamefont, "Gold: " + player.GetStats().gold, new Vector2(x + 10, y + 110), CellColors.GOLD);
        }

    }
}
