# Webhook Delivery Service — Sprint Plan (business requirements)

Five sprints. Each adds one business capability with a business test for "done".
Built only from the requirements draft and CLAUDE.md. Anything they don't settle is listed under **Open questions**, not decided.

---

## Sprint 1 — Accept an order and get it to the shipping company

**Goal:** every order the marketplace sends reaches the shipping company, even if the shipping company is temporarily down.

- The marketplace can submit an order event to the service.
- The service confirms to the marketplace that it received the order. This confirms receipt only, not delivery.
- An order submitted twice by mistake is treated as one order.
- The service forwards each received order to the shipping company.
- If the shipping company doesn't accept it, the service tries again later, waiting longer each time.
- The service never loses an order it confirmed, even if the service itself restarts.
- The shipping company may sometimes receive the same order twice, and that's accepted.

**Done when:** you turn the shipping company off, submit orders, and turn it back on. Every order arrives, and none were lost.

---

## Sprint 2 — The shipping company can trust what it receives

**Goal:** the shipping company can prove an order came from this service and wasn't changed on the way.

- Every order sent carries proof of who sent it.
- The shipping company can reject anything without valid proof.

**Done when:** a changed or fake order is detected and rejected by the shipping company.

---

## Sprint 3 — Orders that keep failing are set aside

**Goal:** an order that can't be delivered stops being retried forever and is kept for someone to look at.

- After a set number of failed attempts, the service stops trying.
- The order is set aside along with the reason it failed.
- Someone can see which orders were set aside and why.

**Done when:** an order that always fails ends up set aside with its reason, and the service has stopped retrying it.

---

## Sprint 4 — Resend on request

**Goal:** a set-aside order, or one sent earlier, can be sent again on purpose.

- A specific order can be resent on request.
- A resent order goes through the same delivery rules as a new one.

**Done when:** a set-aside order is resent after the shipping company is fixed, and it arrives.

---

## Sprint 5 — Tell the marketplace the delivery is done

**Goal:** the marketplace learns that the shipping company received its order.

- Once the shipping company has accepted an order, the marketplace is told.
- The same rules apply in this direction: no lost notices, retries, and duplicates are possible.

**Done when:** the marketplace receives a "done" notice for every order the shipping company accepted.

---

## Open questions (answer before Sprint 1)

1. **Sprint 5's position.** The draft names the marketplace notice as part of the main flow, but CLAUDE.md doesn't list it for Milestone 1. Should it come first, last, or after Sprint 1?
2. **What "delivered" means.** Does it mean the shipping company acknowledged receiving the order, or the parcel actually reaching the customer?
3. **How many shipping companies.** Is it one fixed company, or several that each register where to receive orders?
4. **Giving up.** In Sprint 1, before set-aside orders exist, what happens to an order that never succeeds?
5. **Order sequence.** Must orders reach the shipping company in the order they were submitted?
