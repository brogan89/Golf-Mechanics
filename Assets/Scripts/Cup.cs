using UnityEngine;
using UnityEngine.Events;

public class Cup : MonoBehaviour
{
	public UnityEvent onBallEnterCup = new UnityEvent();

	private void OnTriggerEnter(Collider col)
	{
		if (!col.CompareTag("Ball"))
			return;

		Debug.Log("Ball in hole");
		
		// destroy ball
		Destroy(col.gameObject);
		
		// event
		onBallEnterCup?.Invoke();
	}
}