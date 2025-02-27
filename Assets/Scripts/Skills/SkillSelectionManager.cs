using UnityEngine;

public class SkillSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject skillSelectionPanelPrefab; // Prefab của Panel
    private GameObject skillSelectionPanelInstance;

    private bool isSkillSelectionActive = false;

    public void ShowSkillSelectionPanel()
    {
        if (isSkillSelectionActive)
        {
            Debug.Log("Skill selection panel is already active.");
            return;
        }

        // Tạm dừng game
        Time.timeScale = 0f;

        // Tạo panel chọn kỹ năng
        skillSelectionPanelInstance = Instantiate(skillSelectionPanelPrefab, FindObjectOfType<Canvas>().transform);
        isSkillSelectionActive = true;

        Debug.Log("Skill selection panel created successfully.");

        // Lấy các nút SkillButton trong panel
        SkillButton[] skillButtons = skillSelectionPanelInstance.GetComponentsInChildren<SkillButton>();
        Debug.Log($"Found {skillButtons.Length} skill buttons.");
        // Debug tên từng SkillButton
        foreach (SkillButton button in skillButtons)
        {
            Debug.Log($"SkillButton found: {button.gameObject.name}");
        }

        if (skillButtons == null || skillButtons.Length != 3)
        {
            Debug.LogError($"Skill selection panel must have exactly 3 skill buttons. Found: {skillButtons?.Length ?? 0}");
            return;
        }

        // Lấy danh sách kỹ năng từ SkillManager
        int skillCount = SkillManager.Instance.GetSkillCount();

        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < skillCount)
            {
                // Gán dữ liệu kỹ năng
                var skill = SkillManager.Instance.GetSkill(i);
                skillButtons[i].Setup(skill, this);
                Debug.Log($"Skill button {i} set up with skill: {skill.skillName}");
            }
            else
            {
                // Không đủ kỹ năng, nút dư thừa sẽ bị vô hiệu hóa
                skillButtons[i].Clear();
                Debug.Log($"Skill button {i} has no skill assigned.");
            }
        }
    }

    public void HideSkillSelectionPanel()
    {
        if (skillSelectionPanelInstance != null)
        {
            Destroy(skillSelectionPanelInstance);
        }

        // Tiếp tục game
        Time.timeScale = 1f;
        isSkillSelectionActive = false;
    }

    public void SelectSkill(SkillScriptableObject skill)
    {
        Debug.Log($"Selected skill: {skill.skillName}");
        // Logic khi chọn kỹ năng, ví dụ: thêm kỹ năng cho nhân vật
        
        // Đóng Panel
        HideSkillSelectionPanel();
    }
}
