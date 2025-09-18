// Author: Julian Kim
// File Name: MessageNode.cs
// Project Name: PASS3
// Creation Date: June 8, 2024
// Modified Date: June 12, 2024
// Description: A class representing a message as a linked list node

namespace PASS3
{
    class MessageNode
    {
        //Cargo data to be maintained
        private string text;

        //A reference to the Next Node in the list
        private MessageNode next;

        /// <summary>
        /// Initializes a new instance of <see cref="MessageNode"/>
        /// </summary>
        /// <param name="text"></param>
        public MessageNode(string text)
        {
            this.text = text;
        }


        //Accessors & Modifiers

        /// <summary>
        /// Set the Cargo of the Node to id
        /// </summary>
        /// <param name="id">The new int value for the Cargo</param>
        public void SetText(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// Set the Node this Node references as the next Node in the list to node
        /// </summary>
        /// <param name="node">null or a valid Node in the Linked List</param>
        public void SetNext(MessageNode node)
        {
            next = node;
        }

        /// <summary>
        /// Retrieve the Cargo of the node
        /// </summary>
        /// <returns>an int representing the Node's Cargo</returns>
        public string GetText()
        {
            return text;
        }

        /// <summary>
        /// Retrieve the Node this Node references as the next Node in the list
        /// </summary>
        /// <returns>null or a valid Node in the Linked list</returns>
        public MessageNode GetNext()
        {
            return next;
        }
    }
}
