using UnityEngine;
using UnityEngine.InputSystem;

namespace AfterHours.Gameplay.Player
{
    public sealed class CursorController : MonoBehaviour
    {
        private void OnEnable()
        {
            LockCursor();
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                UnlockCursor();
            }
            else if (Cursor.lockState != CursorLockMode.Locked && Mouse.current.leftButton.wasPressedThisFrame)
            {
                LockCursor();
            }
        }

        private static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private static void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
