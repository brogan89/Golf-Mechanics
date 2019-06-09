using System;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
	private Vector3 reSpawnPosition;

	private void Awake()
	{
		reSpawnPosition = FindObjectOfType<GolfBall>().transform.position;
	}


	private void OnTriggerEnter(Collider col)
	{
		if (!col.CompareTag("Ball"))
			return;

		Debug.Log("Out of bounds!");
		col.GetComponent<Rigidbody>().velocity = Vector3.zero;
		col.transform.position = reSpawnPosition;
	}
}