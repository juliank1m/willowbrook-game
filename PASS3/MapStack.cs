// Author: Julian Kim
// File Name: MapStack.cs
// Project Name: PASS3
// Creation Date: May 23, 2024
// Modified Date: June 12, 2024
// Description: A class holding maps in a stack structure

using System;
using System.Collections.Generic;

namespace PASS3
{
    class MapStack
    {
        // Store maps in list
        private List<Map> maps;

        /// <summary>
        /// Initializes a new instance of <see cref="MapStack"/>
        /// </summary>
        public MapStack()
        {
            // Initialize maps list
            maps = new List<Map>();
        }

        /// <summary>
        /// Add map to top of stack
        /// </summary>
        /// <param name="map">Map to add</param>
        public void Push(Map map)
        {
            maps.Add(map);
        }

        /// <summary>
        /// Remove map from top of stack if not empty
        /// </summary>
        /// <returns>Removed map if not empty, otherwise null</returns>
        public Map Pop()
        {
            if (maps.Count > 0)
            {
                Map map = maps[maps.Count - 1];
                maps.RemoveAt(maps.Count - 1);

                return map;
            }

            return null;
        }

        /// <summary>
        /// Clear stack
        /// </summary>
        public void Clear()
        {
            maps.Clear();
        }

        /// <summary>
        /// Retrieve map from top of stack if not empty
        /// </summary>
        /// <returns>Map from top of stack if not empty, otherwise null</returns>
        public Map Top()
        {
            if (maps.Count > 0)
            {
                return maps[maps.Count - 1];
            }

            return null;
        }

        /// <summary>
        /// Retrieve size of stack
        /// </summary>
        /// <returns>Size of stack</returns>
        public int Size()
        {
            return maps.Count;
        }

        /// <summary>
        /// Determine whether stack is empty
        /// </summary>
        /// <returns>True if empty, otherwise false</returns>
        public bool isEmpty()
        {
            return maps.Count <= 0;
        }

        /// <summary>
        /// Determine wheter stack contains specified map
        /// </summary>
        /// <param name="map">Map to look for</param>
        /// <returns>True if found, otherwise false</returns>
        public bool Contains(Map map)
        {
            return maps.Contains(map);
        }
    }
}
