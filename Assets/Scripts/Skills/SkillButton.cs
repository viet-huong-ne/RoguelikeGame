using UnityEngine;
using UnityEngine.UI;
using TMPro; // Thêm namespace TextMeshPro

public class SkillButton : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillDescriptionText;
    [SerializeField] private Button button;
    
    private SkillScriptableObject skillData;
    private SkillSelectionManager manager;

    public void Setup(SkillScriptableObject skill, SkillSelectionManager selectionManager)
    {
        if (skill == null || selectionManager == null)
        {
            Debug.LogError("Skill or SelectionManager is null.");
            return;
        }
        skillData = skill;
        manager = selectionManager;

        // Gán thông tin kỹ năng lên nút
        skillIcon.sprite = skill.skillIcon;
        skillNameText.text = skill.skillName;
        skillDescriptionText.text = skill.skillDescription;

        // Kích hoạt nút và gán sự kiện bấm
        button.interactable = true;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => manager.SelectSkill(skillData));
    }

    public void Clear()
    {
        // Xóa thông tin kỹ năng khỏi nút
        skillIcon.sprite = null;
        skillNameText.text = "Unavailable";
        skillDescriptionText.text = "";
        button.interactable = false;
        button.onClick.RemoveAllListeners();
    }
}
