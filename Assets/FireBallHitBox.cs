using System.Collections;
using UnityEngine;

public class FireBallHitBox : MonoBehaviour
{
    [SerializeField] private int damageAmount = 50;

    private Cinemachine.CinemachineImpulseSource impulseSource;
    private Animator animator;
    private Rigidbody2D rb;

    [SerializeField] private LayerMask explosionLayers; // Lớp mà FireBall sẽ phát nổ khi va chạm

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D không được tìm thấy trên FireBall!");
        }

        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/FireBallMove"), 1f);

        // Tìm hoặc gắn impulse source
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        if (impulseSource == null)
        {
            Debug.LogError("CinemachineImpulseSource không được tìm thấy trên GameObject!");
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Kiểm tra nếu va chạm với người chơi
        if (col.CompareTag("Player"))
        {
            HeroHealth playerHealth = col.GetComponent<HeroHealth>();
            if (playerHealth != null)
            {
                Explode();
                playerHealth.TakeDamage(damageAmount);
            }
        }
        // Kiểm tra nếu va chạm với tilemap hoặc các đối tượng trong layer xác định
        else if (explosionLayers == (explosionLayers | (1 << col.gameObject.layer)))
        {
            Explode();
        }
    }

    private void Explode()
    {
        // Dừng chuyển động của FireBall
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Dừng chuyển động
            rb.isKinematic = true;     // Ngừng bị ảnh hưởng bởi các lực vật lý
        }

        // Bắt đầu hoạt ảnh nổ
        animator.SetTrigger("Explose");
        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/FireBallExplosion"), 1f);

        // Gọi rung màn hình
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }

        // Hủy đối tượng sau khi hoạt ảnh kết thúc (ví dụ: 0.49s)
        Destroy(gameObject, 0.49f);
    }
}
