using UnityEngine;
using UnityEngine.EventSystems;

public class Arrow : MonoBehaviour
{
	private GolfBall ball;
	private Camera cam;

	// cache raycast info
	private RaycastHit hit;
	private Ray ray;

	public float angle;
	public Vector3 direction;
	public LayerMask layerMask;
	public Transform target;

	public bool use2;
	public bool debug;

	private void Awake()
	{
		ball = FindObjectOfType<GolfBall>();
		cam = Camera.main;
	}

	private void OnEnable()
	{
		Vector3 pos = ball.transform.position;
		pos.y = 0;
		transform.position = pos;

		hit.point = Vector3.forward;
		SetAngle();
	}

	private void Update()
	{
		if (ball)
		{
			if (Input.GetMouseButton(0))
			{
				// return if clicked on ui
				if (EventSystem.current.IsPointerOverGameObject())
					return;

				ray = cam.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, 1000, layerMask))
				{
					if (!use2)
						SetAngle();
					else
						UnityExample();
				}
			}
		}

		if (debug)
		{
			Debug.DrawLine(transform.position, target.position, Color.red);
			Debug.DrawLine(transform.position, hit.point, Color.blue);
		}
	}

	private void SetAngle()
	{
		direction = hit.point - ball.transform.position;
		angle = Vector3.SignedAngle(hit.point, direction, Vector3.up);
		transform.rotation = Quaternion.Euler(Vector3.up * angle);
		if (debug)
			Debug.DrawLine(transform.position, target.position, Color.red);
	}

	private void UnityExample()
	{
		direction = target.position - transform.position;
		angle = Vector3.SignedAngle(direction, transform.forward, Vector3.up);
		transform.rotation = Quaternion.Euler(Vector3.up * angle);
		if (debug)
			Debug.DrawLine(transform.position, target.position, Color.red);
	}
}