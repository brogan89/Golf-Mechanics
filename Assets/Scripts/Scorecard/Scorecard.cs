using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Scorecard : MonoBehaviour
{
	[SerializeField] private Button continueButton = null;
	[SerializeField] private ScoreRow holes = null;
	[SerializeField] private ScoreRow pars = null;
	[SerializeField] private ScoreRow[] players = null;
	private ScoreRow playerRef; // reference for player row, used as prefab
	
	public UnityEvent onContinuePressed;
	
	private void Start()
	{
		SetHolesLabels();
		playerRef = players[0];
		
		continueButton.onClick.AddListener(onContinuePressed.Invoke);
	}

	private void SetHolesLabels()
	{
		holes.SetLabel("Hole");
		holes.SetOutText("Out");
		holes.SetInText("In");
		holes.SetTotalText("Tot");
	}

	public void SetScorecard(Dictionary<int, HoleData> holeData)
	{
		Debug.Log($"Settings Scoreboard: {holeData.ToArrayString()}");
		
		for (int i = 0; i < 18; i++)
		{
			var holeNum = i + 1;
			holes.SetHoleValue(holeNum, holeNum.ToString());
			
			if (holeData.TryGetValue(holeNum, out var data))
			{
				// pars
				pars.SetHoleValue(holeNum, data.par.ToString());

				for (int j = 0; j < data.scores.Count; j++)
					players[j].SetHoleValue(holeNum, data.scores[j].ToString());
			}
			else
			{
				// set values to empty string
				pars.SetHoleValue(holeNum, string.Empty);

				foreach (var p in players)
					p.SetHoleValue(holeNum, string.Empty);
			}
		}
		
		// set totals
		pars.SetLabel("Par");
		SetParLabelsAndTotals(holeData.Values.Select(x => x.par).ToList());
		SetPlayersLabelsAndTotals(holeData);
	}

	private void SetParLabelsAndTotals(List<int> parValues)
	{
		pars.SetOutText(GetFront9Score(parValues).ToString());
		pars.SetInText(GetBack9Score(parValues).ToString());
		pars.SetTotalText(GetTotalScore(parValues).ToString());
	}

	private void SetPlayersLabelsAndTotals(IReadOnlyDictionary<int, HoleData> holeData)
	{
		for (int i = 0; i < players.Length; i++)
		{
			players[i].SetLabel($"Player {i + 1}");

			var playerScores = holeData.Values.Select(x => x.scores[i]).ToList();
			players[i].SetOutText(GetFront9Score(playerScores).ToString());
			players[i].SetInText(GetBack9Score(playerScores).ToString());
			players[i].SetTotalText(GetTotalScore(playerScores).ToString());
		}
	}
	
	private static int GetFront9Score(List<int> scores)
	{
		return scores.GetRange(0, Mathf.Min(8, scores.Count)).Sum();
	}
	
	private static int GetBack9Score(List<int> scores)
	{
		return scores.Count > 9 ? scores.GetRange(9, scores.Count - 1).Sum() : 0;
	}
	
	private static int GetTotalScore(List<int> scores)
	{
		return GetFront9Score(scores) + GetBack9Score(scores);
	}
}

