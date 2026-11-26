---
name: review
description: Review code in this learning project, focused on technical and infrastructure learning (race conditions, transactions, locking, idempotency, background jobs, integration, logging), not on business or production concerns. Use when the owner says "review", "review this", "review this and related", or /review.
---

# Review

This is a learning project for getting hands-on with .NET. It is not a business or production application.
The review exists to teach technical skills, so it reports only findings that serve that goal.

## Scope: what to look for

Report technical and infrastructure problems, and the edge cases that build experience:
- Race conditions and concurrency (two requests or two workers touching the same row).
- Transactions, isolation (incl. RCSI), locking, row claiming.
- Idempotency and at-least-once delivery.
- Background jobs: `BackgroundService`, graceful shutdown, a scoped `DbContext` inside a singleton worker.
- Failure paths: the process dies between two steps, a timeout after the other side already received the request, retries.
- Integration: `IHttpClientFactory`, retry/backoff/jitter, the outbox pattern, EF Core with SQL Server.
- Logging and observability of these paths.
- DI lifetimes, options and configuration when they break behavior.

## Out of scope: never report

- Business scenarios the project doesn't have. Only use the flows that exist in the code or `requirements/`
  (e.g. "the marketplace receives an order", "the marketplace sends the same order twice").
  Don't invent cases like contract cancellation, contract renewal, payload corrections, permissions or pricing.
- Production hardening that teaches nothing here: missing validation rules, wrong HTTP status codes,
  max-length checks, response shapes, auth, naming, redundant code, style.
- Business requirement questions. Ask one only if the technical answer depends on it.

If a point doesn't fit the scope, leave it out. Don't list it as low priority.

## Severity

Only two levels. Rank findings Critical first.
- **Critical:** blocks the application entirely (won't start, every request fails, the worker never runs or crashes for good, data can't be written).
- **High:** has a big side effect or may break the application (duplicate delivery or processing, lost data, two workers picking the same row, a deadlock, a stuck queue, an unhandled crash in a failure path).

Anything below High isn't reported.

## How to write each finding

- Quote `file:line`.
- Describe it as a concrete failure scenario: what happens, in which order, and what the result is.
- Name the topic or doc to read (Microsoft Learn, SQL Server docs, relevant RFCs). Don't give the fix as code.
- Check technical claims against the official docs and cite them. If something can't be verified, say "I don't have enough evidence" or tell the owner how to check it.
- Be critical, and don't soften weaknesses.

## Output

1. One line with the verdict.
2. **Critical**, then **High**. Omit an empty level, or say "none".
3. Optionally, a short list of what is already correct that's worth knowing, kept technical.
4. If the code under review has no tests yet, ask how it will be tested.

Don't edit code, and don't commit.
