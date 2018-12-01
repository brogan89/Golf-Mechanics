using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	private static Menu _instance;
	public static Menu Instance
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
	public const string DRIVING_RANGE = "DrivingRange";
	public const string MINI_PUTT = "MiniPutt";

	[Header("Buttons")]
	[SerializeField] private Button menuButton;
	[SerializeField] private Button drivingBtn;
	[SerializeField] private Button miniPuttBtn;
	[SerializeField] private Button cancelButton;

	[Header("Menu")]
	[SerializeField] private GameObject menu;

	private void Awake()
	{
		if (Instance != this)
			Destroy(this.gameObject);
	}

	private void Start()
	{
		canvas = GetComponent<Canvas>();
		menuButton.onClick.AddListener(() => SetMenu(true));
		drivingBtn.onClick.AddListener(() => SceneManager.LoadScene(DRIVING_RANGE));
		miniPuttBtn.onClick.AddListener(() => SceneManager.LoadScene(MINI_PUTT));
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

		// only show cencel button in other scenes other than first one
		cancelButton.gameObject.SetActive(curScene.buildIndex > 0);
		drivingBtn.gameObject.SetActive(curScene.name != DRIVING_RANGE);
		miniPuttBtn.gameObject.SetActive(curScene.name != MINI_PUTT);
	}

	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
		SetMenu(false);
	}
}