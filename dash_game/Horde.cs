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
		}

		public int Wave
		{
			get { return wave; }
		}

		// Constructor
		public Horde(Texture2D charSprites, Player player, SpriteBatch _spriteBatch)
		{
			this.charSprites = charSprites;
			this.player = player;
			this._spriteBatch = _spriteBatch;
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
					new Rectangle(xPos, yPos, 25, 25),
					charSprites,
					PlayerState.Idle,
					false,
					"ninja"));
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
				// Update the player
				player.Update(enemy, kbState, kbPrevState);

				// Checks for collision
				if (enemy.Rect.Intersects(player.Rect))
				{
					enemy.Health -= player.Damage;
					score += 5;
				}
			}

			// If an enemies health is zero remove it
			for (int i = 0; i < enemies.Count; i++)
			{
				if (enemies[i].Health <= 0)
				{
					enemies.Remove(enemies[i]);
					player.Health += 50;
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
			}
		}
	}
}

