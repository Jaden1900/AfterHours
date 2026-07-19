# AfterHours Development Roadmap

## 0. Project foundation — complete

- Establish the canonical asset, script, test, documentation, and scene layout.
- Keep the project free of custom gameplay code and premature assembly definitions.

## 1. Player movement and camera — active milestone

Build and validate the focused third-person movement foundation in `90_Sandbox`: camera-relative movement, mouse look, grounded jumping, gravity, and cursor handling. Keep it limited to placeholder primitives and focused player components.

## 2. Investigation vertical slice

Build one tightly scoped investigation loop using interaction, a small amount of authored content, evidence, journal presentation, and a test scene. Keep the playable district out of this milestone.

## 3. NPC and quest integration

Add a minimal NPC conversation and scheduling slice, then connect it to a single quest outcome. Validate it in the dedicated test scenes.

## 4. AI integration

Implement a narrow, validated AI service boundary with DTOs, prompts, and deterministic fallback behavior. Never expose provider-specific types outside `Scripts/AI`.

## 5. Persistence and polish

Add versioned save data, recovery tests, accessibility review, performance profiling, and production content workflows.
