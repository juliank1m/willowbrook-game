// Author: Julian Kim
// File Name: DisplayString.cs
// Project Name: PASS3
// Creation Date: May 29, 2024
// Modified Date: June 12, 2024
// Description: A static class for displaying strings in different formats and designs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PASS3
{
    static class DisplayString
    {
        /// <summary>
        /// Displays text horizontally centered at a specified y position
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw</param>
        /// <param name="font">Font used for the text</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="yPos">Y position at which to draw the text</param>
        /// <param name="color">Color of text</param>
        /// <param name="refRec">The reference rectangle to center the text within horizontally</param>
        public static void DrawCenteredX(SpriteBatch spriteBatch, SpriteFont font, string text, float yPos, Color color, Rectangle refRec)
        {
            Vector2 centerPos = new Vector2(refRec.X + refRec.Width / 2 - font.MeasureString(text).X / 2, yPos);
            spriteBatch.DrawString(font, text, centerPos, color);
        }

        /// <summary>
        /// Displays text with a shadow effect at a specified position
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw</param>
        /// <param name="font">Font used for the text</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="pos">Position at which to draw the text</param>
        /// <param name="mainColor">Main color of text</param>
        /// <param name="shadowColor">Shadow color of text</param>
        public static void DrawShadow(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 pos, Color mainColor, Color shadowColor)
        {
            spriteBatch.DrawString(font, text, pos - new Vector2(-2, -2), shadowColor);
            spriteBatch.DrawString(font, text, pos, mainColor);
        }

        /// <summary>
        /// Displays text centered both horizontally and vertically with a shadow effect
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw</param>
        /// <param name="font">Font used for the text</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="mainColor">Main color of text</param>
        /// <param name="shadowColor">Shadow color of text</param>
        /// <param name="refRec">The reference rectangle to center the text within</param>
        public static void DrawCenteredShadow(SpriteBatch spriteBatch, SpriteFont font, string text, Color mainColor, Color shadowColor, Rectangle refRec)
        {
            Vector2 centerPos = new Vector2(refRec.X + refRec.Width / 2 - font.MeasureString(text).X / 2, refRec.Y + refRec.Height / 2 - font.MeasureString(text).Y / 2);
            spriteBatch.DrawString(font, text, centerPos - new Vector2(-2, -2), shadowColor);
            spriteBatch.DrawString(font, text, centerPos, mainColor);
        }

        /// <summary>
        /// Displays text centered horizontally at a specified y position with a shadow effect
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw</param>
        /// <param name="font">Font used for the text</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="yPos">Y position at which to draw the text</param>
        /// <param name="mainColor">Main color of text</param>
        /// <param name="shadowColor">Shadow color of text</param>
        /// <param name="refRec">The reference rectangle to center the text within horizontally</param>
        public static void DrawCenteredXShadow(SpriteBatch spriteBatch, SpriteFont font, string text, float yPos, Color mainColor, Color shadowColor, Rectangle refRec)
        {
            Vector2 centerPos = new Vector2(refRec.X + refRec.Width / 2 - font.MeasureString(text).X / 2, yPos);
            spriteBatch.DrawString(font, text, new Vector2(centerPos.X - 2, centerPos.Y - 2), shadowColor);
            spriteBatch.DrawString(font, text, centerPos, mainColor);
        }
    }
}
