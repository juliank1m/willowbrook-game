// Author: Julian Kim
// File Name: CafeOrdersGame.cs
// Project Name: PASS3
// Creation Date: May 27, 2024
// Modified Date: June 12, 2024
// Description: A minigame with 3 difficulties where the player must memorize an order and make exact order from memory, being rewarded with money if successful

using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace PASS3
{
    class CafeOrdersGame : Minigame
    {
        // Store all ingredient names, images and buttons
        private readonly string[] ingredients = new string[]
        {
            "Milk",
            "Sugar",
            "Marshmallow",
            "Cream"
        };
        private Texture2D[] ingredientImgs;
        private Button[] ingredientBtns;

        // Store all drink names, images and buttons
        private readonly string[] drinkBases = new string[]
        {
            "Coffee",
            "Tea",
            "Juice",
            "Water"
        };
        private Texture2D[] drinkImgs;
        private Button[] drinkBtns;

        // Store all food names, images and buttons
        private readonly string[] foods = new string[]
        {
            "Icecream",
            "Pudding",
            "Chips",
            "Apple Pie",
            "Cookies",
            "Donut",
            "Cake",
        };
        private Texture2D[] foodImgs;
        private Button[] foodBtns;

        // Difficulty levels
        private static int EASY = 0;
        private static int MEDIUM = 1;
        private static int HARD = 2;

        // Constants for number of items in each order
        private const int NUM_OF_INGREDIENTS = 2;
        private const int NUM_OF_DRINKS = 1;
        private const int NUM_OF_FOODS = 2;

        // Store difficulty
        private int difficulty;

        // Store timer to limit time to view order
        private Timer viewOrderTimer;

        // Store number of orders to complete
        private int numOfOrders = 0;

        // Store background image and rectangle
        private Texture2D gameBkg;
        private Rectangle gameRec;

        // Store fonts
        private SpriteFont titleFont;
        private SpriteFont orderFont;

        // Store coffee sign image and rectangle
        private Texture2D coffeeSignImg;
        private Rectangle coffeeSignRec;

        // Store difficulty button and images
        private Texture2D easyImg;
        private Texture2D mediumImg;
        private Texture2D hardImg;
        private Button easyBtn;
        private Button mediumBtn;
        private Button hardBtn;

        // Store given order and player made order
        private Order order;
        private Order orderMade;

        // Store clear button and image
        private Texture2D clearBtnImg;
        private Button clearMadeOrderBtn;

        // Store done button and image
        private Texture2D doneBtnImg;
        private Button doneBtn;

        // Store finish order check
        private bool finishedOrder = false;

        // Store number of completed orders
        private int numCompleted = 0;

        // Store final amount of money earned
        private int finalScore = 0;

        /// <summary>
        /// Initializes a new instance of <see cref="CafeOrdersGame"/>
        /// </summary>
        /// <param name="content">The content manager for loading content</param>
        public CafeOrdersGame(ContentManager content) : base("Cafe Orders")
        {
            // Load background image and rectangle
            gameBkg = content.Load<Texture2D>("Images/Sprites/GameBox");
            gameRec = new Rectangle(Game1.GAME_REC.X + Game1.GAME_REC.Width / 2 - gameBkg.Width, Game1.GAME_REC.Y + Game1.GAME_REC.Height / 2 - gameBkg.Height, gameBkg.Width * 2, gameBkg.Height * 2);

            // Load fonts
            titleFont = content.Load<SpriteFont>("Fonts/TitleFont");
            orderFont = content.Load<SpriteFont>("Fonts/OrderFont");

            // Load coffee sign image and rectangle
            coffeeSignImg = content.Load<Texture2D>("Images/Sprites/CoffeeSign");
            coffeeSignRec = new Rectangle(gameRec.X + 350, gameRec.Y + 300, coffeeSignImg.Width * 2, coffeeSignImg.Height * 2);

            // Load difficulty button images
            easyImg = content.Load<Texture2D>("Images/Sprites/EasyBtn");
            mediumImg = content.Load<Texture2D>("Images/Sprites/MediumBtn");
            hardImg = content.Load<Texture2D>("Images/Sprites/HardBtn");

            // Initialize easy buttons
            easyBtn = new Button(easyImg, new Rectangle(gameRec.X + 100, gameRec.Top + 275, easyImg.Width / 2, easyImg.Height / 2), Color.Gray);
            mediumBtn = new Button(mediumImg, new Rectangle(gameRec.X + 100, easyBtn.GetRec().Bottom + 25, mediumImg.Width / 2, mediumImg.Height / 2), Color.Gray);
            hardBtn = new Button(hardImg, new Rectangle(gameRec.X + 100, mediumBtn.GetRec().Bottom + 25, hardImg.Width / 2, hardImg.Height / 2), Color.Gray);
            easyBtn.Clicked += EasyBtnClick;
            mediumBtn.Clicked += MediumBtnClick;
            hardBtn.Clicked += HardBtnClick;

            // Load food images
            foodImgs = new Texture2D[foods.Length];
            foodImgs[0] = content.Load<Texture2D>("Images/Sprites/Icecream");
            foodImgs[1] = content.Load<Texture2D>("Images/Sprites/Pudding");
            foodImgs[2] = content.Load<Texture2D>("Images/Sprites/Chips");
            foodImgs[3] = content.Load<Texture2D>("Images/Sprites/ApplePie");
            foodImgs[4] = content.Load<Texture2D>("Images/Sprites/Cookies");
            foodImgs[5] = content.Load<Texture2D>("Images/Sprites/Donut");
            foodImgs[6] = content.Load<Texture2D>("Images/Sprites/Cake");

            // Initialize food buttons
            foodBtns = new Button[foodImgs.Length];
            for (int i = 0; i < foodBtns.Length; i++)
            {
                foodBtns[i] = new Button(foodImgs[i], new Rectangle(gameRec.X + 20 + i * 75, gameRec.Y + 100, foodImgs[i].Width, foodImgs[i].Height), Color.Gray);
            }

            // Load drink images
            drinkImgs = new Texture2D[drinkBases.Length];
            drinkImgs[0] = content.Load<Texture2D>("Images/Sprites/Coffee");
            drinkImgs[1] = content.Load<Texture2D>("Images/Sprites/Tea");
            drinkImgs[2] = content.Load<Texture2D>("Images/Sprites/Juice");
            drinkImgs[3] = content.Load<Texture2D>("Images/Sprites/Water");

            // Initialize drink buttons
            drinkBtns = new Button[drinkImgs.Length];
            for (int i = 0; i < drinkBtns.Length; i++)
            {
                drinkBtns[i] = new Button(drinkImgs[i], new Rectangle(gameRec.X + 20 + i * 100, gameRec.Y + 200, drinkImgs[i].Width, drinkImgs[i].Height), Color.Gray);
            }

            // Load ingredient images
            ingredientImgs = new Texture2D[ingredients.Length];
            ingredientImgs[0] = content.Load<Texture2D>("Images/Sprites/Milk");
            ingredientImgs[1] = content.Load<Texture2D>("Images/Sprites/Sugar");
            ingredientImgs[2] = content.Load<Texture2D>("Images/Sprites/Marshmallows");
            ingredientImgs[3] = content.Load<Texture2D>("Images/Sprites/Cream");

            // Initialize ingredient buttons
            ingredientBtns = new Button[ingredientImgs.Length];
            for (int i = 0; i < ingredientBtns.Length; i++)
            {
                ingredientBtns[i] = new Button(ingredientImgs[i], new Rectangle(gameRec.X + 20 + i * 100, gameRec.Y + 300, ingredientImgs[i].Width, ingredientImgs[i].Height), Color.Gray);
            }

            // Initialize orders
            order = new Order();
            orderMade = new Order();

            // Load clear button image and button
            clearBtnImg = content.Load<Texture2D>("Images/Sprites/ClearBtn");
            clearMadeOrderBtn = new Button(clearBtnImg, new Rectangle(gameRec.X + 25, gameRec.Bottom - 100, clearBtnImg.Width / 2, clearBtnImg.Height / 2), Color.Gray);

            // Load done button image and button
            doneBtnImg = content.Load<Texture2D>("Images/Sprites/DoneBtn");
            doneBtn = new Button(doneBtnImg, new Rectangle(gameRec.Right - 150, gameRec.Bottom - 100, doneBtnImg.Width / 2, doneBtnImg.Height / 2), Color.Gray);
        }

        /// <summary>
        /// Starts game and resets game data based on difficulty chosen
        /// </summary>
        public override void StartGame()
        {
            base.StartGame();

            // Sets timer and number of orders to complete based on difficulty chosen
            if (difficulty == EASY)
            {
                viewOrderTimer = new Timer(5000, true);
                numOfOrders = 2;
            }
            else if (difficulty == MEDIUM)
            {
                viewOrderTimer = new Timer(3000, true);
                numOfOrders = 3;
            }
            else
            {
                viewOrderTimer = new Timer(1500, true);
                numOfOrders = 4;
            }

            // Reset all game data 
            order.Clear();
            viewOrderTimer.ResetTimer(true);
            GenerateRandomOrder();
            finalScore = 0;
            numCompleted = 0;
            orderMade.Clear();
            finishedOrder = false;
        }

        /// <summary>
        /// Updates game based on inputs and game states
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        /// <param name="kb">Current keyboard state</param>
        /// <param name="prevKb">Previous keyboard state</param>
        /// <param name="mouse">Current mouse state</param>
        /// <param name="prevMouse">Previous mouse state</param>
        public override void Update(GameTime gameTime, KeyboardState kb, KeyboardState prevKb, MouseState mouse, MouseState prevMouse)
        {
            // Update if the game has been opened
            if (gameOpened)
            {
                // Update based on game state
                if (gameState == MENU)
                {
                    // Update difficulty buttons
                    easyBtn.Update(mouse);
                    mediumBtn.Update(mouse);
                    hardBtn.Update(mouse);

                    // Close game if [x] is pressed
                    if (kb.IsKeyDown(Keys.X) && !prevKb.IsKeyDown(Keys.X))
                    {
                        CloseGame();
                    }
                }
                else if (gameState == GAME)
                {
                    // Update viewing timer
                    viewOrderTimer.Update(gameTime);

                    // Update choosing items stage when timer has ran out
                    if (viewOrderTimer.IsFinished())
                    {
                        // Update buttons
                        UpdateBtns(mouse, prevMouse);
                        
                        // Update post choosing items stage when finished making order
                        if (finishedOrder)
                        {
                            // Play pour sound effect
                            Audio.PourSfx.CreateInstance().Play();

                            // Calculate final score
                            finalScore += CountCorrect() * (difficulty + 1);

                            // Decrement number of orders and end game or restart depending on orders left
                            numOfOrders--;
                            if (numOfOrders <= 0)
                            {
                                gameState = END;
                                Player.Cash += finalScore;
                            }
                            else
                            {
                                ResetRound();
                            }
                        }
                    }
                }
                else
                {
                    // Update done button
                    doneBtn.Update(mouse);
                    if (doneBtn.IsDown() && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        // Exit minigame
                        CloseGame();
                    }
                }
            }
        }

        /// <summary>
        /// Reset game data between orders
        /// </summary>
        private void ResetRound()
        {
            order.Clear();
            orderMade.Clear();
            finishedOrder = false;
            viewOrderTimer.ResetTimer(true);
            GenerateRandomOrder();
        }

        /// <summary>
        /// Update all buttons for adding foods, drinks and ingredients, and clearing or finishing made orders
        /// </summary>
        /// <param name="mouse">Current mouse state</param>
        /// <param name="prevMouse">Previous mouse state</param>
        private void UpdateBtns(MouseState mouse, MouseState prevMouse)
        {
            // Update clear button
            clearMadeOrderBtn.Update(mouse);
            if (clearMadeOrderBtn.IsDown() && prevMouse.LeftButton != ButtonState.Pressed)
            {
                orderMade.Clear();
            }

            // Update done button
            doneBtn.Update(mouse);
            if (doneBtn.IsDown() && prevMouse.LeftButton != ButtonState.Pressed)
            {
                finishedOrder = true;
            }

            // Only update choice buttons if order is not full
            if (orderMade.TotalCount() <= 12)
            {
                // Update all food buttons
                for (int i = 0; i < foodBtns.Length; i++)
                {
                    foodBtns[i].Update(mouse);
                    if (foodBtns[i].IsDown() && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        orderMade.Foods.Add(i);
                    }
                }

                // Update all drink buttons
                for (int i = 0; i < drinkBtns.Length; i++)
                {
                    drinkBtns[i].Update(mouse);
                    if (drinkBtns[i].IsDown() && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        orderMade.Drinks.Add(i);
                    }
                }

                // Update all ingredients buttons
                for (int i = 0; i < ingredientBtns.Length; i++)
                {
                    ingredientBtns[i].Update(mouse);
                    if (ingredientBtns[i].IsDown() && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        orderMade.Ingredients.Add(i);
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
            //Display if the game is opened
            if (gameOpened)
            {
                // Display background image
                spriteBatch.Draw(gameBkg, gameRec, Color.White);

                // Display based on game state
                if (gameState == MENU)
                {
                    // Display menu screen
                    DisplayString.DrawCenteredXShadow(spriteBatch, titleFont, "Welcome to the Cafe!", gameRec.Top + 30, Color.LightCyan, Color.Black, gameRec);
                    DisplayString.DrawShadow(spriteBatch, titleFont, "Choose a Difficulty", new Vector2(gameRec.X + 50, gameRec.Y + 175), Color.LightCyan, Color.Black);
                    spriteBatch.Draw(coffeeSignImg, coffeeSignRec, Color.White);
                    DisplayString.DrawShadow(spriteBatch, orderFont, "Press [X] to exit job", new Vector2(gameRec.Right - 400, gameRec.Bottom - 50), Color.White, Color.Black);

                    // Display difficulty buttons
                    easyBtn.Draw(spriteBatch);
                    mediumBtn.Draw(spriteBatch);
                    hardBtn.Draw(spriteBatch);
                }
                else if (gameState == GAME)
                {
                    // Display game title
                    DisplayString.DrawCenteredXShadow(spriteBatch, titleFont, "Cafe Orders!", gameRec.Top + 30, Color.LightCyan, Color.Black, gameRec);
                    
                    // Display based on timer state
                    if (viewOrderTimer.IsActive())
                    {
                        // Display order given
                        DisplayOrder(spriteBatch);
                    }
                    else
                    {
                        // Display choices and order made
                        DisplayChoices(spriteBatch);
                        DisplayMadeOrder(spriteBatch);
                    }
                }
                else
                {
                    // Display end screen
                    DisplayString.DrawCenteredXShadow(spriteBatch, titleFont, "Cafe Orders Completed: " + numCompleted, gameRec.Top + 30, Color.LightCyan, Color.Black, gameRec);
                    DisplayString.DrawCenteredXShadow(spriteBatch, titleFont, "Paid: " + finalScore, gameRec.Top + 75, Color.LightCyan, Color.Black, gameRec);
                    
                    // Display done button
                    doneBtn.Draw(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Display choice buttons for foods, drinks, and ingredients, as well as clear and done buttons
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw images and strings</param>
        private void DisplayChoices(SpriteBatch spriteBatch)
        {
            // Display all food buttons
            foreach (Button btn in foodBtns)
            {
                btn.Draw(spriteBatch);
            }

            // Display all drink buttons
            foreach (Button btn in drinkBtns)
            {
                btn.Draw(spriteBatch);
            }

            // Display all ingredient buttons 
            foreach (Button btn in ingredientBtns)
            {
                btn.Draw(spriteBatch);
            }

            // Display clear and done buttons
            clearMadeOrderBtn.Draw(spriteBatch);
            doneBtn.Draw(spriteBatch);
        }

        /// <summary>
        /// Display current given order
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DisplayOrder(SpriteBatch spriteBatch)
        {
            // Display title
            DisplayString.DrawCenteredXShadow(spriteBatch, titleFont, "Order", gameRec.Top + 100, Color.White, Color.Black, gameRec);

            // Display foods
            for (int i = 0; i < order.Foods.Count; i++)
            {
                DisplayString.DrawCenteredXShadow(spriteBatch, orderFont, foods[order.Foods[i]], gameRec.Top + 150 + i * 30, Color.White, Color.Black, gameRec);
            }

            // Display drinks
            for (int i = 0; i < order.Drinks.Count; i++)
            {
                DisplayString.DrawCenteredXShadow(spriteBatch, orderFont, drinkBases[order.Drinks[i]], gameRec.Top + 150 + 30 * order.Foods.Count + i * 30, Color.White, Color.Black, gameRec);
            }

            // Display ingredients
            for (int i = 0; i < order.Ingredients.Count; i++)
            {
                DisplayString.DrawCenteredXShadow(spriteBatch, orderFont, ingredients[order.Ingredients[i]], gameRec.Top + 150 + 30 * (order.Foods.Count + order.Drinks.Count) + i * 30, Color.White, Color.Black, gameRec);
            }
        }

        /// <summary>
        /// Display order made by player
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void DisplayMadeOrder(SpriteBatch spriteBatch)
        {
            // Display title
            DisplayString.DrawShadow(spriteBatch, titleFont, "Serving...", new Vector2(gameRec.X + 450, gameRec.Top + 250), Color.White, Color.Black);

            // Display foods
            for (int i = 0; i < orderMade.Foods.Count; i++)
            {
                DisplayString.DrawShadow(spriteBatch, orderFont, foods[orderMade.Foods[i]], new Vector2(gameRec.X + 450, gameRec.Top + 300 + i * 30), Color.White, Color.Black);
            }

            // Display drinks
            for (int i = 0; i < orderMade.Drinks.Count; i++)
            {
                DisplayString.DrawShadow(spriteBatch, orderFont, drinkBases[orderMade.Drinks[i]], new Vector2(gameRec.X + 450, gameRec.Top + 300 + 30 * orderMade.Foods.Count + i * 30), Color.White, Color.Black);
            }

            // Display ingredients
            for (int i = 0; i < orderMade.Ingredients.Count; i++)
            {
                DisplayString.DrawShadow(spriteBatch, orderFont, ingredients[orderMade.Ingredients[i]], new Vector2(gameRec.X + 450, gameRec.Top + 300 + 30 * (orderMade.Foods.Count + orderMade.Drinks.Count) + i * 30), Color.White, Color.Black);
            }
        }

        /// <summary>
        /// Start easy game
        /// </summary>
        private void EasyBtnClick()
        {
            difficulty = EASY;
            StartGame();
        }

        /// <summary>
        /// Start medium game
        /// </summary>
        private void MediumBtnClick()
        {
            difficulty = MEDIUM;
            StartGame();
        }

        /// <summary>
        /// Start hard game
        /// </summary>
        private void HardBtnClick()
        {
            difficulty = HARD;
            StartGame();
        }

        /// <summary>
        /// Generates a random order to be given depending on difficulty
        /// </summary>
        public void GenerateRandomOrder()
        {
            // Store Random instance
            Random random = new Random();

            // Generate random foods
            for (int i = 0; i < NUM_OF_FOODS; i++)
            {
                order.Foods.Add(random.Next(Enum.GetNames(typeof(Foods)).Length));
            }

            // Generate random drinks
            for (int i = 0; i < NUM_OF_DRINKS; i++)
            {
                // Generate based on difficulty
                if (difficulty == EASY)
                {
                    // Generate from between two drinks
                    int easyDrinkIndex = random.Next(2);
                    if (easyDrinkIndex == 0)
                    {
                        order.Drinks.Add((int)Drinks.Juice);
                    }
                    else
                    {
                        order.Drinks.Add((int)Drinks.Water);
                    }
                }
                else
                {
                    // Generate from any of the drinks
                    order.Drinks.Add(random.Next(Enum.GetNames(typeof(Drinks)).Length));
                }
            }

            // Generate ingredients if drinks does not contain juice or water
            if (!order.Drinks.Contains((int)Drinks.Juice) && !order.Drinks.Contains((int)Drinks.Water))
            {
                for (int i = 0; i < NUM_OF_INGREDIENTS; i++)
                {
                    order.Ingredients.Add(random.Next(Enum.GetNames(typeof(Ingredients)).Length));
                }
            }
        }

        /// <summary>
        /// Compare made order and given order and count number of correct items
        /// </summary>
        /// <returns>Total count of correct items in made order</returns>
        public int CountCorrect()
        {
            // Initialize correct count
            int correct = 0;

            // Sort given order
            order.Foods.OrderBy(x => x);
            order.Drinks.OrderBy(x => x);
            order.Ingredients.OrderBy(x => x);

            // Sort made order
            orderMade.Foods.OrderBy(x => x);
            orderMade.Drinks.OrderBy(x => x);
            orderMade.Ingredients.OrderBy(x => x);

            // Compare foods in order
            for (int i = 0; i < order.Foods.Count; i++)
            {
                if (i >= orderMade.Foods.Count)
                {
                    break;
                }
                else if (orderMade.Foods[i] == order.Foods[i])
                {
                    correct++;
                }
            }

            // Compare drinks in order
            for (int i = 0; i < order.Drinks.Count; i++)
            {
                if (i >= orderMade.Drinks.Count)
                {
                    break;
                }
                else if (orderMade.Drinks[i] == order.Drinks[i])
                {
                    correct++;
                }
            }

            // Compare ingredients in order
            for (int i = 0; i < order.Ingredients.Count; i++)
            {
                if (i >= orderMade.Ingredients.Count)
                {
                    break;
                }
                else if (orderMade.Ingredients[i] == order.Ingredients[i])
                {
                    correct++;
                }
            }

            return correct;
        }
    }
}
