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
        public async Task MarkTransactionAsAnomalyAsync(TransactionScore data)
        {
            if (data.Label != "High risk") 
                return;
            try
            {
                FirestoreDb db = FirestoreDb.Create("impactful-shard-374913");
                var path = $"users/{data.UserID}/accounts";

                var accountsQuery = db.Collection(path).WhereEqualTo("externalAccountId", data.ExternalAccountID);
                var snapshot = await accountsQuery.GetSnapshotAsync();
                var account = snapshot.Documents.SingleOrDefault();

                if (account != null) 
                {
                    var transactionsQuery = db.Collection(path + $"/{account.Id}/transactions")
                        .WhereEqualTo("Uuid", data.TransactionID);
                    snapshot = await transactionsQuery.GetSnapshotAsync();
                    var transaction = snapshot.Documents.SingleOrDefault();

                    if (transaction != null) 
                    {
                        var transactionDocument = db.Document(path + $"/{account.Id}/transactions/{transaction.Id}");
                        await transactionDocument.UpdateAsync("IsAnomaly", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
