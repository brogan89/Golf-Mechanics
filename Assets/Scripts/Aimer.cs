using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Aimer : MonoBehaviour
{
	private Camera cam;
	public Transform aimer;
	public LayerMask layerMask;

	private void Start()
	{
		cam = GetComponent<Camera>();
	}

	private void Update()
	{
		if (Input.GetMouseButton(1))
		{
			if (aimer)
			{
				RaycastHit hit;
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit, 1000, layerMask))
				{
					Vector3 hitPoint = hit.point;
					hitPoint.y = 0;
					aimer.position = hitPoint;
				}
			}
		}
	}
}