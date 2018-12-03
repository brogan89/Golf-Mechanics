using UnityEngine;
/*

	got loft and distance references from https://www.vaughns-1-pagers.com/sports/golf-club-data.htm

 */

[System.Serializable]
[CreateAssetMenu]
public class Club : ScriptableObject
{
	[Range(9f, 60f), Tooltip("Angle of club in degrees")]
	public float loft;
	[Tooltip("Avg min distance in metres")]
	public float avgDistMin;
	[Tooltip("Avg max distance in metres")]
	public float avgDistMax;

	public override string ToString()
	{
		return JsonUtility.ToJson(this, true);
	}
}