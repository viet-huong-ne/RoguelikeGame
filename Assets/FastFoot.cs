using System.Collections;
using UnityEngine;

public class FastFoot : MonoBehaviour
{
    public float speedBoost = 4f;  // Tốc độ tăng thêm khi nhấn phím Space
    public float boostDuration = 0.1f;  // Thời gian kéo dài tăng tốc
    public float decelerationTime = 0.5f;  // Thời gian giảm tốc dần về ban đầu
    public float cooldownTime = 10f;  // Thời gian hồi chiêu (10 giây)

    private float currentSpeed;  // Lưu trữ tốc độ hiện tại
    private bool isBoosting = false;  // Kiểm tra xem boost có đang chạy không
    private bool isCooldown = false;  // Kiểm tra xem kỹ năng có đang trong thời gian hồi chiêu không

    private Rigidbody2D rb;  // Sử dụng Rigidbody2D để điều khiển vận tốc nếu HeroKnight sử dụng 2D
    private HeroKnight heroKnight;  // Tham chiếu đến HeroKnight để thay đổi tốc độ

    // Tham chiếu đến script Cooldown để đồng bộ UI
    public Cooldown cooldownUI;
    private AudioClip dashSoundEffect; // Biến lưu âm thanh Dash
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Lấy Rigidbody2D của object
        heroKnight = GetComponentInParent<HeroKnight>();  // Lấy tham chiếu đến HeroKnight (cha của đối tượng này)

        // Lấy tốc độ ban đầu từ HeroKnight
        currentSpeed = heroKnight.speed;

        cooldownUI = FindObjectOfType<Cooldown>();
        // Tải trước âm thanh Dash
        dashSoundEffect = Resources.Load<AudioClip>("SoundEffects/Dash");
        if (dashSoundEffect == null)
        {
            Debug.LogError("Không tìm thấy âm thanh Dash trong Resources/SoundEffects!");
        }
    }

    void Update()
    {
        // Kiểm tra nếu kỹ năng đang trong cooldown, nếu có thì không cho phép sử dụng
        if (isCooldown || isBoosting) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Dash"), 1f);
            // Khi ấn phím Space và chưa kích hoạt tăng tốc và không trong cooldown
            StartCoroutine(BoostSpeed());
        }
    }

    private IEnumerator BoostSpeed()
    {
        isBoosting = true;

        // Lưu lại tốc độ ban đầu
        float originalSpeed = heroKnight.speed;

        // Tăng tốc độ mạnh mẽ ngay lập tức
        currentSpeed = originalSpeed + speedBoost;
        heroKnight.SetSpeed(currentSpeed);  // Cập nhật tốc độ của HeroKnight

        // Duy trì tốc độ tăng trong thời gian boostDuration
        yield return new WaitForSeconds(boostDuration);

        // Giảm dần tốc độ về mức ban đầu
        float elapsedTime = 0f;
        while (elapsedTime < decelerationTime)
        {
            // Tính toán tốc độ giảm dần từ currentSpeed về originalSpeed
            currentSpeed = Mathf.Lerp(originalSpeed + speedBoost, originalSpeed, elapsedTime / decelerationTime);
            heroKnight.SetSpeed(currentSpeed);  // Cập nhật lại tốc độ của HeroKnight

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo tốc độ đã giảm về mức ban đầu
        currentSpeed = originalSpeed;
        heroKnight.SetSpeed(currentSpeed);  // Gán lại tốc độ ban đầu của HeroKnight

        // Kích hoạt thời gian hồi chiêu và đồng bộ UI
        StartCoroutine(Cooldown());

        isBoosting = false;
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true;  // Kích hoạt thời gian hồi chiêu
        cooldownUI.StartCooldown(cooldownTime);  // Gọi phương thức từ Cooldown script để bắt đầu đếm thời gian UI
        yield return new WaitForSeconds(cooldownTime);  // Đợi đến khi hết thời gian hồi chiêu
        isCooldown = false;  // Kết thúc thời gian hồi chiêu
    }
}
