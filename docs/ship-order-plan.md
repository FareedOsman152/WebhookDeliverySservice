# Ship Order Plan

Only what was agreed. Take it phase by phase.

## Scope and assumptions
- One instance of the app (single-instance assumption).
- Each shipper has one active contract.
- Adding/removing shippers, contract changes and expiry are out of scope for now.
- Designed for fewer than 200 shippers.
- Always use UTC.

---

## Phase 1: Order attempt count
- Add an int attempt count to `MarketplaceOrder`, 0 when the order is created.
- Maximum 10 attempts.
- No `AttemptingToShip` status; the attempt count replaces it.

## Phase 2: Shipper data in memory
- A thread-safe dictionary holding, per shipper: `{ShipperId, endpoint URL, IsDown / readyOn, latest update}`.
- One shipper service class serves all shippers.
- Where the data is loaded from: decided when we design the shipper service.

## Phase 3: Mock shipper
- A mock shipper covering all cases: success, 400, 409, timeout, 5xx then recovery.

## Phase 4: Worker loop
- `ShipOrderWorker` inherits from `BackgroundService` with a `while` loop.
- One class serves all shippers.

## Phase 5: Claim per shipper
- For each shipper, check the down state in memory **before** claiming; if its `readyOn` hasn't come yet, skip it.
- One claim per shipper:
  - Up to 20 orders: `ReadyToShipping`, attempts < 10, oldest `LastModifiedOn` first (FIFO).
  - `UPDATE … OUTPUT`: attempts + 1, set `LastModifiedOn`.
  - Insert an "attempt to shipping" event, setting its dates and the other fields the `SaveChanges` override normally fills in.

## Phase 6: Send
- One request per shipper with its list of orders.
- HTTP timeout 2 s (agreed with the shippers).
- All shippers in parallel with `Task.WhenAll`; wait for all, then one `SaveChanges`.

## Phase 7: Results
- **Success (2xx):** update the order.
- **409:** success.
- **400:** order is Refused (final). Refused orders are handled by another worker later.
- **Timeout / 5xx:** failed; the attempt is already counted and the order goes to the end of the queue. Mark the shipper down with `readyOn = UtcNow + n × 10s` (10, 20, 30, …). When `readyOn` arrives, send the whole chunk in the normal way to test the shipper.
- **401:** not handled for now.

---

## Later
- Orders that reach 10 attempts.
- Telling a crash attempt apart from a shipper failure (TODO).
- NOTE comments in `ShipOrderWorker.cs`:
  - Backoff vs. outage length (n × 10s with the whole chunk; reset n on success; n resets on restart).
  - Single-instance assumption (no in-flight marker on the order).
  - `Task.WhenAll` exceptions stopping the host; only the first exception surfaces.

## Open questions
- One request carries a list of orders: is the result (success / 400 / 409) per order, or one for the whole request?
