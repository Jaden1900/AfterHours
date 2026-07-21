using System;
using System.Collections;
using UnityEngine;

namespace AfterHours.Gameplay.Ending
{
    /// <summary>
    /// Controls the presentation and pause state of the vertical-slice ending screen.
    /// </summary>
    public sealed class EndingScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject _endingScreenRoot;
        [SerializeField] private CanvasGroup _endingCanvasGroup;
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private bool _pauseGameWhenShown = true;

        private Coroutine _fadeCoroutine;
        private bool _isEndingShown;
        private bool _hasStoredCursorState;
        private CursorLockMode _cursorLockStateBeforeEnding;
        private bool _cursorVisibleBeforeEnding;

        public event Action EndingShown;

        public bool IsEndingShown => _isEndingShown;

        private void Awake()
        {
            HideScreenVisual();
        }

        /// <summary>
        /// Shows the ending screen once, fades it in when a CanvasGroup is assigned, and pauses after the fade.
        /// </summary>
        public void ShowEnding()
        {
            if (_isEndingShown || _endingScreenRoot == null)
            {
                return;
            }

            _isEndingShown = true;
            StoreCursorState();
            _endingScreenRoot.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            EndingShown?.Invoke();

            if (_endingCanvasGroup == null)
            {
                PauseGameIfConfigured();
                return;
            }

            _endingCanvasGroup.alpha = 0f;
            if (_fadeDuration <= 0f)
            {
                _endingCanvasGroup.alpha = 1f;
                PauseGameIfConfigured();
                return;
            }

            _fadeCoroutine = StartCoroutine(FadeIn());
        }

        /// <summary>
        /// Hides the ending screen and restores the time scale and cursor state captured before it was shown.
        /// </summary>
        public void HideEnding()
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
                _fadeCoroutine = null;
            }

            HideScreenVisual();
            _isEndingShown = false;
            Time.timeScale = 1f;
            RestoreCursorState();
        }

        private IEnumerator FadeIn()
        {
            float elapsed = 0f;

            while (elapsed < _fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                _endingCanvasGroup.alpha = Mathf.Clamp01(elapsed / _fadeDuration);
                yield return null;
            }

            _endingCanvasGroup.alpha = 1f;
            _fadeCoroutine = null;
            PauseGameIfConfigured();
        }

        private void HideScreenVisual()
        {
            if (_endingCanvasGroup != null)
            {
                _endingCanvasGroup.alpha = 0f;
            }

            if (_endingScreenRoot != null)
            {
                _endingScreenRoot.SetActive(false);
            }
        }

        private void PauseGameIfConfigured()
        {
            if (_pauseGameWhenShown)
            {
                Time.timeScale = 0f;
            }
        }

        private void StoreCursorState()
        {
            if (_hasStoredCursorState)
            {
                return;
            }

            _cursorLockStateBeforeEnding = Cursor.lockState;
            _cursorVisibleBeforeEnding = Cursor.visible;
            _hasStoredCursorState = true;
        }

        private void RestoreCursorState()
        {
            if (!_hasStoredCursorState)
            {
                return;
            }

            Cursor.lockState = _cursorLockStateBeforeEnding;
            Cursor.visible = _cursorVisibleBeforeEnding;
            _hasStoredCursorState = false;
        }
    }
}
