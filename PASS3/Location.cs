// Author: Julian Kim
// File Name: Location.cs
// Project Name: PASS3
// Creation Date: May 9, 2024
// Modified Date: June 12, 2024
// Description: A struct representing a grid location in (row, col)

using Microsoft.Xna.Framework;

namespace PASS3
{
    struct Location
    {
        /// <summary>
        /// Stores the row and column indexes
        /// </summary>
        public int Row { get; set; }
        public int Col { get; set; }

        /// <summary>
        /// Stores a constant impossible location
        /// </summary>
        public static Location NO_LOC { get; } = new Location(-1, -1);

        /// <summary>
        /// Initializes a new instance of <see cref="Location"/>
        /// </summary>
        /// <param name="row">The row index of location</param>
        /// <param name="col">The column index of location</param>
        public Location(int row, int col)
        {
            // Set row and column
            Row = row;
            Col = col;
        }

        /// <summary>
        /// Convert location to a Vector2 position
        /// </summary>
        /// <returns>A vector2 representing the position of the grid location</returns>
        public Vector2 ToVector2()
        {
            return new Vector2(Game1.GAME_REC.X + Col * Game1.TILE_SIZE, Game1.GAME_REC.Y + Row * Game1.TILE_SIZE);
        }
    }
}
