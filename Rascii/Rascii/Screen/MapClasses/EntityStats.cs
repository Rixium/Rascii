namespace Rascii.Screen.MapClasses
{

    public class EntityStats
    {
        public string name = "";
        public int level = 1;
        public int currHealth = 10;
        public int maxHealth = 10;
        public int attack = 1;
        public int attackChance = 1;
        public int defenceChance = 1;
        public int defence = 1;
        public int gold = 0;
        public int awareness = 8;
        public int speed = 1;

        public EntityStats(string name)
        {
            this.name = name;
        }
    }
}
