using CloudNative.CloudEvents;
using Google.Cloud.Functions.Framework;
using Google.Events.Protobuf.Cloud.PubSub.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreReader
{
    public class ScoreReaderFunction : ICloudEventFunction<MessagePublishedData>
    {
        public const string EVENT_TYPE_KEY = "pigeon.eventType";
        public const string EVENT_TYPE_VALUE = "walli.TransactionReceived";

        public async Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken)
        {
            var dataContent = data.Message?.TextData;
            data.Message.Attributes.TryGetValue(EVENT_TYPE_KEY, out string eventType);

            if (eventType != EVENT_TYPE_VALUE) 
                return;

            var transaction = JsonConvert.DeserializeObject<TransactionScore>(dataContent);

            Console.WriteLine($"Read object: {JsonConvert.SerializeObject(transaction)}");

            var firestore = new FirestoreService();

            await firestore.MarkTransactionAsAnomalyAsync(transaction);         
        }

    }
}
