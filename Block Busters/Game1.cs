﻿#region Using Statements
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

        enum GameState { Menu, Info, Play, Pause, End }
        Dictionary<GameState, Transform> states;
        GameState currentState;

        int score;

        SpriteFont segoeFont;

        ModelObject ground;
        Cannon cannon;
        Model ball;
        List<Cannonball> cannonballs = new List<Cannonball>();

        List<Block> cubes = new List<Block>();
        Camera[] cameras;
        int curCamera; // index of current cammera
        float cameraAngle = 0.0f;
        
        Skybox skybox;
        Model skyCube;
        TextureCube skyboxTexture;
        Effect skyboxEffect;

        AudioListener listener = new AudioListener();
        AudioEmitter cannonEmitter = new AudioEmitter();
        AudioEmitter breakEmitter = new AudioEmitter();
        AudioEmitter clickEmitter = new AudioEmitter();

        SoundEffect click;
        SoundEffect cannonShot;
        SoundEffect glassHit;
        SoundEffect woodHit;
        SoundEffect rockHit;

        Button playButton;
        Button menuButton;
        Button infoButton;
        Button quitButton;

        ButtonGroup mainMenuButtons;
        ButtonGroup pauseMenuButtons;
        ButtonGroup infoMenuButtons;

        GUIElement pausePanel;
        GUIElement infoPanel;

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
            states[GameState.Info] = new Transform();
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
            woodHit = Content.Load<SoundEffect>("Sounds/wood2");
            rockHit = Content.Load<SoundEffect>("Sounds/crunchy");
            glassHit = Content.Load<SoundEffect>("Sounds/breaking");

            #endregion

            #region Camera Initialization
            cameras = new Camera[3];
            cameras[0] = new Camera();
            cameras[0].Position = new Vector3(0, 0, 10); 
            cameras[0].AspectRatio = GraphicsDevice.Viewport.AspectRatio;
            cameras[1] = new Camera();
            cameras[1].RotateX = -MathHelper.PiOver2; // to look down
            cameras[1].Position = new Vector3(0,10,0);
            cameras[1].AspectRatio = GraphicsDevice.Viewport.AspectRatio;
            cameras[2] = new Camera();
            cameras[2].Position = new Vector3(0, 0, 5);
            cameras[2].AspectRatio = GraphicsDevice.Viewport.AspectRatio;
            
            curCamera = 0; // 0 = behind the cannon
            // 1 = top-down view
            // 2 = first person

            #endregion

            #region Game Objects Initialization
            TextureGenerator generator = new TextureGenerator(GraphicsDevice, 256, 256);

            ground = new ModelObject(Content.Load<Model>("Models/Plane"), Vector3.Zero);
            ground.Position = new Vector3(0, -1, 0);
            ground.Scale *= 10.0f;
            ground.Parent = states[GameState.Play];
            ground.Texture = generator.makeGroundTexture();

            // load cannon
            cannon = new Cannon(Content.Load<Model>("Models/Torus"), Content.Load<Texture2D>("Textures/Stripes"), generator.makeBlank(), new Vector3(0,0,5));
            cannon.Parent = states[GameState.Play];
            cannon.FireEvent += FireCannon;
            ball = Content.Load<Model>("Models/Sphere");

            // create the blocks
            Model cube = Content.Load<Model>("Models/Cube");
            makeBuilding(cube);

            //SkyBox stuff
            skyCube = Content.Load<Model>("Models/Cube");
            skyboxTexture = Content.Load<TextureCube>("Skyboxes/SunnySkybox");
            skyboxEffect = Content.Load<Effect>("Skyboxes/Skybox");
            //Effect temp = Content.Load<Effect>("Skyboxes/Skybox");
            
            skybox = new Skybox(skyCube,skyboxTexture,skyboxEffect);
            #endregion

            #region GUI Intialization

            pausePanel = new GUIElement(Content.Load<Texture2D>("Textures/Square"), 200, 200);
            pausePanel.Position = new Vector3(0,20,-1);
            pausePanel.Parent = states[GameState.Pause];

            infoPanel = new GUIElement(Content.Load<Texture2D>("Textures/Square"), 200, 200);
            infoPanel.Position = new Vector3(0, 20, -1);
            infoPanel.Parent = states[GameState.Info];
            
            playButton = new Button(40, 50, 50, 25, Content.Load<Texture2D>("Textures/Square"), segoeFont);
            playButton.MouseDown += PlayGame;
            playButton.TextColor = Color.Black;
            playButton.HoverColor = Color.YellowGreen;
            playButton.Text = "Play";

            infoButton = new Button(40, 75, 50, 25, Content.Load<Texture2D>("Textures/Square"), segoeFont);
            infoButton.MouseDown += InfoMenu;
            infoButton.TextColor = Color.Black;
            infoButton.HoverColor = Color.YellowGreen;
            infoButton.Text = "Info";

            quitButton = new Button(40, 100, 50, 25, Content.Load<Texture2D>("Textures/Square"), segoeFont);
            quitButton.MouseDown += QuitGame;
            quitButton.TextColor = Color.Black;
            quitButton.HoverColor = Color.YellowGreen;
            quitButton.Text = "Quit";

            menuButton = new Button(40, 100, 50, 25, Content.Load<Texture2D>("Textures/Square"), segoeFont);
            menuButton.MouseDown += ToMenu;
            menuButton.TextColor = Color.Black;
            menuButton.HoverColor = Color.YellowGreen;
            menuButton.Text = "Main Menu";

            mainMenuButtons = new ButtonGroup(Color.YellowGreen, click);
            mainMenuButtons.addButton(playButton);
            mainMenuButtons.addButton(infoButton);
            mainMenuButtons.addButton(quitButton);
            mainMenuButtons.Parent = states[GameState.Menu];

            pauseMenuButtons = new ButtonGroup(Color.YellowGreen, click);
            pauseMenuButtons.addButton(playButton);
            pauseMenuButtons.addButton(infoButton);
            pauseMenuButtons.addButton(menuButton);
            pauseMenuButtons.Parent = states[GameState.Pause];

            infoMenuButtons = new ButtonGroup(Color.YellowGreen, click);
            infoMenuButtons.addButton(playButton);
            infoMenuButtons.addButton(menuButton);
            infoMenuButtons.Parent = states[GameState.Info];
            
            #endregion GUI Intialization
            
            
            // TODO: use this.Content to load your game content here
        }

        void makeBuilding(Model model)
        {
            // create textures
            TextureGenerator generator = new TextureGenerator(GraphicsDevice, 256, 256);
            Texture2D glass = generator.makeGlassTexture();
            Texture2D wood = generator.makeWoodTexture();
            Texture2D stone = generator.makeMarbleTexture();

            // number of each block
            int numGlass = 1;
            int numWood = 1;
            int numStone = 1;

            // add glass blocks to cubes list
            for (int i = 0; i < numGlass; i++)
            {
                // position should vary
                Block block = new Block(model, Vector3.Up * 2, Block.Type.Glass);
                block.Texture = glass;
                block.Parent = states[GameState.Play];
                block.BreakEvent += Broken;
                cubes.Add(block);
            }

            // add wood blocks to cubes list
            for (int i = 0; i < numWood; i++)
            {
                // position should vary
                Block block = new Block(model, Vector3.Right, Block.Type.Wood);
                block.Texture = wood;
                block.Parent = states[GameState.Play];
                block.BreakEvent += Broken;
                cubes.Add(block);
            }

            // add stone blocks to cubes list
            for (int i = 0; i < numStone; i++)
            {
                // position should vary
                Block block = new Block(model, Vector3.Left, Block.Type.Stone);
                block.Texture = stone;
                block.Parent = states[GameState.Play];
                block.BreakEvent += Broken;
                cubes.Add(block);
            }
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
            listener.Position = cameras[curCamera].Position;
            listener.Up = cameras[curCamera].Up;
            listener.Forward = cameras[curCamera].Forward;

            clickEmitter.Position = cameras[curCamera].Position;                             //Sound business
            clickEmitter.Up = cameras[curCamera].Up;
            clickEmitter.Forward = cameras[curCamera].Forward;

            cannonEmitter.Position = cameras[2].Position;                             //Sound business
            cannonEmitter.Up = cameras[2].Up;
            cannonEmitter.Forward = cameras[2].Forward;
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

            // Tab to switch cameras
            if (InputManager.IsKeyPressed(Keys.Tab) && currentState.Equals(GameState.Play))
            {
                if (curCamera + 1 > 2)
                    curCamera = 0;
                else
                    curCamera += 1;
            }

            if (currentState.Equals(GameState.Play))
            {
                #region First Person Camera Stuff
                float elapsedTime = (float)(gameTime.ElapsedGameTime.TotalSeconds);
                #region WASD Movement controls
                if (InputManager.IsKeyDown(Keys.Up))
                {
                    if (cameraAngle < MathHelper.PiOver2) // Vertical
                    {
                        cameraAngle += elapsedTime * 2f;
                        cameras[2].RotatePitch = elapsedTime * 2f;
                    }
                }
                if (InputManager.IsKeyDown(Keys.Down))
                {
                    if (cameraAngle > 0.0f) // Horizontal
                    {
                        cameraAngle -= elapsedTime * 2f;
                        cameras[2].RotatePitch = -elapsedTime * 2f;
                    }
                }
                if (InputManager.IsKeyDown(Keys.Left))
                {
                    cameras[2].RotateY = elapsedTime * 2f;
                }
                if (InputManager.IsKeyDown(Keys.Right))
                {
                    cameras[2].RotateY = -elapsedTime * 2f;
                }
                #endregion
#endregion
                // used a reverse for loop so the game doesn't crash when the cannonball explodes (ironic)
                for (int i = cannonballs.Count - 1; i > -1; i--)
                {
                    Cannonball b = cannonballs[i];
                    b.Update(gameTime);
                    // used another reverse for loop so the game doesn't crash when the block breaks (double irony)
                    for (int j = cubes.Count - 1; j > -1; j--) 
                    {
                        Block c = cubes[j];
                        if (c.DidCollide(b))
                        {
                            // SFX: play collision sound based on block type (glass, wood, stone)
                            c.CurrentState = Block.State.Moved;
                            if (c.BlockType != Block.Type.Glass)
                            {
                                b.Explode();
                            }
                            breakEmitter.Position = c.Position;
                            breakEmitter.Up = c.Up;
                            breakEmitter.Forward = c.Forward;
                           
                        }
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
            GraphicsDevice.DepthStencilState.DepthBufferEnable = true;
            GraphicsDevice.DepthStencilState.DepthBufferWriteEnable = true;
            GraphicsDevice.DepthStencilState = new DepthStencilState();
            // TODO: Add your drawing code here
            switch (currentState)
            {
                case GameState.Menu:
                    GraphicsDevice.Clear(Color.Violet);
                    break;
                case GameState.Play:
                    GraphicsDevice.Clear(Color.Blue);
                    break;
                case GameState.Pause:
                    GraphicsDevice.Clear(Color.Gray);
                    break;
                case GameState.End:
                    GraphicsDevice.Clear(Color.Red);
                    break;
                default: // something weird happened
                    break;
            }
            if (currentState == GameState.Pause)
            {

                states[GameState.Play].Draw(gameTime, cameras[curCamera]);
                states[currentState].Draw(gameTime, cameras[curCamera]);
                foreach (Cannonball b in cannonballs)
                {
                    b.Draw(gameTime, cameras[curCamera]);
                }
            }
            

            if (currentState.Equals(GameState.Play))
            {
                states[currentState].Draw(gameTime, cameras[curCamera]);
                foreach (Cannonball b in cannonballs)
                {
                    b.Draw(gameTime, cameras[curCamera]);
                }
            }

            else
                states[currentState].Draw(gameTime, cameras[curCamera]);

            skybox.Draw(cameras[curCamera].View, cameras[curCamera].Projection, cameras[curCamera].Position);
            graphics.GraphicsDevice.RasterizerState = new RasterizerState();
            // end of 3D Drawing

            // start 2D Drawing
            spriteBatch.Begin();
            if (currentState == GameState.Pause)
            {
                spriteBatch.DrawString(segoeFont, "GAME PAUSED", new Vector2(200, 0), Color.Black);
                spriteBatch.DrawString(segoeFont, "Score:" + score, new Vector2(200, 20), Color.Black);
            }

            if (currentState == GameState.Info)
            {
                spriteBatch.DrawString(segoeFont, "How to play", new Vector2(200, 40), Color.Black);
                spriteBatch.DrawString(segoeFont, "Use the arrow keys to look around" , new Vector2(200, 60), Color.Black);
                spriteBatch.DrawString(segoeFont, "Press the tab key to switch cameras", new Vector2(200, 80), Color.Black);
                spriteBatch.DrawString(segoeFont, "Hold the space bar to charge the cannon", new Vector2(200, 100), Color.Black);
                spriteBatch.DrawString(segoeFont, "realse to fire", new Vector2(200, 120), Color.Black);
                spriteBatch.DrawString(segoeFont, "Destroy the buildings before time runs out", new Vector2(200, 140), Color.Black);
            }

            if (currentState == GameState.End)
            {
                spriteBatch.DrawString(segoeFont, "GAME OVER", new Vector2(200, 0), Color.Black);
                spriteBatch.DrawString(segoeFont, "Your Final Score:" + score , new Vector2(0, 0), Color.Black);
            }

            if (currentState == GameState.Menu)
            {
                spriteBatch.DrawString(segoeFont, "Block Busters!", new Vector2(00, 0), Color.Black);
            }

            if (currentState == GameState.Play)
            {
                spriteBatch.DrawString(segoeFont, "Your Score:" + score, new Vector2(200, 20), Color.Black);
            }
            states[currentState].Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Events
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

        private void InfoMenu(object sender, EventArgs args)
        {
            currentState = GameState.Info;
        }
        private void FireCannon(object sender, EventArgs args)
        {
            // SFX: play fire cannon sound
            SoundEffectInstance cannonShotInstance = cannonShot.CreateInstance();
            cannonShotInstance.Apply3D(listener, breakEmitter);
            cannonShotInstance.Play();
            cannonShotInstance.Apply3D(listener, breakEmitter);
            // create cannonball
            Cannonball cb = new Cannonball(ball, cannon.Position, cannon.Forward, cannon.PowerBar.CurrentFillAmount);
            cb.ExplodeEvent += Explosion;
            cannonballs.Add(cb);

            // decrease number of cannon fire's left
        }

        private void Explosion(object sender, EventArgs args)
        {
            // SFX: play the explosion sound

            // create smoke/fire particle effects (if time permits)

            // remove the cannonball
            cannonballs.Remove((Cannonball)sender);
        }

        private void Broken(object sender, EventArgs args)
        {
            Block block = (Block)sender;
            switch (block.BlockType)
            {
                case Block.Type.Glass:
                    // SFX: play breaking glass sound
                    SoundEffectInstance glassHitInstance = glassHit.CreateInstance();
                    glassHitInstance.Apply3D(listener, breakEmitter);
                    glassHitInstance.Play();
                    glassHitInstance.Apply3D(listener, breakEmitter);
                    // increase score
                    score += 10;
                    break;
                case Block.Type.Wood:
                    // SFX: play breaking wood sound
                    SoundEffectInstance woodHitInstance = woodHit.CreateInstance();
                    woodHitInstance.Apply3D(listener, breakEmitter);
                    woodHitInstance.Play();
                    woodHitInstance.Apply3D(listener, breakEmitter);
                    // increase score
                    score += 10;
                    break;
                case Block.Type.Stone:
                    // SFX: play breaking stone sound
                    SoundEffectInstance rockHitInstance = rockHit.CreateInstance();
                    rockHitInstance.Apply3D(listener, breakEmitter);
                    rockHitInstance.Play();
                    rockHitInstance.Apply3D(listener, breakEmitter);
                    // increase score
                    score += 10;
                    break;
                default:
                    break;
            }

            if (block.Demolished)
            {
                // increase score
                block.Parent = null;
                cubes.Remove(block);
            }
        }
        #endregion
    }       
}
