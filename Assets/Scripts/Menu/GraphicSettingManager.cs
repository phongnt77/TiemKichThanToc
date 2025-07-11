using TMPro;
using UnityEngine;

public class GraphicSettingManager : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown;
    private int previousQualityLevel;
    void Start()
    {
        qualityDropdown.ClearOptions();
        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < QualitySettings.names.Length; i++)
        {
            options.Add(QualitySettings.names[i]);
        }

        qualityDropdown.AddOptions(options);

        int savedQuality = PlayerPrefs.GetInt("GraphicsQuality", QualitySettings.GetQualityLevel());
        qualityDropdown.value = savedQuality;
        previousQualityLevel = savedQuality;
        qualityDropdown.RefreshShownValue();
    }

    public void HandleWhenLoadIntoSetting()
    {
        qualityDropdown.value = previousQualityLevel;
    }


    public void HandleSaveButton()
    {
        PlayerPrefs.SetInt("GraphicsQuality", qualityDropdown.value);
        QualitySettings.SetQualityLevel(qualityDropdown.value);
        previousQualityLevel = qualityDropdown.value;
    }

    public void HandleBackButton()
    {
        PlayerPrefs.SetInt("GraphicsQuality", previousQualityLevel);
    }
}
