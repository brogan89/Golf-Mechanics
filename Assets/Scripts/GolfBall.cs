using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GolfBall : MonoBehaviour
{
	private Rigidbody rb;

	[Header("Physicals")]
	[SerializeField] private float mass = 0.46f;
	// [SerializeField] private float radius = 2.1f;

	[Header("Club Info")]
	[Range(9f, 60f)]
	public float loft = 10; // 10 driver - 60 lob wedge

	[Header("Shot")]
	public Vector3 velocity = Vector3.forward;
	[Range(-1, 1), Tooltip("-1 backspeed,+1 topspin")]
	public float backspin;
	[Range(-1, 1), Tooltip("-1 left, +1 right")]
	public float sideSpin;

	public bool useMagnus;
	public float magnusConstant = 1f;

	[Header("Stats")]
	public bool isHit;
	private long hitFrame;
	private Vector3 startPos;
	public float distance;
	public float magnitude;
	public Vector3 inertiaTensor;
	public float apex;

	// events
	public event Action onShotEnd;
	public event Action onShotStart;

	private TrailRenderer trail;

	private void Awake()
	{
		trail = GetComponent<TrailRenderer>();
		rb = GetComponent<Rigidbody>();
	}

	void Start()
	{
		trail.enabled = true;
		startPos = transform.position;

		// physicals
		rb.mass = mass;
	}

	private void FixedUpdate()
	{
		if (isHit)
		{
			// magnus effect
			if (useMagnus)
				rb.AddForce(magnusConstant * Vector3.Cross(rb.angularVelocity, rb.velocity));

			// end shot conditions
			if (Time.frameCount > hitFrame + 10 && magnitude < 0.25f)
			{
				print("Shot end: " + distance.ToString("0.0") + "m");
				ResetBall();
				onShotEnd?.Invoke();
			}

			GetStats();
		}
		else
		{
			// clean up trail
			if (trail.positionCount > 0)
				trail.Clear();
		}
	}

	private void GetStats()
	{
		// dist
		distance = Vector3.Distance(transform.position, startPos);

		// sqrMag
		magnitude = rb.velocity.magnitude;
		inertiaTensor = rb.inertiaTensor;

		// apex
		if (transform.position.y > apex)
			apex = transform.position.y;
	}

	public void HitBall()
	{
		if (isHit)
			return;

		isHit = true;
		hitFrame = Time.frameCount;
		onShotStart?.Invoke();

		// hit ball
		print($"Hit Ball. Velocity: {velocity}");
		rb.velocity = velocity;

		// add spin
		Vector3 spin = new Vector3(backspin, sideSpin, 0);
		rb.angularVelocity = spin;
	}

	private void ResetBall()
	{
		isHit = false;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		apex = 0;
	}

	public void RestartScene()
	{
		ResetBall();
		transform.position = startPos;
		transform.rotation = Quaternion.identity;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.name == "Ground")
		{
			// print("hit ground");
		}
	}
}