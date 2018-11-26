using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class Club : ScriptableObject
{
	public enum ClubType
	{
		Driver,
		FairwayWood,
		Hybrid,
		Iron1,
		Iron2,
		Iron3,
		Iron4,
		Iron5,
		Iron6,
		Iron7,
		Iron8,
		Iron9,
		PitchingWedge,
		GapWedge,
		SandWedge,
		LobWedge,
		Putter
	}

	public ClubType type;
	[Tooltip("Angle of club in degrees")]
	public float loft;
	public float avgDistMin;
	public float avgDistMax;

	public override string ToString()
	{
		return JsonUtility.ToJson(this, true);
	}
}