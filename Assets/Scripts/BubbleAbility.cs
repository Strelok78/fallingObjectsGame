using System.Collections;
using UnityEngine;

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

    private void Awake()
    {
        ResetAbility();
    }

    private void Update()
    {
        // For testing purposes, trigger the ability with the 'A' key
        if (Input.GetKeyDown(KeyCode.A))
        {
            ActivateAbility();
        }

        // Ensure the bubble follows the player
        if (_currentBubble != null)
        {
            _currentBubble.transform.position = transform.position;
        }
    }

    // Public method to activate the ability directly
    public void ActivateAbility()
    {
        Debug.Log("ActivateAbility called.");  // Confirm that this method is invoked

        if (_isShielded)
        {
            Debug.Log("Ability already active.");
            return;
        }

        _isShielded = true;
        _currentBubble = Instantiate(_bubblePrefab, transform.position, Quaternion.identity);
        _abilityCoroutine = StartCoroutine(AbilityUseTimer());
        Debug.Log("Ability Activated");
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