using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hexes.Control;
using Hexes.HexGrid;
using Hexes.UI;


namespace Hexes
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// //http://emptykeys.com/ui_library
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        //in drawable?
        public static SpriteBatch SpriteBatch;
        HexGrid.HexGrid HexMap;
        int GameHeight;
        int GameWidth;
        public Camera GameCamera;
        public List<LoadModule> Modules = new List<LoadModule>();
        public SpriteFont Font;
        public Debugger Debug;

        //bool HasBeenResized = false;

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
            //
            Drawable.GraphicsDevice = GraphicsDevice;
            Drawable.Sb = SpriteBatch;

            GameWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferWidth = GameWidth / 4;

            GameHeight = GraphicsDevice.DisplayMode.Height;
            graphics.PreferredBackBufferHeight = GameHeight / 4;
            GameCamera = new Camera(GraphicsDevice.Viewport);

            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Font = Content.Load<SpriteFont>("General");
            string ModulesDir = Environment.CurrentDirectory + @"\Modules\";
            DirectoryInfo dirInfo = new DirectoryInfo(ModulesDir);
            List<DirectoryInfo> modules = dirInfo.GetDirectories().ToList();
            foreach (var module in modules)
            {
                var moduleFullPathName = ModulesDir + module.Name;
                var loadedModule = new LoadModule(moduleFullPathName);
                Modules.Add(loadedModule);
            }
            var usedModule = Modules[0];
            HexMap = new HexGrid.HexGrid(usedModule.LoadedMaps, usedModule.LoadedBackgroundTiles, usedModule.LoadedActors, usedModule.ModuleName);
            Debug = new Debugger(SpriteBatch, Font);

            //FileStream fs = new FileStream(@"Content/greenhex.png", FileMode.Open);
            //Texture2D background1 = Texture2D.FromStream(GraphicsDevice, fs);
            //fs.Dispose();

            //get sizes from sprite or scale it
            //HexMap = new HexGrid(2, 2, 100, 100, TileTextures[0]);
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
            var mouseInfo = new HandleMouse(this);
            var mouseLoc = mouseInfo.RelativeMouseLocation;
            GameCamera.UpdateCamera(this.GraphicsDevice.Viewport, mouseLoc);

            SpriteBatch.Begin(transformMatrix: GameCamera.Transform, sortMode: SpriteSortMode.Deferred);

            //so on begin looks like pass in a transform matrix
            //spriteBatch.Begin(transformMatrix: viewMatrix);
            Debug.CamLoc = GameCamera.Transform;

            HexMap.Draw();
            GraphicsDevice.Clear(Color.LawnGreen);
#if DEBUG
            //should be moved somewhere else... :TODO
            GridSelect(mouseInfo, HexMap);
#endif
            SpriteBatch.End();
           // base.Draw(gameTime);
        }

        protected void GridSelect(HandleMouse mouseInfo, HexGrid.HexGrid hexMap)
        {
            if (mouseInfo.MouseState.LeftButton == ButtonState.Pressed)
            {
                var conv = Vector2.Transform(mouseInfo.MouseCords, Matrix.Invert(GameCamera.Transform));
                var selHex = HexMap.SelectedHex(conv);
                if (selHex != null)
                {
                    Debug.Log("Clicked Hex " + selHex.R + ", " + selHex.Q);
                    var hexKey =
                        HexMap.HexStorage.Where(h => h.Key.R == selHex.R && h.Key.Q == selHex.Q).FirstOrDefault();
                    var actorKey =
                        HexMap.ActorStorage.Where(actor => actor.Location.Equals(selHex) ).FirstOrDefault();

                    if (hexKey.Key != null)
                        HexMap.ActiveHex = HexMap.HexStorage[hexKey.Key];

                    if (actorKey != null)
                    {
                        HexMap.ActiveActor = actorKey;
                        var inMoveDistance = HexMap.InRadiusOf(HexMap.ActiveActor.Location, HexMap.ActiveActor.MoveDistance);
                        inMoveDistance.ForEach(h => h.Color = Color.Red );
                    }
                    if (HexMap.ActiveActor != null)
                        HexMap.ActiveActor.Location = selHex;
                    if (HexMap.ActiveActor == null)
                    {
                        //HexMap.ActiveActor
                    }
                }
            }
            if (mouseInfo.MouseState.RightButton == ButtonState.Pressed)
            {
                HexMap.ActiveHex = null;
                HexMap.ActiveActor = null;
            }
        }
    }
}
