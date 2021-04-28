using System;
using Apache.NMS;

namespace SubscriberTwo
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Waiting for messages (Subscriber Two)");

            //Read all messages off the queue.
            while (ReadNextMessage())
            {
                Console.WriteLine("Successfully read message.");
            }

            Console.WriteLine("Finished");
        }

        private static bool ReadNextMessage()
        {
            const string topic = "TestTextTopic";

            var brokerUri = $"activemq:tcp://localhost:61616";

            var connectionFactory = new NMSConnectionFactory(brokerUri);

            using var connection = connectionFactory.CreateConnection("sans-app", "password");
            connection.Start();

            using var session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
            using var destination = session.GetTopic(topic);
            using var consumer = session.CreateConsumer(destination);

            var message = consumer.Receive();
            if (message is ITextMessage)
            {
                var textMessage = message as ITextMessage;
                Console.WriteLine($"Received message: {textMessage.Text}");

                return true;
            }

            Console.WriteLine("Unexpected message type: " + message.GetType().Name);
            return false;
        }
    }
}
