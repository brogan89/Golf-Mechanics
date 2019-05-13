using TMPro;
using UnityEngine;

public class TargetInfoPanel : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI distanceText = null;
	[SerializeField] private TextMeshProUGUI angleText = null;
	[Space]
	[SerializeField] private new Camera camera = null;
	[SerializeField] private Transform target = null;
	[SerializeField] private Vector2 offset = Vector2.zero;
	[SerializeField] private GolfBall ball = null;

	private void Update()
	{
		FollowTarget();
		ShowDistance();
		ShowAngle();
	}

	private void FollowTarget()
	{
		var pos = camera.WorldToScreenPoint(target.position);
		transform.position = pos + (Vector3)offset;
	}

	private void ShowDistance()
	{
		var dist = Vector3.Distance(ball.transform.position, target.position);
		distanceText.text = $"Dist: {dist:0.0}m";
	}

	private void ShowAngle()
	{
		var angle = Vector3.Angle(ball.transform.position, target.position);
		angleText.text = $"Angle: {angle:00}*";
	}
}