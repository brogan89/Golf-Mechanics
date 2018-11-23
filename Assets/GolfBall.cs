using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GolfBall : MonoBehaviour
{
	private Rigidbody rb;
	public Transform target;

	[Header("Club Info")]
	public float degree = 10; // driver

	[Header("Shot")]
	public float force = 5;
	[Range(-1, 1)]
	public float backspin;
	[Range(-1, 1)]
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

	private TrailRenderer trail;

	void Start()
	{
		trail = GetComponent<TrailRenderer>();
		rb = GetComponent<Rigidbody>();
		startPos = transform.position;
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
				ResetBall();
				print("Shot end: " + distance.ToString("0.0") + "m");
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

	public void HitBall()
	{
		isHit = true;
		hitFrame = Time.frameCount;

		Vector3 targetPosition = target.position;

		float height = degree * force / 2f;
		targetPosition.y = height;

		// get direction to target
		Vector3 dir = (targetPosition - transform.position).normalized;

		// hit ball
		print("Hit Ball. Dir: " + dir + ", height: " + height);
		rb.AddForce(dir * force, ForceMode.Impulse);

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