using UnityEngine;

public class InitialVelocity : MonoBehaviour
{
	public new Rigidbody rigidbody;
	public Vector3 initialVelocity;
	public Vector3 angularVelocity;

	[Header("Magnus")]
	public float magnusConst = 1;


	private void Start()
	{
		rigidbody.velocity = initialVelocity;
		rigidbody.angularVelocity = angularVelocity;
	}

	private void FixedUpdate()
	{
		rigidbody.AddForce(magnusConst * Vector3.Cross(angularVelocity, initialVelocity));
	}
}