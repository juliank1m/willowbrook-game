// Author: Julian Kim
// File Name: Player.cs
// Project Name: PASS3
// Creation Date: May 2, 2024
// Modified Date: June 12, 2024
// Description: A class representing the player of the game

using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PASS3
{
    class Player
    {
        /// <summary>
        /// Store money player has
        /// </summary>
        public static int Cash { get; set; } = 0;

        /// <summary>
        /// Store list of player's items
        /// </summary>
        public static List<string> Inventory { get; set; } = new List<string>();

        // Store direction constants
        private const int UP = 0;
        private const int RIGHT = 1;
        private const int DOWN = 2;
        private const int LEFT = 3;

        // Store movement type constants
        private const int IDLE = 0;
        private const int WALK = 1;

        // Store player direction and movement type
        private int playerDir = LEFT;
        private int playerMovement = IDLE;

        // Store player animations
        private Animation[,] anims = new Animation[2, 4];

        // Store player position and grid locations
        private Vector2 pos;
        private Location gridLoc;
        private Location prevLoc;

        // Store player speed
        private float speed = 4f;

        // Store bottom collision rectangle
        private Rectangle btmRec;

        // Store hud font
        private SpriteFont hudFont;

        // Store flag for showing player
        private bool showPlayer = true;

        /// <summary>
        /// Initializes a new instance of <see cref="Player"/>
        /// </summary>
        /// <param name="content"></param>
        public Player(ContentManager content)
        {
            // Load player animations
            anims[IDLE, UP] = new Animation(content.Load<Texture2D>("Images/Sprites/Up_Idle"), 5, 1, 5, 0, 0, Animation.ANIMATE_FOREVER, 500, pos, 1.5f, true);
            anims[IDLE, RIGHT] = new Animation(content.Load<Texture2D>("Images/Sprites/Right_Idle"), 5, 1, 5, 0, 0, Animation.ANIMATE_FOREVER, 500, pos, 1.5f, true);
            anims[IDLE, DOWN] = new Animation(content.Load<Texture2D>("Images/Sprites/Down_Idle"), 5, 1, 5, 0, 0, Animation.ANIMATE_FOREVER, 500, pos, 1.5f, true);
            anims[IDLE, LEFT] = new Animation(content.Load<Texture2D>("Images/Sprites/Left_Idle"), 5, 1, 5, 0, 0, Animation.ANIMATE_FOREVER, 500, pos, 1.5f, true);
            anims[WALK, UP] = new Animation(content.Load<Texture2D>("Images/Sprites/Up_Walk"), 6, 1, 6, 0, 0, Animation.ANIMATE_FOREVER, 500, pos, 1.5f, true);
            anims[WALK, RIGHT] = new Animation(content.Load<Texture2D>("Images/Sprites/Right_Walk"), 6, 1, 6, 0, 0, Animation.ANIMATE_FOREVER, 500, pos, 1.5f, true);
            anims[WALK, DOWN] = new Animation(content.Load<Texture2D>("Images/Sprites/Down_Walk"), 6, 1, 6, 0, 0, Animation.ANIMATE_FOREVER, 500, pos, 1.5f, true);
            anims[WALK, LEFT] = new Animation(content.Load<Texture2D>("Images/Sprites/Left_Walk"), 6, 1, 6, 0, 0, Animation.ANIMATE_FOREVER, 500, pos, 1.5f, true);

            // Load hud font
            hudFont = content.Load<SpriteFont>("Fonts/MessageFont");

            // Store previous location
            prevLoc = gridLoc;

            // Initialize bottom rectangle of player
            btmRec = new Rectangle(anims[playerMovement, playerDir].GetDestRec().X, anims[playerMovement, playerDir].GetDestRec().Y + anims[playerMovement, playerDir].GetDestRec().Height / 2, anims[playerMovement, playerDir].GetDestRec().Width, anims[playerMovement, playerDir].GetDestRec().Height / 2);
        }

        /// <summary>
        /// Retrieve grid location of player
        /// </summary>
        /// <returns>Grid location of player</returns>
        public Location GetLoc()
        {
            return gridLoc;
        }

        /// <summary>
        /// Set grid location of player
        /// </summary>
        /// <param name="row">Row index to set location to</param>
        /// <param name="col">Column index to set location to</param>
        public void SetLoc(int row, int col)
        {
            // Convert grid location to position
            pos.X = col * Game1.TILE_SIZE + Game1.GAME_REC.X;
            pos.Y = row * Game1.TILE_SIZE - GetRec().Height / 2;

            // Update animation position
            anims[playerMovement, playerDir].TranslateTo(pos.X, pos.Y);

            // Update grid location
            gridLoc.Row = row;
            gridLoc.Col = col;
        }

        /// <summary>
        /// Set grid location of player
        /// </summary>
        /// <param name="loc">New location of player to set</param>
        public void SetLoc(Location loc)
        {
            // Store new row and column
            int row = loc.Row;
            int col = loc.Col;

            // Convert new location to position
            pos.X = col * Game1.TILE_SIZE + Game1.GAME_REC.X;
            pos.Y = row * Game1.TILE_SIZE - GetRec().Height / 2;

            // Update animation position
            anims[playerMovement, playerDir].TranslateTo(pos.X, pos.Y);

            // Update grid location
            gridLoc.Row = row;
            gridLoc.Col = col;
        }

        /// <summary>
        /// Set speed of player
        /// </summary>
        /// <param name="speed">New speed to set to</param>
        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        /// <summary>
        /// Retrieve player rectangle
        /// </summary>
        /// <returns>Player rectangle</returns>
        public Rectangle GetRec()
        {
            return anims[playerMovement, playerDir].GetDestRec();
        }

        /// <summary>
        /// Retrieve previous player grid location
        /// </summary>
        /// <returns>Previous location of player</returns>
        public Location GetPrevLoc()
        {
            return prevLoc;
        }

        /// <summary>
        /// Set flag to hide player
        /// </summary>
        public void Hide()
        {
            showPlayer = false;
        }

        /// <summary>
        /// Set flag to show player
        /// </summary>
        public void Show()
        {
            showPlayer = true;
        }

        /// <summary>
        /// Determine whether player is being shown
        /// </summary>
        /// <returns>True if being shown, otherwise false</returns>
        public bool IsShown()
        {
            return showPlayer;
        }

        /// <summary>
        /// Update player
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="map">Map grid of tiles</param>
        /// <param name="kb">Current keyboard state</param>
        /// <param name="prevKb">Previous keyboard state</param>
        public void Update(GameTime gameTime, int[,] map, KeyboardState kb, KeyboardState prevKb)
        {
            // Update player if being shown
            if (showPlayer)
            {
                // Set previous location
                prevLoc = gridLoc;

                // Update player animation
                anims[playerMovement, playerDir].Update(gameTime);

                // Store new position
                Vector2 newPos = pos;

                // Set new position, direction and movement type based on keyboard input
                if (kb.IsKeyDown(Keys.W) && !BoundsCollision(Keys.W))
                {
                    playerDir = UP;
                    playerMovement = WALK;
                    newPos.Y -= speed;
                }
                else if (kb.IsKeyDown(Keys.D) && !BoundsCollision(Keys.D))
                {
                    playerDir = RIGHT;
                    playerMovement = WALK;
                    newPos.X += speed;
                }
                else if (kb.IsKeyDown(Keys.S) && !BoundsCollision(Keys.S))
                {
                    playerDir = DOWN;
                    playerMovement = WALK;
                    newPos.Y += speed;
                }
                else if (kb.IsKeyDown(Keys.A) && !BoundsCollision(Keys.A))
                {
                    playerDir = LEFT;
                    playerMovement = WALK;
                    newPos.X -= speed;
                }
                else
                {
                    playerMovement = IDLE;
                }

                // Set position to new position if new position is walkable
                if (IsWalkable(map, newPos))
                    pos = newPos;

                // Update positions
                anims[playerMovement, playerDir].TranslateTo(pos.X, pos.Y);
                btmRec.X = anims[playerMovement, playerDir].GetDestRec().X;
                btmRec.Y = anims[playerMovement, playerDir].GetDestRec().Y + btmRec.Height;

                // Update grid location
                gridLoc.Row = (int)((pos.Y + GetRec().Height / 4 * 3) / Game1.TILE_SIZE);
                gridLoc.Col = (int)((pos.X - Game1.GAME_REC.X + GetRec().Width / 2) / Game1.TILE_SIZE);

                // Player step sound effect if player is on new grid location
                if (!prevLoc.Equals(gridLoc))
                {
                    SoundEffectInstance step = Audio.StepSfx.CreateInstance();
                    step.Volume = 0.3f;
                    step.Play();
                }
            }
        }

        /// <summary>
        /// Determine whether a tile is walkable
        /// </summary>
        /// <param name="map">Grid map of tiles</param>
        /// <param name="position">New position to check</param>
        /// <returns>True new position is on walkable tile, otherwise false</returns>
        public bool IsWalkable(int[,] map, Vector2 position)
        {
            // Convert position to grid row and column
            int tileCol = (int)((position.X - Game1.GAME_REC.X + GetRec().Width / 2) / Game1.TILE_SIZE);
            int tileRow = (int)((position.Y + GetRec().Height / 4 * 3) / Game1.TILE_SIZE);

            // Return true of not on water or unwalkable tile, otherwise false
            return !(map[tileRow, tileCol] == Map.WATER || map[tileRow, tileCol] == Map.UNWALKABLE);
        }

        /// <summary>
        /// Determine whether player will go out of bounds
        /// </summary>
        /// <param name="keyPressed">Keyboard key pressed</param>
        /// <returns>True if colliding with boundary, otherwise false</returns>
        private bool BoundsCollision(Keys keyPressed)
        {
            // Check collisions based on direction of key
            if (keyPressed == Keys.W)
            {
                if (pos.Y + btmRec.Height <= Game1.GAME_REC.Top)
                {
                    return true;
                }
            }
            else if (keyPressed == Keys.S)
            {
                if (pos.Y + btmRec.Height * 2 >= Game1.GAME_REC.Bottom - 1)
                {
                    return true;
                }
            }
            else if (keyPressed == Keys.A)
            {
                if (pos.X <= Game1.GAME_REC.Left)
                {
                    return true;
                }
            }
            else
            {
                if (pos.X + btmRec.Width >= Game1.GAME_REC.Right - 1)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Display player
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Display player if player is being shown
            if (showPlayer)
                anims[playerMovement, playerDir].Draw(spriteBatch, Color.White);
        }

        /// <summary>
        /// Display hud
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw</param>
        public void DrawHud(SpriteBatch spriteBatch)
        {
            // Display inventory title
            DisplayString.DrawShadow(spriteBatch, hudFont, "Inventory", new Vector2(10, 10), Color.Brown, Color.White);

            // Display all inventory contents
            for (int i = 0; i < Inventory.Count; i++)
            {
                DisplayString.DrawShadow(spriteBatch, hudFont, Inventory[i], new Vector2(10, 50 + i * 40), Color.SandyBrown, Color.Black);
            }

            // Display player's money
            DisplayString.DrawShadow(spriteBatch, hudFont, "Cash: $" + Cash, new Vector2(10, Game1.ScreenHeight - 50), Color.LawnGreen, Color.Black);
        }
    }
}
