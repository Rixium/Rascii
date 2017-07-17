using Rascii.Constants;
using Rascii.Util;
using System;
using System.Collections.Generic;

namespace Rascii.Screen.MapClasses
{
    public class Enemy : Entity
    {

        EntityStats stats;
        public bool dead;
        public bool hasMoved = false;

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

        public int Defend(Player p)
        {
            int blocks = 0;

            for (int i = 0; i < stats.defence; i++)
            {
                if (Randomizer.RandomInt(0, 100) > stats.defenceChance)
                {
                    blocks += 1;
                }
            }

            string message = "";
            message += String.Format(" {0} defended ", stats.name);
            message += String.Format("{0} hits", blocks);
            Messages messages = (Messages)GameManager.game.GetPane("messages").GetContent();
            messages.AddMessage(message);

            return blocks;
        }

        public void Hit(int damage)
        {
            if(stats.currHealth - damage > 0)
            {
                stats.currHealth -= damage;
            } else
            {
                if (!dead)
                {
                    stats.currHealth = 0;
                    dead = true;
                    Messages messages = (Messages)GameManager.game.GetPane("messages").GetContent();
                    messages.AddMessage(String.Format("{0} has died!", stats.name));
                }
            }
        }

        public override void Update(Cell cell)
        {
            base.Update(cell);

            if (!hasMoved)
            {
                PathFinding pathFinder = new PathFinding();
                List<Cell> cells = pathFinder.FindPath(cell, GameManager.player.GetCell());
                if (cells.Count > 0)
                {
                    if (cells[0].GetWalkable() && cells[0].GetEntity() == null)
                    {
                        cell.RemoveEntity();
                        cells[0].AddEntity(this);
                    }
                }
                hasMoved = true;
            }
        }
    }
}
