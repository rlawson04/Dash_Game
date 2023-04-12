using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml;
using System;
using System.Collections.Generic;

namespace dash_game
{
    public class Game1 : Game
    {
        // Enum for the gamestates
        public enum GameState
        {
            Title,
            Horde,
            Adventure,
            Stats,
            GameOver
        }

        // Fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Background texture
        private Texture2D background;

        // Color for menu states
        private Color shader = new Color(Color.Black, 0.5f);

        // Fonts
        private SpriteFont titleFont;
        private SpriteFont gameFont;

        // Menu buttons texture
        private Texture2D button;

        // Creation of different screens
        TitleScreen titleScreen = new TitleScreen();
        Horde horde;
        // Creating a level, at some point this will be able to take in text but currently just runs a trial
        Level currentLevel;

        // Keyboard and mouse states
        KeyboardState kbState;
        MouseState mState;

        KeyboardState kbPrevState;
        MouseState mPrevState;

        // An int that tracks what state to move to
        private int state;

        // Bool to see if the game is paused
        private bool paused = false;

        // Player and enemy declarations
        private Player player;
        private Enemy enemy;

        // Item declaration and random for spawn location
        private Items item1;
        private Items item2;
        private Items item3;
        List<Items> itemList;
        private Random random;
        public GameState currentState = GameState.Title;

        // Texture 2d for the doorsprites
        private Texture2D doorTexture;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 736;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            background = Content.Load<Texture2D>("background");
            button = Content.Load<Texture2D>("buttons");
            titleFont = Content.Load<SpriteFont>("mainFont");
            gameFont = Content.Load<SpriteFont>("buttonFont");

            // Location, Sprite sheet, and player initialization
            Vector2 playerLoc = new Vector2(300f, 300f);
            Texture2D spriteSheet = Content.Load<Texture2D>("SpriteBatchForDash");
            Texture2D speedArrow = Content.Load<Texture2D>("Speed Arrow");
            player = new Player(100, new Vector2(300,300), new Rectangle(300, 300, 50, 50), spriteSheet, PlayerState.IdleRight);

            // Texture for the doors in adventure
            Texture2D doorTexture = Content.Load<Texture2D>("SpriteBatchForDash");

            // Items to spawn randomly
            random = new Random();
            itemList = new List<Items>();
            item1 = new Items(new Rectangle(random.Next(100, 500), random.Next(100, 500), 25, 25), "Speed Boost", false, speedArrow);
            item2 = new Items(new Rectangle(random.Next(100, 500), random.Next(100, 500), 25, 25), "Health Pack", false, speedArrow);
            item3 = new Items(new Rectangle(random.Next(100, 500), random.Next(100, 500), 25, 25), "Attack Boost", false, speedArrow);

            // Creation of both horde and level objects, done here so they can utilize sprites
            horde = new Horde(spriteSheet, player, _spriteBatch);
            currentLevel = new Level(player, "TrialLevel", spriteSheet, speedArrow, _spriteBatch, doorTexture);
            currentLevel.CreateLevel();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            kbState = Keyboard.GetState();

            player.UpdateAnimation(gameTime);
            

            switch (currentState)
            {
                case GameState.Title:
                    // Runs the title screen update method and 
                    state = titleScreen.Update();
                    if (state != 10)
                    {
                        currentState = (GameState)state;
                    }
                    break;

                case GameState.Horde:
                    // Update the players movement
                    if (!paused)
                    {
                        player.Update(kbState, kbPrevState);
                        horde.Update();

                        if (player.Health <= 0)
                        {
                            currentState = GameState.GameOver;
                        }
                    }

                    // Set to pause state
                    if (kbState.IsKeyUp(Keys.P) && kbPrevState.IsKeyDown(Keys.P) && !paused)
                    {
                        paused = true;
                    }
                    else if (kbState.IsKeyUp(Keys.P) && kbPrevState.IsKeyDown(Keys.P) && paused)
                    {
                        paused = false;
                    }

                    foreach(Items i in itemList)
                    {
                        i.CheckCollision(player);
                    }
                    
                    break;

                case GameState.Adventure:
                    // Run updates if the game is not paused
                    if (!paused)
                    {
                        currentLevel.Update();
                        currentLevel.MoveRoom();
                    }

                    // Set to pause state
                    if (kbState.IsKeyUp(Keys.P) && kbPrevState.IsKeyDown(Keys.P) && !paused)
                    {
                        paused = true;
                    }
                    else if (kbState.IsKeyUp(Keys.P) && kbPrevState.IsKeyDown(Keys.P) && paused)
                    {
                        paused = false;
                    }
                    break;

                case GameState.Stats:
                    // Checks if the user has pressed the spacebar and sends them back to the title screen
                    if (kbState.IsKeyUp(Keys.Space) && kbPrevState.IsKeyDown(Keys.Space))
                    {
                        currentState = GameState.Title;
                    }
                    break;

                case GameState.GameOver:
                    // Checks if the user has pressed the spacebar and sends them back to the title screen
                    if (kbState.IsKeyUp(Keys.Space) && kbPrevState.IsKeyDown(Keys.Space))
                    {
                        currentState = GameState.Title;
                    }
                    break;
            }

