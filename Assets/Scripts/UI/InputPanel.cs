using UnityEngine;
using UnityEngine.UI;

public class InputPanel : MonoBehaviour
{
    [SerializeField]
    private Slider verticalSensitivity;
    [SerializeField]
    private Slider horizontalSensitivity;
    [SerializeField]
    private Text VSTextView;
    [SerializeField]
    private Text HSTextView;

    private void Start()
    {
        verticalSensitivity.onValueChanged.AddListener(OnVSValueChange);
        horizontalSensitivity.onValueChanged.AddListener(OnHSValueChange);
    }

    private void OnVSValueChange(float value)
    {
        int displayValue = Mathf.RoundToInt(value * 100);
        VSTextView.text = displayValue.ToString() + "%";
    }

    private void OnHSValueChange(float value)
    {
        int displayValue = Mathf.RoundToInt(value * 100);
        HSTextView.text = displayValue.ToString() + "%";
    }


}
