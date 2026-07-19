# AfterHours Coding Standards

## C# and Unity

- Use four spaces for indentation and braces on their own lines.
- Use `PascalCase` for public types, methods, and properties; use `camelCase` for local variables and parameters.
- Name serialized private fields with a leading underscore (for example, `_questId`).
- Keep MonoBehaviours thin: put domain rules in plain C# classes where practical.
- Do not use `try`/`catch` around imports.
- Avoid global singletons unless an explicit lifecycle and test strategy require one.

## Project boundaries

- Put shared contracts and utilities in `Scripts/Core`; keep them independent of feature folders.
- Keep provider-specific AI implementation details inside `Scripts/AI`.
- Put editor-only code in `Scripts/Editor`; do not reference `UnityEditor` from runtime code.
- Prefer explicit IDs and data assets over scene-name strings and hidden object lookups.
- Author and save Unity scenes through the Unity Editor under normal circumstances. Codex must avoid directly constructing scene YAML unless it is explicitly requested and validated.

## Quality

- Add or update Unity Test Framework tests when implementing behavior.
- Keep public APIs small and document non-obvious decisions.
- Check the Unity Console for errors and missing references before merging changes.
