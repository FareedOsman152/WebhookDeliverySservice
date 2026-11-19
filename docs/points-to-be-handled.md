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

## 2. Every check re-enqueues orders that already have a pending job

**Where:** `Application/Order/ShipOrderWorker.cs` (`CheckReadyToShippingOrders`).

**Scenario:**
1. The check runs every N seconds and enqueues a `ShipOrder` job for every order still in `ReadyToShipping`.
2. The Hangfire workers take longer than N to drain the queue.
3. The next tick finds the same orders still `ReadyToShipping` and enqueues them again.

**Result:** no double shipping (the status check and `RowVersion` let only one job win), but the queue grows every tick, and each extra job costs a read and a failed update or a "can't be shipped" warning.

**Read:** the claim-then-enqueue pattern; Hangfire `DisableConcurrentExecution` and its limits; SQL Server `UPDATE ... OUTPUT` with the `READPAST` / `UPDLOCK` table hints.
