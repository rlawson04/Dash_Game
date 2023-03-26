﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dash_game
{
    /// <summary>
    /// Parent class for the Player and Enemy classes to inherit from
    /// </summary>
    public class Character
    {
        // Health and rectangle for collision
        protected int health;
        protected Rectangle rect;

        // Speed of the character
        protected int movementSpeed = 3;
      
        // Postion and state of character
        protected Vector2 characterPosition;
        protected PlayerState state;
       
        
        // To handle animation 
        protected Texture2D spriteSheet;
        protected int frame;
        protected double timeCounter;
        protected double fps;
        protected double timePerFrame;

        /// <summary>
		/// Gets the current health of the character
		/// </summary>
		public int Health
        {
            get { return health; }
            set { health = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle of the character for collision
        /// </summary>
        public Rectangle Rect 
        {
            get { return rect; }
            set { rect = value; }
        }

        /// <summary>
		/// Gets or sets the current state of the player 
		/// </summary>
		public PlayerState State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// Parameterized constructor of the character parent class
        /// </summary>
        /// <param name="health"> takes in an int for the character's health </param>
        /// <param name="rect"> takes in a rectangle to check collision </param>
        /// <param name="spriteSheet"> takes in a Texture2D to draw the character </param>
        /// <param name="startingState"></param>
        public Character(int health, Vector2 characterPosition, Rectangle rect, Texture2D spriteSheet, PlayerState startingState)
        {
            this.health = health;
            this.characterPosition = characterPosition;
            this.rect = rect;
            this.spriteSheet = spriteSheet;
            this.state = startingState;
        }

        
    }
}
