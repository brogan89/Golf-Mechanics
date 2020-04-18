using TMPro;
using UnityEngine;

public class ScoreRow : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _label = null;
	[SerializeField] private TextMeshProUGUI _out = null;
	[SerializeField] private TextMeshProUGUI _in = null;
	[SerializeField] private TextMeshProUGUI _tot = null;
	[SerializeField] private TextMeshProUGUI[] _holes = null;


	public void SetLabel(string text)
	{
		_label.text = text;
	}

	public void SetOutText(string text)
	{
		_out.text = text;
	}

	public void SetInText(string text)
	{
		_in.text = text;
	}

	public void SetTotalText(string text)
	{
		_tot.text = text;
	}

	public void SetHolesText(string[] values)
	{
		for (int i = 0; i < _holes.Length; i++)
			_holes[i].text = i < values.Length ? values[i] : string.Empty;
	}
	
	public void SetHoleValue(int hole, string value)
	{
		_holes[hole - 1].text = value;
	}
}