# System Documentation

Each topic: **Problem** → **What we did** → **Cases**. Open problems are listed too.

## Index

| # | Topic | Status |
|---|---|---|
| 1 | Database failure | Solved |
| 2 | Race condition: lost update on order status | Solved |
| 3 | Commit retried by `EnableRetryOnFailure` | Open (`points-to-be-handled.md` #1) |
| 4 | Crash after the status save | Open (shipping not built) |
| 5 | Duplicate send to the shipper | Open (shipping not built) |
| 6 | Order enqueued twice | Open (`points-to-be-handled.md` #2) |

---

## 1. Database failure — Solved

**Problem:** the database is down or returns a transient error while a request is handled.

**What we did:**
1. EF Core retries transient errors: `EnableRetryOnFailure` in `src/Persistence/DependencyInjection.cs` (max 5 retries, max 10 s delay).
2. When retries run out, EF Core throws `RetryLimitExceededException`.
3. `src/Api/ErrorHandling/GlobalExceptionHandler.cs` maps it to **503 Service Unavailable** (ProblemDetails: "The database is unavailable. Try again later.").
4. The producer retries the request later.

**Cases:**
- Transient SQL Server error (not only connection loss) → retried by EF Core → 503 if it keeps failing.
- Any other exception → `HandleInternalServerError` → **500**.
- `RetryLimitExceededException` comes only from EF Core's execution strategy, not from Polly or other retry tools.

---

## 2. Race condition: lost update on order status — Solved

**Problem:** Job A and Job B load the same order in `ReadyToShipping`, both pass the status check in memory, and both save `AttemptingToShip` → the order ships twice.

**What we did:**
1. `MarketplaceOrder.RowVersion` mapped with `IsRowVersion` (EF Core optimistic concurrency).
2. EF Core sends `UPDATE ... WHERE RowVersion = X`; only the first save matches the row.
3. The loser gets `DbUpdateConcurrencyException`. `MarketplaceOrderRepository.Update` reads the current row (`GetDatabaseValuesAsync`), logs what changed, and throws `OrderConcurrencyException` with an `OrderUpdateResult`.
4. `ShipOrder` acts on the result (see cases).

**Cases:**
- **Status changed in DB** → `OrderNotReadyToShip` → another job took the order → log warning, stop.
- **Another column changed, not Status** → `OrderUpdateConflict` → rethrow → Hangfire retries the job with a fresh `DbContext`.
- **Status already not `ReadyToShipping` at load** → `AttemptToShipOrder()` throws `InvalidOperationException` → log warning, stop.


---

## 3. Commit retried by `EnableRetryOnFailure` — Open

**Problem:** A's save commits, but the connection drops before the "OK" reaches the app. EF retries the save → 0 rows match → A's own write looks like another job's → classified `OrderNotReadyToShip`.

**Result:** nobody ships; the order is stuck in `AttemptingToShip`.


---

## 4. Crash after the status save — Open

**Problem:** A saves `AttemptingToShip`, then the process dies before calling the shipper. The check only picks `ReadyToShipping` orders.

**Result:** the order is never picked up again.


---

## 5. Duplicate send to the shipper — Open

**Problem:** A calls the shipper, the shipper receives it, but the response times out → A retries → the shipper gets the same order twice.

**Result:** at-least-once delivery means the receiver can see duplicates.


---

## 6. Order enqueued twice — Open

**Problem:** every `CheckReadyToShippingOrders` run enqueues a `ShipOrder` job for each `ReadyToShipping` order, even if one is already pending.

**Result:** extra jobs for the same order; topic 2 stops all but one from saving.

---

## Reference: Shipping flow

1. `CheckReadyToShippingOrders` (`src/Application/Order/ShipOrderWorker.cs`) enqueues one `ShipOrder(orderId)` Hangfire job per `ReadyToShipping` order.
2. `ShipOrder` loads the order with events (`GetOrderWithEvents`) and calls `AttemptToShipOrder()` → status `AttemptingToShip` + `ShippingAttemptStarted` event.
3. `Update` (`src/Persistence/Order/MarketplaceOrderRepository.cs`) saves; conflicts handled as in topic 2.
4. Shipping itself: not built yet (`// TODO: Ship the order`).

**Used:** Hangfire with SQL Server storage (`src/Infrastructure/BackgroundJobs/BackgroundJobService.cs`), EF Core `RowVersion`, `DbUpdateConcurrencyException` + `GetDatabaseValuesAsync`, C# union `OrderUpdateResult` (`src/Application/Order/OrderUpdateResult.cs`).

---

## Change Log

| Date & Time | What Changed | Brief |
|---|---|---|
| 2026-11-18 20:44 | Change Log | Added this change log section. |
| 2026-11-19 11:00 | Shipping an order | Added the ShipOrder flow, concurrency conflict classification, what we used and known gaps. |
| 2026-11-19 11:04 | Shipping an order | Added race conditions and failure cases (solved and not solved) with examples. |
| 2026-11-19 | Whole file | Restructured into topics: Problem → What we did → Cases, with an index. |
