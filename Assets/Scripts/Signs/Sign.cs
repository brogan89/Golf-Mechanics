using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
	private TextMesh textMesh;

	private void Awake()
	{
		textMesh = GetComponentInChildren<TextMesh>();
	}

	private void Start()
	{
		SetText(transform.position.z.ToString());
	}

	public void SetText(string text)
	{
		if (!textMesh)
			Awake();

		textMesh.text = text;
	}
}