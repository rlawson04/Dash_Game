using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* Name: Horde
 * Purpose: Handles all of the logic for the horde mode
 * Modifications: Created and finished the game loop
 */

namespace dash_game
{
	public class Horde
	{
		// List of enemies that will be drawn 
		private List<Enemy> enemies = new List<Enemy>();
		private Texture2D charSprites;
		private SpriteBatch _spriteBatch;
		private Texture2D hitTexture;

		// Use random to determine the spawnpoint for the enemies
		Random rand = new Random();

		// Data for the wave and number of enemies
		private int wave = 0;
		private int numEnemies = 3;

		// Attributes for completing waves
		private int score;

		// Attribute for the playing area to make sure that the player can't walk off screen
		public Rectangle bounds = new Rectangle(32, 98, 1216, 605);

		// Include an instance of the player
		private Player player;

		// Keyboard objects
		KeyboardState kbState;
		KeyboardState kbPrevState;

		// Properties
		public int Score
		{
			get { return score; }
			set { score = value; }
		}

		public int Wave
		{
			get { return wave; }
			set { wave = value; }
		}

		public List<Enemy> Enemies
        {
			get { return enemies; }
			set { enemies = value; }
        }

		public int NumEnemies
        {
			get { return numEnemies; }
			set { numEnemies = value; }
        }

		// Constructor
        public Horde(Texture2D charSprites, Player player, SpriteBatch _spriteBatch, Texture2D hitTexture)
        {
            this.charSprites = charSprites;
            this.player = player;
            this._spriteBatch = _spriteBatch;
            this.hitTexture = hitTexture;
        }

        // Methods
        /// <summary>
        /// Handles everything that will happen everytime there is a new wave
        /// </summary>
        public void newWave()
		{
			for (int i = 0; i < numEnemies; i++)
			{
				// Create an x and y position randomly
				int xPos = rand.Next(32, (1216 - 25));
				int yPos = rand.Next(98, (605 - 25));

				// Add enemies for num enemies
				enemies.Add(new Enemy((2 * wave + 5),
					new Vector2(xPos, yPos),
					new Rectangle(xPos, yPos, 75, 75),
					charSprites,
					PlayerState.IdleRight,
					false,
					"ninja",
					5));
			}

			// Increment wave and modify numEnemies
			wave++;
			numEnemies += 2 * wave;
		}

		/// <summary>
		/// Does all of the logic if the enemy and player collide
		/// </summary>
		public void Update()
		{
			// Goes to the next wave if there are no enemies left
			if (enemies == null || enemies.Count == 0)
			{
				newWave();
			}

			// Updates the enemies
			foreach (Enemy enemy in enemies)
			{
				

				// Checks for collision
				if (enemy.Rect.Intersects(player.Rect))
				{
					enemy.Health -= player.Damage;
					score += 5;
				}

				//checks for collision with hitbox of attack
				if (enemy.Hitbox.Intersects(player.Rect) && enemy.Atk == true)
                {
					player.Health -= enemy.Damage;
                }

			}

			// If an enemies health is zero remove it
			for (int i = 0; i < enemies.Count; i++)
			{
				if (enemies[i].Health <= 0)
				{
					enemies.Remove(enemies[i]);
					score += 10;
				}
			}
		}

		/// <summary>
		/// Draws the enemies
		/// </summary>
		public void Draw()
		{
			// Draws each enemy
			foreach (Enemy enemy in enemies)
			{
				enemy.Draw(_spriteBatch);

				//update the enemy attack timer
				enemy.AtkTimer = enemy.AtkTimer + 1;

				//if the enemies attack timer is greater than 120, set their atk bool to true, thereby triggerin their melee attack
				if  (enemy.AtkTimer > 120)
				{
					enemy.Atk = true;
					enemy.MeleeAttack(_spriteBatch, hitTexture);
					
					//if the attack timer is greater than 130, end the attack by setting it to 0 and resetting the attack bool to false
					//this results in a 10 frame attack
					if (enemy.AtkTimer > 130)
					{
						enemy.AtkTimer = 0;
						enemy.Atk = false;
					}
				}
			}
		}
	}
}

