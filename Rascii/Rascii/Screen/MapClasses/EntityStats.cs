using Rascii.Util;

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

        public void Roll()
        {
            attack = Dice.Roll(1, 3) + level / 3;
            attackChance = Dice.Roll(25, 3);
            awareness = 10;
            defence = Dice.Roll(1, 3) + level / 3;
            defenceChance = Dice.Roll(10, 4);
            gold = Dice.Roll(5, 5);
            currHealth = 10;
            maxHealth = 10;
            speed = 14;
        }
    }
}
