using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCollisionHandler : MonoBehaviour
{
    private TilemapCollider2D tilemapCollider;

    void Start()
    {
        // Lấy Tilemap Collider 2D
        tilemapCollider = GetComponent<TilemapCollider2D>();
        if (tilemapCollider == null)
        {
            Debug.LogError("[TilemapCollisionHandler] Tilemap không có TilemapCollider2D!");
            return;
        }

        // Đảm bảo Collider không bật Is Trigger
        if (tilemapCollider.isTrigger)
        {
            Debug.LogWarning("[TilemapCollisionHandler] Đã tắt Is Trigger để xử lý va chạm vật lý.");
            tilemapCollider.isTrigger = false;
        }
    }

    // Xử lý va chạm với Player hoặc Quái
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("[TilemapCollisionHandler] Player đã va chạm với Tilemap.");
            HandlePlayerCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("[TilemapCollisionHandler] Quái đã va chạm với Tilemap.");
            HandleEnemyCollision(collision);
        }
    }

    // Hàm xử lý logic va chạm với Player
    private void HandlePlayerCollision(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Giữ Player ở ngoài Tilemap
            Vector2 pushBack = collision.contacts[0].normal; // Hướng đẩy
            rb.MovePosition(rb.position + pushBack * 0.1f); // Đẩy ra khỏi Tilemap một khoảng nhỏ
        }
    }

    // Hàm xử lý logic va chạm với Quái
    private void HandleEnemyCollision(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Giữ Quái ở ngoài Tilemap
            Vector2 pushBack = collision.contacts[0].normal; // Hướng đẩy
            rb.MovePosition(rb.position + pushBack * 0.1f); // Đẩy ra khỏi Tilemap một khoảng nhỏ
        }
    }
}
