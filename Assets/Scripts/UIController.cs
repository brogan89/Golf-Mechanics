using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
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

	// Use this for initialization
	void Start()
	{
		resetBtn.onClick.AddListener(RestartScene);
		hitBtn.onClick.AddListener(ball.HitBall);
		aimToggle.onValueChanged.AddListener(ToggleCams);

		spinSlider.onValueChanged.AddListener(OnSpinChanged);
		// spinSlider.Value = new Vector2(ball.backspin, ball.sideSpin); // todo: fix box slider value.set

		powerSlider.onValueChanged.AddListener(OnPowerChanged);
		powerSlider.value = ball.force;

		loftSlider.onValueChanged.AddListener(OnLoftChanged);
		loftSlider.value = ball.loft;

		hintText.gameObject.SetActive(false);
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

	private void OnSpinChanged(Vector2 val)
	{
		spinText.text = "Spin: " + val;
		ball.backspin = val.x;
		ball.sideSpin = val.y;
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