using Rascii.Constants;
using Rascii.Screen.MapClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rascii.Screen
{
    class Exit : Entity
    {

        private Cell currentCell;

        public Exit(Cell cell)
        {
            value = ">";
            color = CellColors.EXIT;
            entityType = EntityTypes.EXIT;
            currentCell = cell;
            currentCell.AddEntity(this);
            currentCell.SetWalkable(true);
            currentCell.isExit = true;
        }
    }
}
