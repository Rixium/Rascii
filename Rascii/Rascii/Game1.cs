using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rascii.Constants;
using Rascii.Screen;
using Rascii.Util;
using System;
using System.Collections.Generic;

namespace Rascii
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentChest contentChest;
        List<Pane> panes = new List<Pane>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Window.Title = String.Format("{0} v{1}", Project.title, Project.version);

            contentChest = ContentChest.Instance;
            contentChest.content = Content;
            contentChest.LoadContent();

            graphics.PreferredBackBufferWidth = Project.screenWidth;
            graphics.PreferredBackBufferHeight = Project.screenHeight;
            graphics.ApplyChanges();

            Map map = new Map(0, Project.inventoryHeight);
            panes.Add(new Pane(Project.screenWidth, Project.screenHeight, 0, 0, Color.Black, "", null));
            panes.Add(new Pane(Project.inventoryWidth, Project.inventoryHeight, 0, 0, Color.Black, "Inventory", new Inventory()));

            panes.Add(new Pane(Project.mapWidth, Project.mapHeight, 0, Project.inventoryHeight, Color.Black, "Map", map));
            panes.Add(new Pane(Project.statWidth, Project.statHeight, Project.inventoryWidth, 0, Color.Black, "Stats", new Stats(Project.inventoryWidth, 0, map.GetPlayer())));
            panes.Add(new Pane(Project.messageWidth, Project.messageWidth, 0, Project.inventoryHeight + Project.mapHeight, Color.Black, "Messages", new Messages()));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            foreach (Pane pane in panes)
            {
                pane.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            foreach (Pane pane in panes)
            {
                pane.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
