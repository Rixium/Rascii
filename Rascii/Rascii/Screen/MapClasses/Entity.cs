using Microsoft.Xna.Framework;

namespace Rascii.Screen.MapClasses
{

    public class Entity
    {

        public int entityType;
        public Color color;
        public string value;

        public virtual void Update(Cell cell)
        {

        }

    }

}
