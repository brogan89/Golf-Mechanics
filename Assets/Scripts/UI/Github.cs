using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Github : MonoBehaviour
{
	[SerializeField] private Text githubStars = null;

	private IEnumerator GetStarGazers()
	{
		// get star count
		var request = UnityWebRequest.Get("https://api.github.com/repos/brogan89/Golf-Mechanics/stargazers");
		yield return request.SendWebRequest();
		bool isError = request.isNetworkError || request.isHttpError;
		if (!isError)
		{
			JArray jArray = JArray.Parse(request.downloadHandler.text);
			Debug.Log($"We have {jArray.Count} Stargazers!");
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
			StartCoroutine(GetStarGazers());
		}
	}
}