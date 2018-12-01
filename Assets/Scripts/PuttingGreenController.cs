using UnityEngine;
using UnityEngine.UI;

public class PuttingGreenController : MonoBehaviour
{
	[Header("UI Controls")]
	[SerializeField] private Button hitButton;
	[SerializeField] private Button resetSceneButton;
	[SerializeField] private Text powerText;
	[SerializeField] private Slider powerSlider;

	[Header("Putter")]
	[SerializeField] private Club club;

	[Header("Scene Objects")]
	[SerializeField] private GolfBall ball;
	[SerializeField] private Arrow arrow;

	private Vector3 ballStartPosition;
	private Quaternion ballStartRotation;

	private void Start()
	{
		ballStartPosition = ball.transform.position;
		ballStartRotation = ball.transform.rotation;
		ball.onShotEnd += OnShotEnd;

		hitButton.onClick.AddListener(HitBall);
		resetSceneButton.onClick.AddListener(ResetBallPosition);

		powerSlider.onValueChanged.AddListener(OnSliderChanged);
		powerSlider.value = 20;
	}

	private void OnSliderChanged(float value)
	{
		ball.velocity = arrow.direction * value;
		powerText.text = $"Power: {value}";
	}

	private void OnDestroy()
	{
		ball.onShotEnd -= OnShotEnd;
	}

	private void HitBall()
	{
		ball.loft = club.loft;
		ball.velocity = arrow.direction * powerSlider.value;
		ball.HitBall();
		arrow.gameObject.SetActive(false);
	}

	private void OnShotEnd()
	{
		arrow.gameObject.SetActive(true);
	}

	private void ResetBallPosition()
	{
		ball.transform.position = ballStartPosition;
		ball.transform.rotation = ballStartRotation;
	}
}