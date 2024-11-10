using System;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class Hitbox : MonoBehaviour
{
    private Collider _trigger;
    private Action<Collider> _onTrigger;

    private void Awake()
    {
        _trigger = GetComponent<Collider>();
        _trigger.isTrigger = true;
    }

    public void Initialize(Action<Collider> onTrigger)
    {
        _onTrigger = onTrigger;
        SetState(false);
    }

    public void SetState(bool activeState)
    {
        gameObject.SetActive(activeState);
    }

    private void OnTriggerEnter(Collider other)
    {
        _onTrigger?.Invoke(other);
    }
}
