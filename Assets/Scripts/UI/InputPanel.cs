using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
        setGain("Look Orbit Y", -value);

    }

    private void OnHSValueChange(float value)
    {
        int displayValue = Mathf.RoundToInt(value * 100);
        HSTextView.text = displayValue.ToString() + "%";
        setGain("Look Orbit X", value);
    }

    private void setGain(string Name, float gain)
    {
        var controller = CameraManager.INSTANCE.Controller;
        for (int i = 0; i < controller.Controllers.Count; i++)
        {
            if (controller.Controllers[i].Name == Name)
            {
                controller.Controllers[i].Input.Gain = gain * 0.1f;
            }
        }
    }

}
