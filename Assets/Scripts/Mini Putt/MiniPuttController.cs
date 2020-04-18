using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniPuttController : Singleton<MiniPuttController>
{
	[SerializeField] private int currentHole = 1;

	[Header("UI Controls")]
	[SerializeField] private PowerUpButton powerButton = null;
	[SerializeField] private Button resetSceneButton = null;
	[SerializeField] private Scorecard scorecard = null;

	private GolfBall ball;
	
	private int currentPlayer = 0; // placeholder for when multiplayer is added
	[SerializeField] private List<HoleData> holeData = new List<HoleData>();
	
	private void Start()
	{
		powerButton.onHit = HitBall;
		resetSceneButton.onClick.AddListener(ResetBallPosition);
		
		scorecard.gameObject.SetActive(false);
		scorecard.onContinuePressed.AddListener(() => StartCoroutine(GoToNextHole()));
	}

	public void SetHoleInfo(int holeNumber, int par, GolfBall ball, Cup cup)
	{
		currentHole = holeNumber;
		this.ball = ball;
		cup.onBallEnterCup.AddListener(ShowScoreCard);
		
		holeData.Add(new HoleData
		{
			holeNumber = holeNumber,
			par = par
		});
	}

	private void HitBall()
	{
		ball.SetLaunchAngle(0);
		ball.force = powerButton.power;
		ball.HitBall();

		var data = holeData.First(x => x.holeNumber == currentHole);
		data.InCreaseScore(currentPlayer);
	}
	
	private void ShowScoreCard()
	{
		// hide HUD
		powerButton.gameObject.SetActive(false);
		resetSceneButton.gameObject.SetActive(false);
		
		// set score
		scorecard.gameObject.SetActive(true);
		scorecard.SetScorecard(holeData);
	}

	#region Scene Loading

	private IEnumerator GoToNextHole()
	{
		Debug.Log("Go to next hole");
		
		scorecard.gameObject.SetActive(false);
		
		yield return SceneManager.LoadSceneAsync($"mini_hole_{currentHole + 1}");
		
		// show HUD
		powerButton.gameObject.SetActive(true);
		resetSceneButton.gameObject.SetActive(true);
	}
	
	private static void ResetBallPosition()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	#endregion
}

[Serializable]
public class HoleData
{
	public int holeNumber;
	public int par;
	public List<int> scores = new List<int>();

	public void InCreaseScore(int currentPlayer)
	{
		if (scores.Count == currentPlayer)
			scores.Add(1);
		else
			scores[currentPlayer] += 1;
	}
}