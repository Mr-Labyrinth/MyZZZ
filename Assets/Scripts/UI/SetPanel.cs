using UnityEngine;
using UnityEngine.UI;

public class SetPanel : MonoBehaviour
{
    public Slider slider;
    public Text valueView;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        int displayValue = Mathf.RoundToInt(value * 100);
        valueView.text = displayValue.ToString() + "%";
    }
}
