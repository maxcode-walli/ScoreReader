using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreReader
{
    public class FirestoreService
    {
        public async Task MarkTransactionAsAnomaly(TransactionScore score)
        {
            FirestoreDb db = FirestoreDb.Create("impactful-shard-374913");

            var path = $"users/{score.UserID}/accounts/{score.ExternalAccountID}/transactions/{score.TransactionID}";
            var doc = await db.Document(path).GetSnapshotAsync();
            if (doc != null && score.Label == "High risk") 
            {
                var updates = new Dictionary<string, object>() { { "IsAnomaly", true } };
                await db.Document(path).UpdateAsync(updates);
            }
        }
    }
}
