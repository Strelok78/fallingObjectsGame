using UnityEngine;
using UnityEngine.Events;

public class AnimationController : MonoBehaviour
{
    private Animator _animator;

    // UnityActions for controlling animations
    public UnityAction<float> OnSetXVelocity;
    public UnityAction<bool> OnFlipSprite;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        // Subscribing UnityActions to their respective methods
        OnSetXVelocity += SetXVelocity;
        OnFlipSprite += FlipSprite;
    }

    private void OnDestroy()
    {
        // Unsubscribing UnityActions to avoid memory leaks
        OnSetXVelocity -= SetXVelocity;
        OnFlipSprite -= FlipSprite;
    }

    private void SetXVelocity(float xVelocity)
    {
        if (xVelocity >= 0)
        {
            _animator.SetFloat("xVelocity", xVelocity / 2);
            AnimateRunning();
        }
        else
        {
            AnimateIdle();
        }
    }

    private void FlipSprite(bool isFacingRight)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = isFacingRight ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    private void AnimateIdle()
    {
        _animator.Play("Idle");
    }

    private void AnimateRunning()
    {
        _animator.Play("Movement");
    }
}