using Cinemachine;
using UnityEngine;

[RequireComponent (typeof(Health))]
public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Gun gun;
    [SerializeField] private CinemachineImpulseSource cameraShake;

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

        cameraShake.GenerateImpulseWithVelocity(Random.onUnitSphere * Random.Range(0.1f, 0.2f));
    }

    private void OnDeath()
    {
        deathSound.Play();
        gun.enabled = false;
        cameraShake.GenerateImpulseWithVelocity(Random.onUnitSphere * Random.Range(0.3f, 0.4f));
        // Do GameOver Sequence
    }
}
