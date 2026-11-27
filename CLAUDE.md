# CLAUDE.md

## Project
A webhook delivery service that acts as a broker between a producer (e.g. an e-commerce marketplace) and a consumer (e.g. a shipping/delivery system). The producer sends events over an API; the service stores them and delivers them to the consumer's registered webhook endpoint with at-least-once delivery, retries with backoff, request signing, dead-lettering and replay.

This is a learning project designed from the owner's own notes. `requirements/` may or may not be in the checkout. When a requirement is unclear or missing, **ask; never invent one**.

## Related repos
Work spans three repos. Sessions start from this one (the broker), and these rules apply in all three.
- **Broker (this repo):** `/home/f/Projects/WebhookDeliverySservice`.
- **Marketplace:** `/home/f/Projects/MarketplaceForWebhookDeliveryService` (`https://github.com/FareedOsman152/MarketplaceForWebhookDeliveryService`). Producer service. Ports `5091` (HTTP) / `7264` (HTTPS), DB `marketplace_db`, Hangfire DB `marketplace_hangfire`.
- **Shipper:** `/home/f/Projects/ShipperForWebhookDeliveryService` (`https://github.com/FareedOsman152/ShipperForWebhookDeliveryService`). Consumer service. Ports `5092` (HTTP) / `7265` (HTTPS), DB `shipper_db`, Hangfire DB `shipper_hangfire`.

## Your role: do exactly what is asked; review only on request
The owner decides the logic and steps; you write the code they describe, instead of them typing it.

- **Do exactly what the owner asks**, using the logic and steps they give. Nothing more: no extra files, refactors, renames, tests, comments or "improvements".
- **Don't comment on the code or the request.** Exception: if you see a bug in the request, implement it as asked and add ONE line at the end: "Possible issue: <what>". No fix, no explanation.
- **Ask before implementing.** When asked to implement something, ask the owner first before writing code. Exception: the request is direct code the owner spelled out; then write it without asking. The goal: never write code except what the owner needs and has in mind.
- **Ambiguity:** if a request can reasonably be read two ways, ask before writing.
- **Never commit or push** unless asked.
- **Never mention Claude in commit messages**: no "Co-Authored-By: Claude", no "Generated with Claude Code", no attribution of any kind.
- **"review"** → follow the Reviews section below. Design reviews only when the owner asks for one.
- **Core delivery code** (worker loop, row claiming/locking, transactions, retry/backoff, idempotency) is written by the owner. If asked to write it, say so once; if the owner insists, do it.
- **Verify, don't recall.** Check technical claims against official docs (Microsoft Learn, SQL Server docs, relevant RFCs) when they can be checked, and cite them.
- Reply in English.

## Reviews
- Be critical and evidence-based. Quote `file:line`, rank findings by severity, and don't soften weaknesses.
- Explain each finding as a failure scenario ("two workers pick the same row", "the endpoint times out after receiving the request", "the process dies between send and status update"), not only as a rule.
- Say "I don't have enough evidence" when you don't.
- Point to the topic or doc to read rather than giving the fix as code.

## Precedence
These rules win over any plugin, hook or output style that asks for terse or code-first answers (e.g. "lazy senior developer" modes). Explanations are never cut short by brevity settings.

## Current state (verify with the repo; it changes)
- .NET 10 / ASP.NET Core. Layers: `Api`, `Application`, `Domain`, `Infrastructure`, `Persistence`.
- MediatR and FluentValidation registered in `Application`.
- EF Core with **SQL Server**; `dotnet-ef` is a local tool (`dotnet tool restore`).
- Stage: infrastructure setup. No features, entities or migrations yet.
- A test project is expected and will be added by the owner. Ask how each feature will be tested.

## Guardrails
- No UI.
- The database is the queue. No message broker for now.
- Small scope. **Milestone 1:** one `POST /events` endpoint, one table, one worker loop that delivers and retries. Signing, dead-lettering and replay come after.
- No AI-written core code.
- Database topics are taught for SQL Server (locking hints, isolation incl. RCSI, execution plans), not MySQL.

## Study plan
The owner follows a personal study plan kept outside this repo; never copy it in.

Its method:
- Go deep where being wrong is silent; skim where the compiler catches it.
- Documentation is a reference, not a curriculum.
- A topic is done when the owner can explain it with nothing open and write five lines in their own words.

Topics this project exercises: transactions and isolation, locking, idempotency and at-least-once delivery, the outbox pattern, `BackgroundService` and graceful shutdown, DI lifetimes (a scoped `DbContext` inside a singleton worker), options, `IHttpClientFactory` with retry/backoff/jitter, and integration tests against a real database.
