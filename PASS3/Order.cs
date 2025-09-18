// Author: Julian Kim
// File Name: Order.cs
// Project Name: PASS3
// Creation Date: May 30, 2024
// Modified Date: June 12, 2024
// Description: A class representing an order from the cafe minigame

using System.Collections.Generic;

namespace PASS3
{
    class Order
    {
        /// <summary>
        /// Store order items
        /// </summary>
        public List<int> Ingredients { get; set; }
        public List<int> Foods { get; set; }
        public List<int> Drinks { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Order"/>
        /// </summary>
        public Order()
        {
            // Initialize item lists
            Ingredients = new List<int>();
            Foods = new List<int>();
            Drinks = new List<int>();
        }

        /// <summary>
        /// Retrieve total count of items
        /// </summary>
        /// <returns>Count of all items</returns>
        public int TotalCount()
        {
            return Foods.Count + Drinks.Count + Ingredients.Count;
        }

        /// <summary>
        /// Clear order
        /// </summary>
        public void Clear()
        {
            // Clear all lists of items
            Foods.Clear();
            Drinks.Clear();
            Ingredients.Clear();
        }
    }

    /// <summary>
    /// Represents various foods
    /// </summary>
    public enum Foods
    {
        Icecream,
        Pudding,
        Chips,
        ApplePie,
        Cookies,
        Donut,
        Cake
    }

    /// <summary>
    /// Represents various drinks
    /// </summary>
    public enum Drinks
    {
        Coffee,
        Tea,
        Juice,
        Water
    }

    /// <summary>
    /// Represents various ingredients
    /// </summary>
    public enum Ingredients
    {
        Milk,
        Sugar,
        Marshmallows,
        Cream
    }
}
