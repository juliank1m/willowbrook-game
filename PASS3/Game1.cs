
// Author: Julian Kim
// File Name: Game1.cs
// Project Name: PASS3
// Creation Date: May 2, 2024
// Modified Date: June 12, 2024
// Description: A 2d top down game where a traveler tries to help the frozen town of Willowbrook, and reunite two villagers

/* Course Concepts:
 * 1. OOP
 * Most functionality throughout the game is handled by individual classes, stored as objects in the main Game class
 * 
 * 2. 2D Arrays and Lists
 * The interaction between the player and map is handled by a 2d array of integers, determining tile types and calling extra functionality for specific tiles
 * Lists are used throughout to hold varying amounts of elements, such as the cafe order drink, ingredients, and foods
 * 
 * 3. File IO
 * The map 2D arrays of tile types are read in from a comma separated text file of integers
 * 
 * 4. Stacks
 * Travelling to other maps uses a stack of Maps to pop if the player is going back to a map already visited, or push a map if the player is entering a new map. The top of the stack is the one being drawn and updated.
 * 
 * 5. Linked Lists
 * A linked list is used to go through each intro message. The node is stored and set to the next when the timer runs out
 */

using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace PASS3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        /// <summary>
        /// Store tile size constant
        /// </summary>
        public static int TILE_SIZE { get; } = 32;

        /// <summary>
        /// Store game boundary rectangle constant
        /// </summary>
        public static Rectangle GAME_REC { get; } = new Rectangle(240, 0, 800, 800);

        /// <summary>
        /// Store screen dimensions
        /// </summary>
        public static int ScreenWidth { get; } = 1280;
        public static int ScreenHeight { get; } = 800;

        /// <summary>
        /// Store whether the state of town is frozen
        /// </summary>
        public static bool IsFrozen { get; set; } = true;

        // Store game state constants
        private const int MENU = 0;
        private const int GAME = 1;
        private const int END = 2;

        // Store current game state
        private int gameState = MENU;

        // Store graphics manager
        private GraphicsDeviceManager graphics;
        
        // Store sprite batch for drawing images and strings
        private SpriteBatch spriteBatch;

        // Store input states
        private KeyboardState kb;
        private KeyboardState prevKb;
        private MouseState mouse;
        private MouseState prevMouse;

        // Store instance of Random class
        private Random random = new Random();

        // Store intro screen images and rectangle
        private Texture2D blankScreenBkg;
        private Texture2D blackScreenBkg;
        private Texture2D currScreenBkg;
        private Rectangle screenRec;

        // Store screen flash count and timer
        private int flashCount;
        private Timer flashTimer;
        private const int MIN_FLASH = 5;
        private const int MAX_FLASH = 51;

        // Store player instance
        private Player player;

        // Store all maps
        private Map outdoor1;
        private Map cafeOutdoor;
        private Map cafeInterior;
        private Map townOutdoor;
        private Map arcadeIndoor;
        private Map station;

        // Store map manager
        private MapManager mapManager;

        // Store fonts
        private SpriteFont titleFont;
        private SpriteFont menuFont;
        private SpriteFont msgFont;

        // Store gray rectangle
        private GameRectangle grayRec;

        // Store intro messages
        private MessageList introMsg;
        private MessageNode currIntroMsg;

        // Store game start button
        private Button startBtn;
        private bool startIntro = false;

        // Store timer for each intro message
        private Timer introTimer;

        // Store game effects
        private Effect grayscale;
        private Texture2D circleMaskImg;

        // Store mask effect factors
        private Timer maskTimer;
        private float circleMaskScale;

        // Store instance of end sequence
        private EndSequence endSequence;

        /// <summary>
        /// Initializes a new instance of <see cref="Game1"/>
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Set window dimensions
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;

            // Enable multi-sampling and disable Vsync
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = false;

            // Enable mouse visibility
            IsMouseVisible = true;

            // Apply changes
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Initialize audio
            Audio.InitializeAudio(Content);
            SoundEffect.MasterVolume = 1f;
            MediaPlayer.Volume = 0.7f;

            // Load effects
            grayscale = Content.Load<Effect>("Effects/Grayscale");
            circleMaskImg = Content.Load<Texture2D>("Images/Backgrounds/Circle");
            maskTimer = new Timer(3000, false);
            circleMaskScale = 20f;

            // Load fonts
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");
            menuFont = Content.Load<SpriteFont>("Fonts/ScoreFont");
            msgFont = Content.Load<SpriteFont>("Fonts/MessageFont");

            // Load intro screen
            blankScreenBkg = Content.Load<Texture2D>("Images/Backgrounds/BlankScreenBkg");
            blackScreenBkg = Content.Load<Texture2D>("Images/Backgrounds/DarkScreenBkg");
            currScreenBkg = blankScreenBkg;
            screenRec = new Rectangle(ScreenWidth / 2 - blankScreenBkg.Width / 2, ScreenHeight / 2 - blankScreenBkg.Height / 2, blankScreenBkg.Width, blankScreenBkg.Height);
            
            // Initialize intro flashing count and timer
            flashCount = 21;
            flashTimer = new Timer(random.Next(MIN_FLASH, MAX_FLASH), false);

            // Initialize gray rectangle
            grayRec = new GameRectangle(GraphicsDevice, new Rectangle(0, 0, ScreenWidth, ScreenHeight));

            // Initialize intro message
            introMsg = new MessageList();
            introMsg.AddToTail(new MessageNode("   In a land far, far away, there lies\na seemingly ordinary village, Willowbrook,\n  a place usually bustling with families,\n      adventurers, and storytellers"));
            introMsg.AddToTail(new MessageNode("  However, the village has been\nunusually silent for a few months,\n  with no clear explanation..."));
            introMsg.AddToTail(new MessageNode("A brave traveler, Sam, had been\n tasked to unravel the mystery\n       of this village"));
            introMsg.AddToTail(new MessageNode("    Rumor has it that fulfilling\n the wishes of certain residents may\nprovide clues to solving this mystery"));
            introMsg.AddToTail(new MessageNode("Good luck explorer!"));
            currIntroMsg = introMsg.GetHead();

            // Initialize intro message timer
            introTimer = new Timer(5000, false);

            // Initialize start button
            startBtn = new Button(GraphicsDevice, new Rectangle(ScreenWidth / 2 - 72, ScreenHeight / 2 - 24, 144, 48), Color.Transparent, Color.Green * 0.25f);
            startBtn.SetText(menuFont, "Start", Color.DarkSeaGreen);

            // Initialize player
            player = new Player(Content);

            // Load maps
            mapManager = new MapManager();
            outdoor1 = new Map(Content.Load<Texture2D>("Images/Backgrounds/Outdoor1Bkg"), new Vector2(GAME_REC.X, GAME_REC.Y), 1, Content);
            cafeOutdoor = new Map(Content.Load<Texture2D>("Images/Backgrounds/CafeOutdoorBkg"), new Vector2(GAME_REC.X, GAME_REC.Y), 2, Content);
            cafeInterior = new Map(Content.Load<Texture2D>("Images/Backgrounds/CafeInteriorBkg"), new Vector2(GAME_REC.X, GAME_REC.Y), 3, Content);
            townOutdoor = new Map(Content.Load<Texture2D>("Images/Backgrounds/TownOutdoorBkg"), new Vector2(GAME_REC.X, GAME_REC.Y), 4, Content);
            arcadeIndoor = new Map(Content.Load<Texture2D>("Images/Backgrounds/AcradeIndoorBkg"), new Vector2(GAME_REC.X, GAME_REC.Y), 5, Content);
            station = new Map(Content.Load<Texture2D>("Images/Backgrounds/StationBkg"), new Vector2(GAME_REC.X, GAME_REC.Y), 6, Content);

            // Load npcs
            outdoor1.AddNpc(new ThiefNpc(
                new Animation(
                    Content.Load<Texture2D>("Images/Sprites/FrontBirdAnim"),
                    4, 1, 4, 0,
                    Animation.NO_IDLE,
                    Animation.ANIMATE_FOREVER,
                    750,
                    new Location(21, 7).ToVector2(),
                    2f,
                    true),
                new Location(21, 7),
                "Blue [Thief]",
                "Loves money!\nSomething may\nhappen if you\nprovide him with\nenough of it...",
                Content,
                100));
            townOutdoor.AddNpc(new GamerNpc(
                new Animation(
                    Content.Load<Texture2D>("Images/Sprites/FrontCatAnim"),
                    4, 1, 4, 0,
                    Animation.NO_IDLE,
                    Animation.ANIMATE_FOREVER,
                    750,
                    new Location(7, 6).ToVector2(),
                    2f,
                    true),
                new Location(7, 6),
                "Calicat [Cat]",
                "Hardcore Gamer\nLoves winning\nprizes from\nthe arcade...",
                Content,
                "Teddy Bear"));

            // Add maps to map manager
            mapManager.Add(outdoor1);
            mapManager.Add(cafeOutdoor);
            mapManager.Add(cafeInterior);
            mapManager.Add(townOutdoor);
            mapManager.Add(arcadeIndoor);
            mapManager.Add(station);
            mapManager.SetCurrMap(6);
            player.SetLoc(7, 10);

            // Initialize end sequence
            endSequence = new EndSequence(
                new Animation(
                    Content.Load<Texture2D>("Images/Backgrounds/EndSceneBkg"),
                    1, 2, 2, 0,
                    Animation.NO_IDLE,
                    Animation.ANIMATE_FOREVER,
                    1000,
                    new Vector2(0, 0),
                    5f,
                    true),
                Content,
                grayscale,
                mapManager,
                titleFont);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Exit game if escape is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update current and previous input states
            prevKb = kb;
            kb = Keyboard.GetState();
            prevMouse = mouse;
            mouse = Mouse.GetState();

            // Update based on current game state
            switch (gameState)
            {
                case MENU:
                    // Update start button
                    startBtn.Update(mouse);
                    
                    // Update timers
                    flashTimer.Update(gameTime);
                    introTimer.Update(gameTime);

                    // Update based on if intro started
                    if (startIntro)
                    {
                        // Update based on flashing state
                        if (flashCount > 0)
                        {
                            UpdateIntroFlashing();
                        }
                        else
                        {
                            UpdateIntroSequence();
                        }
                    }
                    else
                    {
                        // Start intro sequence if start button is pressed
                        if (startBtn.IsDown())
                        {
                            Audio.ButtonClickSfx.CreateInstance().Play();
                            startIntro = true;
                            flashTimer.ResetTimer(true);
                        }
                    }
                    break;

                case GAME:
                    // Repeat background music
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(Audio.BkgMusic);
                    }

                    // Update current map
                    mapManager.UpdateCurr(gameTime, player, kb, prevKb, mouse, prevMouse);

                    // Update player
                    player.Update(gameTime, mapManager.GetCurrMap().GetMap(), kb, prevKb);

                    // Update vision mask timer
                    maskTimer.Update(gameTime);

                    // Decrease mask vision every mask timer cycle
                    if (maskTimer.IsFinished() && circleMaskScale > 1)
                    {
                        maskTimer.ResetTimer(true);
                        circleMaskScale -= 0.2f;
                    }

                    // Cheat
                    if (kb.IsKeyDown(Keys.M))
                    {
                        Player.Cash++;
                    }

                    if (kb.IsKeyDown(Keys.P) && !prevKb.IsKeyDown(Keys.P))
                    {
                        Player.Inventory.Add("Red Heart");
                        Player.Inventory.Add("Blue Heart");
                    }

                    if (kb.IsKeyDown(Keys.O) && !prevKb.IsKeyDown(Keys.O))
                    {
                        Player.Inventory.Add("Prize Ticket");
                    }
                    //Cheat

                    // Go to end game if final game is done
                    if (mapManager.isDoneGame())
                    {
                        // Set game state
                        gameState = END;

                        // Hide player for end sequence
                        player.Hide();

                        // Reset maps
                        mapManager.ResetMaps();
                        mapManager.SetCurrMap(1);

                        // Play end sequence music
                        MediaPlayer.Stop();
                        MediaPlayer.Play(Audio.EndSequenceMusic);
                    }
                    break;

                case END:
                    // Update end sequence
                    endSequence.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Display based on current game state
            switch (gameState)
            {
                case MENU:
                    // Set window background color
                    GraphicsDevice.Clear(Color.SlateGray);

                    // Start sprite batch
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

                    // Display screen background
                    spriteBatch.Draw(currScreenBkg, screenRec, Color.White);

                    // Display start button if intro has not started
                    if (!startIntro)
                    {
                        startBtn.Draw(spriteBatch);
                    }

                    // Display intro message when flashing is finished
                    if (flashCount == 0)
                    {
                        DisplayString.DrawCenteredShadow(spriteBatch, msgFont, currIntroMsg.GetText(), Color.White, Color.Black, screenRec);
                        grayRec.Draw(spriteBatch, Color.Gray * 0.25f, true);
                    }

                    spriteBatch.End();
                    break;

                case GAME:
                    // Set window background color
                    GraphicsDevice.Clear(Color.DarkGray);

                    // Initialize rasterizer state for rendering
                    RasterizerState rasterizerState = new RasterizerState();
                    rasterizerState.ScissorTestEnable = true;
                    GraphicsDevice.ScissorRectangle = GAME_REC;

                    // Begin sprite batch with rasterizer state
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, rasterizerState);

                    // Render differently based on if town is frozen
                    if (IsFrozen)
                    {
                        // Begin sprite batch with grayscale effect
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, rasterizerState, grayscale);

                        // Display current map
                        mapManager.DrawCurr(spriteBatch, player, rasterizerState, grayscale);

                        // Display player
                        player.Draw(spriteBatch);

                        // Begin sprite batch with rasterizer state
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, rasterizerState);

                        // Display vision mask when its scale has reduced to 10
                        if (circleMaskScale <= 10)
                        {
                            spriteBatch.Draw(circleMaskImg, new Rectangle((int)(player.GetRec().X + player.GetRec().Width / 2 - circleMaskImg.Width * circleMaskScale / 2), (int)(player.GetRec().Y + player.GetRec().Height / 2 - circleMaskImg.Height * circleMaskScale / 2), (int)(circleMaskImg.Width * circleMaskScale), (int)(circleMaskImg.Height * circleMaskScale)), Color.White * 0.95f);
                        }
                    }
                    else
                    {
                        // Display current map and player
                        mapManager.DrawCurr(spriteBatch, player, rasterizerState, grayscale);
                        player.Draw(spriteBatch);
                    }

                    // End sprite batch
                    spriteBatch.End();

                    // Begin sprite batch
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

                    // Draw hud
                    mapManager.DrawHud(spriteBatch);
                    player.DrawHud(spriteBatch);

                    // End sprite batch
                    spriteBatch.End();
                    break;

                case END:
                    // Display end sequence
                    GraphicsDevice.Clear(Color.White);
                    endSequence.Draw(spriteBatch, titleFont, player, mapManager, null);
                    break;
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Update the intro flashing
        /// </summary>
        public void UpdateIntroFlashing()
        {
            // Flash screen each flash timer cycle
            if (flashTimer.IsFinished())
            {
                if (currScreenBkg == blankScreenBkg)
                    currScreenBkg = blackScreenBkg;
                else
                    currScreenBkg = blankScreenBkg;
                flashTimer.SetTargetTime(random.Next(MIN_FLASH, MAX_FLASH));
                flashTimer.ResetTimer(true);
                flashCount--;
            }
        }

        /// <summary>
        /// Update intro message sequence
        /// </summary>
        public void UpdateIntroSequence()
        {
            // cheat
            if (kb.IsKeyDown(Keys.Enter))
            {
                Audio.BusSfx.CreateInstance().Play();
                gameState = GAME;
                maskTimer.ResetTimer(true);
            }
            // cheat

            // Update slides based on intro timer state
            if (introTimer.IsFinished())
            {
                // Play next slide sound effect
                Audio.NextSlideSfx.CreateInstance().Play();

                // Reset intro timer
                introTimer.ResetTimer(false);

                // Update based on if next message node is null
                if (currIntroMsg.GetNext() != null)
                {
                    currIntroMsg = currIntroMsg.GetNext();
                }
                else
                {
                    // Set game state to game
                    gameState = GAME;

                    // Play bus sound effect
                    Audio.BusSfx.CreateInstance().Play();
                    
                    // Start vision mask timer
                    maskTimer.ResetTimer(true);
                }
            }
            else if (introTimer.IsInactive())
            {
                // Start intro timer
                introTimer.ResetTimer(true);
            }
        }
    }
}
