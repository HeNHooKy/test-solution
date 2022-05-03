using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SerializerTests.Interfaces;
using SerializerTests.Nodes;

namespace SerializerTests.Implementations
{
    //Specify your class\file name and complete implementation.
    public class JohnSmithSerializer : IListSerializer
    {
        //the constructor with no parameters is required and no other constructors can be used.
        public JohnSmithSerializer()
        {
            //...
        }

        public Task<ListNode> DeepCopy(ListNode head)
        {
            throw new NotImplementedException();
        }

        public Task<ListNode> Deserialize(Stream s)
        {
            throw new NotImplementedException();
        }

        public Task Serialize(ListNode head, Stream s)
        {
            return new Task(() =>
            {
                var node = head;
                var nodeDict = new Dictionary<ListNode, int>();

                var commadot = Encoding.UTF8.GetBytes(";");

                while (node != null)
                {
                    var data = node.Data;
                    var nodeNumber = TryAddToNodeDict(nodeDict, node);
                    var previousNumber = TryAddToNodeDict(nodeDict, node.Previous);
                    var nextNumber = TryAddToNodeDict(nodeDict, node.Next);
                    var randomNumber = TryAddToNodeDict(nodeDict, node.Random);

                    var nodeBuffer = SerializeNode(data, nodeNumber, previousNumber, nextNumber, randomNumber);
                    s.Write(nodeBuffer);

                    if(node.Next != null)
                    {
                        s.Write(commadot);
                    }

                    node = node.Next;
                }
            });
        }

        /// <summary>
        /// Tries to add new node to node dictionary
        /// </summary>
        /// <param name="nodeDict">Ref to node dictionary</param>
        /// <param name="node">Some node you want to add</param>
        /// <returns>number of node in dictionary or null if node is null</returns>
        private static int TryAddToNodeDict(Dictionary<ListNode, int> nodeDict, ListNode node)
        {
            if(node == null)
            {
                return -1;
            }

            nodeDict.TryAdd(node, nodeDict.Count);
            
            return nodeDict[node];
        }


        /// <summary>
        /// Serializes node with fields:
        /// </summary>
        /// <param name="data">Payload</param>
        /// <param name="number">Number of current node</param>
        /// <param name="previous">Ref to the previous node in the list, 0 for head</param>
        /// <param name="next">Ref to the next node in the list, 0 for tail</param>
        /// <param name="random">Ref to the random node in the list, could be 0</param>
        private static byte[] SerializeNode(string data, int number, int previous, int next, int random)
        {
            var node = new
            {
                Number = number,
                Data = data,
                Previous = previous,
                Next = next,
                Random = random
            };

            var serialized = JsonConvert.SerializeObject(node);
            var buffer = Encoding.UTF8.GetBytes(serialized);

            return buffer;
        }
    }
}
