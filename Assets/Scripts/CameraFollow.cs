using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	private Vector3 offset;

	private void Start()
	{
		offset = transform.position - target.position;
	}

	private void Update()
	{
		if (target)
			transform.position = target.position + offset;
	}
}