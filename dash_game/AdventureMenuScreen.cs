using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dash_game
{
	public class AdventureMenuScreen
	{
		// Fields
		private Rectangle[] levels = new Rectangle[6]
		{
            new Rectangle(200, 400, 256, 64),
            new Rectangle(506, 400, 256, 64),
            new Rectangle(812, 400, 256, 64),
            new Rectangle(200, 500, 256, 64),
			new Rectangle(506, 500, 256, 64),
			new Rectangle(812, 500, 256, 64)
		};

		private MouseState mState;
		private MouseState mPrevState;

		private string levelTitle;

		// Create the five levels that are hardcoded
		private char[,] level1 = new char[5, 5]
		{
			{'X', 'X', 'X', 'X', 'X' },
            {'X', 'I', 'X', 'B', 'X' },
            {'X', '3', 'S', '8', 'X' },
            {'X', '5', '4', 'X', 'X' },
            {'X', 'X', 'X', 'X', 'X' }
        };
		private char[,] level2 = new char[5, 5]
        {
            {'X', 'X', 'X', 'X', 'X' },
            {'X', '3', 'S', 'X', 'X' },
            {'X', '5', 'X', 'B', 'X' },
            {'X', 'I', '4', '3', 'X' },
            {'X', 'X', 'X', 'X', 'X' }
        };
        private char[,] level3 = new char[5, 5]
        {
            {'X', 'X', 'X', 'X', 'X' },
            {'X', '5', 'I', '4', 'X' },
            {'X', 'S', 'X', 'B', 'X' },
            {'X', '3', '3', '4', 'X' },
            {'X', 'X', 'X', 'X', 'X' }
        };
        private char[,] level4 = new char[5, 5]
        {
            {'X', 'X', 'X', 'X', 'X' },
            {'X', 'X', 'I', 'X', 'X' },
            {'X', 'B', '5', 'S', 'X' },
            {'X', 'X', '4', 'X', 'X' },
            {'X', 'X', 'X', 'X', 'X' }
        };
        private char[,] level5 = new char[7, 7]
        {
            {'X', 'X', 'X', 'X', 'X', 'X', 'X' },
            {'X', 'X', '3', '4', '5', '6', 'X' },
            {'X', 'X', '2', 'X', 'X', 'I', 'X' },
            {'X', 'S', '1', 'X', 'X', '7', 'X' },
            {'X', 'X', 'X', 'X', 'X', '8', 'X' },
            {'X', 'X', 'B', '9', '9', '9', 'X' },
            {'X', 'X', 'X', 'X', 'X', 'X', 'X' },
        };

        // Constructor
        public AdventureMenuScreen()
		{
		}

		// Methods
		
		/// <summary>
		/// Draws each of the level buttons and displays their title
		/// </summary>
		/// <param name="spriteBatch">Spritebatch for drawing everything</param>
		/// <param name="texture">Texture of the buttons</param>
		/// <param name="gameFont">Font of the labels</param>
		/// <param name="titleFont">Font for the title of the menu</param>
		public void Draw(SpriteBatch spriteBatch, Texture2D texture, SpriteFont gameFont, SpriteFont titleFont)
		{
			// Draw each of the boxes
			foreach (Rectangle rect in levels)
			{
				spriteBatch.Draw(texture, rect, new Rectangle(0, 0, 256, 64), Color.Black);
			}

            // Draw the title
            spriteBatch.DrawString(titleFont, "LEVEL SELECT", new Vector2(640, 200) - (titleFont.MeasureString("LEVEL SELECT") / 2), Color.Black);

			// Draws the title for each level
			spriteBatch.DrawString(gameFont, "Level 1", new Vector2(180, 400) + (gameFont.MeasureString("Level 1") / 2), Color.White);
			spriteBatch.DrawString(gameFont, "Level 2", new Vector2(486, 400) + (gameFont.MeasureString("Level 2") / 2), Color.White);
			spriteBatch.DrawString(gameFont, "Level 3", new Vector2(792, 400) + (gameFont.MeasureString("Level 3") / 2), Color.White);
            spriteBatch.DrawString(gameFont, "Level 4", new Vector2(180, 500) + (gameFont.MeasureString("Level 4") / 2), Color.White);
            spriteBatch.DrawString(gameFont, "Level 5", new Vector2(486, 500) + (gameFont.MeasureString("Level 5") / 2), Color.White);
            spriteBatch.DrawString(gameFont, "Custom", new Vector2(792, 500) + (gameFont.MeasureString("Custom") / 2), Color.White);
        }

		/// <summary>
		/// Checks if any button has been clicked
		/// </summary>
		/// <param name="currentLevel"></param>
		public bool Update(Level currentLevel)
		{
            // Update the states for the mouse
            mState = Mouse.GetState();

            // Run through each button and determine if the button is being pressed
            foreach (Rectangle button in levels)
            {
                if (mState.LeftButton != 0 && button.Contains(mState.Position) && button == levels[5])
                {
					currentLevel.CreateLevel();
					return true;
                }
				else if (mState.LeftButton != 0 && button.Contains(mState.Position))
				{
					currentLevel.CreateLevel(ButtonClick(button));
					return true;
				}
            }
			return false;
        }

		public char[,] ButtonClick(Rectangle button)
		{
			// Returns the proper level for the button clicked
			if (button == levels[0])
			{
				return level1;
			}
			else if (button == levels[1])
			{
				return level2;
			}
			else if (button == levels[2])
			{
				return level3;
			}
			else if (button == levels[3])
			{
				return level4;
			}
			else if (button == levels[4])
			{
				return level5;
			}
			return null;
		}
	}
}

