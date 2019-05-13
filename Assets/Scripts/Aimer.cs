using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class Aimer : MonoBehaviour
{
	private new Camera camera;
	public Transform aimer;
	public LayerMask layerMask;

	private Transform pivot;
	private Vector2 axis;
	public float moveSpeed = 10;
	public float zoomSpeed = 5;

	private void Start()
	{
		camera = GetComponent<Camera>();

		pivot = new GameObject("Camera Pivot").transform;
		pivot.position = Vector3.zero;
		transform.SetParent(pivot);

		pivot.position = aimer.position;
	}

	private void OnEnable()
	{
		if (pivot)
			pivot.position = aimer.position;
	}

	private void Update()
	{
		MoveTarget();
		Movement();
		Rotation();
		Zoom();
	}

	private void MoveTarget()
	{
		if (!aimer || !Input.GetMouseButton(0) || EventSystem.current.IsPointerOverGameObject())
			return;

		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		if (!Physics.Raycast(ray, out var hit, 1000, layerMask))
			return;
		Vector3 hitPoint = hit.point;
		hitPoint.y = 0;
		aimer.position = hitPoint;
	}

	private void Movement()
	{
		var x = Input.GetAxis("Horizontal");
		var y = Input.GetAxis("Vertical");
		pivot.transform.Translate(Time.deltaTime * moveSpeed * new Vector3(x, 0, y), transform);
		var pos = pivot.position;
		pos.y = 0;
		pivot.position = pos;
	}

	private void Rotation()
	{
		if (!Input.GetMouseButton(2))
			return;

		axis.y += Input.GetAxis("Mouse X") * 5;
		axis.x -= Input.GetAxis("Mouse Y") * 5;
		axis.x = Mathf.Clamp(axis.x, 3, 85);
		pivot.eulerAngles = axis;
	}

	private void Zoom()
	{
		var zoom = Input.GetAxis("Mouse ScrollWheel");
		transform.Translate(zoom * zoomSpeed * Vector3.forward);
	}
}