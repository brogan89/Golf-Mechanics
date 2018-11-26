using UnityEngine;
using UnityEngine.UI;

public class PuttingGreenController : MonoBehaviour
{
	[Header("UI Controls")]
	public Button hitButton;
	public Button resetSceneButton;

	[Header("Putter")]
	public Club club;

	[Header("Scene Objects")]
	public GolfBall ball;
	private Vector3 ballStartPosition;
	private Quaternion ballStartRotation;

	private void Start()
	{
		ballStartPosition = ball.transform.position;
		ballStartRotation = ball.transform.rotation;
		ball.target = FindObjectOfType<Cup>().transform;

		hitButton.onClick.AddListener(HitBall);
		resetSceneButton.onClick.AddListener(ResetBallPosition);
	}

	private void HitBall()
	{
		if (club)
			ball.HitBall(club);
		else
			ball.HitBall();
	}

	private void ResetBallPosition()
	{
		ball.transform.position = ballStartPosition;
		ball.transform.rotation = ballStartRotation;
	}
}