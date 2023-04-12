﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* Name: Room
 * Purpose: Defines the logic for each room in the level
 * Modifications: Defined all of the logic for each type of room in the adventure mode, added properties for each room. Ensured that the player would be updated
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

		// Rectangles for the doors and a texture
		private Rectangle northDoor = new Rectangle((1223 / 2), 78, 25, 50);
		private Rectangle southDoor = new Rectangle((1223 / 2), 663, 25, 50);
		private Rectangle eastDoor = new Rectangle(1228, 339, 50, 25);
		private Rectangle westDoor = new Rectangle(12, 339, 50, 25);
		private Texture2D doorTexture;

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

		public bool Cleared
		{
			set { cleared = value; }
		}

        // Constructor
        public Room(char data, Player player, Texture2D charSprites, SpriteBatch spriteBatch, Texture2D doorTexture)
		{
			// Set the player for the room
			this.player = player;
			this.spriteBatch = spriteBatch;
			this.doorTexture = doorTexture;

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
                    enemies.Add(new Enemy(5,
                        new Vector2(xPos, yPos),
                        new Rectangle(xPos, yPos, 25, 25),
                        charSprites,
                        PlayerState.IdleRight,
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
						new Rectangle(500, 400, 100, 100),
						charSprites,
						PlayerState.IdleRight,
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
			// Get keyboard state
			kbState = Keyboard.GetState();

			// Update the player
			player.Update(kbState, kbPrevState);

			// Switch for the room type
			switch (currentRoomType)
			{
				case RoomType.Starting:
                    cleared = true;
					break;

				case RoomType.Battle:
                    // Updates the enemies
                    foreach (Enemy enemy in enemies)
                    {
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

					// Set cleared to true if everything has been defeated
					if (enemies.Count == 0)
					{
						cleared = true;
					}
                    break;
			}
			// Set the previous state to the current
			kbPrevState = kbState;
		}

		/// <summary>
		/// Draws everything for the current room
		/// </summary>
		public void Draw()
		{
			// Player will always be drawn
			player.Draw(spriteBatch);

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
					// Draw the item at the start of the room
					item.Draw(spriteBatch);
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
			if (north != null && cleared)
			{
				spriteBatch.Draw(doorTexture, northDoor, Color.White);
			}

			if (south != null && cleared)
			{
				spriteBatch.Draw(doorTexture, southDoor, Color.White);
			}

			if (east != null && cleared)
			{
				spriteBatch.Draw(doorTexture, eastDoor, Color.White);
			}

			if (west != null && cleared)
			{
				spriteBatch.Draw(doorTexture, westDoor, Color.White);
			}
		}

		/// <summary>
		/// Defines the logic of going through a door
		/// </summary>
		/// <returns>Returns the room that the player has entered</returns>
		public Room DoorLogic()
		{
			if (north != null && northDoor.Intersects(player.Rect) && cleared)
			{
				player.CharacterPosition = new Vector2((1223 / 2), 643);
				player.Rect = new Rectangle((1223 / 2), 623, 25, 25);
				return north;
			}

			if (south != null && southDoor.Intersects(player.Rect) && cleared)
			{
                player.CharacterPosition = new Vector2((1223 / 2), 118);
                player.Rect = new Rectangle((1223 / 2), 138, 25, 25);
                return south;
			}

			if (east != null && eastDoor.Intersects(player.Rect) && cleared)
			{
                player.CharacterPosition = new Vector2(52, 339);
                player.Rect = new Rectangle(52, 339, 25, 25);
                return east;
			}

			if (west != null && westDoor.Intersects(player.Rect) && cleared)
			{
                player.CharacterPosition = new Vector2(1208, 339);
                player.Rect = new Rectangle(1208, 339, 25, 25);
                return south;
			}

			return null;
		}
	}
}
