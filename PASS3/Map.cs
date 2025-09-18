// Author: Julian Kim
// File Name: Map.cs
// Project Name: PASS3
// Creation Date: May 7, 2024
// Modified Date: June 12, 2024
// Description: A class representing a map, managing all functionality and displaying of anything on its map

using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace PASS3
{
    class Map
    {
        /// <summary>
        /// Store connections to other maps
        /// </summary>
        public List<Connection> Connections { get; set; }

        // Store map dimensions
        private static int NUM_ROWS = 25;
        private static int NUM_COLS = 25;

        // Store tile type constants
        private static int GRASS = 1;
        private static int ROAD = 2;
        private static int ROAD_END = 3;
        private static int GAME = 6;
        private static int TILED_PATH = 7;
        private static int NPC = 8;
        private static int END = 9;
        private static int BUY = 10;
        public static int WATER = 4;
        public static int UNWALKABLE = 5;

        // Store file reader
        private StreamReader inFile;

        // Store flag for if game is done
        private bool doneGame = false;

        // Store map animation
        private Animation anim;

        // Store map grid
        private int[,] map;

        // Store map number
        private int mapNum;

        // Store flag for displaying [E] image
        private bool displayEKey = false;

        // Store [E] image
        private Texture2D eKeyImg;

        // Store minigame
        private Minigame minigame;

        // Store npc
        private Npc npc;

        /// <summary>
        /// Initializes a new instance of <see cref="Map"/>
        /// </summary>
        /// <param name="img">Map image</param>
        /// <param name="pos">Position of map</param>
        /// <param name="mapNum">Map number</param>
        /// <param name="content">Content manager for loading content</param>
        public Map(Texture2D img, Vector2 pos, int mapNum, ContentManager content)
        {
            // Initialize connections list
            Connections = new List<Connection>();

            // Initialize map animation
            anim = new Animation(img, 1, img.Height * 2 / Game1.GAME_REC.Height, img.Height * 2 / Game1.GAME_REC.Height, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 500 * (img.Height * 2 / Game1.GAME_REC.Height), pos, 2f, true);
            
            // Set map num
            this.mapNum = mapNum;

            // Set map grid
            LoadMap(content);

            // Load [E] key image
            eKeyImg = content.Load<Texture2D>("Images/Sprites/E_Key");
        }

        /// <summary>
        /// Determines whether game is finished
        /// </summary>
        /// <returns>True if game is completed, otherwise false</returns>
        public bool isDoneGame()
        {
            return doneGame;
        }

        /// <summary>
        /// Pause the map animation
        /// </summary>
        public void PauseAnim()
        {
            anim.Pause();
        }

        /// <summary>
        /// Unpause the map animation
        /// </summary>
        public void UnpauseAnim()
        {
            anim.Resume();
        }

        /// <summary>
        /// Retrieve the map tiletype grid
        /// </summary>
        /// <returns>2D array of integers representing map grid</returns>
        public int[,] GetMap()
        {
            return map;
        }

        /// <summary>
        /// Retrieve map number
        /// </summary>
        /// <returns>Map number</returns>
        public int GetMapNum()
        {
            return mapNum;
        }

        /// <summary>
        /// Add an npc
        /// </summary>
        /// <param name="npc">Npc to add</param>
        public void AddNpc(Npc npc)
        {
            this.npc = npc;
        }

        /// <summary>
        /// Updates the map
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="player">The player instance</param>
        /// <param name="kb">Current keyboard state</param>
        /// <param name="prevKb">Previous keyboard state</param>
        /// <param name="mouse">Current mouse state</param>
        /// <param name="prevMouse">Previous mouse state</param>
        /// <returns>Index of next map if transition is required</returns>
        public int Update(GameTime gameTime, Player player, KeyboardState kb, KeyboardState prevKb, MouseState mouse, MouseState prevMouse)
        {
            // Update based on if minigame is being played or not
            if (minigame != null && minigame.GameOpened())
            {
                // Update minigame
                minigame.Update(gameTime, kb, prevKb, mouse, prevMouse);

                // Hide player
                player.Hide();
            }
            else
            {
                // Show player
                player.Show();

                // Store next map index
                int nextMap = 0;

                // Update map animation
                anim.Update(gameTime);

                // Update npc if exists
                if (npc != null)
                {
                    npc.Update(gameTime);
                }

                // Update tile interactions if player location changes
                if (player.GetPrevLoc().Equals(player.GetLoc()))
                {
                    // Reset flag
                    displayEKey = false;

                    // Update tile interations
                    nextMap = ManageTileInteractions(player, kb, prevKb);
                }

                return nextMap;
            }

            return 0;
        }

        /// <summary>
        /// Update only map animation and npc
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public void UpdateAnimsOnly(GameTime gameTime)
        {
            // Update map animation
            anim.Update(gameTime);

            // Update npc if exists
            if (npc != null)
            {
                npc.Update(gameTime);
            }
        }

        /// <summary>
        /// Manages interaction with tile types on map
        /// </summary>
        /// <param name="player">The player instance</param>
        /// <param name="kb">Current keyboard state</param>
        /// <param name="prevKb">Previous keyboard state</param>
        /// <returns>Index of next map if transition is required</returns>
        public int ManageTileInteractions(Player player, KeyboardState kb, KeyboardState prevKb)
        {
            // Store current tile type that player is on
            int playerTileType = map[player.GetLoc().Row, player.GetLoc().Col];

            // Store next map index
            int nextMap = 0;

            // Update interactions based on current tile type
            if (Math.Sign(playerTileType) == -1)
            {
                // Set next map index if [E] is pressed
                if (kb.IsKeyDown(Keys.E) && !prevKb.IsKeyDown(Keys.E))
                    nextMap = Math.Abs(map[player.GetLoc().Row, player.GetLoc().Col]);

                // Flag to display [E] key
                displayEKey = true;
            }
            else if (playerTileType == GRASS)
            {
                // Set speed
                player.SetSpeed(2.5f);
            }
            else if (playerTileType == ROAD)
            {
                // Set speed
                player.SetSpeed(3.5f);
            }
            else if (playerTileType == ROAD_END)
            {
                // Set speed
                player.SetSpeed(3f);
            }
            else if (playerTileType == GAME)
            {
                // Open minigame if [E] key is pressed
                if (kb.IsKeyDown(Keys.E) && !prevKb.IsKeyDown(Keys.E))
                {
                    minigame.OpenGame();
                }

                // Flag to display [E] key
                displayEKey = true;
            }
            else if (playerTileType == TILED_PATH)
            {
                // Set speed
                player.SetSpeed(4f);
            }
            else if (playerTileType == NPC && npc != null)
            {
                // Interact with npc if [E] key is pressed
                if (kb.IsKeyDown(Keys.E))
                {
                    npc.InteractedWith(true);
                    displayEKey = false;
                }
                else
                {
                    npc.InteractedWith(false);
                }

                // Flag to display [E] key
                displayEKey = true;
            }
            else if (playerTileType == END && Player.Inventory.Contains("Red Heart") && Player.Inventory.Contains("Blue Heart"))
            {
                // Finish game if [E] key is pressed
                if (kb.IsKeyDown(Keys.E) && !prevKb.IsKeyDown(Keys.E))
                {
                    doneGame = true;
                }

                // Flag to display [E] key
                displayEKey = true;
            }
            else if (playerTileType == BUY && Player.Inventory.Contains("Prize Ticket"))
            {
                // Update inventory if [E] key is pressed
                if (kb.IsKeyDown(Keys.E) && !prevKb.IsKeyDown(Keys.E))
                {
                    Player.Inventory.Remove("Prize Ticket");
                    Player.Inventory.Add("Teddy Bear");
                    Audio.CollectSfx.CreateInstance().Play();
                }

                // Flag to display [E] key
                displayEKey = true;
            }

            return nextMap;
        }

        /// <summary>
        /// Displays map
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw</param>
        /// <param name="player">The Player instance</param>
        /// <param name="rasterizerState">The rasterizer state for rendering</param>
        /// <param name="grayScaleFx">The grayscale effect for rendering</param>
        public void Draw(SpriteBatch spriteBatch, Player player, RasterizerState rasterizerState, Effect grayScaleFx)
        {
            // Display map animation
            anim.Draw(spriteBatch, Color.White);

            // Display based on if minigame is opened
            if (minigame != null && minigame.GameOpened())
            {
                // Display minigame without effects
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                minigame.Draw(spriteBatch);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, rasterizerState, grayScaleFx);
            }
            else
            {
                // Display npc is exists
                if (npc != null)
                {
                    npc.Draw(spriteBatch);
                }

                // Display [E] key if flag is true and player is visible
                if (displayEKey && player.IsShown())
                {
                    // Display in position based on player position relative to boundaries
                    if (player.GetLoc().Row == 0)
                    {
                        spriteBatch.Draw(eKeyImg, new Rectangle(player.GetRec().X + player.GetRec().Width / 2 - eKeyImg.Width, player.GetRec().Bottom, eKeyImg.Width * 2, eKeyImg.Height * 2), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(eKeyImg, new Rectangle(player.GetRec().X + player.GetRec().Width / 2 - eKeyImg.Width, player.GetRec().Y - eKeyImg.Height * 2, eKeyImg.Width * 2, eKeyImg.Height * 2), Color.White);
                    }
                }
            }
        }

        /// <summary>
        /// Display hud involving npc
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw</param>
        public void DrawNpcHud(SpriteBatch spriteBatch)
        {
            // Display npc description if exists
            if (npc != null)
            {
                npc.DrawDescription(spriteBatch);
            }
        }

        /// <summary>
        /// Load map grid
        /// </summary>
        /// <param name="content">Content manager for loading content</param>
        public void LoadMap(ContentManager content)
        {
            // Initialize file reader to read from file based on map number
            inFile = File.OpenText("Maps/Map" + mapNum + ".csv");

            // Initialize map grid size
            map = new int[25, 25];

            // Try loading data from file
            try
            {
                // Store lines
                string s;
                
                // Store individual elements of line
                string[] data;

                // Read through each line of data for map grid
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    // Set current line
                    s = inFile.ReadLine();
                    
                    // Split current line to individual elements
                    data = s.Split(',');

                    // Store each individual element into map grid
                    for (int j = 0; j < data.Length; j++)
                    {
                        map[i, j] = Convert.ToInt32(data[j]);

                        // Add connection if negative value (transition tile)
                        if (Math.Sign(map[i, j]) == -1)
                        {
                            Connections.Add(new Connection(new Location(i, j), -map[i, j]));
                        }
                    }
                }

                // Read last line containing other map info
                s = inFile.ReadLine();
                data = s.Split(',');

                // Store minigame if exists
                if (data.Length > 1)
                {
                    if (data[1].Equals("Cafe Orders"))
                    {
                        minigame = new CafeOrdersGame(content);
                    }
                    else if (data[1].Equals("DuckGame"))
                    {
                        minigame = new DuckMinigame(content);
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Formating Error");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not Found");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        /// <summary>
        /// Finds a specified tile type
        /// </summary>
        /// <param name="tileType">Number representing a tile type</param>
        /// <returns>Location of specified tile type</returns>
        public Location FindTileType(int tileType)
        {
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].GetTileType() == tileType)
                {
                    return Connections[i].GetLoc();
                }
            }

            return Location.NO_LOC;
        }
    }
}
