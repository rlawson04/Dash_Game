using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* Name: Room
 * Purpose: Defines the logic for each room in the level
 * Modifications: Defined all of the logic for each type of room in the adventure mode, added properties for each room
 */

namespace dash_game
{
	public class Room
	{
		// Enum for the room type
		public enum RoomType
		{
			Starting,
			Battle,
			Item,
			Boss,
		}

		// Fields
		// Data for enemies
		private List<Enemy> enemies = new List<Enemy>();
		private int numEnemies;

		// Tracking rooms
		private Room north;
		private Room south;
		private Room east;
		private Room west;
		private RoomType currentRoomType;
		private bool cleared;

		// Rectangles for the doors
		private Rectangle northDoor;
		private Rectangle southDoor;
		private Rectangle eastDoor;
		private Rectangle westDoor;

		// Needed for draw method
		private SpriteBatch spriteBatch;

		// Instance of random
		Random rand = new Random();

		// Instance of player that will be passed from the overall level
		private Player player;

		// Instance of an item that can be called upon in item rooms
		private Items item;

		// Keyboard States
		KeyboardState kbState;
		KeyboardState kbPrevState;

		// Properties
		public RoomType CurrentRoomType
		{
			get { return currentRoomType; }
		}

		public Room North
		{
			get { return north; }
			set { north = value; }
		}

        public Room South
        {
			get { return south; }
            set { south = value; }
        }

        public Room East
        {
			get { return east; }
            set { east = value; }
        }

        public Room West
        {
			get { return west; }
            set { west = value; }
        }

        // Constructor
        public Room(char data, Player player, Texture2D charSprites)
		{
			// Set the player for the room
			this.player = player;

			// If data is a number, spawn the enemies
			if (int.TryParse(data.ToString(), out numEnemies))
			{
                // Loop to add enemies to the list
                for (int i = 0; i < numEnemies; i++)
                {
                    // Create an x and y position randomly
                    int xPos = rand.Next(32, (1216 - 25));
                    int yPos = rand.Next(98, (605 - 25));

                    // Add enemies for num enemies
                    enemies.Add(new Enemy((15),
                        new Vector2(xPos, yPos),
                        new Rectangle(xPos, yPos, 25, 25),
                        charSprites,
                        PlayerState.Idle,
                        false,
                        "ninja"));
                }

                // Set the room type
                currentRoomType = RoomType.Battle;
			}
			// I stands for item, create an item room
			else if (data == 'I')
			{
				// Create an item object in the middle of the room

				// Set the room type
				currentRoomType = RoomType.Item;
			}
			// Boss rooms
			else if (data == 'B')
			{
				// Add the boss enemy as the only enemy in the list
				enemies.Add(new Enemy(50,
						new Vector2(500, 400),
						new Rectangle(500, 400, 50, 50),
						charSprites,
						PlayerState.Idle,
						true,
						"ninja"));

                // Set the room type
                currentRoomType = RoomType.Boss;
			}
			// Starting room, does not spawn anything
			else if (data == 'S')
			{
				// Set the room type
				currentRoomType = RoomType.Starting;
			}
		}

		// Methods
		/// <summary>
		/// Updates the room according to the room type
		/// </summary>
		public void Update(Texture2D speedArrow)
		{
			// Switch for the room type
			switch (currentRoomType)
			{
				case RoomType.Starting:
					break;

				case RoomType.Battle:
                    // Updates the enemies
                    foreach (Enemy enemy in enemies)
                    {
                        // Update the player
                        player.Update(enemy, kbState, kbPrevState);

                        // Checks for collision
                        if (enemy.Rect.Intersects(player.Rect))
                        {
                            enemy.Health -= player.Damage;
                        }
                    }

                    // If an enemies health is zero remove it
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i].Health <= 0)
                        {
                            enemies.Remove(enemies[i]);
                            player.Health += 50;
                        }
                    }

					// Logic for clearing a room
					if (enemies.Count == 0)
					{
						cleared = true;
					}
                    break;

				case RoomType.Item:
					item = new Items(new Rectangle(rand.Next(100, 500), rand.Next(100, 500), 25, 25), "Speed Boost", false, speedArrow);
					item.CheckCollision(player);
					break;

				case RoomType.Boss: // Runs the same logic as battle because the boss is currently treated as a special enemy
                    // Updates the enemies
                    foreach (Enemy enemy in enemies)
                    {
                        // Update the player
                        player.Update(enemy, kbState, kbPrevState);

                        // Checks for collision
                        if (enemy.Rect.Intersects(player.Rect))
                        {
                            enemy.Health -= player.Damage;
                        }
                    }

                    // If an enemies health is zero remove it
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i].Health <= 0)
                        {
                            enemies.Remove(enemies[i]);
                            player.Health += 50;
                        }
                    }
                    break;
			}
		}

		/// <summary>
		/// Draws everything for the current room
		/// </summary>
		public void Draw()
		{
            // Switch for the room type
            switch (currentRoomType)
            {
                case RoomType.Battle:
					// Draw each of the enemies
					foreach (Enemy enemy in enemies)
					{
						enemy.Draw(spriteBatch);
					}
                    break;

                case RoomType.Item:
                    break;

                case RoomType.Boss:
                    // Draw each of the enemies
                    foreach (Enemy enemy in enemies)
                    {
                        enemy.Draw(spriteBatch);
                    }
                    break;
            }
        }

		/// <summary>
		/// Draws the corresponding doors if they exist
		/// </summary>
		public void DrawDoors()
		{
			if (north != null)
			{

			}

			if (south != null)
			{

			}

			if (east != null)
			{

			}

			if (west != null)
			{

			}
		}

		/// <summary>
		/// Defines the logic of going through a door
		/// </summary>
		/// <returns>Returns the room that the player has entered</returns>
		public Room DoorLogic()
		{
			if (north != null && northDoor.Intersects(player.Rect))
			{
				return north;
			}

			if (south != null && southDoor.Intersects(player.Rect))
			{
				return south;
			}

			if (east != null && eastDoor.Intersects(player.Rect))
			{
				return east;
			}

			if (south != null && southDoor.Intersects(player.Rect))
			{
				return south;
			}

			return null;
		}
	}
}

