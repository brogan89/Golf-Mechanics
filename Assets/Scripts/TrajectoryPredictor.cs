using UnityEngine;

/*
 * src: https://youtu.be/IvT8hjy6q4o
 */

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPredictor : MonoBehaviour
{
	private LineRenderer lineRenderer;

	public Transform start;
	public Transform target;

	public int resolution = 20;
	public float height = 10;
	public Vector3 velocity;

	private float gravity;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		gravity = Physics.gravity.y;
	}

	private void FixedUpdate()
	{
		if (!target)
			return;

		if (height < 0)
			height = 0;

		velocity = CalculateLaunchData().initialVelocity;
		DrawPath();
	}

	private void DrawPath()
	{
		LaunchData launchData = CalculateLaunchData();
		Vector3 prevDrawPoint = transform.position;

		Vector3[] points = new Vector3[resolution + 1];
		points[0] = prevDrawPoint;

		for (int i = 1; i <= resolution; i++)
		{
			float simulatedTime = i / (float)resolution * launchData.timeToTarget;
			Vector3 displacement = launchData.initialVelocity * simulatedTime + Vector3.up * gravity * simulatedTime * simulatedTime / 2f;
			Vector3 drawPoint = transform.position + displacement;
			points[i] = drawPoint;
			prevDrawPoint = drawPoint;
		}

		// render line
		lineRenderer.positionCount = points.Length;
		lineRenderer.SetPositions(points);
	}

	/// <summary>
	/// Returns the initial velocity based on target position and given height
	/// </summary>
	/// <returns></returns>
	private LaunchData CalculateLaunchData()
	{
		// get displacement
		float displacementY = target.position.y - start.position.y;
		Vector3 displacementXZ = new Vector3(target.position.x - start.position.x, 0, target.position.z - start.position.z);
		float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);

		// get velocity
		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
		Vector3 velocityXZ = displacementXZ / time;

		return new LaunchData(velocityXZ + velocityY, time);
	}

	private struct LaunchData
	{
		internal readonly Vector3 initialVelocity;
		internal readonly float timeToTarget;

		internal LaunchData(Vector3 initialVelocity, float timeToTarget)
		{
			this.initialVelocity = initialVelocity;
			this.timeToTarget = timeToTarget;
		}
	}
}