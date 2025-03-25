using UnityEngine;

public class BarrelArea : MonoBehaviour
{
    [SerializeField] private GameObject portionPrefab; // Prefab của Portion
    [SerializeField] private GameObject enemyPrefab;   // Prefab của Enemy

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Attack"))
        {
            // Kiểm tra GameObject cha để thực hiện phá hủy
            if (transform.parent != null)
            {
                SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/BarrelBreak"), 1f);
                // Xử lý rớt đồ
                DropItem();

                // Phá hủy Barrel
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Debug.LogWarning("Không tìm thấy GameObject cha để phá hủy!");
            }
        }
    }

    private void DropItem()
    {
        // Lấy tham chiếu đến HeroHealth
        HeroHealth heroHealth = FindObjectOfType<HeroHealth>();
        if (heroHealth != null && heroHealth.GetCurrentHealth() < 100)
        {
            // Nếu máu Hero dưới 100, chắc chắn rớt Portion
            Instantiate(portionPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            // Random rớt Portion (80%) hoặc Enemy (20%)
            float dropChance = Random.Range(0f, 1f); // Random giá trị từ 0 đến 1
            if (dropChance <= 0.8f)
            {
                // Rớt Portion
                Instantiate(portionPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                // Rớt Enemy
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
