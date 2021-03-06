﻿using Microsoft.Xna.Framework.Graphics;
using Rascii.Constants;
using Rascii.Util;
using Microsoft.Xna.Framework;
using Rascii.Screen.MapClasses;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Rascii.Screen
{
    public class Map : Content
    {
        int x, y;

        Cell[,] cells = new Cell[Project.mapWidth / Project.tileSize, Project.mapHeight / Project.tileSize];
        List<Cell> inView = new List<Cell>();
        Player player;

        private int maxRooms = 0;
        private int maxRoomSize = 0;
        private int minRoomSize = 0;

        private List<Room> rooms = new List<Room>();
        private List<Door> doors = new List<Door>();
        private bool playerTurn = true;

        private int buttonTimer = 0;
        
        Random random;

        KeyboardState lastState;

        public Map(int x, int y)
        {
            this.x = x;
            this.y = y;
            Random seedGetter = new Random();
            int seed = seedGetter.Next(0, 5000000);
            random = new Random(seed);
            maxRooms = 7;
            maxRoomSize = 20;
            minRoomSize = 10;

            rooms = new List<Room>();
            doors = new List<Door>();

            for(int i = 0; i < Project.mapWidth / Project.tileSize; i++)
            {
                for(int j = 0; j < Project.mapHeight / Project.tileSize; j++)
                {
                    cells[i, j] = new Cell(x + (i * Project.tileSize), y + (j * Project.tileSize), "#", CellColors.WALL, CellColors.WALLBACK, i, j);
                }
            }

            int createdRooms = 0;

            while(createdRooms < maxRooms)
            {
                int roomWidth = random.Next(minRoomSize, maxRoomSize);
                int roomHeight = random.Next(minRoomSize, maxRoomSize);
                int roomX = random.Next(0, Project.mapWidth / Project.tileSize);
                int roomY = random.Next(0, Project.mapHeight / Project.tileSize);

                Rectangle newRoom = new Rectangle(roomX, roomY, roomWidth, roomHeight);

                bool canAdd = true;

                if(roomX + roomWidth > Project.mapWidth / Project.tileSize || roomY + roomHeight > Project.mapHeight / Project.tileSize)
                {
                    canAdd = false;
                }

                if (canAdd)
                {
                    foreach (Room room in rooms)
                    {
                        if (newRoom.Intersects(room.GetBounds()))
                        {
                            canAdd = false;
                        }
                    }

                }
                
                if(canAdd)
                {
                    rooms.Add(new Room(newRoom, roomX, roomY, roomWidth, roomHeight));
                    createdRooms++;
                }
            }

            
            CreateRooms();
            
            foreach(Cell cell in cells)
            {
                cell.startValue = cell.GetValue();
            }

            Populate();
            Messages messages = (Messages)GameManager.game.GetPane("messages").GetContent();
            messages.AddMessage(String.Format("Map created with seed {0}", seed));

            player = new Player(cells[(int)rooms[0].Center().X, (int)rooms[0].Center().Y]);

            cells[(int)player.GetCell().GetCoordinates().X + 1, (int)player.GetCell().GetCoordinates().Y].SetValue("<");
            GameManager.level = this;
        }

        public Map(Player p, int x, int y)
        {
            this.x = x;
            this.y = y;
            Random seedGetter = new Random();
            int seed = seedGetter.Next(0, 5000000);
            random = new Random(seed);
            maxRooms = 7;
            maxRoomSize = 20;
            minRoomSize = 10;

            rooms = new List<Room>();
            doors = new List<Door>();

            for (int i = 0; i < Project.mapWidth / Project.tileSize; i++)
            {
                for (int j = 0; j < Project.mapHeight / Project.tileSize; j++)
                {
                    cells[i, j] = new Cell(x + (i * Project.tileSize), y + (j * Project.tileSize), "#", CellColors.WALL, CellColors.WALLBACK, i, j);
                }
            }

            int createdRooms = 0;

            while (createdRooms < maxRooms)
            {
                int roomWidth = random.Next(minRoomSize, maxRoomSize);
                int roomHeight = random.Next(minRoomSize, maxRoomSize);
                int roomX = random.Next(0, Project.mapWidth / Project.tileSize);
                int roomY = random.Next(0, Project.mapHeight / Project.tileSize);

                Rectangle newRoom = new Rectangle(roomX, roomY, roomWidth, roomHeight);

                bool canAdd = true;

                if (roomX + roomWidth > Project.mapWidth / Project.tileSize || roomY + roomHeight > Project.mapHeight / Project.tileSize)
                {
                    canAdd = false;
                }

                if (canAdd)
                {
                    foreach (Room room in rooms)
                    {
                        if (newRoom.Intersects(room.GetBounds()))
                        {
                            canAdd = false;
                        }
                    }

                }

                if (canAdd)
                {
                    rooms.Add(new Room(newRoom, roomX, roomY, roomWidth, roomHeight));
                    createdRooms++;
                }
                
            }


            CreateRooms();

            foreach (Cell cell in cells)
            {
                cell.startValue = cell.GetValue();
            }

            Populate();
            player = p;
            player.GetCell().RemoveEntity();
            GameManager.player = player;
            player.SetCell(cells[(int)rooms[0].Center().X, (int)rooms[0].Center().Y]);

            cells[(int)player.GetCell().GetCoordinates().X + 1, (int)player.GetCell().GetCoordinates().Y].SetValue("<");

            GameManager.level = this;
        }

        private void Populate()
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());

            foreach (Cell cell in cells)
            {
                if(!cell.HasEntity() && cell.GetWalkable())
                {
                    if(r.Next(0, 200) < 10)
                    {
                        int chance = r.Next(0, 100);
                        
                        int enemyType = r.Next(0, EnemyTypes.ENEMYTYPES);

                        if (chance <= ContentChest.Instance.enemyChance[enemyType] * GameManager.player.GetStats().level)
                        {
                            cell.AddEntity(new Enemy(GameManager.player.GetStats().level, enemyType));
                        }
                    }
                }
            }
        }

        public Cell[,] GetCells()
        {
            return cells;
        }

        private void CheckFOV()
        {
            foreach (Cell cell in cells)
            {
                cell.SetVisible(false);
            }

            double x, y;
            int i;

            // 360 rays around the location.
            for(i = 0; i < 360; i ++)
            {
                // Calculating the increment of both x and y.
                x = Math.Cos(i * Project.tileSize);
                y = Math.Sin(i * Project.tileSize);
                // Passing the values to calculate the FOV.
                DoFOV(x, y);
            }
        }

        private void DoFOV(double x, double y)
        {
            // i increment, so player can have an awareness variable, higher awareness means more blocks will be visible.
            int i;
            double oX, oY;
            // Get the cell X and Y of the players current cell.
            oX = player.GetCell().GetCoordinates().X * Project.tileSize + Project.tileSize / 2;
            oY = player.GetCell().GetCoordinates().Y * Project.tileSize + Project.tileSize / 2;

            // increment i and check up to the players awareness.
            for(i = 0; i < player.GetStats().awareness * Project.tileSize; i++)
            {
                if (oX > 0 && oY > 0 && oX < Project.mapWidth && oY < Project.mapHeight)
                {
                    // set the cell to visible first.
                    cells[(int)oX / Project.tileSize, (int)oY / Project.tileSize].SetVisible(true);
                    // if the cell is a wall, we need to return and stop the checks.
                    Cell c = cells[(int)oX / Project.tileSize, (int)oY / Project.tileSize];
                    if (c.GetValue() == "#" || c.GetEntityType() == "+")
                    {
                        return;
                    }
                    // else we will go to the next block to check.
                    oX += x;
                    oY += y;
                }
            }
        }

        private void AddDoors()
        {
            foreach(Room room in rooms)
            {
                int roomLeft = room.Left();
                int roomRight = room.Right();
                int roomTop = room.Top();
                int roomBottom = room.Bottom();

                List<Cell> edgeCells = new List<Cell>();

                for(int i = roomLeft; i < roomRight; i++)
                {
                    edgeCells.Add(cells[i, roomTop]);
                    edgeCells.Add(cells[i, roomBottom]);
                }
                for(int i = roomTop; i < roomBottom; i++)
                {
                    edgeCells.Add(cells[roomLeft, i]);
                    edgeCells.Add(cells[roomRight, i]);
                }

                foreach(Cell cell in edgeCells)
                {
                    if (cell.GetWalkable())
                    {
                        Cell leftCell = cells[(int)cell.GetCoordinates().X - 1, (int)cell.GetCoordinates().Y];
                        Cell rightCell = cells[(int)cell.GetCoordinates().X + 1, (int)cell.GetCoordinates().Y];
                        Cell topCell = cells[(int)cell.GetCoordinates().X, (int)cell.GetCoordinates().Y - 1];
                        Cell bottomCell = cells[(int)cell.GetCoordinates().X, (int)cell.GetCoordinates().Y + 1];
                        if (!leftCell.GetWalkable() && !rightCell.GetWalkable())
                        {
                            doors.Add(new Door(cell));
                        } else if(!topCell.GetWalkable() && !bottomCell.GetWalkable())
                        {
                            doors.Add(new Door(cell));
                        }
                    }
                }
            }

            AddExit();
        }


        private void AddExit()
        {
            int ranRoom = 0;

            while(rooms[ranRoom].Contains(player.GetCell()))
            {
                ranRoom = Randomizer.RandomInt(0, rooms.Count);
            }

            Cell c = cells[(int)rooms[ranRoom].Center().X, (int)rooms[ranRoom].Center().Y];
            new Exit(c);
        }

        private void CreateRooms()
        {
            foreach (Room room in rooms)
            {
                for (int i = room.x; i < room.x + room.width; i++)
                {
                    for (int j = room.y; j < room.y + room.height; j++)
                    {
                        // We change every tile of the room in to the correct value, the surrounding tiles are always walls. Kinda uneccesary, because they are anyway.
                        if (i == room.x || j == room.y || i == room.x + room.width - 1 || j == room.y + room.height - 1)
                        {
                            cells[i, j].SetValue("#");
                            cells[i, j].SetColor(CellColors.WALL);
                            cells[i, j].SetBackColor(CellColors.WALLBACK);
                        }
                        else
                        {
                            cells[i, j].SetValue(".");
                            cells[i, j].SetColor(CellColors.FLOOR);
                            cells[i, j].SetBackColor(CellColors.FLOORBACK);
                        }
                    }
                }
            }

            ConnectRooms();
            // Need to spawn the player after map generation, else sometimes the halls overwrite the player.
            player = new Player(cells[(int)rooms[0].Center().X, (int)rooms[0].Center().Y]);
            AddDoors();
        }

        private void ConnectRooms()
        {

            for(int r = 1; r <= rooms.Count - 1; r++)
            {
                int lastRoomCenterX = (int)rooms[r - 1].Center().X;
                int lastRoomCenterY = (int)rooms[r - 1].Center().Y;
                int newRoomCenterX = (int)rooms[r].Center().X;
                int newRoomCenterY = (int)rooms[r].Center().Y;

                if(random.Next(1, 2) == 1)
                {
                    CreateHorizontalHall(lastRoomCenterX, newRoomCenterX, lastRoomCenterY);
                    CreateVerticalHall(lastRoomCenterY, newRoomCenterY, newRoomCenterX);
                } else
                {
                    CreateVerticalHall(lastRoomCenterY, newRoomCenterY, lastRoomCenterX);
                    CreateHorizontalHall(lastRoomCenterX, newRoomCenterX, newRoomCenterY);
                }
            }
            
        }

        private void CreateHorizontalHall(int startX, int endX, int yPos)
        {
            for(int x = Math.Min(startX, endX); x <= Math.Max(startX, endX); x++)
            {
                cells[x, yPos].SetValue(".");
                cells[x, yPos].SetColor(CellColors.FLOOR);
                cells[x, yPos].SetBackColor(CellColors.FLOORBACK);
            }
        }

        private void CreateVerticalHall(int startY, int endY, int xPos)
        {
            for (int y = Math.Min(startY, endY); y <= Math.Max(startY, endY); y++)
            {
                cells[xPos, y].SetValue(".");
                cells[xPos, y].SetColor(CellColors.FLOOR);
                cells[xPos, y].SetBackColor(CellColors.FLOORBACK);
            }
        }

        public override void Update()
        {
            base.Update();
            if (playerTurn)
            {
                CheckKeys();
            }

            if (!playerTurn)
            {
                foreach (Cell cell in cells)
                {
                    cell.Update();
                }
                playerTurn = true;
                GameManager.turn++;
                foreach(Cell cell in cells)
                {
                    cell.Reset();
                }
            }

            CheckFOV();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            int i = 1;
            foreach(Cell cell in cells)
            {
                if (cell.GetVisible())
                {
                    if(cell.GetEntity() != null)
                    {
                        Entity e = cell.GetEntity();
                        if(e.entityType == EntityTypes.ENEMY)
                        {
                            Enemy enemy = (Enemy)e;

                            float remainingWidth = Project.statWidth - (int)(Project.panePadding * 2 + ContentChest.Instance.gamefont.MeasureString(String.Format("{0}:", enemy.value)).X);
                            float actualWidth = ((float)enemy.GetStats().currHealth / (float)enemy.GetStats().maxHealth) * remainingWidth;
                            // Draw value.
                            spriteBatch.DrawString(ContentChest.Instance.gamefont, String.Format("{0}:", enemy.value), new Vector2(GameManager.statPanel.x + Project.panePadding, GameManager.statPanel.y + 200 + (i * (ContentChest.Instance.gamefont.MeasureString(enemy.GetStats().name).Y + Project.linePadding))), enemy.color);

                            // Draw health bar.
                            spriteBatch.Draw(ContentChest.Instance.pixel, new Rectangle((int)(GameManager.statPanel.x + (Project.panePadding * 2) + ContentChest.Instance.gamefont.MeasureString(String.Format("{0}:", enemy.value)).X),
                                (int)(GameManager.statPanel.y + 200 + (i * (ContentChest.Instance.gamefont.MeasureString(enemy.GetStats().name).Y + Project.linePadding))), (int)actualWidth, 
                                (int)ContentChest.Instance.gamefont.MeasureString(enemy.GetStats().name).Y), Swatch.PrimaryDarkest);

                            // Draw name on top of health bar.
                            spriteBatch.DrawString(ContentChest.Instance.gamefont, String.Format("{0}", enemy.GetStats().name),
                                new Vector2(GameManager.statPanel.x + (Project.panePadding * 2) + ContentChest.Instance.gamefont.MeasureString(String.Format("{0}:", enemy.value)).X, 
                                GameManager.statPanel.y + 200 + (i * (ContentChest.Instance.gamefont.MeasureString(enemy.GetStats().name).Y + Project.linePadding))), Color.White);
                            i++;
                        }

                        spriteBatch.Draw(ContentChest.Instance.pixel, new Rectangle((int)cell.GetPosition().X, (int)cell.GetPosition().Y, Project.tileSize, Project.tileSize), cell.GetBackColor());
                        spriteBatch.DrawString(ContentChest.Instance.gamefont, e.value, cell.GetPosition(), e.color);

                    } else
                    {
                        spriteBatch.Draw(ContentChest.Instance.pixel, new Rectangle((int)cell.GetPosition().X, (int)cell.GetPosition().Y, Project.tileSize, Project.tileSize), cell.GetBackColor());
                        spriteBatch.DrawString(ContentChest.Instance.gamefont, cell.GetValue(), cell.GetPosition(), cell.GetColor());

                    }
                } else if (cell.GetBeenVisible())
                {
                    spriteBatch.Draw(ContentChest.Instance.pixel, new Rectangle((int)cell.GetPosition().X, (int)cell.GetPosition().Y, Project.tileSize, Project.tileSize), Color.Black);
                    spriteBatch.DrawString(ContentChest.Instance.gamefont, cell.GetValue(), cell.GetPosition(), cell.GetColor());
                }
            }
        }

        private void CheckKeys()
        {
            KeyboardState keyState = Keyboard.GetState();

            Cell playersCell = player.GetCell();
            if (keyState.IsKeyDown(KeyBindings.LEFT) && (lastState.IsKeyUp(KeyBindings.LEFT) || buttonTimer >= Project.buttonWaitTime))
            {
                Cell cell = cells[(int)playersCell.GetCoordinates().X - 1, (int)playersCell.GetCoordinates().Y];
                if (cell.GetWalkable())
                {
                    player.SetCell(cells[(int)playersCell.GetCoordinates().X - 1, (int)playersCell.GetCoordinates().Y]);
                }
                else if (cell.GetEntity() != null)
                {
                    if (cell.GetEntity().entityType == EntityTypes.ENEMY)
                    {
                        Enemy enemy = (Enemy)cell.GetEntity();
                        player.Attack(enemy);
                    } else if (cell.GetEntity().entityType == EntityTypes.DOOR)
                    {
                        Door door = (Door)cell.GetEntity();
                        door.OpenDoor();
                    }
                }
                playerTurn = false;
                buttonTimer = 0;
            }
            else if (keyState.IsKeyDown(KeyBindings.RIGHT) && (lastState.IsKeyUp(KeyBindings.RIGHT) || buttonTimer >= Project.buttonWaitTime))
            {
                Cell cell = cells[(int)playersCell.GetCoordinates().X + 1, (int)playersCell.GetCoordinates().Y];
                if (cell.GetWalkable())
                {
                    player.SetCell(cells[(int)playersCell.GetCoordinates().X + 1, (int)playersCell.GetCoordinates().Y]);
                }
                else if (cell.GetEntity() != null)
                {
                    if (cell.GetEntity().entityType == EntityTypes.ENEMY)
                    {
                        Enemy enemy = (Enemy)cell.GetEntity();
                        player.Attack(enemy);
                    }
                    else if (cell.GetEntity().entityType == EntityTypes.DOOR)
                    {
                        Door door = (Door)cell.GetEntity();
                        door.OpenDoor();
                    }
                }
                playerTurn = false;
                buttonTimer = 0;
            }
            else if (keyState.IsKeyDown(KeyBindings.UP) && (lastState.IsKeyUp(KeyBindings.UP) || buttonTimer >= Project.buttonWaitTime))
            {
                Cell cell = cells[(int)playersCell.GetCoordinates().X, (int)playersCell.GetCoordinates().Y - 1];
                if (cell.GetWalkable())
                {
                    player.SetCell(cells[(int)playersCell.GetCoordinates().X, (int)playersCell.GetCoordinates().Y - 1]);
                }
                else if (cell.GetEntity() != null)
                {
                    if (cell.GetEntity().entityType == EntityTypes.ENEMY)
                    {
                        Enemy enemy = (Enemy)cell.GetEntity();
                        player.Attack(enemy);
                    }
                    else if (cell.GetEntity().entityType == EntityTypes.DOOR)
                    {
                        Door door = (Door)cell.GetEntity();
                        door.OpenDoor();
                    }
                }
                playerTurn = false;
                buttonTimer = 0;
            }
            else if (keyState.IsKeyDown(KeyBindings.DOWN) && (lastState.IsKeyUp(KeyBindings.DOWN) || buttonTimer >= Project.buttonWaitTime))
            {
                Cell cell = cells[(int)playersCell.GetCoordinates().X, (int)playersCell.GetCoordinates().Y + 1];
                if (cell.GetWalkable())
                {
                    player.SetCell(cells[(int)playersCell.GetCoordinates().X, (int)playersCell.GetCoordinates().Y + 1]);
                }
                else if (cell.GetEntity() != null)
                {
                    if (cell.GetEntity().entityType == EntityTypes.ENEMY)
                    {
                        Enemy enemy = (Enemy)cell.GetEntity();
                        player.Attack(enemy);
                    }
                    else if (cell.GetEntity().entityType == EntityTypes.DOOR)
                    {
                        Door door = (Door)cell.GetEntity();
                        door.OpenDoor();
                    }
                }
                playerTurn = false;
                buttonTimer = 0;
            }
            else if (keyState.IsKeyDown(KeyBindings.SKIP) && (lastState.IsKeyUp(KeyBindings.SKIP) || buttonTimer >= Project.buttonWaitTime))
            {
                playerTurn = false;
                buttonTimer = 0;
            }

            if (keyState.IsKeyDown(KeyBindings.PROCEED) && lastState.IsKeyUp(KeyBindings.PROCEED))
            {
                if (player.GetCell().GetValue() == ">")
                {
                    player.LevelUp();
                    GameManager.game.GetPane("map").SetContent(new Map(player, x, y));
                }
                buttonTimer = 0;
            }

            buttonTimer++;

            lastState = keyState;
        }

        public Player GetPlayer()
        {
            return player;
        }

    }
}
