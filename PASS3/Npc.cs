// Author: Julian Kim
// File Name: Npc.cs
// Project Name: PASS3
// Creation Date: May 25, 2024
// Modified Date: June 12, 2024
// Description: A parent class representing a basic NPC

using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PASS3
{
    class Npc
    {
        // Store idle animation
        protected Animation idleAnim;

        // Store position rectangle
        protected Rectangle rec;

        // Store position and grid location
        protected Vector2 pos;
        protected Location gridLoc;

        // Store npc name and description
        protected string name;
        protected string description;

        // Store flag for displaying dialogue
        protected bool displayDialogue = false;

        // Store fonts
        protected SpriteFont dialogueFont;
        protected SpriteFont titleFont;

        // Store flag for whether npc task is complete
        protected bool doneNpc = false;

        // Store timer for displaying drops
        protected Timer displayHintMsgTimer;

        /// <summary>
        /// Initializes a new instance of <see cref="Npc"/>
        /// </summary>
        /// <param name="anim">Npc animation</param>
        /// <param name="startLoc">Starting location of npc</param>
        /// <param name="name">Name of npc</param>
        /// <param name="description">Npc description</param>
        /// <param name="content">Content manager to load content</param>
        public Npc(Animation anim, Location startLoc, string name, string description, ContentManager content)
        {
            // Set idle animation
            idleAnim = anim;

            // Set initial location
            gridLoc = startLoc;

            // Set name and description
            this.name = name;
            this.description = description;

            // Load fonts
            dialogueFont = content.Load<SpriteFont>("Fonts/MessageFont");
            titleFont = content.Load<SpriteFont>("Fonts/OrderFont");

            // Set position and rectangle
            pos = gridLoc.ToVector2();
            rec = anim.GetDestRec();

            // Initialize timer for displaying drops
            displayHintMsgTimer = new Timer(6000, false);
        }

        /// <summary>
        /// Updates npc
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public virtual void Update(GameTime gameTime)
        {
            // Pause or unpause animation based on if town is frozen
            if (Game1.IsFrozen)
                idleAnim.Pause();
            else
                idleAnim.Resume();

            // Update animation
            idleAnim.Update(gameTime);

            // Update drops timer
            displayHintMsgTimer.Update(gameTime);
        }

        /// <summary>
        /// Set whether npc is being interacted with
        /// </summary>
        /// <param name="b">True if npc is being interacted with, otherwise false</param>
        public void InteractedWith(bool b)
        {
            displayDialogue = b;
        }

        /// <summary>
        /// Display npc and dialogue
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Display idle animation
            idleAnim.Draw(spriteBatch, Color.White);

            // Display dialogue if npc is being interacted with
            if (displayDialogue)
            {
                DisplayString.DrawCenteredXShadow(spriteBatch, dialogueFont, "...", pos.Y - 25, Color.White, Color.Black, idleAnim.GetDestRec());
            }
        }

        /// <summary>
        /// Display hud description of npc
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw</param>
        public virtual void DrawDescription(SpriteBatch spriteBatch)
        {
            // Display description if npc is being interacted with
            if (displayDialogue)
            {
                DisplayString.DrawShadow(spriteBatch, titleFont, name, new Vector2(1050, 50), Color.White, Color.Black);
                DisplayString.DrawShadow(spriteBatch, dialogueFont, description, new Vector2(1050, 100), Color.White, Color.Black);
            }
        }
    }
}
