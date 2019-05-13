using UnityEngine;
using UnityEngine.UI;

public class PuttingGreenController : MonoBehaviour
{
	[Header("UI Controls")]
	[SerializeField] private PowerUpButton powerButton = null;
	[SerializeField] private Button resetSceneButton = null;

	[Header("Putter")]
	[SerializeField] private Club club = null;

	[Header("Scene Objects")]
	[SerializeField] private GolfBall ball = null;

	private void Start()
	{
		ball.onShotEnd.AddListener(OnShotEnd);
		powerButton.onHit = HitBall;
		resetSceneButton.onClick.AddListener(ResetBallPosition);
	}

	private void OnDestroy()
	{
		ball.onShotEnd.RemoveListener(OnShotEnd);
	}

	private void HitBall()
	{
		ball.SetLaunchAngle(club.loft);
		ball.force = powerButton.power;
		ball.HitBall();
	}

	private void OnShotEnd()
	{
	}

	private void ResetBallPosition()
	{
		Menu.Instance.ChangeScene(Menu.MINI_PUTT);
	}
}