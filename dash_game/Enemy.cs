using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml;
using System.Collections.Generic;

/* Name: Enemy
 * Purpose: Handles the creation of enemies and methods associated with them
 * Modifications: Initial creation
 */

namespace dash_game
{
	public class Enemy : Character
	{
		// -------------------------------
		// Fields
		// -------------------------------
		
		// Specific information for the enemy
		private string enemyType;
		private bool isBoss;
		private Random rng = new Random();
		private int tempRNG;
		private Rectangle hitbox;
		private int damage;
		private Vector2 adjustedPos;
		private int atkTimer = 0;
		private bool atk = false;

		// -------------------------------
		// Properties
		// -------------------------------

		/// <summary>
		/// Gets the type of the enemy or sets their type
		/// </summary>
		public string EnemyType
		{ 
			get { return enemyType; } 
			set { enemyType = value; } 
		}	

		/// <summary>
		/// Gets whether the enemy is a boss or sets whether it is or not
		/// </summary>
		public bool IsBoss
		{ 
			get { return isBoss; } 
			set { isBoss = value; }
		}

		/// <summary>
		/// Gets or sets the hitbox of the attack
		/// </summary>
		public Rectangle Hitbox
        {
			get { return hitbox; }
			set { hitbox = value; }
        }

		/// <summary>
		/// Gets or sets the damage an enemy deals
		/// </summary>
		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

        /// <summary>
        /// Gets or sets the int that represents the timer, deciding when enemies attack and for how long
        /// </summary>
        public int AtkTimer
		{
			get { return atkTimer; }
			set { atkTimer = value; }
		}

        /// <summary>
        /// Gets or sets whether or net the atk is active, to be used to prevent lingering hitboxes from hurting the player
        /// </summary>
        public bool Atk
		{
			get { return atk; }
			set { atk = value; }
		}


		// -------------------------------
		//Constructor
		// -------------------------------

		/// <summary>
		/// Parameterized constructor for the Enemy child class
		/// </summary>
		/// <param name="health"> takes an int to set as the health value </param>
		/// <param name="characterPosition"> takes a vector 2 to keep track of the location </param>
		/// <param name="rect"> takes a rectangle to check collision </param>
		/// <param name="spriteSheet"> takes a texture2D for drawing and animating </param>
		/// <param name="startingState"> takes the current state for animation </param>
		public Enemy(int health, Vector2 characterPosition, Rectangle rect, Texture2D spriteSheet, PlayerState startingState, bool isBoss, string enemyType, int damage)
            : base(health, characterPosition, rect, spriteSheet,  startingState)
		{
            this.health = health;
			this.characterPosition = characterPosition;
            this.rect = rect;
            this.spriteSheet = spriteSheet;
            this.state = startingState;
			this.isBoss = isBoss;
			this.enemyType = enemyType;
			this.damage = damage;
			tempRNG = rng.Next(2);

			// Initialize
			fps = 10.0;                 // Cycles through at 10 run frames per second
            timePerFrame = 1.0 / fps;   // amount of time in a single image
        }

        // -------------------------------
        // Methods
        // -------------------------------

		/// <summary>
		/// Draws the enemy from the sprite sheet and alters its color and size
		/// </summary>
		/// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
		{
			//50/50 chance of facing left or right
			if (tempRNG == 0)
            {
				spriteBatch.Draw(spriteSheet, characterPosition, new Rectangle(0, 100, 25, 25), Color.Red, 0, Vector2.Zero, 4f, SpriteEffects.None, 0);
			}
			else if (tempRNG == 1)
            {
				//if facing left, agjust the position used to draw it to account for horizontal flip
				adjustedPos = new Vector2(characterPosition.X + 25, characterPosition.Y);
				spriteBatch.Draw(spriteSheet, adjustedPos, new Rectangle(0, 100, 25, 25), Color.Red, 0, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, 0);
			}
        }

		//Melee attack method
		//intended to create a hitbox in front of the enemy
		public void MeleeAttack(SpriteBatch _spriteBatch, Texture2D hitTexture)
        {
			//if tempRNG equals zero they are facing right
			if (tempRNG == 0)
            {
				//create a hitbox for the attack, in front of the enemy it is tied to, then draw a rectangle representing the hitbox.
				//This rectangle to be replaced with sword sprite upin suitablt sprite being found
				hitbox = new Rectangle((int)characterPosition.X + 90, (int)characterPosition.Y + 25, 30, 65);
				_spriteBatch.Draw(hitTexture, hitbox, Color.Red);
			}

			//these enemieas are facing left
			else if (tempRNG == 1)
            {
				//create a hitbox for the attack, the rectangle here has to be adjusted slighly to account for the horizontal flip of an
				//enemy not flipping at the center of the sprite but rather from the origin. Then draw the rectangle representing the hitbox, again
				//with adjusted coordinates due to the flip.
                hitbox = new Rectangle((int)adjustedPos.X - 50, (int)adjustedPos.Y + 20, 30, 65);
				_spriteBatch.Draw(hitTexture, new Rectangle (hitbox.X + 30, hitbox.Y + 5, 30, 65), Color.Red);
			}
		}
        
	}
}

