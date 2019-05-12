using UnityEngine;
using UnityEngine.EventSystems;

public class PuttingCamera : MonoBehaviour
{
	private GolfBall ball;
	private Transform pivot;
	private float pivotH;
	private new Camera camera;
	public Transform arrow;

	private void Awake()
	{
		ball = FindObjectOfType<GolfBall>();
		ball.onShotEnd.AddListener(OnShotEnd);
		ball.onShotStart.AddListener(OnShotStart);

		camera = Camera.main;

		// set up pivot
		pivot = new GameObject("Camera Pivot").transform;
		pivot.position = ball.transform.position;
		transform.SetParent(pivot);

		ResetArrow();
	}

	private void OnDestroy()
	{
		ball.onShotEnd.RemoveListener(OnShotEnd);
		ball.onShotStart.RemoveListener(OnShotStart);
	}

	private void OnShotStart()
	{
		arrow.gameObject.SetActive(false);
	}

	private void OnShotEnd()
	{
		arrow.gameObject.SetActive(true);
		ResetArrow();
	}

	private void Update()
	{
		if (ball.isHit)
			pivot.position = ball.transform.position;

		if (!Input.GetMouseButton(0) || EventSystem.current.IsPointerOverGameObject())
			return;

		pivotH += Input.GetAxis("Mouse X") * 5;
		pivot.eulerAngles = new Vector2(pivot.eulerAngles.x, pivotH);

		ball.transform.eulerAngles = new Vector2(ball.transform.eulerAngles.x, pivotH);

		SetArrowRotation(pivotH);
	}

	private void ResetArrow()
	{
		if (!arrow)
			return;

		arrow.transform.position = ball.transform.position;
		SetArrowRotation(camera.transform.eulerAngles.y);
	}

	private void SetArrowRotation(float y)
	{
		if (!arrow)
			return;

		var rot = arrow.transform.eulerAngles;
		rot.y = y;
		arrow.transform.eulerAngles = rot;
	}
}