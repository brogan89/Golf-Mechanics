using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Github : MonoBehaviour
{
	[SerializeField] private Button githubBtn;
	[SerializeField] private Text githubStars;

	IEnumerator Start()
	{
		githubBtn.onClick.AddListener(() => Application.OpenURL("https://github.com/brogan89/Golf-Mechanics"));

		// get star count
		var request = UnityWebRequest.Get("https://api.github.com/repos/brogan89/Golf-Mechanics/stargazers");
		request.SendWebRequest();
		yield return new WaitUntil(() => request.isDone);
		bool isError = request.isNetworkError || request.isHttpError;
		if (!isError)
		{
			JArray jArray = JArray.Parse(request.downloadHandler.text);
			print($"We have {jArray.Count} Stargazers!");
			githubStars.text = jArray.Count.ToString();
		}
		else
			Debug.LogError("Network Error");
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (focusStatus)
		{
			// check again when entering back into game
			StartCoroutine(Start());
		}
	}
}