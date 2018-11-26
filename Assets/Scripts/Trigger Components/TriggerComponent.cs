using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class TriggerComponent : MonoBehaviour
{
	public LayerMask mask;
	[Serializable]
	public class TriggerEvent : UnityEvent<Collider> { }
	[Space]
	public TriggerEvent onTriggerEnter;

	protected Collider _collider;

	protected virtual void Start()
	{
		_collider = GetComponent<Collider>();
		_collider.isTrigger = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((1 << other.gameObject.layer) == mask)
			onTriggerEnter?.Invoke(other);
	}
}