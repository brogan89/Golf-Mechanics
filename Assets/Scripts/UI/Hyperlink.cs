using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Hyperlink : MonoBehaviour, IPointerDownHandler
{
	private TextMeshProUGUI tmpText;
	private Camera _camera;

	public Color hoverColour = Color.blue;

	[System.Serializable]
	public class ClickEvent : UnityEvent<string> { }
	public ClickEvent onLinkClicked = new ClickEvent();

	private void Awake()
	{
		tmpText = GetComponent<TextMeshProUGUI>();
		_camera = Camera.main;
	}

	private void Update()
	{
		if (TMP_TextUtilities.IsIntersectingRectTransform(tmpText.rectTransform, Input.mousePosition, _camera))
		{
			Debug.Log("hovering text");
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		int linkIndex = TMP_TextUtilities.FindIntersectingLink(tmpText, Input.mousePosition, _camera);
		Debug.Log($"Click Index: {linkIndex}");

		if (linkIndex != -1)
		{
			TMP_LinkInfo linkInfo = tmpText.textInfo.linkInfo[linkIndex];
			onLinkClicked.Invoke(linkInfo.GetLinkID());
		}
	}
}
