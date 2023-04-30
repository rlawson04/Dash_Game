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
        RunningUpLeft,
        RunningUpRight,
        RunningDownLeft,
        RunningDownRight,
        Dashing,
        IdleLeft,
        IdleRight,
        IdleUpRight,
        IdleUpLeft,
        IdleDownRight,
        IdleDownLeft,// Handles the idle animation 
    }

    public class Player : Character
	{
		// -------------------------------
		// Fields
		// -------------------------------

		private bool dashing;
        private int damage = 5;
        private int enemiesDefeated;

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
            set { damage = value; }
        }

        /// <summary>
        /// Allows the game to alter the position of the player, used for doors in the adventure mode
        /// </summary>
        public Vector2 CharacterPosition
        {
            set { characterPosition = value; }
        }

        /// <summary>
        /// Allows other classes to access and change the number of enemies defeated
        /// </summary>
        public int EnemiesDefeated
        {
            get { return enemiesDefeated; }
            set { enemiesDefeated = value; }
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
		public void Update(KeyboardState kbState, KeyboardState kbPrevState)
		{
            Movement(kbState, kbPrevState);
            Dash(kbState, kbPrevState);
        }
        
        /// <summary>
        /// Handles omni-directional movement
        /// </summary>
		public void Movement(KeyboardState kbState, KeyboardState kbPrevState)
		{
            // Depending on the inputs, the state of the player changes and the player may move around
            switch (State)
            {
                // Facing left, the player can face any other direction or run left
                case PlayerState.IdleLeft:
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        State = PlayerState.RunningLeft;
                    }
                    if (kbState.IsKeyDown(Keys.W) && kbPrevState.IsKeyUp(Keys.W))
                    {
                        State = PlayerState.IdleUpLeft;
                    }
                    if (kbState.IsKeyDown(Keys.D) && kbPrevState.IsKeyUp(Keys.D))
                    {
                        State = PlayerState.IdleRight;
                    }
                    if (kbState.IsKeyDown(Keys.S) && kbPrevState.IsKeyUp(Keys.S))
                    {
                        State = PlayerState.IdleDownLeft;
                    }
                    break;

                // Facing right, the player can face any other direction or run right
                case PlayerState.IdleRight:
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        State = PlayerState.RunningRight;
                    }
                    if (kbState.IsKeyDown(Keys.W) && kbPrevState.IsKeyUp(Keys.W))
                    {
                        State = PlayerState.IdleUpRight;
                    }
                    if (kbState.IsKeyDown(Keys.A) && kbPrevState.IsKeyUp(Keys.A))
                    {
                        State = PlayerState.IdleLeft;
                    }
                    if (kbState.IsKeyDown(Keys.S) && kbPrevState.IsKeyUp(Keys.S))
                    {
                        State = PlayerState.IdleDownRight;
                    }
                    break;

                // Facing up, the player can face any other direction or run up
                case PlayerState.IdleUpRight:
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        State = PlayerState.RunningUpRight;
                    }
                    if (kbState.IsKeyDown(Keys.A) && kbPrevState.IsKeyUp(Keys.A))
                    {
                        State = PlayerState.IdleLeft;
                    }
                    if (kbState.IsKeyDown(Keys.D) && kbPrevState.IsKeyUp(Keys.D))
                    {
                        State = PlayerState.IdleRight;
                    }
                    if (kbState.IsKeyDown(Keys.S) && kbPrevState.IsKeyUp(Keys.S))
                    {
                        State = PlayerState.IdleDownRight;
                    }
                    break;

                // Facing up, the player can face any other direction or run up
                case PlayerState.IdleUpLeft:
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        State = PlayerState.RunningUpLeft;
                    }
                    if (kbState.IsKeyDown(Keys.A) && kbPrevState.IsKeyUp(Keys.A))
                    {
                        State = PlayerState.IdleLeft;
                    }
                    if (kbState.IsKeyDown(Keys.D) && kbPrevState.IsKeyUp(Keys.D))
                    {
                        State = PlayerState.IdleRight;
                    }
                    if (kbState.IsKeyDown(Keys.S) && kbPrevState.IsKeyUp(Keys.S))
                    {
                        State = PlayerState.IdleDownLeft;
                    }
                    break;

                // Facing down, the player can face any other direction or run down
                case PlayerState.IdleDownLeft:
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        State = PlayerState.RunningDownLeft;
                    }
                    if (kbState.IsKeyDown(Keys.W) && kbPrevState.IsKeyUp(Keys.W))
                    {
                        State = PlayerState.IdleUpLeft;
                    }
                    if (kbState.IsKeyDown(Keys.A) && kbPrevState.IsKeyUp(Keys.A))
                    {
                        State = PlayerState.IdleLeft;
                    }
                    if (kbState.IsKeyDown(Keys.D) && kbPrevState.IsKeyUp(Keys.D))
                    {
                        State = PlayerState.IdleRight;
                    }
                    break;

                // Facing down, the player can face any other direction or run down
                case PlayerState.IdleDownRight:
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        State = PlayerState.RunningDownRight;
                    }
                    if (kbState.IsKeyDown(Keys.W) && kbPrevState.IsKeyUp(Keys.W))
                    {
                        State = PlayerState.IdleUpRight;
                    }
                    if (kbState.IsKeyDown(Keys.A) && kbPrevState.IsKeyUp(Keys.A))
                    {
                        State = PlayerState.IdleLeft;
                    }
                    if (kbState.IsKeyDown(Keys.D) && kbPrevState.IsKeyUp(Keys.D))
                    {
                        State = PlayerState.IdleRight;
                    }
                    break;

                // Running left, the player can face left or run up and down diagonally
                case PlayerState.RunningLeft:
                    if (kbState.IsKeyDown(Keys.A) && characterPosition.X > 32)
                    {
                        characterPosition.X -= movementSpeed;
                        rect.X -= movementSpeed;

                        if (kbState.IsKeyDown(Keys.W) == true && characterPosition.Y > 98)
                        {
                            characterPosition.Y -= movementSpeed;
                            rect.Y -= movementSpeed;
                        }
                        if (kbState.IsKeyDown(Keys.S) == true && (characterPosition.Y + rect.Height) < 605)
                        {
                            characterPosition.Y += movementSpeed;
                            rect.Y += movementSpeed;
                        }
                    }
                    else
                    {
                        State = PlayerState.IdleLeft;
                    }

                    break;

                // Running right, the player can face right or run up and down diagonally
                case PlayerState.RunningRight:
                    if (kbState.IsKeyDown(Keys.D) && (characterPosition.X + rect.Width) < 1216)
                    {
                        characterPosition.X += movementSpeed;
                        rect.X += movementSpeed;

                        if (kbState.IsKeyDown(Keys.W) == true && characterPosition.Y > 98)
                        {
                            characterPosition.Y -= movementSpeed;
                            rect.Y -= movementSpeed;
                        }
                        if (kbState.IsKeyDown(Keys.S) == true && (characterPosition.Y + rect.Height) < 605)
                        {
                            characterPosition.Y += movementSpeed;
                            rect.Y += movementSpeed;
                        }
                    }
                    else
                    {
                        State = PlayerState.IdleRight;
                    }

                    break;

                // Running up, the player can face up or run left and right diagonally
                case PlayerState.RunningUpRight:
                    if (kbState.IsKeyDown(Keys.W) == true && characterPosition.Y > 98)
                    {
                        characterPosition.Y -= movementSpeed;
                        rect.Y -= movementSpeed;

                        if (kbState.IsKeyDown(Keys.A) && characterPosition.X > 32)
                        {
                            characterPosition.X -= movementSpeed;
                            rect.X -= movementSpeed;
                        }
                        if (kbState.IsKeyDown(Keys.D) && (characterPosition.X + rect.Width) < 1216)
                        {
                            characterPosition.X += movementSpeed;
                            rect.X += movementSpeed;

                        }
                    }
                    else
                    {
                        State = PlayerState.IdleUpRight;
                    }

                    break;

                // Running up, the player can face up or run left and right diagonally
                case PlayerState.RunningUpLeft:
                    if (kbState.IsKeyDown(Keys.W) == true && characterPosition.Y > 98)
                    {
                        characterPosition.Y -= movementSpeed;
                        rect.Y -= movementSpeed;

                        if (kbState.IsKeyDown(Keys.A) && characterPosition.X > 32)
                        {
                            characterPosition.X -= movementSpeed;
                            rect.X -= movementSpeed;
                        }
                        if (kbState.IsKeyDown(Keys.D) && (characterPosition.X + rect.Width) < 1216)
                        {
                            characterPosition.X += movementSpeed;
                            rect.X += movementSpeed;

                        }
                    }
                    else
                    {
                        State = PlayerState.IdleUpLeft;
                    }

                    break;

                // Running down, the player can face down or run left and right diagonally
                case PlayerState.RunningDownRight:
                   
                    if (kbState.IsKeyDown(Keys.S) == true && (characterPosition.Y + rect.Height) < 605)
                    {
                        characterPosition.Y += movementSpeed;
                        rect.Y += movementSpeed;

                        if (kbState.IsKeyDown(Keys.A) && characterPosition.X > 32)
                        {
                            characterPosition.X -= movementSpeed;
                            rect.X -= movementSpeed;
                        }
                        if (kbState.IsKeyDown(Keys.D) && (characterPosition.X + rect.Width) < 1216)
                        {
                            characterPosition.X += movementSpeed;
                            rect.X += movementSpeed;

                        }
                    }
                    else
                    {
                        State = PlayerState.IdleDownRight;
                    }

                    break;

                // Running down, the player can face down or run left and right diagonally
                case PlayerState.RunningDownLeft:

                    if (kbState.IsKeyDown(Keys.S) == true && (characterPosition.Y + rect.Height) < 605)
                    {
                        characterPosition.Y += movementSpeed;
                        rect.Y += movementSpeed;

                        if (kbState.IsKeyDown(Keys.A) && characterPosition.X > 32)
                        {
                            characterPosition.X -= movementSpeed;
                            rect.X -= movementSpeed;
                        }
                        if (kbState.IsKeyDown(Keys.D) && (characterPosition.X + rect.Width) < 1216)
                        {
                            characterPosition.X += movementSpeed;
                            rect.X += movementSpeed;

                        }
                    }
                    else
                    {
                        State = PlayerState.IdleDownLeft;
                    }

                    break;
            }

           
        }

        /// <summary>
        /// Draws the character from the spritesheet and scales to size
        /// </summary>
        /// <param name="spriteBatch"> takes the spritebatch from the game's draw method </param>
		public void Draw(SpriteBatch spriteBatch)
        {
            // Draws the player based on their current state
            switch (state)
            {
                case PlayerState.RunningLeft:
                    DrawWalking(SpriteEffects.FlipHorizontally, spriteBatch);
                    break;

                case PlayerState.RunningRight:
                    DrawWalking(SpriteEffects.None, spriteBatch);
                    break;

                case PlayerState.RunningUpRight:
                    DrawWalking(SpriteEffects.None, spriteBatch);
                    break;

                case PlayerState.RunningUpLeft:
                    DrawWalking(SpriteEffects.FlipHorizontally, spriteBatch);
                    break;

                case PlayerState.RunningDownRight:
                    DrawWalking(SpriteEffects.None, spriteBatch);
                    break;

                case PlayerState.RunningDownLeft:
                    DrawWalking(SpriteEffects.FlipHorizontally, spriteBatch);
                    break;

                case PlayerState.IdleLeft:
                    DrawStanding(SpriteEffects.FlipHorizontally, spriteBatch);
                    break;

                case PlayerState.IdleRight:
                    DrawStanding(SpriteEffects.None, spriteBatch);
                    break;

                case PlayerState.IdleUpRight:
                    DrawStanding(SpriteEffects.None, spriteBatch);
                    break;

                case PlayerState.IdleUpLeft:
                    DrawStanding(SpriteEffects.FlipHorizontally, spriteBatch);
                    break;

                case PlayerState.IdleDownRight:
                    DrawStanding(SpriteEffects.None, spriteBatch);
                    break;

                case PlayerState.IdleDownLeft:
                    DrawStanding(SpriteEffects.FlipHorizontally, spriteBatch);
                    break;

                case PlayerState.Dashing:
                    DrawDashing(SpriteEffects.None, spriteBatch);
                    break;
            }
        }

        

        /// <summary>
        /// Draws the player dashing
        /// </summary>
        /// <param name="flipSprite"> effect to flip sprite if needed </param>
        /// <param name="spriteBatch"> spritebatch initialized in game </param>
        public void DrawDashing(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet,
                characterPosition,
                new Rectangle
                (124,
                rectOffsetY,
                rectWidth,
                rectHeight),
                Color.White, 0, Vector2.Zero, 4f,
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

        
        /// <summary>
        /// Updates Player's animation as necessary
        /// </summary>
        /// <param name="gameTime">Time information</param>
        public void UpdateAnimation(GameTime gameTime)
        {
            // Handle animation timing
            // - Add to the time counter
            // - Check if we have enough "time" to advance the frame

            // How much time has passed?  
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // If enough time has passed:
            if (timeCounter >= timePerFrame)
            {
                frame += 1;                     // Adjust the frame to the next image

                if (frame > walkFrameCount)     // Check the bounds - have we reached the end of walk cycle?
                    frame = 1;                  // Back to 1 (since 0 is the "standing" frame)

                timeCounter -= timePerFrame;    // Remove the time we "used" - don't reset to 0
                                                // This keeps the time passed 
            }
        }
        
    }
}


