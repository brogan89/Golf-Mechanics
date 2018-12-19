using System.Runtime.InteropServices;
using UnityEngine;

public class LinkOpener : MonoBehaviour
{
	public void OpenURL(string url)
	{
		Debug.Log($"Opening link {url}. platform: {Application.platform}");

		if (Application.platform == RuntimePlatform.WebGLPlayer)
			openWindow(url);
		else
			Application.OpenURL(url);
	}

	[DllImport("__Internal")]
	private static extern void openWindow(string url);
}