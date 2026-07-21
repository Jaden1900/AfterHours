# Ending Screen Setup

1. Create the ending UI under an existing Canvas and assign its root GameObject to an `EndingScreenController` component. The controller hides this root on startup.
2. Optionally assign a `CanvasGroup` on the ending UI. It fades from transparent to opaque using unscaled time; leave it unassigned for an immediate display.
3. Set **Fade Duration** (one second by default) and choose whether the game pauses after the screen is visible.
4. On the Prime Suspect's `NPCInteractable`, enable **Show Ending When Dialogue Closes** and assign the scene's `EndingScreenController`. Leave both fields unset on every other NPC.
5. Play the scene and complete the Prime Suspect conversation using the dialogue UI's Close button or final Continue action. The existing `DialogueClosed` event triggers the ending only for that configured NPC.

`HideEnding()` is available for a restart, return-to-menu, or debug button. It restores `Time.timeScale` to `1` and returns the cursor to the state it had before the ending opened.
