
using System;
using UnityEngine;

[Serializable]
public struct BallStats
{
	public float distance;
	public float height;

	public override string ToString()
	{
		return JsonUtility.ToJson(this, true);
	}
}