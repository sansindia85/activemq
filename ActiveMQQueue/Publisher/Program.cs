using System;
using Apache.NMS;

namespace Publisher
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                var textMessage = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(textMessage))
                    return;
                SendNewMessage(textMessage);
            }
        }

        private static void SendNewMessage(string textMessage)
        {
            const string queueName = "TestTextQueue";
            Console.WriteLine($"Add messaging to queue : {queueName}");

            var brokerUri = $"amqp://guest:guest@localhost:5672";

            var connectionFactory = new NMSConnectionFactory(brokerUri);

            using var connection = connectionFactory.CreateConnection();
            connection.Start();

            using var session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
            using var destination = session.GetQueue(queueName);
            using var producer = session.CreateProducer(destination);
            producer.DeliveryMode = MsgDeliveryMode.Persistent;

            producer.Send(session.CreateTextMessage(textMessage));
            Console.WriteLine($"Sent {textMessage} messages");
        }
    }
}
