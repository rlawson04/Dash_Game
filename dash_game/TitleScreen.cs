using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* Name: Title Screen
 * Purpose: Manages everything that will be drawn and interacted with on the title screen.
 * Modifications: Finished everything for the title screen
 */

namespace dash_game
{
	public class TitleScreen
	{
		// Create an array for the buttons
		Rectangle[] buttons = new Rectangle[3]
		{
			new Rectangle(200, 450, 256, 64),
			new Rectangle(506, 450, 256, 64),
			new Rectangle(812, 450, 256, 64)
		};

		// Create mouse states
		MouseState mState;
		MouseState mPrevState;

		/// <summary>
		/// Draws each of the needed buttons for the title screen
		/// </summary>
		/// <param name="_spriteBatch"></param>
		/// <param name="texture">Texture to use for the buttons</param>
		public void Draw(SpriteBatch _spriteBatch, Texture2D texture, SpriteFont gameFont)
		{
			// Draws each button in the array
			foreach (Rectangle button in buttons)
			{
				_spriteBatch.Draw(texture, button, new Rectangle(0, 0, 256, 64), Color.Black);
			}

			// Draws the text for each button individually
			_spriteBatch.DrawString(gameFont, "Horde", new Vector2(200, 450) + (gameFont.MeasureString("Horde") / 2), Color.WhiteSmoke);
			_spriteBatch.DrawString(gameFont, "Adventure", new Vector2(420, 450) + (gameFont.MeasureString("Adventure") / 2), Color.WhiteSmoke);
			_spriteBatch.DrawString(gameFont, "Stats", new Vector2(822, 450) + (gameFont.MeasureString("Stats") / 2), Color.WhiteSmoke);
		}

		/// <summary>
		/// Runs through each button and checks if the user has clicked it.
		/// </summary>
		public int Update()
		{
			// Update the states for the mouse
			mState = Mouse.GetState();

			// Run through each button and determine if the button is being pressed
			foreach (Rectangle button in buttons)
			{
				if (mState.LeftButton != 0 && button.Contains(mState.Position))
				{
					return OnClick(button);
				}
			}

			mPrevState = mState;
			return 10;
		}

		public int OnClick(Rectangle button)
		{
			if (button == buttons[0])
			{
				return 1;
			}
			else if (button == buttons[1])
			{
				return 2;
			}
			else
			{
				return 3;
			}
		}
	}
}

