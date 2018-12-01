using UnityEngine;
using UnityEngine.EventSystems;

public class Arrow : MonoBehaviour
{
	private GolfBall ball;
	private float h;

	private void Awake()
	{
		ball = FindObjectOfType<GolfBall>();

		ball.onShotStart += OnShotStart;
		ball.onShotEnd += OnShotEnd;

		ResetArrow();
	}

	private void OnShotStart()
	{
		gameObject.SetActive(false);
	}

	private void OnShotEnd()
	{
		gameObject.SetActive(true);
		ResetArrow();
	}

	private void ResetArrow()
	{
		Vector3 pos = ball.transform.position;
		pos.y = 0;
		transform.position = pos;

		var rot = transform.eulerAngles;
		rot.y = Camera.main.transform.eulerAngles.y;
		transform.eulerAngles = rot;
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

				h += Input.GetAxis("Mouse X") * 5f;
				transform.eulerAngles = new Vector2(transform.eulerAngles.x, h);
			}
		}
	}
}