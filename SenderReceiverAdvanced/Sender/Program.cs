using Amqp;
using Amqp.Framing;
using CommonClass;
using System;
using System.Threading.Tasks;

namespace Sender
{
    class Program
    {
        static async Task Main()
        {
            var connection = await Connection.Factory.CreateAsync(
                new Address("amqp://guest:guest@localhost:5672"));

            Session session = new Session(connection);

            for (int index = 0; index < 1000; ++index)
            {
                var ship = new Ship
                {
                    Name = $"Vessel {index}"
                };

                var message = new Message()
                {
                    BodySection = new AmqpValue<Ship>(ship)
                };
                
                SenderLink sender = new SenderLink(session, "sender-link", "q1");
                sender.Send(message);

                Console.WriteLine(ship.Name);

                await sender.CloseAsync();
                await Task.Delay(1000);
            }
           

            
            await session.CloseAsync();
            await connection.CloseAsync();
            
            Console.ReadLine();
        }
    }
}
