using UnityEngine;

public class MiniPuttHole : MonoBehaviour
{
	[SerializeField] private GolfBall ball = null;
	[SerializeField] private Cup cup = null;
	
	public int holeNumber;
	public int par;

	private void Start()
	{
		MiniPuttController.Instance.SetHoleInfo(holeNumber, par, ball, cup);
	}
}