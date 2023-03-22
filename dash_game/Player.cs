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
	public class Player : Character
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

		// -------------------------------
		// Fields
		// -------------------------------
		
		// Health and rectangle for collision
		private int health;
		private Rectangle rect;

		// Postion and state of player
		private Vector2 playerPosition;
		private PlayerState state;
		
		// To handle animation 
		private Texture2D spriteSheet;
		private int frame;
		private double timeCounter;
		private double fps;
		private double timePerFrame;
		

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
		/// Gets or sets the rectangle of the Player for collision
		/// </summary>
		public Rectangle Rect 
		{ 
			get { return rect; }
			set { rect = value; }
		}

		/// <summary>
		/// Gets or sets the current state of the player 
		/// </summary>
		public PlayerState State
        {
			get { return state; }
			set { state = value; }
        }

		/// <summary>
		/// Get and set the current location of the player
		/// </summary>
		public float X
		{
			get { return this.playerPosition.X; }
			set { this.playerPosition.X = value; }
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
		public Player(int health, Rectangle rect, Texture2D spriteSheet, Vector2 playerPostition, PlayerState startingState)
		{ 
			this.health = health;
			this.rect = rect;
			this.spriteSheet = spriteSheet;
			this.playerPosition = playerPostition;
			this.state = startingState;

			// Initialize
			fps = 10.0;					// Cycles through at 10 run frames per second
			timePerFrame = 1.0 / fps;   // amount of time in a single image
		}

		
		// -------------------------------
		// Methods
		// -------------------------------

	}
}


