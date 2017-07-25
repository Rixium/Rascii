using Microsoft.Xna.Framework;
using Rascii.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rascii.Screen.MapClasses
{
    public class Cell
    {

        String value;
        String oldValue;
        int x, xCord;
        int y, yCord;
        Vector2 pos;
        Color color, oldColor;
        Color backColor, oldBackColor;
        bool visible = false;
        bool beenVisible = false;
        bool walkable;
        Entity entity;

        // Pathfinding
        public int fValue = 0;
        public int hValue = 0;
        public int gValue = 0;
        public bool open = true;
        public Cell parent;
        public string startValue;

        public bool isExit = false;

        public bool canUpdate;

        public Cell(int x, int y, string value, Color color, Color backcolor, int xCord, int yCord)
        {
            this.x = x;
            this.y = y;
            this.value = value;
            startValue = value;
            this.pos = new Vector2(x, y);
            this.color = color;
            this.backColor = backcolor;
            this.xCord = xCord;
            this.yCord = yCord;
        }

        public string GetValue()
        {
            return value;
        }

        public Vector2 GetPosition()
        {
            return pos;
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, Project.tileSize, Project.tileSize);
        }

        public Color GetColor()
        {
            return color;
        }
        
        public Color GetBackColor()
        {
            return backColor;
        }

        public Vector2 GetCoordinates()
        {
            return new Vector2(xCord, yCord);
        }

        public void SetValue(string value)
        {
            this.oldValue = this.value;
            this.value = value;

            if(this.value == ".")
            {
                walkable = true;
            }
        }

        public void Reset()
        {
            if(entity != null)
            {
                if(entity.entityType == EntityTypes.ENEMY)
                {
                    Enemy enemy = (Enemy)entity;
                    enemy.hasMoved = false;
                }
            }
        }
        public void Update()
        {
            if(entity != null)
            {
                if (visible)
                {
                    entity.Update(this);

                    if (entity != null)
                    {
                        if (entity.entityType == EntityTypes.ENEMY)
                        {
                            Enemy enemy = (Enemy)entity;
                            if (enemy.dead)
                            {
                                RemoveEntity();
                            }
                        }
                    }
                }
            }
        }

        public void SetVisible(bool visible)
        {
            this.visible = visible;
            if(visible)
            {
                beenVisible = true;
            }
        }

        public bool GetBeenVisible()
        {
            return beenVisible;
        }

        public void ShowPath()
        {
            this.color = Color.Red;
            this.backColor = Color.Red;
        }

        public void RemoveEntity()
        {
            this.entity = null;
            this.walkable = true;
        }

        public bool HasEntity()
        {
            if(entity != null)
            {
                return true;
            } else
            {
                if (walkable)
                {
                    return false;
                } else
                {
                    return true;
                }
            }
        }

        public Entity GetEntity()
        {
            return entity;
        }

        public string GetEntityType()
        {
            if(entity != null)
            {
                if(entity.entityType == EntityTypes.DOOR)
                {
                    Door d = (Door)entity;
                    if(!d.GetOpen())
                    {
                        return "+";
                    }
                }
            }
            return "";
        }

        public void AddEntity(Entity e)
        {
            if(e.entityType == EntityTypes.DOOR)
            {
                oldValue = e.value;
                value = e.value;
                color = e.color;
            } else if (entity != null)
            {
                if (entity.entityType == EntityTypes.DOOR)
                {
                    Door d = (Door)entity;
                    if(d.GetOpen())
                    {
                        oldValue = d.value;
                        value = d.value;
                    }
                } else if (entity.entityType == EntityTypes.EXIT)
                {
                    Exit ex = (Exit)entity;
                    oldValue = ex.value;
                    value = ex.value;
                    isExit = true;
                }
            }

            entity = e;
            
            walkable = false;
        }

        public void SetColor(Color color)
        {
            this.oldColor = this.color;
            this.color = color;
        }

        public void SetBackColor(Color color)
        {
            this.oldBackColor = this.backColor;
            this.backColor = color;
        }

        public void SetWalkable(bool w)
        {
            this.walkable = w;
        }

        public bool GetVisible()
        {
            return visible;
        }

        public bool GetWalkable()
        {
            return walkable;
        }

    }
}
