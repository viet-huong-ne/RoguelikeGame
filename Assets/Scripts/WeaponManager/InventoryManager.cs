using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6);
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int[] passiveItemLevels = new int[6];
    public List<Image> passiveItemUISlots = new List<Image>(6);
    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int passiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemData;
    }
    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text upgradeNameDisplay;
        public TMP_Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
        public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>(); //List of upgrade options for weapons
        public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>(); //List of upgrade options for passive items
        public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>(); //List of ui for upgrade options present in the scene
    //    Playerstats player;
    //    void Start()
    //    {
    //        player = GetComponent<PlayerStats>();
    //    }
    //}
    //public void Addweapon(int slotIndex, WeaponController weapon) //Add a weapon to a specific slot
    //{
    //    weaponSlots[slotIndex] = weapon;
    //    weaponLevels[slotIndex] = weapon.weaponData.Level;
    //    weaponUISlots[slotIndex].enabled = true;
    //    //Enable the image component
    //    weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;
    //    if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
    //    {
    //        GameManager.instance.EndLevelUp();
    //    }
    //    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem) //Add a passive item to a specific slot
    //    {
    //        passiveItemSlots[slotIndex] = passiveItem;
    //        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
    //        passiveItemUISlots[slotIndex].enabled = true; //Enable the image component
    //        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;
    //        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
    //        {
    //            GameManager.instance.EndLevelUp();
    public class WeaponUpgrade
    {
        public int weaponUpgradeIndex;
        public GameObject initialweapon;
        public WeaponScriptableObject weaponData;
    }



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    }
}
