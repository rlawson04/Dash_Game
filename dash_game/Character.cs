using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* Name: Character
 * Purpose: Creates the interface that the enemy and player class will inherit
 * Modifications: Initial creation
 */

namespace dash_game
{
	public interface Character
	{
		/// <summary>
		/// Gets the current health of the character
		/// </summary>
		int Health  { get; set; }

		/// <summary>
		/// Gets or sets the rectangle of the character for collision
		/// </summary>
		Rectangle Rect { get; set; }	

		
	}
}