            kbPrevState = kbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            // Background will always be drawn
            _spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);

            // Logic for the enum
            switch (currentState)
            {
                // Draws the title and displays buttons for actions
                case GameState.Title:
                    // Draws the backgound shader
                    _spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), shader);

                    // Title for the game
                    _spriteBatch.DrawString(titleFont, "DASH", new Vector2(640, 200) - (titleFont.MeasureString("DASH") / 2), Color.Black);

                    // Draw the buttons that when clicked will change the game state
                    titleScreen.Draw(_spriteBatch, button, gameFont);
                    break;

                // Draws enemies, items, and the player for horde mode
                case GameState.Horde:
                    // Drawing the player and enemies
                    player.Draw(_spriteBatch);
                    
                    foreach(Items i in itemList)
                    {
                        if (i.PickedUp == false)
                        {
                            i.Draw(_spriteBatch);
                        }
                    }
                    
                    
                    horde.Draw();

                    // Draw the score up in the top right
                    _spriteBatch.DrawString(gameFont, "Score " + horde.Score.ToString(), new Vector2(1050, 0), Color.Black);
                    _spriteBatch.DrawString(gameFont, "Wave " + horde.Wave.ToString(), new Vector2(0, 0), Color.Black);
                    _spriteBatch.DrawString(gameFont, "Press P to pause", new Vector2(0, 25), Color.Black);

                    // Draws certain elements based on the game being paused
                    if (paused)
                    {
                        _spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), shader);
                        _spriteBatch.DrawString(titleFont, "Paused", new Vector2(640, 400) - (titleFont.MeasureString("Paused") / 2), Color.Black);
                        _spriteBatch.DrawString(gameFont, "Press P to un-pause", new Vector2(640, 500) - (gameFont.MeasureString("Press P to un-pause") / 2), Color.Black);
                    }
                    break;

                // Draws enemies, items, and the player for the classic mode
                case GameState.Adventure:
                    currentLevel.Draw();
                    player.Draw(_spriteBatch);
                    // Draws certain elements based on the game being paused
                    if (paused)
                    {
                        _spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), shader);
                        _spriteBatch.DrawString(titleFont, "Paused", new Vector2(640, 400) - (titleFont.MeasureString("Paused") / 2), Color.Black);
                        _spriteBatch.DrawString(gameFont, "Press P to un-pause", new Vector2(640, 500) - (gameFont.MeasureString("Press P to un-pause") / 2), Color.Black);
                    }
                    break;

                // Draws everything for the pause menu
                case GameState.Stats:
                    // Draws the background shader
                    _spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), shader);
                    break;

                // Draws the game over screen
                case GameState.GameOver:
                    // Draws the background shader
                    _spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), shader);

                    // Draws a game over message
                    _spriteBatch.DrawString(titleFont, "GAME OVER", new Vector2(640, 368) - (titleFont.MeasureString("GAME OVER") / 2), Color.Black);
                    _spriteBatch.DrawString(gameFont, "Press space to return to title screen",
                        new Vector2(640, 500) - (gameFont.MeasureString("Press space to return to title screen") / 2), Color.Black);
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        
    }
}