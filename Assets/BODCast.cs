using UnityEngine;
using System.Collections;

public class BODCast : MonoBehaviour
{
    [SerializeField] private GameObject deathSpellPrefab;
    [SerializeField] private float spawnOffsetY = 1.5f;
    [SerializeField] private float castAnimationDuration = 1.5f;
    [SerializeField] private int numberOfCasts = 3;
    [SerializeField] private float castInterval = 1f;
    [SerializeField] private float castCooldown = 7f;  // Time between automatic casts

    private Animator animator;
    private bool isCasting = false;
    public GameObject player;

    private BODMovement bodMovement;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        else
        {
            Debug.LogError("Player not found!");
        }

        bodMovement = GetComponent<BODMovement>();

        // Start the automatic casting after a delay
        StartCoroutine(AutoCastRoutine());
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            FinishCasting();
        }
    }

    // This coroutine handles the automatic casting every 15 seconds
    IEnumerator AutoCastRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(castCooldown);  // Wait for the cooldown period before casting
            if (!isCasting) // Only cast if not already casting
            {
                StartCoroutine(CastDeathSpellMultipleTimes());
            }
        }
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

    void SpawnDeathSpell()
    {
        Vector3 spawnPosition = player.transform.position + new Vector3(0, spawnOffsetY, 0);
        Instantiate(deathSpellPrefab, spawnPosition, Quaternion.identity);
    }

    void FinishCasting()
    {
        isCasting = false;

        if (bodMovement != null)
        {
            bodMovement.ResumeMovement();
        }
        else
        {
            Debug.LogWarning("BODMovement is missing!");
        }
    }
}
