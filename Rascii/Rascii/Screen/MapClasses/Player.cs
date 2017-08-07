using Rascii.Constants;
using Rascii.Util;
using System;
using System.Collections.Generic;

namespace Rascii.Screen.MapClasses
{
    public class Player : Entity
    {

        Cell currentCell;
        string name = "Player";
        EntityStats stats;
        private bool dead = false;

        public Player(Cell cell)
        {
            value = "@";
            color = CellColors.PLAYER;
            entityType = EntityTypes.PLAYER;
            GameManager.player = this;
            currentCell = cell;
            currentCell.AddEntity(this);
            Messages messages = (Messages)GameManager.game.GetPane("messages").GetContent();
            messages.AddMessage(String.Format("Player spawned at {0}:{1}", cell.GetCoordinates().X, cell.GetCoordinates().Y));
            stats = new EntityStats(name);
            ResetStats();
        }

        public void ResetStats()
        {
            stats.level = 1;
            stats.currHealth = 10;
            stats.maxHealth = 10;
            stats.attack = 2;
            stats.attackChance = 50;
            stats.defenceChance = 50;
            stats.defence = 2;
            stats.gold = 0;
            stats.awareness = 8;
            stats.speed = 10;
        }
        
        public void LevelUp()
        {
            stats.level++;
            stats.attack++;
            stats.defence++;
            stats.maxHealth += 10;
            stats.currHealth++;
        }

        public Cell GetCell()
        {
            return currentCell;
        }

        public void SetCell(Cell cell)
        {
            currentCell.RemoveEntity();
            currentCell = cell;
            currentCell.AddEntity(this);
            if(currentCell.isExit)
            {
                Messages messages = (Messages)GameManager.game.GetPane("messages").GetContent();
                messages.AddMessage(String.Format("Press {0} to proceed to the next level, or continue exploring.", "."));
            } else if (currentCell.GetValue() == "<")
            {
                Messages messages = (Messages)GameManager.game.GetPane("messages").GetContent();
                messages.AddMessage(String.Format("You recently descended these stairs."));
            }
        }

        public bool CanSee(Cell cell)
        {
            if(Math.Abs(cell.GetCoordinates().X - currentCell.GetCoordinates().X) < stats.awareness && Math.Abs(cell.GetCoordinates().Y - currentCell.GetCoordinates().Y ) < stats.awareness) {
                return true;
            } else {
                return false;
            }
        }

        public void Hit(int damage)
        {
            if (stats.currHealth - damage > 0)
            {
                stats.currHealth -= damage;
            }
            else
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

        public int Defend()
        {
            int blocks = 0;

            for (int i = 0; i < stats.defence; i++)
            {
                if (Randomizer.RandomInt(0, 100) > stats.defenceChance)
                {
                    blocks += 1;
                }
            }

            return blocks;
        }

        public void Attack(Enemy e)
        {
            int hits = 0;
            int blocks = 0;

            string message = "";
            message += String.Format("{0} attacked {1} ", stats.name, e.GetStats().name);

            for(int i = 0; i < stats.attack; i++)
            {
                int roll = Randomizer.RandomInt(0, 100);

                if (roll > stats.attackChance)
                {
                    hits += 1;
                }
            }
            Messages messages = (Messages)GameManager.game.GetPane("messages").GetContent();

            if (hits > 0)
            {
                if (hits > 1)
                {
                    message += String.Format("scoring {0} hits!", hits);
                } else
                {
                    message += String.Format("scoring {0} hit!", hits);
                }

                messages.AddMessage(message);

                blocks = e.Defend(this);

                int successHits = hits - blocks;
                successHits = Math.Abs(successHits);

                messages.AddMessage(String.Format("{0} was hit {1} times!", e.GetStats().name, successHits));

                e.Hit(successHits);
            } else
            {
                message += "but missed!";
                messages.AddMessage(message);
            }
            
        }

        public EntityStats GetStats()
        {
            return this.stats;
        }

    }
}
