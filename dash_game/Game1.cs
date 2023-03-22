using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dash_game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Background texture
        private Texture2D background;

        // Enum for the gamestates
        private enum GameState
        {
            Title,
            Horde,
            Classic,
            Pause,
            GameOver
        }

        private GameState currentState = GameState.Title;

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

            Texture2D spriteSheet = Content.Load<Texture2D>("SpriteBatchForDash");
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (currentState)
            {
                case GameState.Title:
                    break;

                case GameState.Horde:
                    break;

                case GameState.Classic:
                    break;

                case GameState.Pause:
                    break;

                case GameState.GameOver:
                    break;
            }

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
                    break;

                // Draws enemies, items, and the player for horde mode
                case GameState.Horde:
                    break;

                // Draws enemies, items, and the player for the classic mode
                case GameState.Classic:
                    break;

                // Draws everything for the pause menu
                case GameState.Pause:
                    break;

                // Draws the game over screen
                case GameState.GameOver:
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}