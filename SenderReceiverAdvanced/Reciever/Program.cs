using Amqp;
using CommonClass;
using System;
using System.Threading.Tasks;

namespace Reciever
{
    class Program
    {
        static async Task Main()
        {
            var connection = await Connection.Factory.CreateAsync(
                new Address("amqp://guest:guest@localhost:5672"));
                        
            Session session = new Session(connection);
            ReceiverLink receiver = new ReceiverLink(session, "reciever-link", "q1");

            Console.WriteLine("Reciever connected to broker.");
            while (true)
            {
                
                Message message = await receiver.ReceiveAsync();
                var ship = message.GetBody<Ship>();

                Console.WriteLine(ship.Name);
                receiver.Accept(message);
                
                if (ship.Name.Equals("Vessel is 999"))
                    break;
            }

            await receiver.CloseAsync();
            await session.CloseAsync();
            await connection.CloseAsync();

            Console.ReadLine();
        }
    }
}
