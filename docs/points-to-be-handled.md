# Points to be handled

## 1. `EnableRetryOnFailure` can retry a `SaveChanges` that already committed

**Where:** `Persistence/DependencyInjection.cs` (`sql.EnableRetryOnFailure(...)`). It affects every `SaveChanges` in the system, not only shipping.

**Scenario:**
1. A worker calls `SaveChanges`, and the transaction commits in SQL Server.
2. The connection drops before the success reaches the app, so EF sees a transient error.
3. The execution strategy retries the same `SaveChanges`.
4. The retry fails even though the first attempt succeeded:
   - with a rowversion token, the `UPDATE ... WHERE RowVersion = @original` matches 0 rows, so a `DbUpdateConcurrencyException` is thrown and the winner thinks it lost;
   - an insert of a row with a client-generated key (such as an `OrderEvent`) hits a duplicate-key error.

**Result:** the operation succeeded in the database, but the app reports it as failed.

**Read:** [Connection resiliency: Transaction commit failure and the idempotency issue](https://learn.microsoft.com/ef/core/miscellaneous/connection-resiliency#transaction-commit-failure-and-the-idempotency-issue)
