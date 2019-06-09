using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public bool isPointerDown;
	public Image fillImage;
	public TextMeshProUGUI powerText;
	public float power;
	public float maxPower = 100;

	private float t;
	public Action onHit;

	private void Start()
	{
		fillImage.transform.parent.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (isPointerDown)
		{
			t += Time.deltaTime;
			power = Mathf.Lerp(0, maxPower, Mathf.PingPong(t, 1));

			powerText.text = power.ToString("0");
			fillImage.fillAmount = Mathf.Clamp01(power / maxPower);
		}
		else
		{
			t = 0;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		fillImage.transform.parent.gameObject.SetActive(true);
		isPointerDown = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		StartCoroutine(Timer());
		isPointerDown = false;
		onHit?.Invoke();
	}

	private IEnumerator Timer()
	{
		yield return new WaitForSeconds(1);
		fillImage.transform.parent.gameObject.SetActive(false);
	}
}
