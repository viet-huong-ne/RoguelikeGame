using UnityEngine;
[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject startingWeapon;
    public GameObject StartingWeapon { get => startingWeapon; set => startingWeapon = value; }

    [SerializeField]
    float maxhealth;
    public float Maxhealth { get => maxhealth; set => maxhealth = value; }
    [SerializeField]
    float recovery;
    public float Recovery { get => recovery; set => recovery = value; }
    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    [SerializeField]
    float might;
    public float Might { get => might; set => might = value; }
    [SerializeField]
    float projecttileSpeed;
    public float ProjecttileSpeed { get => projecttileSpeed; set => projecttileSpeed = value; }



}
