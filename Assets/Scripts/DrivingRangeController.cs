using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class DrivingRangeController : MonoBehaviour
{
	public GolfBall ball;
	public GameObject ballCam;
	public GameObject aimCam;


	[Header("UI")]
	public Button resetBtn;
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

	void Start()
	{
		resetBtn.onClick.AddListener(RestartScene);
		hitBtn.onClick.AddListener(ball.HitBall);
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
		presetDropdown.AddOptions(clubPresets.Select(x => x.type.ToString()).ToList());
		presetDropdown.onValueChanged.AddListener(OnPresetChanged);

		// hint
		hintText.gameObject.SetActive(false);
	}

	private void OnPresetChanged(int arg0)
	{
		print("preset changed " + clubPresets[arg0]);
	}

	private void RestartScene()
	{
		aimToggle.isOn = false;
		ball.RestartScene();
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