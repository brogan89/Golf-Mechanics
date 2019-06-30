using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RangeStatsPanel : MonoBehaviour
{
	[SerializeField] private Button closeButton = null;

	public Text distanceText;
	public Text heightText;
	public UnityAction onClose;

	private void Awake()
	{
		closeButton.onClick.AddListener(CloseButtonPressed);
	}

	public void ShowData(BallStats ballStats)
	{
		distanceText.text = $"Distance: {ballStats.distance:0.0}m";
		heightText.text = $"Height: {ballStats.height:0.0}m";
	}

	private void CloseButtonPressed()
	{
		gameObject.SetActive(false);
		onClose?.Invoke();
	}
}
