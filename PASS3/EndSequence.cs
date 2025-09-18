// Author: Julian Kim
// File Name: EndSequence.cs
// Project Name: PASS3
// Creation Date: June 11, 2024
// Modified Date: June 12, 2024
// Description: A class to display and manage functions of the end sequence of the game

using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace PASS3
{
    class EndSequence
    {
        // Store background animation and opacity
        private Animation bkgAnim;
        private float bkgOpacity = 0;

        // Store grayscale effect
        private Effect grayScaleFx;
        private float grayScaleFactor = 0;

        // Store end sequence states
        private bool undoThief = true;
        private bool undoGamer = false;

        // Store map manager
        private MapManager mapManager;

        // Store npc animations
        private Animation birdAnim;
        private Animation catAnim;

        // Store title font
        private SpriteFont titleFont;

        // Store final message data
        private float messagePosY;
        private float messageFinalY;
        private float messageTheta;
        private const float MESSAGE_MAX_THETA = 360;
        private const float MESSAGE_MIN_THETA = 180;
        private string message;

        /// <summary>
        /// Initializes a new instance of <see cref="EndSequence"/>
        /// </summary>
        /// <param name="bkgAnim">Background animation</param>
        /// <param name="content">Content manager for loading content</param>
        /// <param name="grayScaleFx">Effect for grayscale rendering</param>
        /// <param name="mapManager">Manager for handling game maps</param>
        /// <param name="titleFont">Font for displaying title</param>
        public EndSequence(Animation bkgAnim, ContentManager content, Effect grayScaleFx, MapManager mapManager, SpriteFont titleFont)
        {
            // Set background animation
            this.bkgAnim = bkgAnim;

            // Set grayscale effect
            this.grayScaleFx = grayScaleFx;
            
            // Set map manager
            this.mapManager = mapManager;

            // Load npc animation
            birdAnim = new Animation(content.Load<Texture2D>("Images/Sprites/BackBirdAnim"), 4, 1, 4, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 1250, new Vector2(490, 160), 5f, true);
            catAnim = new Animation(content.Load<Texture2D>("Images/Sprites/BackCatAnim"), 4, 1, 4, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 1250, new Vector2(650, 160), 5f, true);
            
            // Set title font
            this.titleFont = titleFont;

            // Set final message data
            message = "Thank You!";
            messageTheta = 180;
            messagePosY = 350;
            messageFinalY = messagePosY;
        }

        /// <summary>
        /// Update end sequence based on states
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Update based on current scene
            if (undoThief)
            {
                UpdateBirdScene(gameTime);
            }
            else if (undoGamer)
            {
                UpdateCatScene(gameTime);
            }
            else
            {
                UpdateTogetherScene(gameTime);

                // Exit program when music is finished playing
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Update bird scene
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        private void UpdateBirdScene(GameTime gameTime)
        {
            // Update current map
            mapManager.UpdateCurr(gameTime);

            // Increment gray scale factor while under 1
            if (grayScaleFactor < 1)
            {
                grayScaleFactor += 0.0025f;
                if (grayScaleFactor >= 0.3f)
                {
                    // Unfreeze map
                    Game1.IsFrozen = false;
                }
            }
            else
            {
                // Go to next scene
                undoGamer = true;
                undoThief = false;
                mapManager.SetCurrMap(4);

                // Reset grayscale and frozen check
                grayScaleFactor = 0;
                Game1.IsFrozen = true;
            }
        }

        /// <summary>
        /// Update cat scene
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        private void UpdateCatScene(GameTime gameTime)
        {
            // Update current map
            mapManager.UpdateCurr(gameTime);

            // Increment gray scale factor while under 1
            if (grayScaleFactor < 1)
            {
                grayScaleFactor += 0.0025f;
                if (grayScaleFactor >= 0.3f)
                {
                    // Unfreeze game
                    Game1.IsFrozen = false;
                }
            }
            else
            {
                // Go to next scene
                undoGamer = false;
            }
        }

        /// <summary>
        /// Update end scene
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public void UpdateTogetherScene(GameTime gameTime)
        {
            // Update background animation
            bkgAnim.Update(gameTime);

            // Update npc animations
            birdAnim.Update(gameTime);
            catAnim.Update(gameTime);

            // Increment background opacity while under 1
            if (bkgOpacity < 1)
            {
                bkgOpacity += 0.001f;
            }

            // Update final message y position
            messageTheta += 1f;
            messageTheta = MathHelper.Clamp(messageTheta, MESSAGE_MIN_THETA, MESSAGE_MAX_THETA);
            messagePosY = messageFinalY - 50 * (float)Math.Sin(MathHelper.ToRadians(MESSAGE_MAX_THETA)) + 50 * (float)Math.Sin(MathHelper.ToRadians(messageTheta));
        }

        /// <summary>
        /// Displays the end sequence
        /// </summary>
        /// <param name="spriteBatch">The sprite batch for drawing</param>
        /// <param name="spriteFont">Font for displaying text</param>
        /// <param name="player">Player instance</param>
        /// <param name="mapManager">MapManager instance</param>
        /// <param name="rasterizerState">RasterizerState instance for rendering</param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, Player player, MapManager mapManager, RasterizerState rasterizerState)
        {
            // Display based on current scene
            if (undoThief || undoGamer)
            {
                // Set grayscale factor
                grayScaleFx.Parameters["blendFactor"].SetValue(grayScaleFactor);

                // Begin sprite batch with grayscale rendering
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, grayScaleFx);

                // Display current map
                mapManager.DrawCurr(spriteBatch, player, rasterizerState, grayScaleFx);

                // End sprite batch
                spriteBatch.End();
            }
            else
            {
                // Begin sprite batch
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

                // Display background animation
                bkgAnim.Draw(spriteBatch, Color.White * bkgOpacity);

                // Display npc animations
                birdAnim.Draw(spriteBatch, Color.White * bkgOpacity);
                catAnim.Draw(spriteBatch, Color.White * bkgOpacity);

                // Display final message
                DisplayString.DrawCenteredXShadow(spriteBatch, titleFont, message, messagePosY, Color.White * bkgOpacity, Color.Black * bkgOpacity, new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight));
                
                // End sprite batch
                spriteBatch.End();
            }
        }
    }
}
