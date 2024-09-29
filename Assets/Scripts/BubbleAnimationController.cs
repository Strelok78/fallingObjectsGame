using UnityEngine;

public class BubbleAnimationController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayBubblePoppedAnimation()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("BubbleAnimation"))
        {
            _animator.Play("BubbleAnimation");
        }
    }

    public float GetAnimationLength(string animationName)
    {
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0f;
    }
}