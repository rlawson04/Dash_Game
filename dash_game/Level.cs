using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* Name: Level
 * Purpose: Defines the logic for the maps
 * Modifications: Added the logic for reading in a file, currently will just load the trial that I have made because I have not created a gui for the adventure menu
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

		// Constructor
		public Level(Player player, string fileName, Texture2D charSprites)
		{
			this.player = player;
			this.fileName = fileName;
			this.charSprites = charSprites;
		}

		// Methods
		/// <summary>
		/// Creates the level from an inputted file
		/// </summary>
		public void CreateLevel()
		{
			// Read the file that is taken in, currently will only open one file.
			reader = new StreamReader("TrialLevel");

			// Create the array based on the first line of the file
			string[] dimensions = reader.ReadLine().Split(",");
			rooms = new Room[int.Parse(dimensions[0]), int.Parse(dimensions[1])];

			// Fill in the array
			for (int i = 0; i < rooms.GetLength(0); i++)
			{
				for (int j = 0; j < rooms.GetLength(1); j++)
				{
					char data = char.Parse(reader.Read().ToString());
					if (data == 'X')
					{
						rooms[i, j] = null;
					}
					else
					{
						rooms[i, j] = new Room(data, player, charSprites);
					}
				}
				reader.ReadLine();
			}

			// Close the reader
			reader.Close();

			// Call LinkRooms on the starting room
			for (int i = 0; i < rooms.GetLength(0); i++)
			{
				for (int j = 0; j < rooms.GetLength(1); j++)
				{
					if (rooms[i, j].CurrentRoomType == Room.RoomType.Starting)
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
			if (rooms[row, collumn + 1] != null && current.North == null)
			{
				current.North = rooms[row, collumn + 1];
				LinkRooms(rooms[row, collumn + 1], row, collumn + 1);
			}

			// Check all nodes south of current node
			if (rooms[row, collumn - 1] != null && current.South == null)
            {
                current.South = rooms[row, collumn - 1];
                LinkRooms(rooms[row, collumn - 1], row, collumn - 1);
            }

            // Check all nodes east of current node
            if (rooms[row + 1, collumn] != null && current.East == null)
            {
                current.East = rooms[row + 1, collumn];
                LinkRooms(rooms[row + 1, collumn], row + 1, collumn);
            }

			// Check all nodes west of current node
			if (rooms[row - 1, collumn] != null && current.West == null)
            {
                current.West = rooms[row - 1, collumn];
                LinkRooms(rooms[row - 1, collumn], row - 1, collumn);
            }
        }
	}
}

