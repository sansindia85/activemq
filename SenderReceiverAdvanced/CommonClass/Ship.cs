using Amqp.Serialization;

namespace CommonClass
{
    [AmqpContract]
    public class Ship
    {
        [AmqpMember]
        public string Name { get; set; }
    }
}
