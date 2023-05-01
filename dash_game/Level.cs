using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* Name: Level
 * Purpose: Defines the logic for the maps
 * Modifications: Added the logic for reading in a file, currently will just load the trial that I have made because I have not created a gui for the adventure menu
 * All logic for creating a level has been created and tested.
 * Finished adding comments that follow coding standards
 */

namespace dash_game
{
	public class Level
	{
		// Fields
		// Necessary fields for the rooms
		private Room current;
		private StreamReader reader;
		private Room[,] rooms;

		// Stores file names that are passed into create level to turn a text file into a level
		private string fileName;

		// An instance of the player that will be passed from Game 1
		private Player player;

		//field for boss for ending level
		private Enemy levelBoss;

		// Fields that deal with sprites and drawing of assets
        private SpriteBatch spriteBatch;
        private Texture2D charSprites;
		private Texture2D itemSprites;
		private Texture2D doorTexture;
		private Texture2D hitTexture;

		//property for Boss
		public Enemy LevelBoss { get { return levelBoss; } }

		// Constructor
		public Level(Player player, string fileName, Texture2D charSprites, Texture2D itemSprites, SpriteBatch spriteBatch, Texture2D doorTexture, Texture2D hitTexture)
		{
			this.player = player;
			this.fileName = fileName;
			this.charSprites = charSprites;
			this.itemSprites = itemSprites;
			this.spriteBatch = spriteBatch;
			this.doorTexture = doorTexture;
			this.hitTexture = hitTexture;
		}

		// Methods
		/// <summary>
		/// Creates the level from an inputted file
		/// </summary>
		public void CreateLevel()
		{
			// Read the file that is taken in, currently will only open one file.
			reader = new StreamReader("../../../Content/Levels/Custom.txt");

			// Create the array based on the first line of the file
			string[] dimensions = reader.ReadLine().Split(",");
			rooms = new Room[int.Parse(dimensions[0]), int.Parse(dimensions[1])];
			char[] data;

			// Fill in the array
			for (int i = 0; i < rooms.GetLength(0); i++)
			{
                data = reader.ReadLine().ToCharArray();
                for (int j = 0; j < rooms.GetLength(1); j++)
				{
					if (data[j] == 'X')
					{
						rooms[i, j] = null;
					}
					else if (data[j] == 'S')
					{
						rooms[i, j] = new Room(data[j], player, charSprites, spriteBatch, doorTexture, hitTexture, itemSprites);
						current = rooms[i, j];
					}
					else if (data[j] == 'B')
                    {
						rooms[i, j] = new Room(data[j], player, charSprites, spriteBatch, doorTexture, hitTexture, itemSprites);
						this.levelBoss = rooms[i, j].Enemies[0];
					}
					else
					{
						rooms[i, j] = new Room(data[j], player, charSprites, spriteBatch, doorTexture, hitTexture, itemSprites);
					}
				}
			}

			// Close the reader
			reader.Close();

			// Call LinkRooms on the starting room
			for (int i = 0; i < rooms.GetLength(0); i++)
			{
				for (int j = 0; j < rooms.GetLength(1); j++)
				{
					if (rooms[i,j] != null && rooms[i, j].CurrentRoomType == Room.RoomType.Starting)
					{
						LinkRooms(rooms[i, j], i, j);
					}
				}
			}
		}

		/// <summary>
		/// Create level method that uses premade hardcoded rooms instead of a file
		/// </summary>
		/// <param name="data">Array of characters that will become the rooms</param>
		public void CreateLevel(char[,] data)
		{
			// Set the bounds of the room array
			rooms = new Room[data.GetLength(0), data.GetLength(1)];

			// Create each room based on the data
			for (int i = 0; i < data.GetLength(0); i++)
			{
				for (int j = 0; j < data.GetLength(1); j++)
				{
                    if (data[i, j] == 'X')
                    {
                        rooms[i, j] = null;
                    }
                    else if (data[i, j] == 'S')
                    {
                        rooms[i, j] = new Room(data[i, j], player, charSprites, spriteBatch, doorTexture, hitTexture, itemSprites);
                        current = rooms[i, j];
                    }
                    else if (data[i, j] == 'B')
                    {
                        rooms[i, j] = new Room(data[i, j], player, charSprites, spriteBatch, doorTexture, hitTexture, itemSprites);
                        levelBoss = rooms[i, j].Enemies[0];
                    }
                    else
                    {
                        rooms[i, j] = new Room(data[i, j], player, charSprites, spriteBatch, doorTexture, hitTexture, itemSprites);
                    }
                }
			}

            // Run the logic to set the neighbors of each room
            for (int i = 0; i < rooms.GetLength(0); i++)
            {
                for (int j = 0; j < rooms.GetLength(1); j++)
                {
                    if (rooms[i, j] != null && rooms[i, j].CurrentRoomType == Room.RoomType.Starting)
                    {
                        LinkRooms(rooms[i, j], i, j);
                    }
                }
            }
        }

		/// <summary>
		/// Links all of the rooms in the array
		/// </summary>
		/// <param name="current">Current room to check</param>
		/// <param name="row">Row of current room</param>
		/// <param name="collumn">Collumn of current room</param>
		public void LinkRooms(Room current, int row, int collumn)
		{
			// Check all nodes north of current node
			if (rooms[row - 1, collumn] != null && current.North == null)
			{
				current.North = rooms[row - 1, collumn];
				LinkRooms(rooms[row - 1, collumn], row - 1, collumn);
			}

			// Check all nodes south of current node
			if (rooms[row + 1, collumn] != null && current.South == null)
            {
                current.South = rooms[row + 1, collumn];
                LinkRooms(rooms[row + 1, collumn], row + 1, collumn);
            }

            // Check all nodes east of current node
            if (rooms[row, collumn + 1] != null && current.East == null)
            {
                current.East = rooms[row, collumn + 1];
                LinkRooms(rooms[row, collumn + 1], row, collumn + 1);
            }

			// Check all nodes west of current node
			if (rooms[row, collumn - 1] != null && current.West == null)
            {
                current.West = rooms[row, collumn - 1];
                LinkRooms(rooms[row, collumn - 1], row, collumn - 1);
            }
        }

		/// <summary>
		/// Changes the room the player is in if they go through a door
		/// </summary>
		/// <param name="newRoom">The new room that the player will be sent to</param>
		public void MoveRoom()
		{
			Room newRoom;
			// If the player is intersecting with a door change the room
			newRoom = current.DoorLogic();
			if (newRoom != null)
			{
				current = newRoom;

				// If the room is a battle room change cleared to false
				if (current.CurrentRoomType == Room.RoomType.Battle || current.CurrentRoomType == Room.RoomType.Boss || current.CurrentRoomType == Room.RoomType.Item)
				{
					current.Cleared = false;
				}
				else
				{
					current.Cleared = true;
				}
			}
		}

		/// <summary>
		/// Calls all of the update methods of the current room
		/// </summary>
		public void Update()
		{
			current.Update();
		}

		/// <summary>
		/// Calls all of the draw methods for the current room
		/// </summary>
		public void Draw()
		{
			w current.Draw();
			current.DrawDoors();
		}
	}
}

