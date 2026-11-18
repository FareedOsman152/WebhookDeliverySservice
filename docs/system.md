# System Documentation

## Handling database failure

When the database is unavailable, the service does not accept the request. It returns **503 Service Unavailable**, and the producer (marketplace) is expected to retry later.

### Flow

1. **EF Core retries transient errors.** `EnableRetryOnFailure` is configured in `src/Persistence/DependencyInjection.cs`:
   - max retry count: 5
   - max delay between retries: 10 seconds
2. **The retries are exhausted.** EF Core throws `RetryLimitExceededException` (`Microsoft.EntityFrameworkCore.Storage`).
3. **The global exception handler maps it to 503.** In `src/Api/ErrorHandling/GlobalExceptionHandler.cs`, `RetryLimitExceededException` is routed to `HandleDatabaseUnavailable`, which writes a ProblemDetails response:
   - Status: `503`
   - Title: `Service Unavailable`
   - Detail: `The database is unavailable. Try again later.`
4. **The producer retries** the same request later.

### Notes

- `RetryLimitExceededException` is thrown only by EF Core's execution strategy, not by other retry mechanisms (e.g. Polly).
- It covers any SQL Server error that EF Core treats as transient, not only connection failures.
- Any other exception goes to `HandleInternalServerError` and returns `500`.

## Change Log

| Date & Time | What Changed | Brief |
|---|---|---|
| 2026-11-18 20:44 | Change Log | Added this change log section. |
