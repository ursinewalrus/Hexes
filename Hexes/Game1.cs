using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Hexes
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        HexGrid HexMap;
        int GameHeight;
        int GameWidth;

        bool HasBeenResized = false;
        bool RedrawAll = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameWidth = GraphicsDevice.DisplayMode.Width / 3;
            graphics.PreferredBackBufferWidth = GameWidth;

            GameHeight = GraphicsDevice.DisplayMode.Height / 3;
            graphics.PreferredBackBufferHeight = GameHeight;

            //Line.Sb = spriteBatch;
            //Hex.gameWidth = GameWidth;

            //HexMap = new HexGrid(7, 7);
            //var t = HexMap.HexStorage[0, 0];
            //t.DrawEdges();
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Line.Sb = spriteBatch;
            Hex.gameWidth = GameWidth;
            Hex.HexOrientation = Hex.PointyCorners;

            HexMap = new HexGrid(7, 7);
            int amtDraw = gameTime.TotalGameTime.Seconds;
            int drawn = 0;
            var h_drawn = new List<Hex>();
            foreach(var h in HexMap.HexStorage)
            {
                //if (drawn >= amtDraw && amtDraw < 2)
                //{
                //    var xView = GraphicsDevice.Viewport.X;
                //    var yView = GraphicsDevice.Viewport.Y;
                //    var wView = GraphicsDevice.Viewport.Width;
                //    var hView = GraphicsDevice.Viewport.Height;

                //    GraphicsDevice.Viewport = new Viewport(xView-1,yView-1,wView+1,hView+1);
                    
                //}
                h.DrawEdges();
                h_drawn.Add(h);
                drawn++;
            }
            HandleMouse.HandleMouseAction(this, Mouse.GetState());
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
