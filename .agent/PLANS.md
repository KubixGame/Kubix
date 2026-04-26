# ExecPlan Rules For Kubix

This repository keeps its primary living execution plan in `PLAN.md` at the repository root.

Use these rules when creating or updating any ExecPlan:

- Treat the reader as someone who only has the current repository state and the plan file.
- Keep the plan self-contained. Do not rely on chat history or undocumented decisions.
- Every plan must explain the user-visible outcome first, then the implementation path, then the validation steps.
- Every plan must contain and maintain these sections:
  - `Progress`
  - `Surprises & Discoveries`
  - `Decision Log`
  - `Outcomes & Retrospective`
- Keep plans living and current. When work starts, changes direction, or pauses, update the plan immediately.
- Prefer plain language over jargon. If a technical term is necessary, define it in the plan.
- Be specific about file paths, commands, expected outputs, and how to verify the result.
- For major features and architectural changes, do not start implementation before updating the relevant ExecPlan.

`PLAN.md` is the default project-level plan. Create additional ExecPlan files only when a feature becomes large enough that it needs its own dedicated living document.
