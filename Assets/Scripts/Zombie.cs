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

    private NavMeshAgent _agent;
    private Transform _target;

    private int AnimSpeed = Animator.StringToHash("Speed");
    private int AnimAttack = Animator.StringToHash("Attack");
    private int AnimDamage = Animator.StringToHash("Damage");

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
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
        if (_target == null) { return; }
        if (health.IsAlive == false) { return; }

        anim.SetBool(AnimAttack, false);

        if(Vector3.Distance(transform.position, _target.position) <= attackRange)
        {
            anim.SetFloat(AnimSpeed, 0);
            anim.SetBool(AnimAttack, true);
        }
        else
        {
            _agent.SetDestination(_target.position);
            anim.SetFloat(AnimSpeed, _agent.speed);
        }       
    }

    private void OnDamage()
    {
        StartCoroutine(OnDamageEffect());
    }

    private IEnumerator OnDamageEffect()
    {
        anim.SetBool(AnimDamage, true);
        float speed = _agent.speed;
        _agent.speed = 0;
        _agent.SetDestination(transform.position);
        yield return new WaitForEndOfFrame();
        anim.SetBool(AnimDamage, false);

        yield return new WaitForSeconds(1);
        _agent.speed = speed;
    }

    private void OnDeath()
    {
        Destroy(gameObject);
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
