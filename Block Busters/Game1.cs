#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using Common;
using Common.GUI;
using Common.Shapes;
#endregion

namespace Block_Busters
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameState
        {
            Menu,
            Play,
            Pause,
            End
        }
        Dictionary<GameState, Transform> states;
        GameState currentState;

        SpriteFont segoeFont;

        ModelObject ground;
        ModelObject[] cubes;
        Camera[] cameras;

        AudioListener listener = new AudioListener();
        AudioEmitter cannonEmitter = new AudioEmitter();
        AudioEmitter breakEmitter = new AudioEmitter();
        AudioEmitter clickEmitter = new AudioEmitter();

        SoundEffect click;
        SoundEffect cannonShot;
        SoundEffect breaking;

        Button playButton;
        Button menuButton;
        Button quitButton;

        ButtonGroup mainMenuButtons;
        ButtonGroup pauseMenuButtons;

        public Game1()
            : base()
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
            // set up scene states
            states = new Dictionary<GameState, Transform>();
            states[GameState.Menu] = new Transform();
            states[GameState.Play] = new Transform();
            states[GameState.Pause] = new Transform();
            states[GameState.End] = new Transform();
            currentState = GameState.Menu;

            InputManager.Initialize();
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

            segoeFont = Content.Load<SpriteFont>("Fonts/Segoe");

            #region Sound Intialization

            click = Content.Load<SoundEffect>("Sounds/click_tiny");
            cannonShot = Content.Load<SoundEffect>("Sounds/cannon");

            #endregion

            #region Camera Initialization
            cameras = new Camera[2];
            cameras[0] = new Camera();
            cameras[0].Position = new Vector3(0, 0, 10); 
            //cameras[0].RotateX = -MathHelper.PiOver2; // to look down
            cameras[0].AspectRatio = GraphicsDevice.Viewport.AspectRatio;
            cameras[1] = new Camera();
            //cameras[1].Position = whatever;
            cameras[1].AspectRatio = GraphicsDevice.Viewport.AspectRatio;

            #endregion

            #region Game Objects Initialization

            ground = new ModelObject(Content.Load<Model>("Models/Plane"), Vector3.Zero);
            ground.Position = new Vector3(0, -1, 0);
            ground.Parent = states[GameState.Play];

            TextureGenerator generator = new TextureGenerator(GraphicsDevice, 256, 256);

            cubes = new ModelObject[3];
            cubes[0] = new ModelObject(Content.Load<Model>("Models/Cube"), Vector3.Up*2);
            cubes[0].Texture = generator.makeGlassTexture();
            cubes[0].Parent = states[GameState.Play];
            cubes[1] = new ModelObject(cubes[0].Model, Vector3.Right);
            cubes[1].Texture = generator.makeWoodTexture();
            cubes[1].Parent = states[GameState.Play];
            cubes[2] = new ModelObject(cubes[0].Model, Vector3.Left);
            cubes[2].Texture = generator.makeMarbleTexture();
            cubes[2].Parent = states[GameState.Play];

            #endregion

            #region GUI Intialization

            playButton = new Button(40, 30, 50, 25, Content.Load<Texture2D>("Textures/Square"), segoeFont);
            playButton.MouseDown += PlayGame;
            playButton.TextColor = Color.Black;
            playButton.HoverColor = Color.YellowGreen;
            playButton.Text = "Play";

            quitButton = new Button(40, 100, 50, 25, Content.Load<Texture2D>("Textures/Square"), segoeFont);
            quitButton.MouseDown += QuitGame;
            quitButton.TextColor = Color.Black;
            quitButton.HoverColor = Color.YellowGreen;
            quitButton.Text = "Quit";

            menuButton = new Button(40, 170, 50, 25, Content.Load<Texture2D>("Textures/Square"), segoeFont);
            menuButton.MouseDown += ToMenu;
            menuButton.TextColor = Color.Black;
            menuButton.HoverColor = Color.YellowGreen;
            menuButton.Text = "Main Menu";

            mainMenuButtons = new ButtonGroup(Color.YellowGreen, click);
            mainMenuButtons.addButton(playButton);
            mainMenuButtons.addButton(quitButton);
            mainMenuButtons.Parent = states[GameState.Menu];

            pauseMenuButtons = new ButtonGroup(Color.YellowGreen, click);
            pauseMenuButtons.addButton(playButton);
            pauseMenuButtons.addButton(menuButton);
            pauseMenuButtons.Parent = states[GameState.Pause];
            #endregion GUI Intialization

            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            states[currentState].Update(gameTime);
            states[currentState].Update(gameTime, Matrix.Identity);

            #region Sound listeners and emitters
            listener.Position = cameras[0].Position;
            listener.Up = cameras[0].Up;
            listener.Forward = cameras[0].Forward;

            clickEmitter.Position = cameras[0].Position;                             //Sound business
            clickEmitter.Up = cameras[0].Up;
            clickEmitter.Forward = cameras[0].Forward;
            #endregion

            // to go directly to game state
            if (InputManager.IsKeyReleased(Keys.D1))
            {
                currentState = GameState.Menu;
            }
            if (InputManager.IsKeyReleased(Keys.D2))
            {
                currentState = GameState.Play;
            }
            if (InputManager.IsKeyReleased(Keys.D3))
            {
                currentState = GameState.Pause;
            }
            if (InputManager.IsKeyReleased(Keys.D4))
            {
                currentState = GameState.End;
            }

      

            // P to Pause
            if (currentState.Equals(GameState.Play) || currentState.Equals(GameState.Pause))
            {
                if (InputManager.IsKeyReleased(Keys.P))
                {
                    if (currentState.Equals(GameState.Play))
                    {
                        currentState = GameState.Pause;
                    }
                    else
                    {
                        currentState = GameState.Play;
                    }
                }
            }

            InputManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            switch (currentState)
            {
                case GameState.Menu:
                    GraphicsDevice.Clear(Color.Gray);
                    break;
                case GameState.Play:
                    GraphicsDevice.Clear(Color.Blue);
                    break;
                case GameState.Pause:
                    GraphicsDevice.Clear(Color.Green);
                    break;
                case GameState.End:
                    GraphicsDevice.Clear(Color.Red);
                    break;
                default: // something weird happened
                    break;
            }
            states[currentState].Draw(gameTime, cameras[0]);
            // end of 3D Drawing

            // start 2D Drawing
            spriteBatch.Begin();
            states[currentState].Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void QuitGame(object sender, EventArgs args)
        {
            Exit();
        }

        private void PlayGame(object sender, EventArgs args)
        {
            currentState = GameState.Play;
        }

        private void ToMenu(object sender, EventArgs args)
        {
            currentState = GameState.Menu;
        }
    }       
}
