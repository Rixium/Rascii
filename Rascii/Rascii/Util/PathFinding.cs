using Rascii.Constants;
using Rascii.Screen;
using Rascii.Screen.MapClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rascii.Util
{
    public class PathFinding
    {

        private List<Cell> open;
        private List<Cell> closed;
        private List<Cell> path;

        public PathFinding()
        {

        }

        public List<Cell> FindPath(Cell start, Cell end)
        {
            // New list of cells to check.
            open = new List<Cell>();
            // List of cells that have been checked.
            closed = new List<Cell>();
            // Path to return.
            path = new List<Cell>();
            // Store current cell getting checked.
            Cell currentCell;

            // Adding start to the open list.
            open.Add(start);

            // Setting the parent of start.
            start.parent = start;
            // Setting the H, G and F of start.
            Compute(start, end);

            // Iterate until open is empty.
            while(open.Count > 0)
            {
                // Set the current cell to the first value of the open list.
                currentCell = open[0];

                // Remove the cell from the open list.
                open.Remove(currentCell);
                // Add the cell to the closed list as its been checked.
                closed.Add(currentCell);

                // Check if closed contains the destination.
                if (closed.Contains(end))
                {
                    // Set the cell to the end's parent. Which will be the cell next to the destination as we don't want 
                    // to walk on players spot.
                    Cell c = end.parent;

                    // Add the cell to the path until its equal to the start tile.
                    while(c != start)
                    {
                        // Add the cell.
                        path.Add(c);
                        // Set the new cell to check to the parent of this cell.
                        // We are building the path backwards.
                        c = c.parent;
                    }

                    // We need to reverse the path as it has been built backwards.
                    path.Reverse();
                    // Break out of the while loop.
                    break;
                }

                
                // Create a list of neighbours that are walkable, from the current cell.
                // We also want to add the end cell to the neighbours list so we must check that
                // we dont remove that. We could use end.parent to check for end too but I'd rather
                // do it this way.

                List<Cell> neighbours = GetNeighbours(currentCell, end);

                // Iterate through each neighbour.
                foreach(Cell c in neighbours)
                {

                    // Check if the neighbour is on the closed list.
                    // If so we can break out of this and continue to the
                    // next neighbour.
                    if(closed.Contains(c))
                    {
                        continue;
                    }
                    
                    // If the neighbour isn't already on the open list.
                    if(!open.Contains(c))
                    {
                        // We set the parent of the neighbour to the current cell.
                        c.parent = currentCell;
                        // We calculate the new values.
                        Compute(c, end);
                        // We add it to the open list, so we can check it next time.
                        open.Add(c);
                    } else
                    {
                        // We check if the current cells fValue + 1 is less the neighbours fValue.
                        // If so we can change the fValue as we know this path is shorter than the last.
                        if(currentCell.fValue + 1 <= c.fValue)
                        {
                            // Set the parent of the neighbour cell to this cell. Building a path this way.
                            c.parent = currentCell;

                            // We need to compute the values again, with the new parent.
                            Compute(c, end);
                        }
                    }
                }

                // We sort the f value so we can find the lowest one in the next iteration.
                open.Sort((c1, c2) => c1.fValue.CompareTo(c2.fValue));
            }

            return path;
        }


        private void Compute(Cell c, Cell end)
        {
            // F = total of G and H;
            // H = Distance from End;
            // G = Distance from Start;
            c.gValue = c.parent.gValue + 1;
            c.hValue = GetDistance(c, end);
            c.fValue = c.gValue + c.hValue;
        }

        private int GetDistance(Cell c, Cell e)
        {

            int dx = Math.Abs((int)c.GetCoordinates().X - (int)e.GetCoordinates().X);
            int dy = Math.Abs((int)c.GetCoordinates().Y - (int)e.GetCoordinates().Y);

            if (dx > dy)
                return (2000 * dy + 1 * (dx - dy));
            return (2000 * dx + 1 * (dy - dx));
        }

        private List<Cell> GetNeighbours(Cell cell, Cell end)
        {
            Cell[,] cells = GameManager.level.GetCells();
            List<Cell> neighbours = new List<Cell>();

            neighbours.Add(cells[(int)cell.GetCoordinates().X - 1, (int)cell.GetCoordinates().Y]);
            neighbours.Add(cells[(int)cell.GetCoordinates().X + 1, (int)cell.GetCoordinates().Y]);
            neighbours.Add(cells[(int)cell.GetCoordinates().X, (int)cell.GetCoordinates().Y - 1]);
            neighbours.Add(cells[(int)cell.GetCoordinates().X, (int)cell.GetCoordinates().Y + 1]);

            foreach(Cell c in new List<Cell>(neighbours))
            {
                
                if(!c.GetWalkable() && c != end)
                {
                    neighbours.Remove(c);
                }
            }

            return neighbours;
        }
    }
}
