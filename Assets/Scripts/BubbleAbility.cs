using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BubbleAbility : MonoBehaviour
{
    [SerializeField] private GameObject _bubblePrefab;

    private GameObject _currentBubble;
    private float _abilityCooldown;
    private float _abilityTimer;
    private float _abilityBaseCooldown = 5f;
    private float _abilityBaseTimer = 5f;
    private Coroutine _abilityCoroutine;
    private bool _isShielded = false;

    public UnityAction OnAbilityActivated;
    public UnityAction OnAbilityDeactivated;

    private void Awake()
    {
        ResetAbility();
    }

    private void Update()
    {
        if (_currentBubble != null)
        {
            // Ensure the bubble follows the player
            _currentBubble.transform.position = transform.position;
        }
    }

    public void ActivateAbility()
    {
        _isShielded = true;
        _currentBubble = Instantiate(_bubblePrefab, transform.position, Quaternion.identity);
        _abilityCoroutine = StartCoroutine(AbilityUseTimer());
        OnAbilityActivated?.Invoke();
    }

    public void DeactivateAbility()
    {
        _isShielded = false;
        DestroyBubble();

        if (_abilityCoroutine != null)
        {
            StopCoroutine(_abilityCoroutine);
        }

        _abilityCoroutine = StartCoroutine(AbilityCooldown());
        OnAbilityDeactivated?.Invoke();
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
        _abilityCooldown = _abilityBaseCooldown;
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

        _abilityTimer = 0f;
        Debug.Log("Ability timer ended, starting cooldown.");
        DeactivateAbility();
    }

    private IEnumerator AbilityCooldown()
    {
        while (_abilityCooldown > 0)
        {
            yield return new WaitForSeconds(1f);
            _abilityCooldown -= 1f;
            Debug.Log("Cooldown remaining: " + _abilityCooldown);
        }

        ResetAbility();
        Debug.Log("Ability is ready again.");
    }

    public bool IsShielded => _isShielded;
    public GameObject CurrentBubble => _currentBubble;
}