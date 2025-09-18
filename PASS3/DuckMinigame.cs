// Author: Julian Kim
// File Name: DuckMinigame.cs
// Project Name: PASS3
// Creation Date: June 5, 2024
// Modified Date: June 12, 2024
// Description: A minigame where a duck must navigate through an oddly very woody river to find its ducklings

using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PASS3
{
    class DuckMinigame : Minigame
    {
        // Store constants
        private const int TILE_SIZE = 48;
        private const int EMPTY = 0;
        private const int OBSTACLE = 1;
        private const int DUCKLING = 2;

        // Store duck images, rectangles and grid location
        private Texture2D duckNormImg;
        private Texture2D duckHurtImg;
        private Texture2D duckCurrImg;
        private Rectangle duckRec;
        private Location loc;

        // Store duckling image and rectangles
        private Texture2D ducklingImg;
        private Rectangle ducklingRec;
        private Rectangle ducklingHudRec;

        // Store score font
        private SpriteFont scoreFont;

        // Store heart image
        private Texture2D filledHeartImg;

        // Store obstacle image
        private Texture2D obstaclesImg;

        // Store pixel image
        private Texture2D pixelImg;

        // Store background image and rectangle
        private Texture2D bkgImg;
        private Rectangle bkgRec;

        // Store controls image and rectangle
        private Texture2D controlsImg;
        private Rectangle controlsRec;

        // Store play button and image
        private Texture2D playBtnImg;
        private Button playBtn;

        // Store screen scrolling timer
        private Timer scrollTimer;

        // Store score
        private int score;

        // Store health
        private int health;

        // Store map as integer tile types
        private int[,] map;

        // Store flags 
        private bool isObstacleCol;
        private bool ducklingSpawned;

        /// <summary>
        /// Initialize a new instance of <see cref="DuckMinigame"/>
        /// </summary>
        /// <param name="content">The content manager for loading content</param>
        public DuckMinigame(ContentManager content) : base("Duck Game")
        {
            // Load score font
            scoreFont = content.Load<SpriteFont>("Fonts/ScoreFont");

            // Load images
            pixelImg = content.Load<Texture2D>("Images/Sprites/Pixel");
            bkgImg = content.Load<Texture2D>("Images/Backgrounds/DuckBkg");
            duckNormImg = content.Load<Texture2D>("Images/Sprites/DuckNormImg");
            duckHurtImg = content.Load<Texture2D>("Images/Sprites/DuckHurtImg");
            duckCurrImg = duckNormImg;
            obstaclesImg = content.Load<Texture2D>("Images/Sprites/ObstaclesImg");
            ducklingImg = content.Load<Texture2D>("Images/Sprites/DucklingImg");
            controlsImg = content.Load<Texture2D>("Images/Sprites/DuckControlsImg");
            filledHeartImg = content.Load<Texture2D>("Images/Sprites/FilledHeartImg");

            // Initialize background rectangle
            bkgRec = new Rectangle(Game1.GAME_REC.X + Game1.GAME_REC.Width / 2 - bkgImg.Width / 2, Game1.GAME_REC.Y + Game1.GAME_REC.Height / 2 - bkgImg.Height / 2, bkgImg.Width, bkgImg.Height);

            // Initialize duck rectangle and location
            loc = new Location(2, 3);
            duckRec = new Rectangle(bkgRec.X + loc.Col * TILE_SIZE, bkgRec.Y + loc.Row * TILE_SIZE + 144, duckNormImg.Width, duckNormImg.Height);

            // Initialize duckling rectangles
            ducklingRec = new Rectangle(0, 0, ducklingImg.Width, ducklingImg.Height);
            ducklingHudRec = new Rectangle(bkgRec.X + 96, bkgRec.Y + 24, ducklingImg.Width, ducklingImg.Height);

            // Load play button image and button
            playBtnImg = content.Load<Texture2D>("Images/Sprites/DuckPlayBtn");
            playBtn = new Button(playBtnImg, new Rectangle(bkgRec.X + bkgRec.Width / 2 - playBtnImg.Width / 2, bkgRec.Bottom - 150, playBtnImg.Width, playBtnImg.Height), Color.Gray);

            // Initialize controls rectangle
            controlsRec = new Rectangle(bkgRec.Right - 150, bkgRec.Y + bkgRec.Height / 2 - controlsImg.Height / 2, controlsImg.Width, controlsImg.Height);

            // Initialize screen scrolling timer
            scrollTimer = new Timer(800, false);

            // Initialize map size
            map = new int[5, 12];
        }

        /// <summary>
        /// Starts game and resets game data
        /// </summary>
        public override void StartGame()
        {
            base.StartGame();

            score = 0;
            health = 3;
            duckCurrImg = duckNormImg;
            loc.Row = 2;
            duckRec.Y = bkgRec.Y + loc.Row * TILE_SIZE + 144;
            map.Initialize();
            scrollTimer.ResetTimer(true);
            isObstacleCol = true;
            ducklingSpawned = false;
        }

        /// <summary>
        /// Update game based on inputs and game states
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="kb">Current keyboard state</param>
        /// <param name="prevKb">Previous keyboard stae</param>
        /// <param name="mouse">Current mouse state</param>
        /// <param name="prevMouse">Previous mouse state</param>
        public override void Update(GameTime gameTime, KeyboardState kb, KeyboardState prevKb, MouseState mouse, MouseState prevMouse)
        {
            // Update if game is opened
            if (gameOpened)
            {
                // Update based on game state
                if (gameState == MENU)
                {
                    // Update play button
                    playBtn.Update(mouse);
                    if (playBtn.IsDown() && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        StartGame();
                    }

                    // Close game if [x] is pressed
                    if (kb.IsKeyDown(Keys.X) && !prevKb.IsKeyDown(Keys.X))
                    {
                        CloseGame();
                    }
                }
                else if (gameState == GAME)
                {
                    // Update screen scrolling timer
                    scrollTimer.Update(gameTime);

                    // Scroll map when timer is finished
                    if (scrollTimer.IsFinished())
                    {
                        // Reset scroll timer
                        scrollTimer.ResetTimer(true);

                        // Reset duck image
                        duckCurrImg = duckNormImg;

                        // Scroll integer map
                        int[,] newmap = new int[map.GetLength(0), map.GetLength(1)];
                        for (int i = 0; i < map.GetLength(0); i++)
                        {
                            for (int j = 0; j < map.GetLength(1); j++)
                            {
                                if (j != 0)
                                {
                                    newmap[i, j - 1] = map[i, j];
                                }
                            }
                        }

                        // Store instance of Random
                        Random random = new Random();

                        // Store random row index
                        int sigRow = random.Next(newmap.GetLength(0));

                        // Set new last column of integer map
                        for (int i = 0; i < newmap.GetLength(0); i++)
                        {
                            // Set tile values based on alternating obstacle column flag
                            if (!isObstacleCol)
                            {
                                // Set tile based on if duck is already on the map
                                if (i == sigRow && !ducklingSpawned)
                                {
                                    newmap[i, newmap.GetLength(1) - 1] = DUCKLING;
                                    ducklingSpawned = true;
                                }
                                else
                                {
                                    newmap[i, newmap.GetLength(1) - 1] = EMPTY;
                                }
                            }
                            else if (i == sigRow)
                            {
                                newmap[i, newmap.GetLength(1) - 1] = EMPTY;
                            }
                            else
                            {
                                newmap[i, newmap.GetLength(1) - 1] = OBSTACLE;
                            }
                        }

                        // Alternate obstacle column flag
                        isObstacleCol = !isObstacleCol;
                        
                        // Set map to new scrolled map
                        map = newmap;
                    }

                    // Move duck up or down based on player input
                    if (kb.IsKeyDown(Keys.Up) && !prevKb.IsKeyDown(Keys.Up))
                    {
                        if (loc.Row - 1 >= 0)
                        {
                            loc.Row--;
                        }
                    }
                    else if (kb.IsKeyDown(Keys.Down) && !prevKb.IsKeyDown(Keys.Down))
                    {
                        if (loc.Row + 1 < map.GetLength(0))
                        {
                            loc.Row++;
                        }
                    }

                    // Update duck position
                    duckRec.Y = bkgRec.Y + 144 + loc.Row * TILE_SIZE;

                    // Check for collisions with duckling or obstacles
                    if (map[loc.Row, loc.Col] == DUCKLING)
                    {
                        // Increase score
                        score++;

                        // Set flag to spawn new duckling
                        ducklingSpawned = false;

                        // Set current map location to empty
                        map[loc.Row, loc.Col] = EMPTY;
                    }
                    else if (map[loc.Row, loc.Col] == OBSTACLE)
                    {
                        // Play splash sound effect
                        Audio.HitSplashSfx.CreateInstance().Play();

                        // Decrement health
                        health--;

                        // Set duck image to hurt image
                        duckCurrImg = duckHurtImg;

                        // Set current map location to empty
                        map[loc.Row, loc.Col] = EMPTY;
                    }

                    // End game if score is 5 or above, or health is depleted
                    if (score >= 5 || health <= 0)
                    {
                        gameState = END;
                    }
                }
                else
                {
                    // Check for player inputting [x]
                    if (kb.IsKeyDown(Keys.X) && !prevKb.IsKeyDown(Keys.X))
                    {
                        // Close game
                        CloseGame();

                        // Give player prize if player won game
                        if (health > 0)
                        {
                            Player.Inventory.Add("Prize Ticket");

                            // Play collect sound effect
                            Audio.CollectSfx.CreateInstance().Play();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Displays minigame elements
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw images and strings</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Display background
            spriteBatch.Draw(pixelImg, Game1.GAME_REC, Color.CadetBlue);
            spriteBatch.Draw(bkgImg, bkgRec, Color.White);

            // Display minigame hud
            spriteBatch.Draw(ducklingImg, ducklingHudRec, Color.White);
            spriteBatch.DrawString(scoreFont, score + "", new Vector2(ducklingHudRec.Right, ducklingHudRec.Y), Color.Brown);
            for (int i = 0; i < health; i++)
            {
                spriteBatch.Draw(filledHeartImg, new Vector2(bkgRec.X + 192 + i * 48, bkgRec.Y + 24), Color.White);
            }

            // Display based on game state
            if (gameState == MENU)
            {
                // Display play button
                playBtn.Draw(spriteBatch);

                // Display controls
                spriteBatch.Draw(controlsImg, controlsRec, Color.White);

                // Display duck
                spriteBatch.Draw(duckCurrImg, duckRec, Color.White);

                // Display instructions to exit game
                DisplayString.DrawCenteredXShadow(spriteBatch, scoreFont, "Press [X] to exit game", bkgRec.Bottom - 50, Color.White, Color.Black, bkgRec);
            }
            else if (gameState == GAME)
            {
                // Display map
                DisplayMap(spriteBatch);

                // Display duck
                spriteBatch.Draw(duckCurrImg, duckRec, Color.White);
            }
            else
            {
                // Display end message based on player win or loss
                if (health > 0)
                {
                    DisplayString.DrawCenteredX(spriteBatch, scoreFont, "You Won!", bkgRec.Bottom - 150, Color.Green, bkgRec);
                }
                else
                {
                    DisplayString.DrawCenteredX(spriteBatch, scoreFont, "You Lost...", bkgRec.Bottom - 150, Color.Red, bkgRec);
                }

                // Display instructions to exit game
                DisplayString.DrawCenteredXShadow(spriteBatch, scoreFont, "Press [X] to exit game", bkgRec.Bottom - 50, Color.White, Color.Black, bkgRec);
            }
        }

        /// <summary>
        /// Displays the non empty tiles of game map
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DisplayMap(SpriteBatch spriteBatch)
        {
            // Displays all non empty tiles
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    if (map[row, col] == DUCKLING)
                    {
                        spriteBatch.Draw(ducklingImg, new Rectangle(bkgRec.X + col * TILE_SIZE, bkgRec.Y + row * TILE_SIZE + 144, ducklingRec.Width, ducklingRec.Height), Color.White);
                    }
                    else if (map[row, col] == OBSTACLE)
                    {
                        spriteBatch.Draw(obstaclesImg, new Rectangle(bkgRec.X + col * TILE_SIZE, bkgRec.Y + row * TILE_SIZE + 144, obstaclesImg.Width * 6, obstaclesImg.Height * 6), Color.White);
                    }
                }
            }
        }
    }
}