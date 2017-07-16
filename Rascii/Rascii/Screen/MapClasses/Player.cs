using Rascii.Constants;
using System;

namespace Rascii.Screen.MapClasses
{
    public class Player : Entity
    {

        Cell currentCell;
        string name = "Player";
        EntityStats stats;

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
            stats.currHealth = 100;
            stats.maxHealth = 100;
            stats.attack = 2;
            stats.attackChance = 50;
            stats.defenceChance = 50;
            stats.defence = 2;
            stats.gold = 0;
            stats.awareness = 8;
            stats.speed = 10;
        }
        

        public Cell GetCell()
        {
            return currentCell;
        }

        public void SetCell(Cell cell)
        {
            currentCell.RemoveEntity();
            currentCell = cell;
            cell.SetValue("@");
            cell.SetColor(CellColors.PLAYER);
        }

        public bool CanSee(Cell cell)
        {
            if(Math.Abs(cell.GetCoordinates().X - currentCell.GetCoordinates().X) < stats.awareness && Math.Abs(cell.GetCoordinates().Y - currentCell.GetCoordinates().Y ) < stats.awareness) {
                return true;
            } else {
                return false;
            }
        }

        public EntityStats GetStats()
        {
            return this.stats;
        }
    }
}
