using TMPro;
using UnityEngine;

public class CreditItem : MonoBehaviour
{
	public TextMeshProUGUI creditText;

	[Header("Info")]
	public string item;
	public string author;
	public string license;
	public string link;

	private void Start()
	{
		SetText();
	}

	private void OnValidate()
	{
		SetText();
	}

	private void SetText()
	{
		creditText?.SetText($"<link={link}>\"<u>{item}</u>\"</b></link> by <b>{author}</b> is licensed under <i>{license}</i>");
	}
}