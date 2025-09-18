// Author: Julian Kim
// File Name: GamerNpc.cs
// Project Name: PASS3
// Creation Date: June 11, 2024
// Modified Date: June 12, 2024
// Description: A class representing a gamer npc

using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PASS3
{
    class GamerNpc : Npc
    {
        // Store item needed to progress
        private string itemNeeded;

        /// <summary>
        /// Initializes a new instance of <see cref="GamerNpc"/>
        /// </summary>
        /// <param name="anim">Animation of npc</param>
        /// <param name="startLoc">Starting location</param>
        /// <param name="name">Name of npc</param>
        /// <param name="description">Description of npc</param>
        /// <param name="content">Content manager used to load content</param>
        /// <param name="itemNeeded">Item needed to progress</param>
        public GamerNpc(Animation anim, Location startLoc, string name, string description, ContentManager content, string itemNeeded) : base(anim, startLoc, name, description, content)
        {
            // Set item needed
            this.itemNeeded = itemNeeded;
        }

        /// <summary>
        /// Update gamer npc
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Complete npc task if player interacts with item needed in inventory
            if (displayDialogue && Player.Inventory.Contains(itemNeeded) && !doneNpc)
            {
                // Start timer
                displayHintMsgTimer.ResetTimer(true);
                
                // Add and remove items from player inventory
                Player.Inventory.Remove(itemNeeded);
                Player.Inventory.Add("Red Heart");

                // Play collect sound effect
                Audio.CollectSfx.CreateInstance().Play();

                // Flag npc task is done
                doneNpc = true;
            }
        }

        /// <summary>
        /// Display npc description
        /// </summary>
        /// <param name="spriteBatch">A sprite batch used to draw</param>
        public override void DrawDescription(SpriteBatch spriteBatch)
        {
            base.DrawDescription(spriteBatch);

            // Display npc drops message if timer is active
            if (displayHintMsgTimer.IsActive())
            {
                DisplayString.DrawShadow(spriteBatch, dialogueFont, "Red Heart\nwas dropped", new Vector2(1050, 250), Color.Gold, Color.Black);
            }
        }
    }
}
