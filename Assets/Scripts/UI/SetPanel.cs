using UnityEngine;
using UnityEngine.UI;

public class SetPanel : MonoBehaviour
{
    public Slider mainSlider;
    public Slider musicSlider;
    public Slider soundSlider;
    public Text mainValueView;
    public Text musicValueView;
    public Text soundValueView;

    private void Start()
    {
        mainSlider.onValueChanged.AddListener(OnMainValueChanged);
        musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
        soundSlider.onValueChanged.AddListener(OnSoundValueChanged);
    }

    private void OnMainValueChanged(float value)
    {
        int displayValue = Mathf.RoundToInt(value * 100);
        mainValueView.text = displayValue.ToString() + "%";
    }

    private void OnMusicValueChanged(float value)
    {
        int displayValue = Mathf.RoundToInt(value * 100);
        musicValueView.text = displayValue.ToString() + "%";
    }

    private void OnSoundValueChanged(float value)
    {
        int displayValue = Mathf.RoundToInt(value * 100);
        soundValueView.text = displayValue.ToString() + "%";
    }
}
