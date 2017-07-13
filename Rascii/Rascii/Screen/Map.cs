using Microsoft.Xna.Framework.Graphics;
using Rascii.Constants;
using Rascii.Util;
using Microsoft.Xna.Framework;
using Rascii.Screen.MapClasses;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Rascii.Screen
{
    class Map : Content
    {
        int x, y;

        Cell[,] cells = new Cell[Project.mapWidth / Project.tileSize, Project.mapHeight / Project.tileSize];
        List<Cell> inView = new List<Cell>();
        Player player;

        private int maxRooms = 0;
        private int maxRoomSize = 0;
        private int minRoomSize = 0;

        private List<Room> rooms = new List<Room>();

        Random random = new Random();

        KeyboardState lastState;

        public Map(int x, int y)
        {
            this.x = x;
            this.y = y;

            maxRooms = 20;
            maxRoomSize = 15;
            minRoomSize = 5;

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
                    Console.WriteLine("Creating Room");
                    createdRooms++;
                }
            }

            CreateRooms();
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
            oX = player.GetCell().GetCoordinates().X;
            oY = player.GetCell().GetCoordinates().Y;

            // increment i and check up to the players awareness.
            for(i = 0; i < player.awareness; i++)
            {
                if (oX > 0 && oY > 0 && oX < Project.mapWidth / Project.tileSize && oY < Project.mapHeight / Project.tileSize)
                {
                    // set the cell to visible first.
                    cells[(int)oX, (int)oY].SetVisible(true);
                    // if the cell is a wall, we need to return and stop the checks.
                    if (cells[(int)oX, (int)oY].GetValue() == "#")
                    {
                        return;
                    }
                    // else we will go to the next block to check.
                    oX += x;
                    oY += y;
                }
            }
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
            CheckKeys();
            CheckFOV();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach(Cell cell in cells)
            {
                if (cell.GetVisible())
                {
                    spriteBatch.Draw(ContentChest.Instance.pixel, new Rectangle((int)cell.GetPosition().X, (int)cell.GetPosition().Y, Project.tileSize, Project.tileSize), cell.GetBackColor());
                    spriteBatch.DrawString(ContentChest.Instance.gamefont, cell.GetValue(), cell.GetPosition(), cell.GetColor());
                } else if (cell.GetBeenVisible())
                {
                    spriteBatch.Draw(ContentChest.Instance.pixel, new Rectangle((int)cell.GetPosition().X, (int)cell.GetPosition().Y, Project.tileSize, Project.tileSize), Color.White * 0.1f);
                }
            }
        }

        private void CheckKeys()
        {
            KeyboardState keyState = Keyboard.GetState();

            Cell playersCell = player.GetCell();
            if(keyState.IsKeyDown(KeyBindings.LEFT) && lastState.IsKeyUp(KeyBindings.LEFT))
            {
                if (cells[(int)playersCell.GetCoordinates().X - 1, (int)playersCell.GetCoordinates().Y].GetValue() == ".") {
                    player.SetCell(cells[(int)playersCell.GetCoordinates().X - 1, (int)playersCell.GetCoordinates().Y]);
                }
            } else if (keyState.IsKeyDown(KeyBindings.RIGHT) && lastState.IsKeyUp(KeyBindings.RIGHT))
            {
                if (cells[(int)playersCell.GetCoordinates().X + 1, (int)playersCell.GetCoordinates().Y].GetValue() == ".")
                {
                    player.SetCell(cells[(int)playersCell.GetCoordinates().X + 1, (int)playersCell.GetCoordinates().Y]);
                }
            } else if(keyState.IsKeyDown(KeyBindings.UP) && lastState.IsKeyUp(KeyBindings.UP))
            {
                if (cells[(int)playersCell.GetCoordinates().X, (int)playersCell.GetCoordinates().Y - 1].GetValue() == ".")
                {
                    player.SetCell(cells[(int)playersCell.GetCoordinates().X, (int)playersCell.GetCoordinates().Y - 1]);
                }
            } else if (keyState.IsKeyDown(KeyBindings.DOWN) && lastState.IsKeyUp(KeyBindings.DOWN))
            {
                if (cells[(int)playersCell.GetCoordinates().X, (int)playersCell.GetCoordinates().Y + 1].GetValue() == ".")
                {
                    player.SetCell(cells[(int)playersCell.GetCoordinates().X, (int)playersCell.GetCoordinates().Y + 1]);
                }
            }

            lastState = keyState;
        }

        public Player GetPlayer()
        {
            return player;
        }

    }
}
