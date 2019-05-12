using UnityEngine;
using UnityEngine.UI;

public class PuttingGreenController : MonoBehaviour
{
	[Header("UI Controls")]
	[SerializeField] private Button hitButton = null;
	[SerializeField] private Button resetSceneButton = null;
	[SerializeField] private Text powerText = null;
	[SerializeField] private Slider powerSlider = null;

	[Header("Putter")]
	[SerializeField] private Club club = null;

	[Header("Scene Objects")]
	[SerializeField] private GolfBall ball = null;

	private void Start()
	{
		ball.onShotEnd.AddListener(OnShotEnd);

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
		ball.onShotEnd.RemoveListener(OnShotEnd);
	}

	private void HitBall()
	{
		ball.SetLaunchAngle(club.loft);
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