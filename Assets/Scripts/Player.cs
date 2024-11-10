using UnityEngine;

[RequireComponent (typeof(Health))]
public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Gun gun;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource damageSound;
    [SerializeField] private AudioSource deathSound;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health> ();
    }
    private void OnEnable()
    {
        _health.OnDamage += OnDamage;
        _health.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        _health.OnDamage -= OnDamage;
        _health.OnDeath -= OnDeath;
    }

    private void OnDamage()
    {
        if (!damageSound.isPlaying)
        {
            damageSound.Play();
        }
    }

    private void OnDeath()
    {
        deathSound.Play();
        gun.enabled = false;
        // Do GameOver Sequence
    }
}
