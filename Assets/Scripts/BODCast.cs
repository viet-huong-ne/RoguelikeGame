using UnityEngine;
using System.Collections;

public class BODCast : MonoBehaviour
{
    [SerializeField] private GameObject deathSpellPrefab;
    [SerializeField] private GameObject monsterPrefab; // Prefab của monster
    [SerializeField] private GameObject bigDeathSpellPrefab;
    [SerializeField] private float spawnOffsetY = 1.5f;
    [SerializeField] private float castAnimationDuration = 1.5f;
    [SerializeField] private int numberOfCasts = 3;
    [SerializeField] private float castInterval = 1f;
    [SerializeField] private float castDeathSpellsCooldown = 7f;
    [SerializeField] private float castBIGDeathSpellsCooldown = 30f;
    [SerializeField] private float monsterSpawnInterval = 20f; // Interval for spawning monsters
    [SerializeField] private float spawnDistance = 2.5f;

    private Animator animator;
    private bool isCasting = false;
    private Transform player;

    private BODMovement bodMovement;

    void Start()
    {
        animator = GetComponent<Animator>();

        HeroKnight heroKnight = FindObjectOfType<HeroKnight>();
        if (heroKnight != null)
        {
            player = heroKnight.transform;
        }
        else
        {
            Debug.LogError("No HeroKnight found in the scene.");
        }

        bodMovement = GetComponent<BODMovement>();

        StartCoroutine(AutoCastDeathSpellRoutine());
        StartCoroutine(AutoSummonSpellRoutine());
        StartCoroutine(AutoCastBIGDeathSpellRoutine());
    }

    void Update()
    {
        if (isCasting)
        {
            bodMovement.StopMovement();

            // Kiểm tra nếu animation "Cast" đã hoàn thành
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Cast") && stateInfo.normalizedTime >= 1f)
            {
                FinishCasting();
            }
            return;
        }
    }

    // This coroutine handles the automatic casting every `castCooldown` seconds
    IEnumerator AutoCastDeathSpellRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(castDeathSpellsCooldown);
            if (!isCasting)
            {
                StartCoroutine(CastDeathSpellMultipleTimes());
            }
        }
    }

    IEnumerator AutoSummonSpellRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(monsterSpawnInterval);
            if (!isCasting)
            {
                StartCoroutine(CastSummonSpell());
            }
        }
    }

    IEnumerator AutoCastBIGDeathSpellRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(castBIGDeathSpellsCooldown);
            if (!isCasting)
            {
                StartCoroutine(CastBIGDeathSpellMultipleTimes());
            }
        }
    }

    IEnumerator CastBIGDeathSpellMultipleTimes()
    {
        isCasting = true;

        if (bodMovement != null)
        {
            bodMovement.StopMovement();
        }
        else
        {
            Debug.LogWarning("BODMovement is missing!");
        }

        animator.SetTrigger("Cast");
        yield return new WaitForSeconds(castAnimationDuration);

        SpawnBIGDeathSpell();

        // Check if the animation is done
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Cast"))
        {
            yield return null;
        }

        FinishCasting();
    }

    IEnumerator CastDeathSpellMultipleTimes()
    {
        isCasting = true;

        // Stop movement during casting
        if (bodMovement != null)
        {
            bodMovement.StopMovement();
        }
        else
        {
            Debug.LogWarning("BODMovement is missing!");
        }

        animator.SetTrigger("Cast");

        for (int i = 0; i < numberOfCasts; i++)
        {
            SpawnDeathSpell();

            yield return new WaitForSeconds(castInterval);
        }

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Cast"))
        {
            yield return null;
        }

        FinishCasting();
    }

    IEnumerator CastSummonSpell()
    {
        isCasting = true;

        if (bodMovement != null)
        {
            bodMovement.StopMovement();
        }
        else
        {
            Debug.LogWarning("BODMovement is missing!");
        }

        animator.SetTrigger("Cast");
        yield return new WaitForSeconds(castAnimationDuration);

        SummonMonstesAroundPlayer();

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Cast"))
        {
            yield return null;
        }

        FinishCasting();
    }

    void SpawnDeathSpell()
    {
        Vector3 spawnPosition = player.transform.position + new Vector3(0, spawnOffsetY, 0);
        Instantiate(deathSpellPrefab, spawnPosition, Quaternion.identity);
    }

    void SpawnBIGDeathSpell()
    {
        Vector3 spawnPosition = player.transform.position + new Vector3(0, spawnOffsetY + 3, 0);
        Instantiate(bigDeathSpellPrefab, spawnPosition, Quaternion.identity);
    }

    void SummonMonstesAroundPlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned.");
            return;
        }

        if (monsterPrefab == null)
        {
            Debug.LogWarning("Monster prefab is not assigned.");
            return;
        }
        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/SummonSpell"), 1f);
        Vector3[] spawnOffsets = new Vector3[]
        {
            new Vector3(spawnDistance, spawnDistance, 0),    // Chéo trên phải
            new Vector3(-spawnDistance, spawnDistance, 0),   // Chéo trên trái
            new Vector3(spawnDistance, -spawnDistance, 0),   // Chéo dưới phải
            new Vector3(-spawnDistance, -spawnDistance, 0)   // Chéo dưới trái
        };

        foreach (Vector3 offset in spawnOffsets)
        {
            Vector3 spawnPosition = player.transform.position + offset;

            Collider2D hitCollider = Physics2D.OverlapCircle(spawnPosition, 0.5f, LayerMask.GetMask("Object"));
            if (hitCollider == null)
            {
                // Instantiate the monsterPrefab
                Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
                Debug.Log("Spawned a monster at: " + spawnPosition);
            }
            else
            {
                Debug.Log("Skipped spawning at position due to collision: " + spawnPosition);
            }
        }
    }

    void FinishCasting()
    {
        if (!isCasting) return;

        isCasting = false;

        if (bodMovement != null)
        {
            bodMovement.ResumeMovement();
        }
        else
        {
            Debug.LogWarning("BODMovement is missing!");
        }

        Debug.Log("Casting finished. Movement resumed.");
    }
}
