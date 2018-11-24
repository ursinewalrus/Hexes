using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Hexes
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// //http://emptykeys.com/ui_library
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch SpriteBatch;
        HexGrid HexMap;
        int GameHeight;
        int GameWidth;
        public Camera GameCamera;
        bool MapDrawn = false;

        bool HasBeenResized = false;
        bool RedrawAll = false;

        public List<Texture2D> TileTextures = new List<Texture2D>();


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
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            GameCamera = new Camera(GraphicsDevice.Viewport);

            GameWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferWidth = GameWidth / 2;

            GameHeight = GraphicsDevice.DisplayMode.Height;
            graphics.PreferredBackBufferHeight = GameHeight / 2;

            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            FileStream fs = new FileStream(@"Content/badhex.png", FileMode.Open);
            Texture2D background1 = Texture2D.FromStream(GraphicsDevice, fs);
            fs.Dispose();
            TileTextures.Add(background1);

            graphics.ApplyChanges();

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

            //so on begin looks like pass in a transform matrix
            //spriteBatch.Begin(transformMatrix: viewMatrix);
            var mouseInfo = new HandleMouse(this);

            var mouseLoc = mouseInfo.RelativeMouseLocation;

            GameCamera.UpdateCamera(this.GraphicsDevice.Viewport, mouseLoc);
            GraphicsDevice.Clear(Color.LawnGreen);
            //is slow because spritebatch used bad

            Hex.gameWidth = GameWidth;
            Hex.HexOrientation = Hex.PointyCorners;
            
            HexMap = new HexGrid(13, 13, TileTextures[0]);

            SpriteBatch.Begin(transformMatrix: GameCamera.Transform);

            HexMap.Draw();

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
