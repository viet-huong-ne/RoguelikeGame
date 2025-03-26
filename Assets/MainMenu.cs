using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("FieldScene");
    }

    public void ExitGame()
    {
        Application.Quit();

    }

    public void PlayClickSound()
    {
        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Click"), 1f);
    }
}
