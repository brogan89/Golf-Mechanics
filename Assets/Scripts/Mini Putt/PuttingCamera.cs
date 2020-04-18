using UnityEngine;
using UnityEngine.EventSystems;

public class PuttingCamera : MonoBehaviour
{
	private GolfBall ball;
	private Transform pivot;
	private float pivotH;
	private new Camera camera;
	
	[SerializeField] private Transform arrowPrefab = null;
	private Transform arrow;

	private const float CAM_ROTATE_SPEED = 5f;

	private void Awake()
	{
		ball = FindObjectOfType<GolfBall>();
		ball.onShotEnd.AddListener(OnShotEnd);
		ball.onShotStart.AddListener(OnShotStart);

		camera = GetComponent<Camera>();

		// set up pivot
		pivot = new GameObject("Camera Pivot").transform;
		pivot.position = ball.transform.position;
		transform.SetParent(pivot);

		arrow = Instantiate(arrowPrefab);
		ResetArrow();
	}

	private void OnShotStart()
	{
		arrow.gameObject.SetActive(false);
	}

	private void OnShotEnd()
	{
		ResetArrow();
	}

	private void Update()
	{
		if (!ball)
			return;

		// always follow ball
		if (ball)
			pivot.position = ball.transform.position;
		
		// change pivot value
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
			pivotH += Input.GetAxis("Mouse X") * CAM_ROTATE_SPEED;

		// rotate camera pivot
		pivot.eulerAngles = new Vector3(pivot.eulerAngles.x, pivotH, 0);
		
		// only rotate ball when its not currently being hit
		if (!ball.isHit)
			ball.transform.eulerAngles = new Vector3(0, pivotH, 0);

		SetArrowRotation(pivotH);
	}

	private void ResetArrow()
	{
		if (!arrow)
			return;

		arrow.gameObject.SetActive(true);
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