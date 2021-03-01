using Amqp;
using System;

namespace Reciever
{
    class Program
    {
        static void Main(string[] args)
        {
            Address address = new Address("amqp://guest:guest@localhost:5672");
            Connection connection = new Connection(address);
            Session session = new Session(connection);
            ReceiverLink receiver = new ReceiverLink(session, "reciever-link", "q1");

            Console.WriteLine("Reciever connected to broker.");
            Message message = receiver.Receive();
            Console.WriteLine("Received " + message.Body);

            receiver.Close();
            session.Close();
            connection.Close();

        }
    }
}
