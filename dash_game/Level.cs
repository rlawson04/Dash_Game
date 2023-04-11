using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* Name: Level
 * Purpose: Defines the logic for the maps
 * Modifications: Added the logic for reading in a file, currently will just load the trial that I have made because I have not created a gui for the adventure menu
 * All logic for creating a level has been created and tested.
 */

namespace dash_game
{
	public class Level
	{
		// Fields
		private Room current;
		private StreamReader reader;
		private Room[,] rooms;
		private string fileName;
		private Player player;
		private Texture2D charSprites;
		private Texture2D itemSprites;
		private SpriteBatch spriteBatch;
		private Texture2D doorTexture;

		// Constructor
		public Level(Player player, string fileName, Texture2D charSprites, Texture2D itemSprites, SpriteBatch spriteBatch, Texture2D doorTexture)
		{
			this.player = player;
			this.fileName = fileName;
			this.charSprites = charSprites;
			this.spriteBatch = spriteBatch;
			this.doorTexture = doorTexture;
		}

		// Methods
		/// <summary>
		/// Creates the level from an inputted file
		/// </summary>
		public void CreateLevel()
		{
			// Read the file that is taken in, currently will only open one file.
			reader = new StreamReader("../../../Content/TrialLevel.txt");

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
						rooms[i, j] = new Room(data[j], player, charSprites, spriteBatch, doorTexture);
						current = rooms[i, j];
					}
					else
					{
						rooms[i, j] = new Room(data[j], player, charSprites, spriteBatch, doorTexture);
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
				if (current.CurrentRoomType == Room.RoomType.Battle || current.CurrentRoomType == Room.RoomType.Boss)
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
			current.Update(itemSprites);
		}

		/// <summary>
		/// Calls all of the draw methods for the current room
		/// </summary>
		public void Draw()
		{
			current.Draw();
			current.DrawDoors();
		}
	}
}

