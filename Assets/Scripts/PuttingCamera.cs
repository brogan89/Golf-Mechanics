using UnityEngine;

public class PuttingCamera : MonoBehaviour
{
	private GolfBall ball;

	private Transform pivot;
	private float pivotH;

	private void Awake()
	{
		ball = FindObjectOfType<GolfBall>();

		ball.onShotEnd.AddListener(OnShotEnd);
		ball.onShotStart.AddListener(OnShotStart);

		// set up pivot
		pivot = new GameObject("Camera Pivot").transform;
		pivot.position = ball.transform.position;
		this.transform.SetParent(pivot);
	}

	private void OnDestroy()
	{
		ball.onShotEnd.RemoveListener(OnShotEnd);
		ball.onShotStart.RemoveListener(OnShotStart);
	}

	private void OnShotEnd()
	{

	}

	private void OnShotStart()
	{

	}

	private void Update()
	{
		if (ball.isHit)
		{
			pivot.position = ball.transform.position;
		}

		if (Input.GetMouseButton(1))
		{
			pivotH += Input.GetAxis("Mouse X") * 2;
			pivot.eulerAngles = new Vector2(pivot.eulerAngles.x, pivotH);
		}
	}
}