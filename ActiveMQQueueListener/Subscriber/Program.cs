﻿using System;
using Apache.NMS;

namespace Subscriber
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Waiting for messages");

            //Read all messages off the queue.
            ReadNextMessage();
            
            Console.WriteLine("Successfully read message.");
            Console.WriteLine("Finished");
        }

        private static void ReadNextMessage()
        {
            const string queueName = "TestTextQueue";

            var brokerUri = $"amqp://guest:guest@localhost:5672";

            var connectionFactory = new NMSConnectionFactory(brokerUri);

            using var connection = connectionFactory.CreateConnection();
            connection.Start();

            using var session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
            using var destination = session.GetQueue(queueName);
            using var consumer = session.CreateConsumer(destination);

            consumer.Listener += Consumer_Listener;

            Console.ReadLine();
        }

        private static void Consumer_Listener(IMessage message)
        {
            if (message is ITextMessage textMessage)
                Console.WriteLine($"Received message: {textMessage}");
        }
    }
}
