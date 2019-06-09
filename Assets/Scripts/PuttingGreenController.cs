using UnityEngine;
using UnityEngine.SceneManagement;
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
	[SerializeField] private Cup cup = null;

	private int shotCount;

	private void Start()
	{
		ball.onShotEnd.AddListener(OnShotEnd);
		powerButton.onHit = HitBall;
		resetSceneButton.onClick.AddListener(ResetBallPosition);
		cup.onBallEnterCup.AddListener(OnEndHole);
	}

	private void OnEndHole()
	{
		Debug.Log($"Shot count: {shotCount}");
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
		shotCount++;
	}

	private void OnShotEnd()
	{
	}

	private static void ResetBallPosition()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}