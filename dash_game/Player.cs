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
        private int damage = 5;

		// -------------------------------
		// Properties
		// -------------------------------

        /// <summary>
        /// Gets and  sets the movement speed of the player
        /// </summary>
        public int MovementSpeed
        {
            get { return movementSpeed; }
            set { movementSpeed = value; }

        }

        /// <summary>
        /// Get and set property to determine if the player is dashing, or to change it
        /// </summary>
        public bool isDashing
        {
            get { return dashing; }
            set { dashing = value; }
        }

        /// <summary>
        /// Gets the players damage value
        /// </summary>
        public int Damage
        {
            get { return damage; }
        }

		// -------------------------------
		// Constructor
		// -------------------------------

		/// <summary>
        /// Parameterized constructor for the Player child class
        /// </summary>
        /// <param name="health"> takes an int to set as the health value </param>
        /// <param name="characterPosition"> takes a vector 2 to keep track of the location </param>
        /// <param name="rect"> takes a rectangle to check collision </param>
        /// <param name="spriteSheet"> takes a texture2D for drawing and animating </param>
        /// <param name="startingState"> takes the current state for animation </param>
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

        /// <summary>
        /// Updates the player's movement, dashing, and collision
        /// </summary>
        /// <param name="enemy"> takes an instance of the enemy class </param>
        /// <param name="kbState"> takes the game classes current kbstate </param>
        /// <param name="kbPrevState"> takes the game classes previous kbstate </param>
		public void Update(Enemy enemy, KeyboardState kbState, KeyboardState kbPrevState)
		{
            Movement(kbState);
            Dash(kbState, kbPrevState);
			

        }

        /// <summary>
        /// Draws the character from the spritesheet and scales to size
        /// </summary>
        /// <param name="spriteBatch"> takes the spritebatch from the game's draw method </param>
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(spriteSheet, characterPosition, 
                new Rectangle(0, 100, 25, 25), 
                Color.White, 0, Vector2.Zero, 4f, 
                SpriteEffects.None, 0);
		}

        /// <summary>
        /// Handles the movement in the 4 directions
        /// </summary>
		public void Movement(KeyboardState kbState)
		{
            // Moves the player position and rectangle based on each of the four inputs
            if (kbState.IsKeyDown(Keys.W) == true && characterPosition.Y > 98)
            {
                characterPosition.Y -= movementSpeed;
                rect.Y -= movementSpeed;
            }
            if (kbState.IsKeyDown(Keys.A) == true && characterPosition.X > 32)
            {
				characterPosition.X -= movementSpeed;	
                this.rect.X -= movementSpeed;
            }
            if (kbState.IsKeyDown(Keys.S) == true && (characterPosition.Y + rect.Height) < 605)
            {
				characterPosition.Y += movementSpeed;
                rect.Y += movementSpeed;
            }
            if (kbState.IsKeyDown(Keys.D) == true && (characterPosition.X + rect.Width) < 1216)
            {
				characterPosition.X += movementSpeed;
                rect.X += movementSpeed;
            }
            
        }

        public void DrawWalking(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, 
                this.characterPosition, 
                new Rectangle(
                    30,10,25,25
                    ), 
                Color.White, 0, 
                Vector2.Zero, 4.0f, 
                flipSprite, 0);
        }

        /// <summary>
        /// Dashes in the direction being moved when space is pressed
        /// </summary>
        /// <param name="kbState"> takes the current state from the game class </param>
        /// <param name="kbPrevState"> takes the previous state from the game class </param>
		public void Dash(KeyboardState kbState, KeyboardState kbPrevState)
		{
            
            // Checks the direction the player is moving and
            // then checks if space was pressed once for all directions
            if (kbState.IsKeyDown(Keys.W) == true && kbState.IsKeyDown(Keys.Space) == true
                && kbPrevState.IsKeyUp(Keys.Space) == true)
            {
                characterPosition.Y -= movementSpeed * 15;
                rect.Y -= movementSpeed * 15;
            }
            if (kbState.IsKeyDown(Keys.A) == true && kbState.IsKeyDown(Keys.Space) == true
                && kbPrevState.IsKeyUp(Keys.Space) == true)
            {
                characterPosition.X -= movementSpeed * 15;
                this.rect.X -= movementSpeed * 15;
            }
            if (kbState.IsKeyDown(Keys.S) == true && kbState.IsKeyDown(Keys.Space) == true
                && kbPrevState.IsKeyUp(Keys.Space) == true)
            {
                characterPosition.Y += movementSpeed * 15;
                rect.Y += movementSpeed * 15;
            }
            if (kbState.IsKeyDown(Keys.D) == true && kbState.IsKeyDown(Keys.Space) == true
                && kbPrevState.IsKeyUp(Keys.Space) == true)
            {
                characterPosition.X += movementSpeed * 15;
                rect.X += movementSpeed * 15;
            }
        }
    }
}


