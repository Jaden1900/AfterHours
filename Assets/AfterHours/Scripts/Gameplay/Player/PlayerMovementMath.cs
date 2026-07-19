using UnityEngine;

namespace AfterHours.Gameplay.Player
{
    public static class PlayerMovementMath
    {
        public static Vector2 ClampMoveInput(Vector2 input)
        {
            return Vector2.ClampMagnitude(input, 1f);
        }

        public static float CalculateJumpVelocity(float gravity, float jumpHeight)
        {
            return Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
