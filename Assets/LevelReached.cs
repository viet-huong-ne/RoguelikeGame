using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class LevelReached : MonoBehaviour
{
    private TextMeshProUGUI levelReachedText;
    private int levelReached;
    [SerializeField]
    private HeroExperience heroExperience;

    void Start()
    {
        levelReachedText = GetComponent<TextMeshProUGUI>();
        if (levelReachedText != null)
        {
            levelReachedText.text = "0";
        }

        heroExperience = FindObjectOfType<HeroExperience>();
    }

    public void Update()
    {
        if (heroExperience != null)
        {
            levelReached = heroExperience.GetHeroLevel();

            if (levelReachedText != null)
            {
                levelReachedText.text = $"{levelReached}";
            }
        }
    }
}
