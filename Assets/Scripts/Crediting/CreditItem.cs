using System.Collections.Generic;
using System.Linq;
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
		//SetText();
	}

	private void SetText()
	{
		creditText?.SetText($"<link={link}>\"<u>{item}</u>\"</b></link> by <b>{author}</b> is licensed under <i>{license}</i>");
	}

	private List<Color32[]> SetLinkToColor(int linkIndex, Color32 color)
	{
		TMP_LinkInfo linkInfo = creditText.textInfo.linkInfo[linkIndex];

		var oldVertColors = new List<Color32[]>(); // store the old character colors

		for (int i = 0; i < linkInfo.linkTextLength; i++)
		{ // for each character in the link string
			int characterIndex = linkInfo.linkTextfirstCharacterIndex + i; // the character index into the entire text
			var charInfo = creditText.textInfo.characterInfo[characterIndex];
			int meshIndex = charInfo.materialReferenceIndex; // Get the index of the material / sub text object used by this character.
			int vertexIndex = charInfo.vertexIndex; // Get the index of the first vertex of this character.

			Color32[] vertexColors = creditText.textInfo.meshInfo[meshIndex].colors32; // the colors for this character
			oldVertColors.Add(vertexColors.ToArray());

			if (charInfo.isVisible)
			{
				vertexColors[vertexIndex + 0] = color;
				vertexColors[vertexIndex + 1] = color;
				vertexColors[vertexIndex + 2] = color;
				vertexColors[vertexIndex + 3] = color;
			}
		}

		// Update Geometry
		creditText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

		return oldVertColors;
	}
}