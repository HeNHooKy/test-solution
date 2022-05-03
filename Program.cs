using SerializerTests.Implementations;
using SerializerTests.Nodes;
using System;
using System.IO;
using System.Text;

namespace LinkedListSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            var head = new ListNode
            {
                Data = "first"
            };

            var current = head;
            
            for(int i = 0; i < 65535; i++)
            {
                var newNode = new ListNode()
                {
                    Data = $"{i}"
                };

                current.Next = newNode;
                newNode.Previous = current;
                current = newNode;
            }

            var ser = new JohnSmithSerializer();

            var stream = new MemoryStream();


            var task = ser.Serialize(head, stream);

            DateTime start = DateTime.Now;
            task.Start();

            task.Wait();
            DateTime end = DateTime.Now;

            var diff = end - start;

            var data = stream.GetBuffer();

            var str = Encoding.UTF8.GetString(data);

            Console.WriteLine(str);
        }
    }
}
