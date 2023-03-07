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
        public async Task MarkTransactionAsAnomaly(string userId, string externalAccountId, string transactionId)
        {
            FirestoreDb db = FirestoreDb.Create("impactful-shard-374913");

            var doc = (await db.Document($"users/{userId}/accounts/{externalAccountId}/transactions/{transactionId}").GetSnapshotAsync());
            
            db.Document("").UpdateAsync()

            return user;
        }
    }
}
