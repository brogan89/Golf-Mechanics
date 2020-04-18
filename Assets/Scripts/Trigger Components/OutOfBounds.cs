using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
	private Vector3 reSpawnPosition;

	private void Awake()
	{
		var ball = FindObjectOfType<GolfBall>();
		
		if (ball)
			reSpawnPosition = ball.transform.position;
		else
			Debug.LogError("Ball not found");
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