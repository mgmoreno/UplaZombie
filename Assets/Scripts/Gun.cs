using Cinemachine;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private new Transform camera;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioSource gunSound;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject impactPrefab;
    [SerializeField] private CinemachineImpulseSource cameraShake;

    [Header("Properties")]
    [SerializeField] private LayerMask fireMask;
    [SerializeField] private int damage = 1;
    [SerializeField] private int clipSize = 6;

    private int _currentAmmo;
    private bool _reloading;

    private int AnimFire = Animator.StringToHash("Fire");
    private int AnimReload = Animator.StringToHash("Reload");

    private void Awake()
    {
        _currentAmmo = clipSize;
    }

    private void Update()
    {
        animator.SetBool(AnimFire, false);
        animator.SetBool(AnimReload, false);

        if (_reloading) 
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            Reload();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_currentAmmo <= 0) 
            {
                Reload();
                return;
            }

            gunSound.Play();
            animator.SetBool(AnimFire, true);
            muzzleFlash.Play();
            cameraShake.GenerateImpulseWithVelocity(Random.onUnitSphere * Random.Range(0.02f, 0.04f));
            Shoot();
        }
    }

    private void Shoot()
    {
        _currentAmmo--;
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, 100, fireMask, QueryTriggerInteraction.Ignore))
        {
            GameObject impact = Instantiate(impactPrefab, hit.point, Quaternion.identity);
            Destroy(impact, .5f);

            Health health = hit.collider.GetComponent<Health>();
            if (health != null)
            {
                health.ApplyDamage(damage);
            }
        }
    }

    private void Reload()
    {
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        animator.SetBool(AnimReload, true);
        _reloading = true;
        yield return new WaitForSeconds(0.5f);
        _reloading = false;
        _currentAmmo = clipSize;
    }
}
