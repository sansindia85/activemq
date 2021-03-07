using System;
using Apache.NMS;

namespace Subscriber
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Waiting for messages");

            //Read all messages off the queue.
            while (ReadNextMessage())
            {
                Console.WriteLine("Successfully read message.");
            }

            Console.WriteLine("Finished");
        }

        private static bool ReadNextMessage()
        {
            const string queueName = "TestTextQueue";

            var brokerUri = $"amqp://guest:guest@localhost:5672";

            var connectionFactory = new NMSConnectionFactory(brokerUri);

            using var connection = connectionFactory.CreateConnection();
            connection.Start();

            using var session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
            using var destination = session.GetQueue(queueName);
            using var consumer = session.CreateConsumer(destination);

            var message = consumer.Receive();
            if (message is ITextMessage textMessage)
            {
                Console.WriteLine($"Received message: {textMessage}");

                return true;
            }

            Console.WriteLine("Unexpected message type: " + message.GetType().Name);
            return false;
        }
    }
}
