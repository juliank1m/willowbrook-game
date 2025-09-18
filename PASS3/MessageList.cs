// Author: Julian Kim
// File Name: DisplayString.cs
// Project Name: PASS3
// Creation Date: June 8, 2024
// Modified Date: June 12, 2024
// Description: A class that can store MessageNodes as a linked list structure

using System;

namespace PASS3
{
    class MessageList
    {
        //Store the head of the list only which may be null if the list is empty
        private MessageNode head;

        //Maintain the total number of Nodes in the list
        private int count;

        /// <summary>
        /// Initializes a new instance of <see cref="MessageList"/>
        /// </summary>
        public MessageList()
        {
            count = 0;
        }

        /// <summary>
        /// Retrieve the current head of the Linked List
        /// </summary>
        /// <returns>The linked list head</returns>
        public MessageNode GetHead()
        {
            return head;
        }

        /// <summary>
        /// Retrienve the current number of Nodes in the Linked List
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return count;
        }

        /// <summary>
        /// Add a single Node to the end of the Linked List by iterating through the list until either it is found the list is
        /// empty, in which case the new Node is set to be the head or a Node that points to null is located, which will then
        /// point to the new Node
        /// </summary>
        /// <param name="newNode"></param>
        public void AddToTail(MessageNode newNode)
        {
            //Set the new node to the head if the list is currently empty
            if (count == 0)
            {
                head = newNode;
            }
            else
            {
                //Track which node we are currently accessing (looking at), this process begins at the head
                MessageNode curNode = head;

                //Find the tail, curNode will be the tail when the loop ends
                while (curNode.GetNext() != null)
                {
                    //We haven't found the end yet so set the current Node to be the one next in the list
                    curNode = curNode.GetNext();
                }

                //Add the new Node by setting the tail's next reference to the new Node
                curNode.SetNext(newNode);
            }

            //Add one to the count since a new Node was added
            count++;
        }

        /// <summary>
        /// Iterate through the entire Linked List starting at the head and display the Cargo of each Node
        /// </summary>
        public void PrintList()
        {
            //Track which node we are currently accessing (looking at), this process begins at the head
            MessageNode curNode = head;

            //Iterate through the list until we have gone through all Nodes
            for (int i = 0; i < count; i++)
            {
                //Display the current Node's Cargo
                Console.WriteLine(i + ": " + curNode.GetText());

                //We haven't accessed each Node yet so set the current Node to be the one next in the list
                curNode = curNode.GetNext();
            }
        }

        /// <summary>
        /// Delete the head of the Linked List
        /// </summary>
        public void DeleteHead()
        {
            //Set the head to be the next Node in the list
            head = head.GetNext();
            count--;
        }

        /// <summary>
        /// Remove the node at a given position, this will also account for the head position
        /// </summary>
        /// <param name="position">a valid Node position beyond the head</param>
        public void Delete(int position)
        {
            //Call DeleteHead if the position given is the first position in the list.
            if (position == 0)
            {
                DeleteHead();
            }
            else if (position < count)
            {
                //Find the node before the position
                MessageNode curNode = head;
                for (int i = 0; i < position - 1; i++)
                {
                    curNode = curNode.GetNext();
                }

                //If the tail is being deleted, set it to null
                if (position == count - 1)
                {
                    curNode.SetNext(null);
                }
                else
                {
                    //Delete the position Node by having the previous node point
                    //the position's next, bypassing the Node at position
                    curNode.SetNext(curNode.GetNext().GetNext());
                }

                count--;
            }
        }
    }
}
