using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dash_game
{
    internal class Items
    {
        // -------------------------------
        // Fields
        // -------------------------------

        private Rectangle rect;
        private string name;
        private bool pickedUp;
        private Texture2D texture;

        // -------------------------------
        // Properties
        // -------------------------------

        /// <summary>
        /// Gets the rectangle of the item
        /// </summary>
        public Rectangle Rectangle 
        { 
            get { return rect; } 
            set { rect = value; }
        }

        /// <summary>
        /// Gets and sets whether the item has been picked up
        /// </summary>
        public bool PickedUp
        { 
            get { return pickedUp; }
            set { pickedUp = value; }
        }

        // -------------------------------
        // Constructor
        // -------------------------------

        /// <summary>
        /// Parameterized constructor for the Items class
        /// </summary>
        /// <param name="rect"> takes a rectangle to check collision </param>
        /// <param name="name"> takes a string for the type of item </param>
        /// <param name="pickedUp"> takes a bool to check if the item has been picked up </param>
        /// <param name="texture"> takes a texture2D to draw the item </param>
        public Items(Rectangle rect, string name, bool pickedUp, Texture2D texture)
        {
            this.rect = rect;
            this.name = name;
            this.pickedUp = pickedUp;
            this.texture = texture;
        }

        // -------------------------------
        // Methods
        // -------------------------------

        /// <summary>
        /// Draws the item at the randomly generated location
        /// </summary>
        /// <param name="spriteBatch"> takes in a texture for the item </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(texture, new Vector2(rect.X, rect.Y),
                new Rectangle(0, 0, 5000, 5000), Color.White, 0, Vector2.Zero, 0.015f,
                SpriteEffects.None, 0);
        }

        /// <summary>
        /// Uses rectangle intersection to check collision with player
        /// </summary>
        /// <param name="player"> takes the player instance from the game </param>
        public void CheckCollision(Player player)
        {
            // When they intersect decrement health
            if (this.rect.Intersects(player.Rect))
            {
                pickedUp = true;
                PowerUp(player);
            }

            
        }

        /// <summary>
        /// Used with check collision to apply power ups
        /// </summary>
        /// <param name="player"> takes the player instance from the game </param>
        public void PowerUp(Player player)
        {
            // Switch based on the type of power up
            switch (name)
            {
                case "Speed Boost":

                    if (pickedUp)
                    {
                        player.MovementSpeed = 5;
                    }
                    break;
            }
               
           
        }
    }
}
