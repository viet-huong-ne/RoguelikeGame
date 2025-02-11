using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroLevel : MonoBehaviour
{
    public int currentLevel = 1;  // Cấp độ hiện tại của người chơi
    public int currentExperience = 0;  // Số EXP hiện tại của người chơi
    public int experienceToNextLevel = 100;  // Số EXP cần để lên cấp tiếp theo

    // Thêm EXP cho người chơi
    public void AddExperience(int amount)
    {
        currentExperience += amount;

        // Kiểm tra nếu người chơi đủ EXP để lên cấp
        if (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    // Tăng cấp độ của người chơi
    private void LevelUp()
    {
        currentLevel++;  // Tăng cấp
        currentExperience -= experienceToNextLevel;  // Trừ EXP đã tiêu tốn cho cấp hiện tại

        // Mỗi khi người chơi lên cấp, tăng số EXP yêu cầu cho cấp tiếp theo
        experienceToNextLevel = Mathf.FloorToInt(experienceToNextLevel * 1.2f); // Tăng 20% số EXP yêu cầu

        Debug.Log("Leveled up! Current Level: " + currentLevel);
    }
}
