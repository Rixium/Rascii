
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rascii.Util {

    public class ContentChest {

        public ContentManager content { get; set; }

        private static ContentChest instance;

        public Texture2D pixel;
        public SpriteFont gamefont;

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
        }

    }

}
