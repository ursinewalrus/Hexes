using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Hexes.Control;
using Hexes.HexGrid;
using Hexes.UI;
using Hexes.Utilities;

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
        public HandleMouse MouseHandler;

        //private List<Type> UIElements;

        //private EmptyKeys.UserInterface.Generated.ActorActions ActorActions;

        //bool HasBeenResized = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.DeviceCreated += GraphicsDeviceCreated;

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
            //Drawable.GraphicsDevice = GraphicsDevice;
            //Drawable.Sb = SpriteBatch;

            GameWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferWidth = GameWidth / 2;

            GameHeight = GraphicsDevice.DisplayMode.Height;
            graphics.PreferredBackBufferHeight = GameHeight / 2;
            GameCamera = new Camera(GraphicsDevice.Viewport);

            this.IsMouseVisible = true;

            MouseHandler = new HandleMouse();
           // GetUIElements();

            base.Initialize();
        }

        //void GraphicsDeviceCreated(object sender, System.EventArgs e)
        //{
        //    Engine engine = new MonoGameEngine(GraphicsDevice, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);
        //}

        //private void GetUIElements()
        //{
        // EmptyKeys.UserInterface.Input.InputManager -> throwing null, that my issue?
        //    var elements =
        //        Assembly.GetExecutingAssembly()
        //            .GetTypes()
        //            .Where(t => t.Namespace == "EmptyKeys.UserInterface.Generated")
        //            .ToList();

        //    ;
        //    UIElements = elements;
        //    //var actorActions = UIElements.Where(e => e.Name == "ActorActions").First();
        //    //Type type = Type.GetType("EmptyKeys.UserInterface.Generated.ActorActions");
        //    //var instance = Activator.CreateInstance(type);
        //}


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Font = Content.Load<SpriteFont>("General");

            //FontManager.DefaultFont = Engine.Instance.Renderer.CreateFont(Font);
            //ActorActions = new EmptyKeys.UserInterface.Generated.ActorActions(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //FontManager.Instance.LoadFonts(Content);

            //static constructor?? :TODO
            Drawable.GraphicsDevice = GraphicsDevice;
            Drawable.Sb = SpriteBatch;
            Drawable.Font = Font;
            Drawable.Camera = GameCamera;
            Debugger.Font = Font;
            #region load UI Textures
            Hex.HighlightedTexture = Content.Load<Texture2D>(@"UIElements\yellowTransparentHex");
            Hex.SelectedTexture = Content.Load<Texture2D>(@"UIElements\yellowSelecthex");
            ActorRotateClockWise.Texture = Content.Load<Texture2D>(@"UIElements\rotateClockWise");
            ActorRotateCounterClockWise.Texture = Content.Load<Texture2D>(@"UIElements\rotateCounterClockWise");
            #endregion

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //ActorActions.UpdateInput(gameTime.ElapsedGameTime.Milliseconds);
            //ActorActions.UpdateLayout(gameTime.ElapsedGameTime.Milliseconds);

            // TODO: Add your update logic here
            //:TODO update all actor actions / stats

            #region ray test
            if(Keyboard.GetState().IsKeyDown(Keys.Left) == true)
            {
                if (debugRayIndex > 0)
                {
                    debugRayIndex--;
                    lastPressedIndexRot = gameTime.TotalGameTime.Milliseconds;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true)
            {
                if(HexMap.DebugLines.Count() - 1 > debugRayIndex)
                {
                    debugRayIndex++;
                    lastPressedIndexRot = gameTime.TotalGameTime.Milliseconds;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                debugRayIndex = 0;

            }
            #endregion


            base.Update(gameTime);
        }
        #region ray test
        public static int debugRayIndex = 0;
        public int lastPressedIndexRot = 0;
        #endregion
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            MouseHandler.SetMouseState(this);
            var mouseLoc = MouseHandler.RelativeMouseLocation;
            GameCamera.UpdateCamera(this.GraphicsDevice.Viewport, mouseLoc);

            SpriteBatch.Begin(transformMatrix: GameCamera.Transform, sortMode: SpriteSortMode.Deferred);

            GraphicsDevice.Clear(Color.LawnGreen);
            HexMap.Draw();
            #region ray test
            if (HexMap.DebugLines.Any())
            {
                var debugRay = HexMap.DebugLines[debugRayIndex];
                debugRay.Line.Draw();
                debugRay.DrawDebugStrings();
                ;
            }
            #endregion

            //:TODO replace with calls to UIGridbag, init one on the left somehow
            foreach (var hexUI in ActiveHexUIElements.AvailibleUIElements)
            {
                hexUI.Value.Draw();
            }
            //ActorActions.Draw(gameTime.ElapsedGameTime.TotalMilliseconds);

            ;
            //should be moved somewhere else... :TODO
            MouseHandler.TacticalViewMouseHandle(HexMap, GameCamera);
            SpriteBatch.End();
           // base.Draw(gameTime);
        }

       
    }
}
