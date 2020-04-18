using UnityEngine;

namespace Helpers
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static T Instance { get; private set; }

		protected virtual void Awake()
		{
			if (!Instance)
			{
				Instance = GetComponent<T>();
				DontDestroyOnLoad(Instance);
			}
			else if (Instance != this)
			{
				Destroy(gameObject);
			}
		}
	}
}