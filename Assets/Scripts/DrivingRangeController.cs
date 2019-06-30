using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class DrivingRangeController : MonoBehaviour
{
	[Header("Ball")]
	public GolfBall ball;
	public GameObject ballCam;
	public GameObject aimCam;

	[Header("UI")]
	public Button hitBtn;
	public Toggle aimToggle;
	[Space]
	public BoxSlider spinSlider;
	public Text spinText;
	[Space]
	public Slider powerSlider;
	public Text powerText;
	[Space]
	public Slider loftSlider;
	public Text loftText;

	[Space]
	public Dropdown presetDropdown;
	public Club[] clubPresets;
	private Club currentClub;

	[Header("Stats")]
	[SerializeField] private RangeStatsPanel statsPanel = null;

	private void Start()
	{
		hitBtn.onClick.AddListener(HitBall);
		aimToggle.onValueChanged.AddListener(ToggleCams);

		// presets
		presetDropdown.ClearOptions();
		presetDropdown.AddOptions(clubPresets.Select(x => x.name).ToList());
		presetDropdown.onValueChanged.AddListener(OnPresetChanged);
		OnPresetChanged(0);

		// spin
		spinSlider.ValueX = ball.sideSpin;
		spinSlider.ValueY = ball.backspin;
		spinSlider.OnValueChanged.AddListener(OnSpinChanged);
		OnSpinChanged(spinSlider.ValueX, spinSlider.ValueY);

		// power
		powerSlider.minValue = 100;
		powerSlider.maxValue = 210;
		powerSlider.value = powerSlider.maxValue;
		powerSlider.onValueChanged.AddListener(OnPowerChanged);
		OnPowerChanged(powerSlider.value);

		// loft
		loftSlider.onValueChanged.AddListener(OnLoftChanged);
		OnLoftChanged(loftSlider.value);

		ball.onShotEnd.AddListener(ShotEnd);
		statsPanel.gameObject.SetActive(false);
		statsPanel.onClose = RestartScene;
	}

	private void OnDestroy()
	{
		ball.onShotEnd.RemoveListener(ShotEnd);
	}

	private void HitBall()
	{
		ball.HitBall();

		hitBtn.gameObject.SetActive(false);
		aimToggle.gameObject.SetActive(false);
		presetDropdown.transform.parent.gameObject.SetActive(false);
	}

	private void ShotEnd()
	{
		statsPanel.gameObject.SetActive(true);
		statsPanel.ShowData(ball.stats);
	}

	private void OnPresetChanged(int index)
	{
		Debug.Log("club preset changed " + clubPresets[index]);
		currentClub = clubPresets[index];
		loftSlider.value = currentClub.loft;
	}

	private void RestartScene()
	{
		aimToggle.isOn = false;
		ball.stats = new BallStats();
		ball.transform.position = new Vector3(0, 0.11f, 0);
		ball.transform.rotation = Quaternion.identity;

		presetDropdown.transform.parent.gameObject.SetActive(true);
		aimToggle.gameObject.SetActive(true);
		hitBtn.gameObject.SetActive(true);

		// reset values
		OnLoftChanged(loftSlider.value);
	}

	private void OnLoftChanged(float val)
	{
		loftText.text = val + "*";
		ball.SetLaunchAngle(val);
	}

	private void OnPowerChanged(float val)
	{
		powerText.text = val.ToString("0");
		ball.force = val;
	}

	private void OnSpinChanged(float x, float y)
	{
		spinText.text = $"Spin: h{x:0.0}, v{y:0.0}";
		if (x != ball.sideSpin)
			ball.sideSpin = x;
		if (y != ball.backspin)
			ball.backspin = y;
	}

	private void ToggleCams(bool isOn)
	{
		ballCam.SetActive(!isOn);
		aimCam.SetActive(isOn);
		hitBtn.interactable = !isOn;
	}
}