using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private new Transform camera;
    [SerializeField] private LayerMask fireMask;
    [SerializeField] private int damage = 1;
    [SerializeField] private AudioSource gunSound;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject impactPrefab;

    private int AnimFire = Animator.StringToHash("Fire");

    private void Update()
    {
        animator.SetBool(AnimFire, false);

        if (Input.GetMouseButtonDown(0))
        {
            gunSound.Play();
            animator.SetBool(AnimFire, true);
            Shoot();
        }
    }

    public void Shoot()
    {       
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
}
