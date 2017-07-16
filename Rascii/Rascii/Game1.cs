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
        Dictionary<string, Pane> panes = new Dictionary<string, Pane>();

        public Game1()
        {
            GameManager.game = this;
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

            
            panes.Add("none", new Pane(Project.screenWidth, Project.screenHeight, 0, 0, Color.Black, "", null));
            panes.Add("inventory", new Pane(Project.inventoryWidth, Project.inventoryHeight, Project.statWidth, 0, Color.Black, "Inventory", new Inventory()));
            
            panes.Add("messages", new Pane(Project.messageWidth, Project.messageHeight, Project.statWidth, Project.inventoryHeight + Project.mapHeight, Color.Black, "Messages", new Messages(Project.statWidth, Project.inventoryHeight + Project.mapHeight, Project.messageHeight)));
            Map map = new Map(Project.statWidth, Project.inventoryHeight);
            panes.Add("stats", new Pane(Project.statWidth, Project.statHeight, 0, 0, Color.Black, "Stats", new Stats(0, 0, map.GetPlayer())));
            panes.Add("map", new Pane(Project.mapWidth, Project.mapHeight, Project.statWidth, Project.inventoryHeight, Color.Black, "Map", map));
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public Pane GetPane(string pane)
        {
            return panes[pane];
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<string, Pane> pane in panes)
            {
                pane.Value.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            foreach (KeyValuePair<string, Pane> pane in panes)
            {
                pane.Value.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
