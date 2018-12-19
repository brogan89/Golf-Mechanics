using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Hyperlink : MonoBehaviour, IPointerClickHandler
{
	private TextMeshProUGUI tmpText;
	public List<string> linkInfo;

	private Camera _camera;

	private void Awake()
	{
		tmpText = GetComponent<TextMeshProUGUI>();
		_camera = Camera.main;
	}

	private void Update()
	{
		foreach (var info in tmpText.textInfo.linkInfo)
		{
			if (!linkInfo.Contains(info.GetLinkID()))
				linkInfo.Add(info.GetLinkID());
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		int linkIndex = TMP_TextUtilities.FindIntersectingLink(tmpText, Input.mousePosition, _camera);
		Debug.Log($"Click Index: {linkIndex}");

		if (linkIndex != -1)
		{
			TMP_LinkInfo linkInfo = tmpText.textInfo.linkInfo[linkIndex];
			Application.OpenURL(linkInfo.GetLinkID());
		}
	}
}
