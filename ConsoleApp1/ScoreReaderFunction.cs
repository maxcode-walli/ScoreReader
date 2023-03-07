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
        public async Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken)
        {
            var dataContent = data.Message?.TextData;
            var transaction = JsonConvert.DeserializeObject<TransactionScore>(dataContent);

            Console.WriteLine($"Read object: {JsonConvert.SerializeObject(transaction)}");

            var label = GetLabel(transaction.Score);

            if (label.Equals("High risk")) 
            {
                // set a field Anomaly = true in Firestore
                var firestore = new FirestoreService();
                firestore.MarkTransactionAsAnomaly(transaction.UserID, transaction.ExternalAccountID, transaction.TransactionID)
            }
            
        }

        private string GetLabel(float score)
        {
            if (score >= 800)
            {
                return "High risk";
            }
            if (score >= 400 && score < 800)
            {
                return "Low risk";
            }

            return "Legit";
        }
    }
}
