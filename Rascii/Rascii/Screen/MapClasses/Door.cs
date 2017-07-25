using Rascii.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rascii.Screen.MapClasses
{
    class Door : Entity
    {

        private Cell currentCell;
        private bool open = false;

        public Door(Cell cell)
        {
            value = "+";
            color = CellColors.DOOR;
            entityType = EntityTypes.DOOR;
            currentCell = cell;
            currentCell.AddEntity(this);
        }

        public override void Update(Cell cell)
        {
            base.Update(cell);
        }

        public void OpenDoor()
        {
            if(open)
            {
                open = false;
                currentCell.SetWalkable(false);
                value = "+";
            } else
            {
                open = true;
                currentCell.SetWalkable(true);
                value = "-";
            }
        }

        public bool GetOpen()
        {
            return open;
        }

    }
}
