using CloudNative.CloudEvents;
using Google.Cloud.Functions.Framework;
using Google.Events.Protobuf.Cloud.PubSub.V1;
using Newtonsoft.Json;

namespace ScoreReader
{
    public class ScoreReaderFunction : ICloudEventFunction<MessagePublishedData>
    {
        public const string EVENT_TYPE_KEY = "pigeon.eventType";
        public const string EVENT_TYPE_VALUE = "walli.TransactionAnomalyScoreCalculatedEventV1";

        public async Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken)
        {
            var dataContent = data.Message?.TextData;
            data.Message.Attributes.TryGetValue(EVENT_TYPE_KEY, out string eventType);

            if (eventType != EVENT_TYPE_VALUE) 
            {
                Console.WriteLine($"Missing or incorrect value for attribute {EVENT_TYPE_KEY}");
                return;
            }

            var transaction = JsonConvert.DeserializeObject<TransactionScore>(dataContent);

            Console.WriteLine($"Read object: {JsonConvert.SerializeObject(transaction)}");

            var firestore = new FirestoreService();

            await firestore.MarkTransactionAsAnomalyAsync(transaction);         
        }

    }
}
