using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class GolfBall : MonoBehaviour
{
	private new Rigidbody rigidbody;

	[Header("Shot")]
	public float force = 20;
	[Range(-1, 1), Tooltip("-1 backspin,+1 topspin")]
	public float backspin;
	[Range(-1, 1), Tooltip("-1 left, +1 right")]
	public float sideSpin;

	public float magnusConstant = 0.03f;

	[Header("Stats")]
	public bool isHit;
	public BallStats stats;

	// internal stats
	private long hitFrame;
	private Vector3 startPos;

	// events
	public UnityEvent onShotStart = new UnityEvent();
	public UnityEvent onShotEnd = new UnityEvent();

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		startPos = transform.position;
	}

	private void FixedUpdate()
	{
		if (!isHit)
			return;

		// magnus effect
		if (magnusConstant > 0)
			rigidbody.AddForce(magnusConstant * Vector3.Cross(rigidbody.angularVelocity, rigidbody.velocity));

		// end shot conditions
		if (Time.frameCount > hitFrame + 10 && rigidbody.velocity.magnitude < 0.25f)
		{
			Debug.Log($"Shot end: {stats}");
			isHit = false;
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			onShotEnd?.Invoke();
			return;
		}

		// dist
		stats.distance = Vector3.Distance(transform.position, startPos);

		// apex
		if (transform.position.y > stats.height)
			stats.height = transform.position.y;
	}

	public void HitBall()
	{
		if (isHit)
			return;

		startPos = transform.position;
		isHit = true;
		hitFrame = Time.frameCount;
		onShotStart?.Invoke();

		// hit ball
		rigidbody.AddRelativeForce(Vector3.forward * force, ForceMode.Impulse);

		// add spin
		var spin = new Vector3(backspin, sideSpin, 0);
		rigidbody.AddRelativeTorque(spin, ForceMode.Impulse);
	}

	public void SetLaunchAngle(float launchAngle)
	{
		var rot = transform.eulerAngles;
		rot.x = -launchAngle;
		transform.eulerAngles = rot;
	}
}