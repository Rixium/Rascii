
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Rascii.Constants;
using System.Collections.Generic;

namespace Rascii.Util {

    public class ContentChest {

        public ContentManager content { get; set; }

        private static ContentChest instance;

        public Texture2D pixel;
        public SpriteFont gamefont;

        public Dictionary<int, string> names = new Dictionary<int, string>();
        public Dictionary<int, string> values = new Dictionary<int, string>();
        public Dictionary<int, Color> enemyColors = new Dictionary<int, Color>();
        public Dictionary<int, int> enemyChance = new Dictionary<int, int>();

        public static ContentChest Instance {
            get {
                if (instance == null) {
                    instance = new ContentChest();
                }
                return instance;
            }
        }

        public void LoadContent() {
            pixel = content.Load<Texture2D>("Misc/pixel");
            gamefont = content.Load<SpriteFont>("fonts/gamefont");

            CreateEnemy(EnemyTypes.KOBOLD, "Kobold", "k", 50, Swatch.DbBrightWood);
            CreateEnemy(EnemyTypes.GOBLIN, "Goblin", "g", 20, Swatch.DbGrass);
            CreateEnemy(EnemyTypes.SPIDER, "Spider", "s", 70, Swatch.AlternateLighter);
            CreateEnemy(EnemyTypes.WOLF, "Wolf", "w", 30, Swatch.DbOldStone);
            CreateEnemy(EnemyTypes.DRAGON, "Dragon", "D", 1, Swatch.DbBlood);
        }

        private void CreateEnemy(int type, string name, string value, int chance, Color color)
        {
            names.Add(type, name);
            values.Add(type, value);
            enemyColors.Add(type, color);
            enemyChance.Add(type, chance);
        }

    }

}
