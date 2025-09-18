// Author: Julian Kim
// File Name: ThiefNpc.cs
// Project Name: PASS3
// Creation Date: June 10, 2024
// Modified Date: June 12, 2024
// Description: A class representing a thief npc

using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PASS3
{
    class ThiefNpc : Npc
    {
        // Store money needed to progress
        private int moneyNeeded;

        /// <summary>
        /// Initializes a new instance of <see cref="ThiefNpc"/>
        /// </summary>
        /// <param name="anim">Thief animation</param>
        /// <param name="startLoc">Starting grid location</param>
        /// <param name="name">Npc name</param>
        /// <param name="description">Npc description</param>
        /// <param name="content">Content manager to load content</param>
        /// <param name="moneyNeeded">Money needed to progress</param>
        public ThiefNpc(Animation anim, Location startLoc, string name, string description, ContentManager content, int moneyNeeded) : base(anim, startLoc, name, description, content)
        {
            // Set money needed
            this.moneyNeeded = moneyNeeded;
        }

        /// <summary>
        /// Update thief npc
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Finish npc task if player has enough money and interacts with thief
            if (displayDialogue && Player.Cash >= moneyNeeded && !doneNpc)
            {
                // Start drop timer
                displayHintMsgTimer.ResetTimer(true);

                // Give and remove player items
                Player.Cash -= moneyNeeded;
                Player.Inventory.Add("Arcade Token");
                Player.Inventory.Add("Blue Heart");

                // Play collect sound effect
                Audio.CollectSfx.CreateInstance().Play();

                // Flag done with npc task
                doneNpc = true;
            }
        }

        /// <summary>
        /// Display thief description
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw</param>
        public override void DrawDescription(SpriteBatch spriteBatch)
        {
            base.DrawDescription(spriteBatch);

            // Display drops description if timer is active
            if (displayHintMsgTimer.IsActive())
                DisplayString.DrawShadow(spriteBatch, dialogueFont, "Arcade Token\nand\nBlue Heart\nwas dropped", new Vector2(1050, 250), Color.Gold, Color.Black);
        }
    }
}
