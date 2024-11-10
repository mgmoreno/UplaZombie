using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;

    private int _currentHealth;

    public bool IsAlive => _currentHealth > 0;

    public delegate void OnHealthEvent();
    public OnHealthEvent OnDamage;
    public OnHealthEvent OnDeath;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    public void ApplyDamage(int damage)
    {
        if (!IsAlive) { return; }

        _currentHealth -= damage;

        if (_currentHealth < 0) 
        { 
            _currentHealth = 0;
        }

        if(_currentHealth > 0)
        {
            OnDamage?.Invoke();
        }
        else
        {
            OnDeath?.Invoke();
        }
    }
}
