---
name: update-system-doc
description: Add a point to docs/system.md and log the edit in its Change Log. Use ONLY when the owner explicitly says "add this point to system doc" (or /update-system-doc). Never use it on your own initiative.
---

# Update System Documentation

Target file: `docs/system.md`.

## Steps

1. Read `docs/system.md`.
2. Add the point the owner gave:
   - If it belongs to an existing topic, add it there. Otherwise create a new numbered topic `## <N>. <Topic> — Solved|Open` **above** `## Reference: Shipping flow` (and above `## Change Log`), separated by `---`.
   - Topic format, all brief:
     - `**Problem:**` one or two sentences, with a concrete example (e.g. "Job A and Job B load the same order ...").
     - Solved topics: `**What we did:**` numbered steps 1, 2, 3 (file paths in backticks), then `**Cases:**` bullets `case → result → action`.
     - Open topics: `**Result:**` what goes wrong, in one sentence.
     - No `**Read:**` section, no links.
   - Add or update the topic's row in the `## Index` table (number, topic, status). When an open topic gets solved, change its status in both the heading and the index.
   - Write only what the owner said. Do not invent requirements or details; if the point is unclear, ask.
3. Append ONE row to the end of the `## Change Log` table:
   - `Date & Time`: current local time, format `YYYY-MM-DD HH:MM` (get it with `date '+%Y-%m-%d %H:%M'`).
   - `What Changed`: the section name added or edited.
   - `Brief`: one short sentence.
   - One row per change; never edit or delete earlier rows.
4. `## Change Log` always stays the last section of the file.
5. Do not commit.
