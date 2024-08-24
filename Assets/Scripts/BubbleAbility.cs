using System.Collections;
using UnityEngine;

public class BubbleAbility : MonoBehaviour
{
    [SerializeField] private GameObject _bubblePrefab;
    
    public bool IsShielded => _isShielded;

    private GameObject _currentBubble;
    private Coroutine _abilityCoroutine;

    private float _abilityTimer;
    private float _abilityBaseTimer = 5f;
    private bool _isShielded = false;

    // Public method to activate the ability directly
    public void ActivateAbility()
    {
        if (_isShielded)
        {
            ResetAbility();
            return;
        }

        _isShielded = true;
        _currentBubble = Instantiate(_bubblePrefab, transform.position, Quaternion.identity);
        _abilityCoroutine = StartCoroutine(AbilityUseTimer());
    }

    private void Awake()
    {
        ResetAbility();
    }

    private void Update()
    {
        // Ensure the bubble follows the player
        if (_currentBubble != null)
        {
            _currentBubble.transform.position = transform.position;
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
            Destroy(_currentBubble);
            _currentBubble = null;
        }
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