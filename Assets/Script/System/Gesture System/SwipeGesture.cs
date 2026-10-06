using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SwipeGesture : MonoBehaviour
{
    public event Action OnSwipeUp;
    public event Action OnSwipeDown;
    public event Action OnSwipeLeft;
    public event Action OnSwipeRight;

    public event Action OnTap;
    public event Action OnHold;

    [SerializeField] private float minSwipeDistance = 50f;
    [SerializeField] private float holdDuration = 0.5f;

    private Vector2 startPosition;

    private float holdTimer;
    private bool isHolding;
    private bool holdTriggered;

    private void Update()
    {
        if (Touchscreen.current == null)
            return;

        var touch = Touchscreen.current.primaryTouch;

        // INIZIO TOUCH
        if (touch.press.wasPressedThisFrame)
        {
            startPosition = touch.position.ReadValue();

            holdTimer = 0f;
            isHolding = true;
            holdTriggered = false;
        }

        // TOUCH MANTENUTO
        if (isHolding && touch.press.isPressed)
        {
            holdTimer += Time.deltaTime;

            if (!holdTriggered && holdTimer >= holdDuration)
            {
                holdTriggered = true;
                OnHold?.Invoke();
            }
        }

        // FINE TOUCH
        if (touch.press.wasReleasedThisFrame)
        {
            Vector2 endPosition = touch.position.ReadValue();

            Vector2 swipe = endPosition - startPosition;

            isHolding = false;
            holdTimer = 0f;

            // Se era un HOLD, non deve diventare anche TAP/SWIPE
            if (holdTriggered)
            {
                return;
            }

            // TAP
            if (swipe.magnitude < minSwipeDistance)
            {
                OnTap?.Invoke();
                return;
            }

            // SWIPE
            DetectSwipe(swipe);
        }
    }

    private void DetectSwipe(Vector2 swipe)
    {
        float angle = Mathf.Atan2(swipe.y, swipe.x) * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360f;

        // ↗ 45° = UP
        if (IsAngleBetween(angle, 45f))
        {
            OnSwipeUp?.Invoke();
        }
        // ↙ 225° = DOWN
        else if (IsAngleBetween(angle, 225f))
        {
            OnSwipeDown?.Invoke();
        }
        // ↖ 135° = LEFT
        else if (IsAngleBetween(angle, 135f))
        {
            OnSwipeLeft?.Invoke();
        }
        // ↘ 315° = RIGHT
        else if (IsAngleBetween(angle, 315f))
        {
            OnSwipeRight?.Invoke();
        }
    }

    private bool IsAngleBetween(float angle, float targetAngle)
    {
        float difference = Mathf.Abs(Mathf.DeltaAngle(angle, targetAngle));

        return difference <= 30f;
    }
}