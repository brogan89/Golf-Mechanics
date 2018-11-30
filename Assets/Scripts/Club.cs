using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class Club : ScriptableObject
{
	[Tooltip("Angle of club in degrees")]
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