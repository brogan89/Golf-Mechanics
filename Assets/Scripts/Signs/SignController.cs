using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SignController : MonoBehaviour
{
	[Range(0, 20)]
	public int count = 1;
	public int interval = 25;
	public float width = 10;
	[SerializeField]
	private Sign signPrefab;

	public Stack<GameObject> pool = new Stack<GameObject>();

	// debug only
#pragma warning disable 414
	[SerializeField]
	private string[] _pool;
#pragma warning restore 414

	private void Start()
	{
		pool.Clear();
	}

	private void Update()
	{
		if (Application.isPlaying)
			return;

		if (count != pool.Count)
			AddOrRemoveSigns();
		else
			EnsureInterval();

		EnsureWidth();
	}

	private void EnsureInterval()
	{
		int i = 0;
		foreach (var item in pool)
		{
			int metre = (i + 1) * interval;
			item.name = metre + "m";
			item.transform.position = new Vector3(0, 0, metre);
			item.transform.GetChild(0).GetComponent<Sign>().SetText(metre.ToString());
			item.transform.GetChild(1).GetComponent<Sign>().SetText(metre.ToString());
			i++;
		}
	}

	private void EnsureWidth()
	{
		foreach (var item in pool)
		{
			if (item.transform.childCount == 2)
			{
				item.transform.GetChild(0).localPosition = new Vector3(-width, 0, 0);
				item.transform.GetChild(1).localPosition = new Vector3(width, 0, 0);
			}
		}
	}

	private void AddOrRemoveSigns()
	{
		// add
		if (count > pool.Count)
		{
			for (int i = 0; i < count; i++)
			{
				int metre = (i + 1) * interval;
				if (!Exists(metre))
				{
					GameObject go = new GameObject(metre + "m");
					go.transform.SetParent(this.transform);
					go.transform.position = new Vector3(0, 0, metre);
					print("adding " + go.name);

					if (signPrefab)
					{
						// left
						Sign leftSign = Instantiate(signPrefab, go.transform);
						leftSign.SetText(metre.ToString());

						// right
						Sign rightSign = Instantiate(signPrefab, go.transform);
						rightSign.SetText(metre.ToString());
					}

					pool.Push(go);
				}
			}
		}
		// remove
		else if (count < pool.Count)
		{
			for (int i = pool.Count; i > count; i--)
			{
				GameObject go = pool.Pop();
				print("removing " + go.name);
				DestroyImmediate(go);
			}
		}

		// debug
		_pool = pool.Select(x => x.name).ToArray();
	}

	private bool Exists(int m)
	{
		// exits in pool
		foreach (GameObject item in pool)
		{
			string str = item.name.Replace("m", "");
			int metre = int.Parse(str);
			if (metre == m)
				return true;
		}

		// check in scene 
		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject child = transform.GetChild(i).gameObject;
			string str = child.name.Replace("m", "");
			int metre = int.Parse(str);
			if (metre == m)
			{
				pool.Push(child);
				return true;
			}
		}

		return false;
	}
}