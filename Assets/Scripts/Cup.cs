using UnityEngine;

public class Cup : MonoBehaviour
{
	public SphereTriggerComponent trigger;

	private void Start()
	{
		trigger.onTriggerEnter.AddListener(OnEnter);
	}

	private void OnEnter(Collider collider)
	{
		print(collider.name + " entered cup");
	}
}