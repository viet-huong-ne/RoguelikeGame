    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class HeroHealth : Singleton<HeroHealth>, IHealth
    {
        [SerializeField] public int health = 200;
        [SerializeField] public int MAX_HEALTH = 200;
        [SerializeField] private GameObject resultPanelPrefab; // Prefab của Panel
        private GameObject resultPanelInstance;
        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Timer timer;
        private GameObject healthBarInstance;
        private Image healthBarImage;  // Biến lưu trữ Image của thanh Health
        private SlashController slashController;
        private GarlicController garlicController;
        private KnifeController knifeController;
        [SerializeField] private float damageEffectDuration = 0.2f; // Thời gian hiệu ứng đỏ
        private SpriteRenderer spriteRenderer;
        private Color originalColor;
        private Animator animator;
        private HeroKnight heroKnight;
        private HeroAttack heroAttack; 
        private HealthBarBehavior healthBarBehavior;
        private bool isDead = false;
        private void Start()
        {
            // Lấy SpriteRenderer và lưu màu gốc
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>(); 
            heroKnight = GetComponent<HeroKnight>(); 
            heroAttack = GetComponent<HeroAttack>();
            // Nếu SlashController gắn vào GameObject khác (ví dụ, heroKnight)
            if (heroKnight != null)
            {
                // Giả sử SlashController được gắn vào GameObject của heroKnight
                slashController = heroKnight.GetComponentInChildren<SlashController>();
                garlicController = heroKnight.GetComponentInChildren<GarlicController>();
                knifeController = heroKnight.GetComponentInChildren<KnifeController>();
            }
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }

            // Instantiate the health bar and set its parent to the Canvas
            if (healthBarPrefab != null && canvas != null)
            {
                healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
                healthBarInstance.transform.SetParent(canvas.transform, false); // Set the parent of the health bar to Canvas

                // Lấy Image của thanh máu từ Prefab
                healthBarImage = healthBarInstance.transform.Find("Health").GetComponent<Image>(); // Lấy Image của phần "Health"

                // Gán target cho HealthBarFollow
                healthBarBehavior = healthBarInstance.GetComponent<HealthBarBehavior>();
                if (healthBarBehavior != null)
                {
                    healthBarBehavior.Target = transform;
                }

            }
            
            // Khởi tạo thanh máu đúng theo lượng máu ban đầu
            UpdateHealthBar();
        }

        private void Update()
        {
            // Cập nhật thanh máu mỗi frame
            UpdateHealthBar();
        }

        public int GetCurrentHealth(){
            return health;
        }

        private void UpdateHealthBar()
        {
            if (healthBarImage != null)
            {
                // Tính toán giá trị fillAmount dựa trên tỷ lệ máu
                healthBarImage.fillAmount = Mathf.Clamp((float)health / MAX_HEALTH, 0f, 1f);
            }
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0)
            {
                throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
            }

            // Kiểm tra nếu nhân vật đã chết, không nhận thêm sát thương
            if (health <= 0)
            {
                return;
            }
            
            SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Hurt"), 1f);

            health -= amount;

            // Gọi Die nếu máu <= 0
            if (health <= 0)
            {
                health = 0; // Đảm bảo không để giá trị âm
                Die();
                return; // Thoát khỏi hàm sau khi chết
            }

            // Kích hoạt hiệu ứng bị đánh
            if (spriteRenderer != null)
            {
                StartCoroutine(ShowDamageEffect());
            }

            // Cập nhật thanh máu ngay khi nhận sát thương
            UpdateHealthBar();
        }

        public void Heal(int amount)
        {
            if (amount < 0)
            {
                throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
            }

            bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

            if (wouldBeOverMaxHealth)
            {
                health = MAX_HEALTH;
            }
            else
            {
                health += amount;
            }

            // Cập nhật thanh máu khi hồi máu
            UpdateHealthBar();
        }

        private void Die()
        {
            heroKnight.SetSpeed(0);
            Debug.Log("I am dead!");
            SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/GameOver"), 1f);
            // Stop background music
            GameObject backgroundMusicObject = GameObject.Find("BackgroundMusicManager");
            if (backgroundMusicObject != null)
            {
                BackgroundMusicController musicController = backgroundMusicObject.GetComponent<BackgroundMusicController>();
                if (musicController != null)
                {
                    musicController.StopMusic();
                }
                else
                {
                    Debug.LogWarning("BackgroundMusicController component not found on BackgroundMusic object!");
                }
            }
            else
            {
                Debug.LogWarning("BackgroundMusic object not found in the scene!");
            }
            slashController?.ResetDamage();
            garlicController?.ResetDamage();
            knifeController?.ResetDamage();

            isDead = true;
            timer.StopTimer();

            if (animator != null)
            {
                animator.SetTrigger("Death");
            }

            if (heroKnight != null)
            {
                heroKnight.DisableMovement();
            }

            if (heroAttack != null)
            {
                heroAttack.DisableAttack();
            }

            StartCoroutine(WaitForDeathAnimation());
        }

        public bool IsDead() => isDead;

        private IEnumerator WaitForDeathAnimation()
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float animationDuration = stateInfo.length;

            yield return new WaitForSecondsRealtime(animationDuration + 1f);

            resultPanelInstance = Instantiate(resultPanelPrefab, FindObjectOfType<Canvas>().transform);
            Time.timeScale = 0f;
        }

        private IEnumerator ShowDamageEffect()
        {
            // Đổi màu nhân vật sang đỏ
            spriteRenderer.color = Color.red;

            // Chờ trong thời gian hiệu ứng
            yield return new WaitForSeconds(damageEffectDuration);

            // Khôi phục màu gốc
            spriteRenderer.color = originalColor;
        }
    }