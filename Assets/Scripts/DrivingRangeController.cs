using System.Collections;
using System.Linq;
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
	public Text hintText;
	private bool showHintText = true;

	[Space]
	public Dropdown presetDropdown;
	public Club[] clubPresets;

	[Header("Stats")]
	[SerializeField] private GameObject statsPanel;

	private void Start()
	{
		hitBtn.onClick.AddListener(HitBall);
		aimToggle.onValueChanged.AddListener(ToggleCams);

		// splin
		spinSlider.ValueX = ball.sideSpin;
		spinSlider.ValueY = ball.backspin;
		spinSlider.OnValueChanged.AddListener(OnSpinChanged);

		// power
		powerSlider.onValueChanged.AddListener(OnPowerChanged);
		powerSlider.value = ball.force;

		// loft
		loftSlider.onValueChanged.AddListener(OnLoftChanged);
		loftSlider.value = ball.loft;

		// presets
		presetDropdown.ClearOptions();
		presetDropdown.AddOptions(clubPresets.Select(x => x.name).ToList());
		presetDropdown.onValueChanged.AddListener(OnPresetChanged);
		OnPresetChanged(0);

		// hint
		hintText.gameObject.SetActive(false);

		ball.onShotEnd = ShotEnd;
		statsPanel.SetActive(false);
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
		statsPanel.SetActive(true);
	}

	private void OnPresetChanged(int index)
	{
		print("preset changed " + clubPresets[index]);
		Club club = clubPresets[index];
		ball.loft = club.loft;
		// ball.force = club.avgDistMax - (club.avgDistMax - club.avgDistMin);
	}

	public void RestartScene()
	{
		aimToggle.isOn = false;
		ball.RestartScene();

		presetDropdown.transform.parent.gameObject.SetActive(true);
		aimToggle.gameObject.SetActive(true);
		hitBtn.gameObject.SetActive(true);
	}

	private void OnLoftChanged(float val)
	{
		loftText.text = val + "*";
		ball.loft = val;
	}

	private void OnPowerChanged(float val)
	{
		powerText.text = "Power: " + val;
		ball.force = val;
	}

	private void OnSpinChanged(float x, float y)
	{
		spinText.text = string.Format("Spin: ({0},{1})", x.ToString("0.0"), y.ToString("0.0"));
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

		hintText.gameObject.SetActive(isOn && showHintText);
		if (hintText.gameObject.activeInHierarchy)
			StartCoroutine(WaitForMouseClick());
	}

	IEnumerator WaitForMouseClick()
	{
		yield return new WaitUntil(() => Input.GetMouseButtonDown(1));
		showHintText = false;
		hintText.gameObject.SetActive(false);
	}
}