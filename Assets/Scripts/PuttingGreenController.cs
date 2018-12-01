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

	private void Start()
	{
		ball.onShotEnd += OnShotEnd;

		hitButton.onClick.AddListener(HitBall);
		resetSceneButton.onClick.AddListener(ResetBallPosition);

		powerSlider.onValueChanged.AddListener(OnSliderChanged);
		powerSlider.value = 20;
	}

	private void OnSliderChanged(float value)
	{
		powerText.text = $"Power: {value}";
	}

	private void OnDestroy()
	{
		ball.onShotEnd -= OnShotEnd;
	}

	private void HitBall()
	{
		ball.Loft = club.loft;
		ball.transform.rotation = arrow.transform.rotation;
		ball.force = powerSlider.value;
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