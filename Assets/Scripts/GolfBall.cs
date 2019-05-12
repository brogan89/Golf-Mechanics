using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class GolfBall : MonoBehaviour
{
	private Rigidbody rb;

	[Header("Physicals")]
	[SerializeField] private float mass = 0.46f;

	[Header("Club Info")]
	[SerializeField] private float clubSpeed;

	[Header("Shot")]
	public float launchAngle; // TODO: calculate launch angle
	public float force = 20;
	[Range(-1, 1), Tooltip("-1 backspin,+1 topspin")]
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
	public float apex;

	// events
	public UnityEvent onShotStart = new UnityEvent();
	public UnityEvent onShotEnd = new UnityEvent();

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		//trail.enabled = true;
		startPos = transform.position;

		// physicals
		rb.mass = mass;
	}

	private void FixedUpdate()
	{
		if (!isHit)
			return;

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

	private void GetStats()
	{
		// dist
		distance = Vector3.Distance(transform.position, startPos);

		// sqrMag
		magnitude = rb.velocity.magnitude;

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
		print($"Hit Ball. Force: {force}");
		rb.AddRelativeForce(Vector3.forward * force, ForceMode.Impulse);

		// add spin
		var spin = new Vector3(backspin, sideSpin, 0);
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

	public void SetLaunchAngle(float launchAngle)
	{
		this.launchAngle = launchAngle;

		var rot = transform.eulerAngles;
		rot.x = -launchAngle;
		transform.eulerAngles = rot;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.name == "Ground")
		{
			// print("hit ground");
		}
	}
}