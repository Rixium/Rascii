
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

            names.Add(EnemyTypes.KOBOLD, "Kobold");
            values.Add(EnemyTypes.KOBOLD, "k");
            enemyColors.Add(EnemyTypes.KOBOLD, Swatch.DbBrightWood);
            names.Add(EnemyTypes.GOBLIN, "Goblin");
            values.Add(EnemyTypes.GOBLIN, "g");
            enemyColors.Add(EnemyTypes.GOBLIN, Swatch.DbGrass);
        }

    }

}
