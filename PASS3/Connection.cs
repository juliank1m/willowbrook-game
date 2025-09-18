// Author: Julian Kim
// File Name: Connection.cs
// Project Name: PASS3
// Creation Date: May 27, 2024
// Modified Date: June 12, 2024
// Description: A class representing a connection between two maps, holding a location of the tile to transport to next map and the map number it is connected to

namespace PASS3
{
    class Connection
    {
        // Store location
        private Location loc;

        // Store tile type
        private int tileType;

        /// <summary>
        /// Initializes a new instance of <see cref="Connection"/>
        /// </summary>
        /// <param name="loc">The location of this connection</param>
        /// <param name="tileType">The tile type of associated with connection</param>
        public Connection(Location loc, int tileType)
        {
            // Set initial values
            this.loc = loc;
            this.tileType = tileType;
        }

        /// <summary>
        /// Retrieve location of connection
        /// </summary>
        /// <returns>Location of this connection</returns>
        public Location GetLoc()
        {
            return loc;
        }

        /// <summary>
        /// Retrieve tile type associated with this connection
        /// </summary>
        /// <returns>Tile type associated with this connection</returns>
        public int GetTileType()
        {
            return tileType;
        }
    }
}
