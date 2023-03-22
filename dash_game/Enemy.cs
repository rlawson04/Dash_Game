using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
		private int health;
		private Rectangle rect;
		private string enemyType;
		private bool isBoss;

		// -------------------------------
		// Properties
		// -------------------------------

		/// <summary>
		/// Gets the current health of the player or sets it to the new value
		/// </summary>
		public int Health
		{
			get { return health; }
			set { health = value; }
		}

		/// <summary>
		/// Gets or sets the rectangle of the character for collision
		/// </summary>
		public Rectangle Rect
        {
			get { return rect; }
			set { rect = value; }
        }
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

		// -------------------------------
		//Constructor
		// -------------------------------

		/// <summary>
		/// Parameterized constructor for the player class 
		/// </summary>
		/// <param name="health"> takes an int to set the health of the player </param>
		public Enemy(int health)
		{
			this.health = health;
		}

		// -------------------------------
		// Methods
		// -------------------------------

	}
}

