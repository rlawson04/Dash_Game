using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* Name: Room
 * Purpose: Defines the logic for each room in the level
 * Modifications: Defined all of the logic for each type of room in the adventure mode, added properties for each room. Ensured that the player would be updated
 * Added comments that follow coding standards.
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
		private Texture2D hitTexture;

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
		/// <summary>
		/// Gets the current room type, used by level to define the starting room
		/// </summary>
        public RoomType CurrentRoomType
		{
			get { return currentRoomType; }
		}

		/// <summary>
		/// All directions include gets and sets so level creation works properly
		/// </summary>
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

		/// <summary>
		/// Allows the cleared value to be set in order to define rooms such as starting and item to have a default state of true
		/// </summary>
		public bool Cleared
		{
			set { cleared = value; }
		}

		public List<Enemy> Enemies { get { return enemies; } }

        // Constructor
        public Room(char data, Player player, Texture2D charSprites, SpriteBatch spriteBatch, Texture2D doorTexture, Texture2D hitTexture, Texture2D itemSprites)
		{
			// Set the player for the room
			this.player = player;
			this.spriteBatch = spriteBatch;
			this.doorTexture = doorTexture;
			this.hitTexture = hitTexture;

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
                    enemies.Add(new Enemy(2,
                        new Vector2(xPos, yPos),
                        new Rectangle(xPos, yPos, 75, 75),
                        charSprites,
                        PlayerState.IdleRight,
                        false,
                        "ninja",
						5));
                }

                // Set the room type
                currentRoomType = RoomType.Battle;
			}
			// I stands for item, create an item room
			else if (data == 'I')
			{
                // Create an item object in the middle of the room
                item = new Items(new Rectangle(rand.Next(100, 500), rand.Next(100, 500), 25, 25), "Speed Boost", false, itemSprites);

                // Set the room type
                currentRoomType = RoomType.Item;
			}
			// Boss rooms
			else if (data == 'B')
			{
				// Add the boss enemy as the only enemy in the list
				enemies.Add(new Enemy(500,
						new Vector2(500, 400),
						new Rectangle(500, 400, 150, 150),
						charSprites,
						PlayerState.IdleRight,
						true,
						"ninja",
						5));

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
		public void Update()
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
                        }
                    }

					// Logic for clearing a room
					if (enemies.Count == 0)
					{
						cleared = true;
					}
                    break;

				case RoomType.Item:
					item.CheckCollision(player);
					// Make sure that the player picks up the item
					if (item.PickedUp == true)
					{
						cleared = true;
					}
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

                        //update the enemy attack timer
                        enemy.AtkTimer = enemy.AtkTimer + 1;

                        //if the enemies attack timer is greater than 120, set their atk bool to true, thereby triggering their melee attack
                        if (enemy.AtkTimer > 120)
                        {
                            enemy.Atk = true;
                            enemy.MeleeAttack(spriteBatch, hitTexture);

                            //if the attack timer is greater than 130, end the attack by setting it to 0 and resetting the attack bool to false
                            //this results in a 10 frame attack
                            if (enemy.AtkTimer > 130)
                            {
                                enemy.AtkTimer = 0;
                                enemy.Atk = false;
                            }
                        }
                    }
                    break;

                case RoomType.Item:
					// Draw the item at the start of the room
					if (item.PickedUp == false)
					{
						item.Draw(spriteBatch);
					}
                    break;

                case RoomType.Boss:
                    // Draw each of the enemies
                    foreach (Enemy enemy in enemies)
                    {
                        enemy.Draw(spriteBatch);

                        //update the enemy attack timer
                        enemy.AtkTimer = enemy.AtkTimer + 1;

                        //if the enemies attack timer is greater than 120, set their atk bool to true, thereby triggering their melee attack
                        if (enemy.AtkTimer > 120)
                        {
                            enemy.Atk = true;
                            enemy.MeleeAttack(spriteBatch, hitTexture);

                            //if the attack timer is greater than 130, end the attack by setting it to 0 and resetting the attack bool to false
                            //this results in a 10 frame attack
                            if (enemy.AtkTimer > 130)
                            {
                                enemy.AtkTimer = 0;
                                enemy.Atk = false;
                            }
                        }
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
                return west;
			}

			return null;
		}
	}
}

