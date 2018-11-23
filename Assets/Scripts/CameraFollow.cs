using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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