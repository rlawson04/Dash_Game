using System;

/* Name: Player
 * Purpose: Handles the player objects and methods associated with them
 * Modifications: Initial creation
 */

namespace dash_game
{
	public class Player : Character
	{
		// Enum
		private enum playerState
		{
			running,
			dashing,
			idle // Handles the idle animation
		}

		public Player()
		{
		}
	}
}

