# DESIGN.md — shae-csharp-desktop-task-manager

SHAE Field Manual Design System applied to the C# Task Manager
(console / text UI).

## Color palette (conceptual, mapped to terminal)

| Token | Value | Use |
|-------|-------|-----|
| Warm paper | `#F2EEE5` | Terminal page/ground (default background) |
| Ink | `#191712` | Primary text |
| Oxide signal | `#B3341B` | App header, command prompts, status highlights |

One oxide-red signal per view: the application header (`SHAE Task Manager`).
No gradients, glows, glass cards, shadows, or pills — appropriate for a text
UI, which is inherently flat and ink-on-paper.

## Layout & geometry

- `0px` radius; flat physical presentation (text UI has no chrome).
- Hairline rules drawn with dashes (`========================================`).
- Border-defined "cards": task table framed by dash rule header/footer.
- 4px spacing base expressed as blank-line separation between sections.
- 1280px max container concept → table kept within a readable measure
  (62–68ch for prose; aligned data columns).
- 44px touch targets → in a console app this maps to full-line commands
  that are easy to target with the keyboard.
- No horizontal overflow; column widths are fixed and truncated with ellipsis.

## Typography

- Body/data: monospace (JetBrains Mono concept) — aligned columns for
  ID / TITLE / DUE / PRIORITY / STATUS.
- Short labels uppercase: `ID`, `TITLE`, `DUE`, `PRIORITY`, `STATUS`,
  `[todo]`, `[done]`.
- Header uses display emphasis (uppercase, spaced).

## Motion

- Text UI: state changes are instant screen redraws; conceptually
  140–240ms transitions reduced to a single frame. No animation libraries.
- Reduced motion: no animated spinners or progress indicators.

## Accessibility

- WCAG AA contrast equivalent: high-contrast ink-on-paper terminal colors.
- Focus/selection: the prompt (`>`) always indicates where input is expected;
  status is shown as text (`[todo]` / `[done]`), not only by color.
- Complete keyboard paths: every action reachable by typing a command and
  Enter; no mouse required.
- Input validation: empty titles rejected, bad dates rejected with a clear
  message; no unhandled crashes on corrupt storage.

## Files

- `src/TaskManager.cs` — `TaskItem`, `Priority`, `TaskManager` with JSON
  persistence.
- `src/Program.cs` — console UI, formatted task table, command loop.
- `DESIGN.md` — this document.
