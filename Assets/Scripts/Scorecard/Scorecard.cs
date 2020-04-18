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

	public void SetScorecard(List<HoleData> holeData)
	{
		// SetPars(holeData.Select(x => x.par).ToList());
		// SetScore(holeData.Select(x => x.scores).ToList());

		for (int i = 0; i < 18; i++)
		{
			holes.SetHoleValue(i + 1, (i + 1).ToString());
			
			if (i < holeData.Count)
			{
				pars.SetHoleValue(i + 1, holeData[i].par.ToString());

				for (int j = 0; j < holeData[i].scores.Count; j++)
					players[j].SetHoleValue(i + 1, holeData[i].scores[j].ToString());
			}
			else
			{
				// set values to empty string
				pars.SetHoleValue(i + 1, string.Empty);

				foreach (var p in players)
					p.SetHoleValue(i + 1, string.Empty);
			}
		}
	}

	private void SetPars(List<int> parValues)
	{
		pars.SetLabel("Par");

		// front nine
		pars.SetOutText(parValues.Count >= 9 
			? parValues.GetRange(0, 8).Sum().ToString()
			: string.Empty);

		// back nine
		if (parValues.Count >= 18)
		{
			pars.SetInText(parValues.GetRange(9, 17).Sum().ToString());
			pars.SetTotalText(parValues.Sum().ToString());
		}
		else
		{
			pars.SetInText(string.Empty);
			pars.SetTotalText(string.Empty);
		}
		
		// create random par numbers for now
		pars.SetHolesText(parValues.Select(x => x.ToString()).ToArray());
	}

	private void SetScore(IReadOnlyList<List<int>> holeScores)
	{
		Debug.Log($"Scores count: {holeScores.Count}");
		
		for (int i = 0; i < holeScores.Count; i++)
		{
			for (int j = 0; j < holeScores[i].Count; j++)
			{
				players[j].SetLabel($"Player {j + 1}");
				players[j].SetHoleValue(i+ 1, holeScores[i][j].ToString());
				players[j].SetOutText("");
				players[j].SetInText("");
				players[j].SetTotalText("");
			}
		}
	}
}

