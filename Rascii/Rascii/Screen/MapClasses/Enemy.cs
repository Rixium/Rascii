using Rascii.Constants;
using Rascii.Util;

namespace Rascii.Screen.MapClasses
{
    class Enemy : Entity
    {

        EntityStats stats;

        public Enemy(int level, int enemyType)
        {
            entityType = EntityTypes.ENEMY;
            stats = new EntityStats(ContentChest.Instance.names[enemyType]);
            value = ContentChest.Instance.values[enemyType];
            color = ContentChest.Instance.enemyColors[enemyType];
            stats.level = level;
            stats.Roll();
        }

        public EntityStats GetStats()
        {
            return this.stats;
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
