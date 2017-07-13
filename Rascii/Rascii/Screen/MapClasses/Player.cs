using Rascii.Constants;
using System;

namespace Rascii.Screen.MapClasses
{
    class Player
    {

        Cell currentCell;

        public string name = "Player";
        public int level = 1;
        public int currHealth = 10;
        public int maxHealth = 10;
        public int attack = 1;
        public int defence = 1;
        public int gold = 0;
        public int awareness = 8;


        public Player(Cell cell)
        {
            this.currentCell = cell;
            currentCell.SetValue("@");
            currentCell.SetColor(CellColors.PLAYER);
        }
        

        public Cell GetCell()
        {
            return currentCell;
        }

        public void SetCell(Cell cell)
        {
            this.currentCell.RemoveEntity();
            this.currentCell = cell;
            cell.SetValue("@");
            cell.SetColor(CellColors.PLAYER);
        }

        public bool CanSee(Cell cell)
        {
            if(Math.Abs(cell.GetCoordinates().X - currentCell.GetCoordinates().X) < awareness && Math.Abs(cell.GetCoordinates().Y - currentCell.GetCoordinates().Y ) < awareness) {
                return true;
            } else {
                return false;
            }
        }
    }
}
