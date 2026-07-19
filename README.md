# AfterHours

AfterHours is a production-oriented, AI-powered investigation game built with Unity 6 and URP.

## Player movement sandbox

Open `Assets/AfterHours/Scenes/90_Sandbox.unity` in Unity `6000.5.4f1` (Unity 6), then enter Play Mode.

| Control | Action |
| --- | --- |
| WASD | Move relative to the camera |
| Mouse | Orbit the third-person camera |
| Space | Jump while grounded |
| E | Interact with the targeted test object |
| Escape | Unlock the cursor |
| Click the Game view | Relock the cursor |

### Manual test checklist

1. Open the sandbox scene and verify that the capsule stands on the ground near the three cube obstacles.
2. Enter Play Mode. Move with WASD while facing different camera directions; movement should remain camera-relative and diagonal movement should not be faster.
3. Verify the capsule smoothly turns toward its movement direction.
4. Move the mouse and verify the camera follows the player, orbits around them, and cannot rotate beyond its vertical limits.
5. Press Space while grounded, then verify the capsule lands on the ground without passing through it.
6. Press Escape to release the cursor; click in the Game view to lock it again.
7. Follow the interaction setup below, then aim at the test cube and press E. Confirm the Unity Console reports the interaction.
8. Confirm the Unity Console has no errors or missing-script warnings.

### Interaction setup

The interaction system is intentionally scene-agnostic. Wire it in the Unity Editor without modifying source or creating a prefab:

1. Open `Assets/AfterHours/Scenes/90_Sandbox.unity`. Select the player GameObject and add `PlayerInteractionController`.
2. Assign its **Input Reader** field to the player's existing `PlayerInputReader`. Assign **View Transform** to the third-person camera transform. Leave **Interaction Distance** at `3` or set the desired range.
3. Create a layer named `Interactable` through **Edit > Project Settings > Tags and Layers**, assign that layer to a new Cube, then set the controller's **Interaction Layers** mask to `Interactable`. (Use `Everything` temporarily if the layer is not needed.)
4. Add `TestInteractable` to the cube and configure its **Prompt** and **Can Interact** fields. Keep the cube's Box Collider enabled.
5. Create a Canvas through **GameObject > UI > Canvas**, then add **UI > Text - TextMeshPro** as a child. Import TMP Essentials if Unity prompts you to do so.
6. Add `InteractionPromptPresenter` to the Canvas or another UI GameObject. Assign the player `PlayerInteractionController` and the TMP text component. Disable the text GameObject initially if desired; the presenter controls its visibility at runtime.
7. Enter Play Mode, aim the camera at the cube, and press E. The prompt is visible only for a valid target and the Console logs `Test interactable '<cube name>' was interacted with by '<player name>'.`

### Current limitations

This sandbox deliberately uses unanimated primitive geometry and has no camera collision handling, production interaction content, combat, NPC, quest, inventory, journal, AI, or save systems. The reusable interaction system currently supports raycast targeting, prompt presentation, and a sandbox-only test component; it does not yet provide production doors, evidence, NPC, quest, or inventory behaviors.

## Project layout

- `Assets/AfterHours`: game-owned art, audio, content, prefabs, scenes, settings, tests, and scripts.
- `Docs/Architecture.md`: architecture and dependency boundaries.
- `Docs/DevelopmentRoadmap.md`: staged development plan.
- `Docs/CodingStandards.md`: C# and Unity conventions.
