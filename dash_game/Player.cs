using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* Name: Player
 * Purpose: Handles the player objects and methods associated with them
 * Modifications: Initial creation
 */

namespace dash_game
{
    // Enum
    public enum PlayerState
    {
        RunningLeft, // Animations for running
        RunningRight,
        RunningUp,
        RunningDown,
        Dashing,
        Idle, // Handles the idle animation 
    }

    public class Player : Character
	{
		// -------------------------------
		// Fields
		// -------------------------------

		private bool dashing;

		// -------------------------------
		// Properties
		// -------------------------------


		/// <summary>
		/// Gets or sets the current state of the player 
		/// </summary>
		public PlayerState State
        {
			get { return state; }
			set { state = value; }
        }


		// -------------------------------
		// Constructor
		// -------------------------------

		/// <summary>
		/// Parameterized constructor for the player class
		/// </summary>
		/// <param name="health"> takes in an int for the player's health </param>
		/// <param name="rect"> takes in a rectangle to handle collision </param>
		/// <param name="spriteSheet"> takes in loaded content for animation </param>
		/// <param name="playerPostition"> takes in a vector2 to keep track of postion </param>
		/// <param name="startingState"> takes in the enum for animation directions </param>
		public Player(int health, Vector2 characterPosition, Rectangle rect, Texture2D spriteSheet,  PlayerState startingState)
			: base(health, characterPosition, rect, spriteSheet, startingState)
		{ 
			this.health = health;
			this.characterPosition = characterPosition;
			this.rect = rect;
			this.spriteSheet = spriteSheet;
			this.state = startingState;

			// Initialize
			fps = 10.0;					// Cycles through at 10 run frames per second
			timePerFrame = 1.0 / fps;   // amount of time in a single image
		}

		
		// -------------------------------
		// Methods
		// -------------------------------

		public void Update(Enemy enemy, KeyboardState kbState, KeyboardState kbPrevState)
		{
            Movement();
            Dash(kbState, kbPrevState);
			CheckCollision(enemy);

        }

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(spriteSheet, characterPosition, new Rectangle(0, 100, 25, 25), Color.White, 0, Vector2.Zero, 4f, SpriteEffects.None, 0);
		}

		public void Movement()
		{
            // State for the keyboard
            KeyboardState state = Keyboard.GetState();

            // Moves the player based on each of the four inputs
            if (state.IsKeyDown(Keys.W) == true)
            {
                characterPosition.Y -= movementSpeed;
                rect.Y -= movementSpeed;
            }
            if (state.IsKeyDown(Keys.A) == true)
            {
				characterPosition.X -= movementSpeed;	
                this.rect.X -= movementSpeed;
            }
            if (state.IsKeyDown(Keys.S) == true)
            {
				characterPosition.Y += movementSpeed;
                rect.Y += movementSpeed;
            }
            if (state.IsKeyDown(Keys.D) == true)
            {
				characterPosition.X += movementSpeed;
                rect.X += movementSpeed;
            }
            
        }

        public void CheckCollision(Enemy enemy)
        {
			if(this.rect.Intersects(enemy.Rect))
			{
				health--;
			}
				
        }

		public void Dash(KeyboardState kbState, KeyboardState kbPrevState)
		{
            

            if (kbState.IsKeyDown(Keys.W) == true && kbState.IsKeyDown(Keys.Space) == true && kbPrevState.IsKeyUp(Keys.Space) == true)
            {
                characterPosition.Y -= movementSpeed *15;
                rect.Y -= movementSpeed * 15;
            }
            if (kbState.IsKeyDown(Keys.A) == true && kbState.IsKeyDown(Keys.Space) == true && kbPrevState.IsKeyUp(Keys.Space) == true)
            {
                characterPosition.X -= movementSpeed * 15;
                this.rect.X -= movementSpeed * 15;
            }
            if (kbState.IsKeyDown(Keys.S) == true && kbState.IsKeyDown(Keys.Space) == true && kbPrevState.IsKeyUp(Keys.Space) == true)
            {
                characterPosition.Y += movementSpeed * 15;
                rect.Y += movementSpeed * 15;
            }
            if (kbState.IsKeyDown(Keys.D) == true && kbState.IsKeyDown(Keys.Space) == true && kbPrevState.IsKeyUp(Keys.Space) == true)
            {
                characterPosition.X += movementSpeed * 15;
                rect.X += movementSpeed * 15;
            }
        }
    }
}


