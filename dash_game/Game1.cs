﻿using Microsoft.Xna.Framework;
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
            LevelSelect,
            Adventure,
            Stats,
            GameOver,
            Victory
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
        AdventureMenuScreen levelSelect;

        // Keyboard and mouse states
        KeyboardState kbState;
        MouseState mState;

        KeyboardState kbPrevState;
        MouseState mPrevState;

        // An int that tracks what state to move to
        private int state;

        //ints for highscore and wave
        private int highScore = 0;
        private int highWave = 0;

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
            Texture2D shuriken = Content.Load<Texture2D>("Shuriken");
            player = new Player(100, new Vector2(300,300), new Rectangle(300, 300, 50, 50), spriteSheet, PlayerState.IdleRight);
            Texture2D hitTexture = new Texture2D(GraphicsDevice, 1, 1);
            hitTexture.SetData(new[] { Color.White });

            // Texture for the doors in adventure
            Texture2D doorTexture = Content.Load<Texture2D>("Door");

            
            // The two main items for the horde mode
            itemList = new List<Items>();
            item1 = new Items(new Rectangle(500, 330, 25, 25), "Speed Boost", false, speedArrow);
            item2 = new Items(new Rectangle(600, 350, 25, 25), "Health Pack", false, shuriken);
            itemList.Add(item1);
            itemList.Add(item2);
           

            // Create an instance of the adventure modes title screen
            levelSelect = new AdventureMenuScreen();

            // Creation of both horde and level objects, done here so they can utilize sprites
            horde = new Horde(spriteSheet, player, _spriteBatch, hitTexture);
            currentLevel = new Level(player, "TrialLevel", spriteSheet, speedArrow, _spriteBatch, doorTexture, hitTexture);
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

                        //update high score and higehst wave if they are higher than that already recorded
                        if (horde.Score > highScore)
                        {
                            highScore = horde.Score;
                        }
                        if (horde.Wave > highWave)
                        {
                            highWave = horde.Wave;
                        }

                        //end game if player is out of health
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

                case GameState.LevelSelect:
                    // Runs the update for the level select mode and initializes the adventure level
                    if (levelSelect.Update(currentLevel))
                    {
                        currentState = GameState.Adventure;
                    }
                    break;

                case GameState.Adventure:
                    // Run updates if the game is not paused
                    if (!paused)
                    {
                        currentLevel.Update();
                        currentLevel.MoveRoom();

                        if (player.Health <= 0)
                        {
                            currentState = GameState.GameOver;
                        }
                        if (currentLevel.LevelBoss.Health <= 0)
                        {
                            currentState = GameState.Victory;
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
                        //This is basically resetting all the stats once a gameover is reached, so that you can immediately go back into horde mode.
                        currentState = GameState.Title;
                        horde.Wave = 0;
                        horde.Enemies.Clear();
                        player.Health = 100;
                        horde.NumEnemies = 3;
                        horde.Score = 0;
                        player.MovementSpeed = 3;
                    }
                    break;

                case GameState.Victory:
                    // Checks if the user has pressed the spacebar and sends them back to the title screen
                    if (kbState.IsKeyUp(Keys.Space) && kbPrevState.IsKeyDown(Keys.Space))
                    {
                        //Reset all stats once a level is beaten, does basically the same thing as game over, the main difference is what is drawn to the screen
                        currentState = GameState.Title;
                        horde.Wave = 0;
                        horde.Enemies.Clear();
                        player.Health = 100;
                        horde.NumEnemies = 3;
                        horde.Score = 0;
                        player.MovementSpeed = 3;
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
                        if (!i.PickedUp && player.EnemiesDefeated % 10 <= 5)
                        {
                            i.Draw(_spriteBatch);
                        }
                    }
                    
                   
                    
                    horde.Draw();

                    // Draw the player health in the top right
                    _spriteBatch.DrawString(gameFont, $"Health: {player.Health}", new Vector2(0, 0), Color.Black);

                    // Draw the score up in the top right
                    _spriteBatch.DrawString(gameFont, "Score " + horde.Score.ToString(), new Vector2(1050, 0), Color.Black);
                    _spriteBatch.DrawString(gameFont, "Wave " + horde.Wave.ToString(), new Vector2(300, 0), Color.Black);
                    _spriteBatch.DrawString(gameFont, "Press P to pause", new Vector2(500, 0), Color.Black);

                    // Draws certain elements based on the game being paused
                    if (paused)
                    {
                        _spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), shader);
                        _spriteBatch.DrawString(titleFont, "Paused", new Vector2(640, 400) - (titleFont.MeasureString("Paused") / 2), Color.Black);
                        _spriteBatch.DrawString(gameFont, "Press P to un-pause", new Vector2(640, 500) - (gameFont.MeasureString("Press P to un-pause") / 2), Color.Black);
                    }
                    break;

                case GameState.LevelSelect:
                    // Draws the backgound shader
                    _spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), shader);

                    // Draws everything needed for the screen
                    levelSelect.Draw(_spriteBatch, button, gameFont, titleFont);
                    break;

                // Draws enemies, items, and the player for the classic mode
                case GameState.Adventure:
                    currentLevel.Draw();
                    player.Draw(_spriteBatch);

                    // Draw the player health in the top right
                    _spriteBatch.DrawString(gameFont, $"Health: {player.Health}", new Vector2(0, 0), Color.Black);

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

                    //draw return to menu text
                    _spriteBatch.DrawString(gameFont, "Press space to return to title screen",
                        new Vector2(640, 500) - (gameFont.MeasureString("Press space to return to title screen") / 2), Color.Black);

                    //draw score / wave
                    _spriteBatch.DrawString(gameFont, "High Score: " + highScore,
                        new Vector2(640, 300) - (gameFont.MeasureString("High Score: " + highScore) / 2), Color.Black);
                    _spriteBatch.DrawString(gameFont, "Highest Wave: " + highWave,
                        new Vector2(640, 400) - (gameFont.MeasureString("Highest Wave: " + highWave) / 2), Color.Black);
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

                //draws the victory Screen
                case GameState.Victory:
                    // Draws the background shader
                    _spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), shader);

                    // Draws a game over message
                    _spriteBatch.DrawString(titleFont, "VICTORY!", new Vector2(640, 368) - (titleFont.MeasureString("VICTORY!") / 2), Color.Black);
                    _spriteBatch.DrawString(gameFont, "Press space to return to title screen",
                        new Vector2(640, 500) - (gameFont.MeasureString("Press space to return to title screen") / 2), Color.Black);
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        
    }
}