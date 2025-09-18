// Author: Julian Kim
// File Name: MapManager.cs
// Project Name: PASS3
// Creation Date: May 22, 2024
// Modified Date: June 12, 2024
// Description: A class to manage all maps

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PASS3
{
    class MapManager
    {
        // Store maps in list
        private List<Map> maps;

        // Store maps in stack
        private MapStack stack;

        /// <summary>
        /// Initializes a new instance of <see cref="MapManager"/>
        /// </summary>
        public MapManager()
        {
            // Initialize maps list
            maps = new List<Map>();
            
            // Initialize maps stack
            stack = new MapStack();
        }

        /// <summary>
        /// Retrieve current map
        /// </summary>
        /// <returns>Current map</returns>
        public Map GetCurrMap()
        {
            return stack.Top();
        }

        /// <summary>
        /// Adds a map to list
        /// </summary>
        /// <param name="map">Map to add</param>
        public void Add(Map map)
        {
            maps.Add(map);
        }

        /// <summary>
        /// Resets stack of maps
        /// </summary>
        public void ResetMaps()
        {
            stack.Clear();
        }

        /// <summary>
        /// Sets map to specified map num
        /// </summary>
        /// <param name="mapNum">A map represented by a number id</param>
        /// <returns>True if map with specified id exists, otherwise false</returns>
        public bool SetCurrMap(int mapNum)
        {
            // Check if map exists
            if (mapNum - 1 < maps.Count && mapNum - 1 >= 0)
            {
                // Pop or push from stack depending on map
                if (stack.Contains(maps[mapNum - 1]))
                {
                    stack.Pop();
                }
                else
                {
                    stack.Push(maps[mapNum - 1]);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Update current map animation and transitions
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="player">The player instance</param>
        /// <param name="kb">Current keyboard state</param>
        /// <param name="prevKb">Previous keyboard state</param>
        /// <param name="mouse">Current mouse state</param>
        /// <param name="prevMouse">Previous mouse state</param>
        public void UpdateCurr(GameTime gameTime, Player player, KeyboardState kb, KeyboardState prevKb, MouseState mouse, MouseState prevMouse)
        {
            // Store next map index
            int nextMap = stack.Top().Update(gameTime, player, kb, prevKb, mouse, prevMouse);

            // Transition to next map if next map index is valid
            if (nextMap > 0)
            {
                // Disallow entry to map if player does not hold specified item
                if (nextMap == 5 && !Player.Inventory.Contains("Arcade Token"))
                {
                    // Play locked sound effect
                    Audio.LockedSfx.CreateInstance().Play();
                    return;
                }

                // Play door sound effect
                Audio.DoorSfx.CreateInstance().Play();

                // Store current map index
                int currMapNum = GetCurrMap().GetMapNum();

                // Set map to next map index
                SetCurrMap(nextMap);

                // Set player location based on previous map index
                player.SetLoc(GetCurrMap().FindTileType(currMapNum));
            }

            // Pause or unpause animation based on if town is frozen or not
            if (Game1.IsFrozen)
            {
                stack.Top().PauseAnim();
            }
            else
            {
                stack.Top().UnpauseAnim();
            }
        }

        /// <summary>
        /// Update only current map animation
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateCurr(GameTime gameTime)
        {
            stack.Top().UpdateAnimsOnly(gameTime);
        }

        /// <summary>
        /// Determine whether game is finished
        /// </summary>
        /// <returns>True if game is completed, otherwise false</returns>
        public bool isDoneGame()
        {
            return GetCurrMap().isDoneGame();
        }

        /// <summary>
        /// Display current map
        /// </summary>
        /// <param name="spriteBatch">A sprite batch to draw</param>
        /// <param name="titleFont">Font for displaying strings</param>
        /// <param name="player">The player instance</param>
        /// <param name="rasterizerState">Rasterizer state for rendering</param>
        /// <param name="grayScaleFx">Grayscale effect for rendering</param>
        public void DrawCurr(SpriteBatch spriteBatch, Player player, RasterizerState rasterizerState, Effect grayScaleFx)
        {
            // Display map at top of stack
            stack.Top().Draw(spriteBatch, player, rasterizerState, grayScaleFx);
        }

        /// <summary>
        /// Display hud
        /// </summary>
        /// <param name="spriteBatch">A sprite batch to draw</param>
        public void DrawHud(SpriteBatch spriteBatch)
        {
            // Display npc hud
            stack.Top().DrawNpcHud(spriteBatch);
        }
    }
}
