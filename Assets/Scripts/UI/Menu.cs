using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	private static Menu _instance;

	private static Menu Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = FindObjectOfType<Menu>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	private Canvas canvas;
	private const string DRIVING_RANGE = "DrivingRange";
	private const string MINI_PUTT = "mini_hole_1";

	[Header("Buttons")]
	[SerializeField] private Button menuButton = null;
	[SerializeField] private Button drivingBtn = null;
	[SerializeField] private Button miniPuttBtn = null;
	[SerializeField] private Button cancelButton = null;

	[Header("Menu")]
	[SerializeField] private GameObject menu = null;

	private void Awake()
	{
		if (Instance != this)
			Destroy(gameObject);
	}

	private void Start()
	{
		canvas = GetComponent<Canvas>();
		menuButton.onClick.AddListener(() => SetMenu(true));
		drivingBtn.onClick.AddListener(LoadDrivingRange);
		miniPuttBtn.onClick.AddListener(LoadMiniPutt);
		cancelButton.onClick.AddListener(() => SetMenu(false));
		cancelButton.gameObject.SetActive(false);
		SceneManager.sceneLoaded += OnSceneLoaded;

		if (SceneManager.GetActiveScene().buildIndex == 0)
			SetMenu(true);
	}

	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		print($"Scene loaded: {arg0.name}");
		SetMenu(false);
	}

	private void SetMenu(bool active)
	{
		print($"Setting menu: {active}");

		// set menu active/deactive
		menu.SetActive(active);

		var rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();
		foreach (var obj in rootObjs)
		{
			var canvas = obj.GetComponent<Canvas>();
			if (canvas && canvas != this.canvas)
				canvas.gameObject.SetActive(!active);
		}

		// sho
		menuButton.gameObject.SetActive(!active);


		Scene curScene = SceneManager.GetActiveScene();

		// only show cancel button in other scenes other than first one
		cancelButton.gameObject.SetActive(curScene.buildIndex > 0);
		drivingBtn.gameObject.SetActive(curScene.name != DRIVING_RANGE);
		miniPuttBtn.gameObject.SetActive(curScene.name != MINI_PUTT);
	}

	private static void LoadDrivingRange()
	{
		if (MiniPuttController.Instance)
			Destroy(MiniPuttController.Instance.gameObject);
		
		SceneManager.LoadScene(DRIVING_RANGE);
	}

	private static void LoadMiniPutt()
	{
		SceneManager.LoadScene(MINI_PUTT);
	}
}