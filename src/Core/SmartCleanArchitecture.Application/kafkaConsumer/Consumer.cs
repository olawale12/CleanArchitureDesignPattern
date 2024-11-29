using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaLibrary;
using KafkaLibrary.Consumer;
using KafkaLibrary.Interface;
using KafkaLibrary.KafkaProducer;

namespace SmartCleanArchitecture.Application.kafkaConsumer
{
    public class Consumer : ConsumerBase
    {

        public Consumer(KafkaConfig configuration, IMessageProducers producers, IMessageAdmin messageAdmin) : base("test1", configuration, messageAdmin)
        {

        }

        public override async Task Invoke()
        {

            await ConsumeAsync<string>("testTopic", (value) =>
            {  // put your action here
                Console.WriteLine(value);


            });



            await base.Invoke();
        }
    }
}
