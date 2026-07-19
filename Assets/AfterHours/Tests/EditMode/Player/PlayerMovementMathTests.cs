using AfterHours.Gameplay.Player;
using NUnit.Framework;
using UnityEngine;

namespace AfterHours.Tests.EditMode.Player
{
    public sealed class PlayerMovementMathTests
    {
        [Test]
        public void ClampMoveInput_LimitsDiagonalMagnitudeToOne()
        {
            Vector2 clampedInput = PlayerMovementMath.ClampMoveInput(new Vector2(1f, 1f));

            Assert.That(clampedInput.magnitude, Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void CalculateJumpVelocity_UsesGravityAndJumpHeight()
        {
            float velocity = PlayerMovementMath.CalculateJumpVelocity(-20f, 1.5f);

            Assert.That(velocity, Is.EqualTo(Mathf.Sqrt(60f)).Within(0.0001f));
        }

        [Test]
        public void ClampPitch_ConstrainsPitchToConfiguredRange()
        {
            float pitch = ThirdPersonCameraController.ClampPitch(80f, -30f, 60f);

            Assert.That(pitch, Is.EqualTo(60f));
        }
    }
}
