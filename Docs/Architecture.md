# AfterHours Architecture

## Foundation

AfterHours is a Unity 6 Universal Render Pipeline (URP) project written in C#. The project currently contains a directory and scene foundation only; it deliberately contains no gameplay systems or custom assembly definitions.

## Asset layout

All new game-owned assets belong in `Assets/AfterHours`:

- `Art`, `Audio`, `Content`, `Prefabs`, `Scenes`, and `Settings` hold authored Unity assets by type.
- `Scripts` holds runtime and editor C# source.
- `Tests` holds Unity Test Framework tests once behavior exists to test.

`Scripts` is organized by feature and responsibility. `Core` contains shared, dependency-light building blocks; feature folders (such as `NPCs`, `Quests`, and `AI`) own their domain code. Editor-only scripts belong in `Scripts/Editor`.

## Dependencies

Keep dependencies directed toward `Core`. A feature may use `Core`, but `Core` must not depend on gameplay features. Features should communicate through small interfaces, data objects, or events rather than referencing one another's concrete controllers.

No `.asmdef` files are included yet. With no custom production code, Unity's default assembly is the simplest correct option. Introduce assembly definitions only when a concrete module needs faster compile isolation or a well-defined dependency boundary; document references at that time and keep the graph acyclic.

## Scenes

The source-controlled scene set is intentionally minimal:

- `00_Bootstrap`: future application initialization entry point.
- `01_MainMenu`: future menu entry point.
- `90_Sandbox`: isolated general-purpose experimentation.
- `91_AIConversationTest`: isolated AI conversation integration testing.
- `92_QuestTest`: isolated quest-flow testing.

These are placeholder scenes only. They must not become the playable district.
