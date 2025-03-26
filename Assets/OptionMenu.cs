using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    public GameObject OptionMenuPanel;
    
    public void PlayClickSound()
    {
        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Click"), 1f);
    }

    public void Continue()
    {
        OptionMenuPanel.SetActive(false);
    }

    public void OpenOption()
    {
        OptionMenuPanel.SetActive(true);
    }
}
