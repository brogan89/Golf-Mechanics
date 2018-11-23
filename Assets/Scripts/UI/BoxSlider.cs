using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BoxSlider : MonoBehaviour, IDragHandler
{
	public RectTransform handle;
	[SerializeField]
	private Vector2 _value = new Vector2();
	public Vector2 Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;

			// todo: set position as from -1 to +1 
			((RectTransform)transform).GetWorldCorners(corners);
			float x = Mathf.Lerp(corners[0].x, corners[2].x, value.x);
			float y = Mathf.Lerp(corners[0].y, corners[2].y, value.y);
			handle.position = new Vector2(x, y);

			onValueChanged.Invoke(value);
		}
	}

	// event
	[System.Serializable]
	public class SliderEvent : UnityEvent<Vector2> { }
	public SliderEvent onValueChanged = new SliderEvent();

	private Vector3[] corners = new Vector3[4];

	public void OnDrag(PointerEventData eventData)
	{
		((RectTransform)transform).GetWorldCorners(corners);
		Vector2 pos = eventData.position;
		pos.x = Mathf.Clamp(pos.x, corners[0].x, corners[2].x);
		pos.y = Mathf.Clamp(pos.y, corners[0].y, corners[2].y);

		// set pos
		handle.position = pos;

		// get values
		_value.x = Mathf.Clamp(GetPercentage(corners[2].x - corners[0].x, corners[2].x, handle.position.x), -1f, 1f);
		_value.y = Mathf.Clamp(GetPercentage(corners[2].y - corners[0].y, corners[2].y, handle.position.y), -1f, 1f);

		// event
		onValueChanged.Invoke(_value);
	}

	/// <summary>
	/// Returns percentage of input based on min and max values
	/// </summary>
	private float GetPercentage(float min, float max, float input)
	{
		return (input - min) / (max - min);
	}
}