using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class DrivingRangeController : MonoBehaviour
{
	[Header("Ball")]
	[SerializeField] private GolfBall ball = null;
	[SerializeField] private GameObject ballCam = null;
	[SerializeField] private AimCam aimCam = null;
	[SerializeField] private Transform aimTarget = null;	

	[Header("UI")]
	[SerializeField] private Button hitBtn = null;
	[SerializeField] private Toggle aimToggle = null;
	
	[Space]
	[SerializeField] private BoxSlider spinSlider = null;
	[SerializeField] private Text spinText = null;
	
	[Space]
	[SerializeField] private Slider powerSlider = null;
	[SerializeField] private Text powerText = null;
	
	[Space]
	[SerializeField] private Toggle followCamButton = null;	
	
	[Space]
	[SerializeField] private Dropdown presetDropdown = null;
	[SerializeField] private Club[] clubPresets = null;
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
		powerSlider.minValue = 0;
		powerSlider.maxValue = 1;
		powerSlider.wholeNumbers = false;
		powerSlider.value = 0.9f;
		powerSlider.onValueChanged.AddListener(OnPowerChanged);
		OnPowerChanged(powerSlider.value);

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
		ball.force = Mathf.Lerp(currentClub.avgDistMin, currentClub.avgDistMax, powerSlider.value);
		ball.HitBall();

		hitBtn.gameObject.SetActive(false);
		aimToggle.gameObject.SetActive(false);
		presetDropdown.transform.parent.gameObject.SetActive(false);
		followCamButton.gameObject.SetActive(false);
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
		
		// set ball angle
		ball.SetLaunchAngle(currentClub.loft);
		
		// set aimTarget position
		var dist = Mathf.Lerp(currentClub.avgDistMin, currentClub.avgDistMax, powerSlider.value);
		aimTarget.transform.position = new Vector3(0, 0, dist);
		aimCam.UpdatePosition();
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
		followCamButton.gameObject.SetActive(true);

		// reset values
		OnPresetChanged(Array.IndexOf(clubPresets, currentClub));
	}

	private void OnPowerChanged(float val)
	{
		powerText.text = (val * 100).ToString("0");
	}

	private void OnSpinChanged(float x, float y)
	{
		spinText.text = $"Spin: h{x:0.0}, v{y:0.0}";
		
		if (Math.Abs(x - ball.sideSpin) > 0.01f)
			ball.sideSpin = x;
		if (Math.Abs(y - ball.backspin) > 0.01f)
			ball.backspin = y;
	}

	private void ToggleCams(bool isOn)
	{
		ballCam.SetActive(!isOn);
		aimCam.gameObject.SetActive(isOn);
		hitBtn.interactable = !isOn;
	}
}