using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GolfBall : MonoBehaviour
{
	private Rigidbody rb;

	[Header("Physicals")]
	[SerializeField] private float mass = 0.46f;
	[SerializeField] private float radius = 2.1f;

	[Header("Club Info")]
	[Range(9f, 60f)]
	public float loft = 10; // 10 driver - 60 lob wedge

	[Header("Shot")]
	public Vector3 direction = Vector3.forward;
	public float force = 5;
	[Range(-1, 1), Tooltip("-1 backspeed,+1 topspin")]
	public float backspin;
	[Range(-1, 1), Tooltip("-1 left, +1 right")]
	public float sideSpin;

	private bool isHit;
	private long hitFrame;
	private Vector3 startPos;

	public bool useMagnus;
	public float magnusConstant = 1f;

	[Header("Stats")]
	public float distance;
	public float sqrMagnitude;
	public float apex;
	public Action onShotEnd;

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
			if (Time.frameCount > hitFrame + 10 && sqrMagnitude < 0.25f)
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
		sqrMagnitude = rb.velocity.magnitude;

		// apex
		if (transform.position.y > apex)
			apex = transform.position.y;
	}

	public void HitBall(Club club)
	{
		loft = club.loft;
		HitBall();
	}

	public void HitBall()
	{
		if (isHit)
			return;

		isHit = true;
		hitFrame = Time.frameCount;

		float height = loft / 4f; // tmp for testing TODO: inplement calculation
		direction.y = 0.5f;

		// hit ball
		print("Hit Ball. Dir: " + direction + ", force: " + force);
		rb.AddForce(direction * force, ForceMode.Impulse);

		// add spin
		Vector3 spin = new Vector3(backspin, sideSpin, 0);
		rb.AddTorque(spin * force, ForceMode.Impulse);
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