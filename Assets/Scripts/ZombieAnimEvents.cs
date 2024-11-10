using UnityEngine;

public class ZombieAnimEvents : MonoBehaviour
{
    public delegate void AnimDelegate();
    public AnimDelegate OnAttackStart;
    public AnimDelegate OnAttackEnd;

    public void AttackStart()
    {
        OnAttackStart?.Invoke();
    }

    public void AttackEnd() 
    { 
        OnAttackEnd?.Invoke();
    }
}
