using System.Collections;
using UnityEngine;

public class BubbleAbility : MonoBehaviour
{
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private float _abilityBaseTimer = 5f;

    public bool IsShielded => _isShielded;

    private GameObject _currentBubble;
    private Coroutine _popping;
    private Coroutine _abilityCoroutine;
    private BubbleAnimationController _bubbleAnimationController;

    private float _abilityTimer;
    private bool _isShielded = false;

    public void ActivateAbility()
    {
        if (_isShielded)
        {
            ResetAbility();
            return;
        }

        _isShielded = true;
        _currentBubble = Instantiate(_bubblePrefab, transform.position, Quaternion.identity);
        _bubbleAnimationController = _currentBubble.GetComponent<BubbleAnimationController>();
        _abilityCoroutine = StartCoroutine(AbilityUseTimer());
    }

    private void Awake()
    {
        ResetAbility();
    }

    private void Update()
    {
        if (_currentBubble != null)
        {
            _currentBubble.transform.position = transform.position;
        }
        else if (_popping != null)
        {
            StopCoroutine(_popping);
        }
    }

    public void DeactivateAbility()
    {
        _isShielded = false;
        DestroyBubble();
        ResetAbility();

        if (_abilityCoroutine != null)
        {
            StopCoroutine(_abilityCoroutine);
        }
    }

    private void DestroyBubble()
    {
        if (_currentBubble != null)
        {
            _bubbleAnimationController.PlayBubblePoppedAnimation();
            _popping = StartCoroutine(AnimatePopping());
        }
    }

    private IEnumerator AnimatePopping()
    {
        yield return new WaitForSeconds(_bubbleAnimationController.GetAnimationLength("BubbleAnimation"));
        Destroy(_currentBubble);
        _currentBubble = null;
    }

    private void ResetAbility()
    {
        _abilityTimer = _abilityBaseTimer;
    }

    private IEnumerator AbilityUseTimer()
    {
        while (_abilityTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            _abilityTimer -= 1f;
            Debug.Log("Time remaining for Ability: " + _abilityTimer);
        }

        DeactivateAbility();
    }
}