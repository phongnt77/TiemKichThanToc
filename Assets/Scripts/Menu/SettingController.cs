using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public Button muteButton;

    public Button invisibleButton;

    public AudioSource backgroundSource;

    private bool previousMuteInteractable;

    private bool previousMuteBackgroundSound;

    private bool previousInvisibleButtonActive;

    private void Start()
    {
        previousMuteInteractable = muteButton.interactable;
        previousMuteBackgroundSound = backgroundSource.mute;
        previousInvisibleButtonActive = invisibleButton.gameObject.activeSelf;
    }
    public void HandleMuteButton()
    {
        muteButton.interactable = false;
        backgroundSource.mute = true;
    }
    public void HandleInvisibleButton()
    {
        muteButton.interactable = true;
        backgroundSource.mute = false;
    }
    public void HandleBackSettingButton()
    {
        muteButton.interactable = previousMuteInteractable;
        backgroundSource.mute = previousMuteBackgroundSound;
        invisibleButton.gameObject.SetActive(previousInvisibleButtonActive);
    }
    public void HandleSaveSettingButton()
    {
        previousMuteInteractable = muteButton.interactable;
        previousMuteBackgroundSound = backgroundSource.mute;
        previousInvisibleButtonActive = invisibleButton.gameObject.activeSelf;
    }
}
