# AfterHours Development Roadmap

## 0. Project foundation — complete

- Establish the canonical asset, script, test, documentation, and scene layout.
- Keep the project free of custom gameplay code and premature assembly definitions.

## 1. Player movement and camera — complete

Build and validate the focused third-person movement foundation in `90_Sandbox`: camera-relative movement, mouse look, grounded jumping, gravity, and cursor handling. Keep it limited to placeholder primitives and focused player components.

## 2. Reusable interaction system — active milestone

Provide a scene-agnostic raycast interaction foundation with a prompt contract, player targeting, UI presentation, and sandbox validation. Keep production doors, evidence, NPCs, quests, inventory, saving, and AI out of scope.

## 3. Investigation vertical slice

Build one tightly scoped investigation loop using interaction, a small amount of authored content, evidence, journal presentation, and a test scene. Keep the playable district out of this milestone.

## 4. NPC and quest integration

Add a minimal NPC conversation and scheduling slice, then connect it to a single quest outcome. Validate it in the dedicated test scenes.

## 5. AI integration

Implement a narrow, validated AI service boundary with DTOs, prompts, and deterministic fallback behavior. Never expose provider-specific types outside `Scripts/AI`.

## 6. Persistence and polish

Add versioned save data, recovery tests, accessibility review, performance profiling, and production content workflows.

## Milestone 3: NPC and Basic Dialogue Framework — complete

### Scripts

- `NPCData` is an authored ScriptableObject containing an NPC ID, display name, optional interaction prompt, and one initial dialogue line.
- `NPCInteractable` implements the existing `IInteractable` contract and delegates dialogue opening to an explicitly assigned `DialogueController`.
- `DialogueController` owns the active speaker/text state and publishes open/close events without depending on a UI implementation.
- `DialoguePresenter` renders controller events through assigned TextMesh Pro text and a close button.
- `DialogueCancelInputHandler` consumes the existing UI Cancel action from `PlayerInputReader` while dialogue is open.

### Manual Unity setup

1. Create an NPC data asset with **Assets > Create > AfterHours > NPC Data**. Set **NPC Id**, **Display Name**, and **Initial Dialogue Text**. Leave **Interaction Prompt** empty to use `Talk to <Display Name>`.
2. Add `DialogueController` to a dedicated gameplay GameObject.
3. Create a Canvas and dialogue-panel child manually. Add TMP text fields for speaker name and dialogue body, plus a UI Button for close.
4. Add `DialoguePresenter` to an active UI GameObject (not the panel if the panel is disabled). Assign the controller, panel root, TMP fields, and close button.
5. Add `DialogueCancelInputHandler` to a gameplay GameObject. Assign the same dialogue controller and the player's `PlayerInputReader`. The existing UI `Cancel` binding (Escape / the appropriate device cancel control) closes active dialogue.
6. Add `NPCInteractable` and a Collider to each NPC GameObject. Assign the NPC data asset and the same dialogue controller. Ensure its layer is included by `PlayerInteractionController`.

### Current limitations and future extensions

- Each NPC currently opens one authored dialogue line only; there is no dialogue progression or branching.
- There is no AI dialogue integration, NPC animation, quest linkage, or scripted conversation sequencing yet.
- Player movement and camera input remain active during dialogue. A future refinement can add a focused input-lock policy without changing dialogue state ownership.
- Prompt text does not yet show a dynamic input-binding label.
- The controller state and events are deliberately UI-independent so later dialogue graphs, branching, quests, and AI conversation providers can build on the same boundary.

## Milestone 4: Investigation State & Evidence System — complete

### Architecture

- `InvestigationState` is an explicitly assigned runtime component that owns collected evidence IDs and story flags. It uses duplicate-safe sets and publishes `EvidenceCollected` and `FlagChanged` events; it has no singleton, object lookup, persistence, or UI dependency.
- `EvidenceData` is an authored ScriptableObject with an evidence ID, display name, description, and optional icon.
- `EvidenceInteractable` implements `IInteractable`, receives an `EvidenceData` asset and an `InvestigationState` through serialized references, and becomes unavailable after successful collection.
- `DialogueConditionEvaluator` is a small reusable adapter over `InvestigationState` for future dialogue branches. It evaluates evidence and flags without knowing NPC identities or authored IDs.
- `EvidenceNotificationPresenter` listens to the state event and resolves IDs through an explicitly assigned evidence catalog before displaying the evidence-found panel. Presentation does not own progress or alter collection rules.

### Manual Unity setup

1. Add `InvestigationState` to one active runtime GameObject, such as a gameplay systems object.
2. Create evidence assets through **Assets > Create > AfterHours > Investigation > Evidence Data**. Give every asset a unique, non-empty **Evidence Id**.
3. Add a Collider and `EvidenceInteractable` to each physical evidence GameObject. Assign its `EvidenceData` and the shared `InvestigationState`; ensure the collider layer is included by `PlayerInteractionController`.
4. For the optional notification, create the panel and three TMP text fields manually. Add `EvidenceNotificationPresenter` to an active UI GameObject, then assign the shared state, panel, heading, display-name, and description fields. Populate **Evidence Catalog** with each evidence asset that can notify.
5. When choosing dialogue content at an NPC, construct `DialogueConditionEvaluator` with that same `InvestigationState` and call `HasEvidence("your_evidence_id")` or `HasFlag("your_flag")` before opening the appropriate existing dialogue pages.

### Current limitations and future extensions

- There is intentionally no inventory, objective, journal, quest, or save UI in this milestone.
- The evidence catalog is only a presentation lookup; the runtime state remains the authority for progress.
- Future objectives, inventory, persistence, AI conversations, and quests can subscribe to the same state events or query the existing public APIs without changing interaction or dialogue ownership.

## Milestone 4.5: Simple Door Interaction — complete

### Manual Unity setup

1. Add `DoorInteractable` and a Collider to the door GameObject. Keep the collider on a layer included by `PlayerInteractionController` so the player can target it.
2. Set **Prompt** to the desired closed-door instruction. Assign **Closed Door Visual** to the object that represents the closed door.
3. Optionally assign **Open Door Visual** for a separate open-door object, and **Blocking Collider** for the collider that should stop blocking the doorway once opened.
4. Enable **Starts Open** when the door should load already open. Otherwise, interacting once hides the closed visual, shows the optional open visual, disables the optional blocker, and makes the door unavailable for further interaction.
