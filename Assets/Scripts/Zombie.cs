using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Hitbox hitbox;
    [SerializeField] private ZombieAnimEvents animEvents;
    [SerializeField] private Health health;

    [Header("Properties")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float hitStuntTime = .5f;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource damageSound;
    [SerializeField] private AudioSource deathSound;

    private NavMeshAgent _agent;
    private Collider _collider;
    private Transform _target;

    private bool _inDamage;

    private int AnimSpeed = Animator.StringToHash("Speed");
    private int AnimAttack = Animator.StringToHash("Attack");
    private int AnimDamage = Animator.StringToHash("Damage");
    private int AnimDeath = Animator.StringToHash("Death");
    private int AnimDeathClip = Animator.StringToHash("DeathClip");

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        hitbox.Initialize(OnHitDetected);        
    }

    private void OnEnable()
    {
        animEvents.OnAttackStart += AttackStart;
        animEvents.OnAttackEnd += AttackEnd;
        health.OnDamage += OnDamage;
        health.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        animEvents.OnAttackStart -= AttackStart;
        animEvents.OnAttackEnd -= AttackEnd;
        health.OnDamage -= OnDamage;
        health.OnDeath -= OnDeath;
    }

    private void Update()
    {
        anim.SetBool(AnimAttack, false);

        if (_target == null) { return; }
        if (health.IsAlive == false) { return; }        

        if(Vector3.Distance(transform.position, _target.position) <= attackRange)
        {
            anim.SetFloat(AnimSpeed, 0);
            anim.SetBool(AnimAttack, true);

            if (!attackSound.isPlaying)
            {
                attackSound.Play();
            }            
        }
        else
        {
            _agent.SetDestination(_target.position);
            anim.SetFloat(AnimSpeed, _agent.speed);
        }       
    }

    private void OnDamage()
    {
        if (_inDamage) 
        {
            return;
        }
        StartCoroutine(OnDamageEffect());
    }

    private IEnumerator OnDamageEffect()
    {
        _inDamage = true;
        if (!damageSound.isPlaying)
        {
            damageSound.Play();
        }

        anim.SetBool(AnimDamage, true);
        _agent.isStopped = true;

        // Wait one frame to reset flag so we don't keep transitioning to damage animation
        yield return new WaitForEndOfFrame();
        anim.SetBool(AnimDamage, false);

        yield return new WaitForSeconds(hitStuntTime);

        // Only resume movement if zombie is still alive
        if (health.IsAlive)
        {            
            _agent.isStopped = false;
        }
        _inDamage = false;
    }

    private void OnDeath()
    {
        StartCoroutine(OnDeathEffect());
    }

    private IEnumerator OnDeathEffect()
    {
        if (!deathSound.isPlaying)
        {
            deathSound.Play();
        }

        anim.SetFloat(AnimDeathClip, Random.Range(0, 2));
        anim.SetBool(AnimDeath, true);
        anim.SetBool(AnimDamage, false);
        anim.SetFloat(AnimSpeed, 0);

        _agent.isStopped = true;
        _collider.enabled = false;
        Destroy(gameObject, 2);

        // Wait one frame to reset flag so we don't keep transitioning to death animation
        yield return new WaitForEndOfFrame();
        anim.SetBool(AnimDeath, false);
    }

    private void AttackStart()
    {
        hitbox.SetState(true);
    }

    private void AttackEnd()
    {
        hitbox.SetState(false);
    }

    private void OnHitDetected(Collider hitted)
    {
        Health health = hitted.GetComponent<Health>();
        if (health == null) { return; }

        health.ApplyDamage(attackDamage);
    }
}
