// Author: Julian Kim
// File Name: Minigaame.cs
// Project Name: PASS3
// Creation Date: May 27, 2024
// Modified Date: June 12, 2024
// Description: A parent class representing a basic minigame

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PASS3
{
    class Minigame
    {
        // Store minigame name
        protected string gameName;

        // Store game state constants
        protected const int MENU = 0;
        protected const int GAME = 1;
        protected const int END = 2;

        // Store current game state
        protected int gameState = 0;

        // Store flag for if game is open
        protected bool gameOpened = false;

        /// <summary>
        /// Initializes a new instance of <see cref="Minigame"/>
        /// </summary>
        /// <param name="name"></param>
        public Minigame(string name)
        {
            gameName = name;
        }

        /// <summary>
        /// Determines whether game is open
        /// </summary>
        /// <returns>True if open, otherwise false</returns>
        public bool GameOpened()
        {
            return gameOpened;
        }

        /// <summary>
        /// Update minigame
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="kb">Current keyboard state</param>
        /// <param name="prevKb">Previous keyboard state</param>
        /// <param name="mouse">Current mouse state</param>
        /// <param name="prevMouse">Previous mouse state</param>
        public virtual void Update(GameTime gameTime, KeyboardState kb, KeyboardState prevKb, MouseState mouse, MouseState prevMouse)
        {

        }

        /// <summary>
        /// Opens game
        /// </summary>
        public virtual void OpenGame()
        {
            gameState = 0;
            gameOpened = true;
        }

        /// <summary>
        /// Closes game
        /// </summary>
        public void CloseGame()
        {
            gameState = 0;
            gameOpened = false;
        }
        
        /// <summary>
        /// Start game
        /// </summary>
        public virtual void StartGame()
        {
            gameState = 1;
        }

        /// <summary>
        /// End game
        /// </summary>
        public virtual void StopGame()
        {
            gameState = END;
        }

        /// <summary>
        /// Display minigame
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
