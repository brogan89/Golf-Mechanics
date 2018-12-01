using UnityEngine;
using UnityEngine.EventSystems;

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
		if (aimer)
		{
			if (Input.GetMouseButton(0))
			{
				// return if clicked on ui
				if (EventSystem.current.IsPointerOverGameObject())
					return;

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