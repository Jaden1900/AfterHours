# AfterHours

AfterHours is a production-oriented, AI-powered investigation game built with Unity 6 and URP.

## Player movement sandbox

Open `Assets/AfterHours/Scenes/90_Sandbox.unity` in Unity `6000.5.4f1` (Unity 6), then enter Play Mode.

| Control | Action |
| --- | --- |
| WASD | Move relative to the camera |
| Mouse | Orbit the third-person camera |
| Space | Jump while grounded |
| Escape | Unlock the cursor |
| Click the Game view | Relock the cursor |

### Manual test checklist

1. Open the sandbox scene and verify that the capsule stands on the ground near the three cube obstacles.
2. Enter Play Mode. Move with WASD while facing different camera directions; movement should remain camera-relative and diagonal movement should not be faster.
3. Verify the capsule smoothly turns toward its movement direction.
4. Move the mouse and verify the camera follows the player, orbits around them, and cannot rotate beyond its vertical limits.
5. Press Space while grounded, then verify the capsule lands on the ground without passing through it.
6. Press Escape to release the cursor; click in the Game view to lock it again.
7. Confirm the Unity Console has no errors or missing-script warnings.

### Current limitations

This sandbox deliberately uses unanimated primitive geometry and has no camera collision handling, interactions, UI, combat, NPC, quest, inventory, journal, AI, or save systems.

## Project layout

- `Assets/AfterHours`: game-owned art, audio, content, prefabs, scenes, settings, tests, and scripts.
- `Docs/Architecture.md`: architecture and dependency boundaries.
- `Docs/DevelopmentRoadmap.md`: staged development plan.
- `Docs/CodingStandards.md`: C# and Unity conventions.
