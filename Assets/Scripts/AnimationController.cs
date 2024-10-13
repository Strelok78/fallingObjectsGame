using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class AnimationController : MonoBehaviour
{
    private Animator _animator;
    private CapsuleCollider2D _collider;

    public UnityAction<float> OnSetXVelocity;
    public UnityAction<bool> OnFlipSprite;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider2D>();
        OnSetXVelocity += SetXVelocity;
        OnFlipSprite += FlipSprite;
    }

    private void OnDestroy()
    {
        OnSetXVelocity -= SetXVelocity;
        OnFlipSprite -= FlipSprite;
    }

    private void SetXVelocity(float xVelocity)
    {
        if (xVelocity > 0)
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
    
    public void AnimateShooting()
    {
        _animator.Play("Shoot");
    }

    public void AnimateDeath()
    {
        _animator.Play("Death");
    }

    public void SwitchColliderSize()
    {
        _collider.size = new Vector2(_collider.size.x, 0.6f);
    }

    public void StopAnimations()
    {
        _animator.speed = 0;
    }
}