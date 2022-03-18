using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour
{
    public static ItemInventory instance;

    private ItemLibrary library;

    //TRY TO KEEP PRIVATE!!
    private List<CharacterData> charactersOwned = new List<CharacterData>();
    private List<WeaponData> weaponsOwned = new List<WeaponData>();
    private List<HeadgearData> headgearsOwned = new List<HeadgearData>();
    private List<UtilityData> utilitiesOwned = new List<UtilityData>();

    [SerializeField] private Transform inventoryCanvas;
    [SerializeField] private Button equipButton, returnButton;
    private Text equipButtonText;

    #region Left UI
    private GameObject statusButton;
    private ScrollRect statusPanel, statsPanel;
    private Transform lorePanel;
    //status
    private Image status_WeaponIcon, status_HeadgearIcon, status_ApparelIcon, status_UtilityIcon;
    private Toggle status_Character, status_Weapon, status_Headgear, status_Apparel, status_Utility;
    //stats
    private Transform statsEquip, statsSelect;
    private Image statsEquip_Icon, statsSelect_Icon;
    private Text statsEquip_Name, statsSelect_Name;
    //lore
    private Image lore_Icon;
    private Text lore_Name, lore_Description;
    private Button levelUp;
    private Button limitBreak;
    #endregion

    #region Right UI
    private GameObject characterButton;
    private ToggleGroup itemPanel;
    private ScrollRect characterPanel, weaponPanel, headgearPanel, utilityPanel;
    #endregion

    [SerializeField] private GameObject slotPrefab;

    [Header("DefaultEquipments")]
    //[SerializeField] private Weapon defaultSword;

    //CharacterDisplay
    [SerializeField] private Animator displayModel;

    private CharacterData characterSelected;
    private Toggle slotSelected;

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        library = ItemLibrary.instance;

        //Left UI
        statusButton = inventoryCanvas.Find("InfoButton").GetChild(0).gameObject;

        statusPanel = inventoryCanvas.Find("InfoPanel").GetChild(0).GetComponent<ScrollRect>();
        status_WeaponIcon = statusPanel.content.GetChild(0).Find("Weapon_Icon").GetComponent<Image>();
        status_HeadgearIcon = statusPanel.content.GetChild(0).Find("Headgear_Icon").GetComponent<Image>();
        status_UtilityIcon = statusPanel.content.GetChild(0).Find("Utility_Icon").GetComponent<Image>();
        status_Character = statusPanel.content.Find("Character").GetChild(0).GetComponent<Toggle>();
        status_Weapon = statusPanel.content.Find("Weapon").GetChild(0).GetComponent<Toggle>();
        status_Headgear = statusPanel.content.Find("Headgear").GetChild(0).GetComponent<Toggle>();
        status_Utility = statusPanel.content.Find("Utility").GetChild(0).GetComponent<Toggle>();

        statsPanel = inventoryCanvas.Find("InfoPanel").GetChild(1).GetComponent<ScrollRect>();
        statsEquip = statsPanel.content.GetChild(0);
        statsEquip_Icon = statsEquip.GetChild(0).GetComponent<Image>();
        statsEquip_Name = statsEquip.GetChild(1).GetComponent<Text>();
        statsSelect = statsPanel.content.GetChild(1);
        statsSelect_Icon = statsSelect.GetChild(0).GetComponent<Image>();
        statsSelect_Name = statsSelect.GetChild(1).GetComponent<Text>();

        lorePanel = inventoryCanvas.Find("InfoPanel").GetChild(2);
        lore_Icon = lorePanel.GetChild(0).GetComponent<Image>();
        lore_Name = lorePanel.GetChild(1).GetComponent<Text>();
        lore_Description = lorePanel.GetChild(2).GetComponent<Text>();
        //levelUp = infoPanel.Find("Level Up").GetComponent<Button>();
        //limitBreak = infoPanel.Find("Limit Break").GetComponent<Button>();

        //Right UI
        characterButton = inventoryCanvas.Find("ItemButton").GetChild(0).gameObject;

        itemPanel = inventoryCanvas.Find("ItemPanel").GetComponent<ToggleGroup>();
        characterPanel = itemPanel.transform.GetChild(0).GetComponent<ScrollRect>();
        weaponPanel = itemPanel.transform.GetChild(1).GetComponent<ScrollRect>();
        headgearPanel = itemPanel.transform.GetChild(2).GetComponent<ScrollRect>();
        utilityPanel = itemPanel.transform.GetChild(4).GetComponent<ScrollRect>();

        equipButtonText = equipButton.transform.GetChild(0).GetComponent<Text>();

        LoadInventory();
        RefreshInventory();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Debug.Log(charactersOwned.Count);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            itemPanel.EnsureValidState();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            RefreshInventory();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            FormatInventory();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            SaveInventory();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { 
            LoadInventory();
        }
    }

    private void RefreshInventory() {
        foreach (Transform child in characterPanel.content)
            Destroy(child.gameObject);
        foreach (Transform child in weaponPanel.content)
            Destroy(child.gameObject);
        foreach (Transform child in headgearPanel.content)
            Destroy(child.gameObject);
        foreach (Transform child in utilityPanel.content)
            Destroy(child.gameObject);

        foreach (CharacterData characterOwned in charactersOwned) {
            SetupSlot(Instantiate(slotPrefab, characterPanel.content).GetComponent<Toggle>(), characterOwned, itemPanel);
        }
        foreach (WeaponData weaponOwned in weaponsOwned) {
            SetupSlot(Instantiate(slotPrefab, weaponPanel.content).GetComponent<Toggle>(), weaponOwned, itemPanel);
        }
        foreach (HeadgearData headgearOwned in headgearsOwned) {
            SetupSlot(Instantiate(slotPrefab, headgearPanel.content).GetComponent<Toggle>(), headgearOwned, itemPanel);
        }
        foreach (UtilityData utilityOwned in utilitiesOwned) {
            SetupSlot(Instantiate(slotPrefab, utilityPanel.content).GetComponent<Toggle>(), utilityOwned, itemPanel);
        }
    }

    #region SetupSlot
    private void SetupSlot(Toggle slot, CharacterData characterData, ToggleGroup itemToggleGroup) {
        Character character = (Character)library.items[characterData.name];
        #region SetActiveParts
        if (character.ItemSprite != null) {
            slot.transform.GetChild(1).GetComponent<Image>().sprite = character.ItemSprite;
            slot.transform.GetChild(1).gameObject.SetActive(true);
        }
        slot.transform.GetChild(2).GetComponent<Image>().sprite = library.heroClassIcons[(int)character.HeroClasses[0]];
        if (character.HeroClasses.Length < 2) {
            SetActiveComponents(slot.transform, true, false, true, true);
        } else {
            slot.transform.GetChild(3).GetComponent<Image>().sprite = library.heroClassIcons[(int)character.HeroClasses[1]];
            SetActiveComponents(slot.transform, true, true, true, true);
        }
        slot.transform.GetChild(4).GetComponent<Image>().sprite = library.elementIcons[(int)character.Element];
        slot.transform.GetChild(6).GetComponent<Text>().text = characterData.exp.ToString();
        #endregion
        if (itemToggleGroup != null) {
            characterData.slot = slot;
            characterData.equip = slot.transform.GetChild(7).GetComponent<Image>();
            characterData.character = character;
            slot.group = itemToggleGroup;
        } else slot.interactable = false;
        slot.onValueChanged.AddListener(delegate { ViewItem(slot, character, characterData); });
        slot.transform.GetChild(8).GetComponent<Button>().onClick.AddListener(delegate { EditCharacter(characterData); });
    }
    private void SetupSlot(Toggle slot, WeaponData weaponData, ToggleGroup itemToggleGroup) {
        Weapon weapon = (Weapon)library.items[weaponData.name];
        #region SetActiveParts
        if (weapon.ItemSprite != null) {
            slot.transform.GetChild(1).GetComponent<Image>().sprite = weapon.ItemSprite;
            slot.transform.GetChild(1).gameObject.SetActive(true);
        }
        slot.transform.GetChild(2).GetComponent<Image>().sprite = library.heroClassIcons[(int)weapon.HeroClass];
        SetActiveComponents(slot.transform, true, false, false, true);
        slot.transform.GetChild(6).GetComponent<Text>().text = weaponData.level.ToString();

        if (weaponData.character != null) slot.transform.GetChild(7).gameObject.SetActive(true);
        #endregion

        if (weaponData.character != null) {
            CharacterData characterData = weaponData.character as CharacterData;
            characterData.weaponData = weaponData;
        }

        if (itemToggleGroup != null) {
            weaponData.slot = slot;
            weaponData.equip = slot.transform.GetChild(7).GetComponent<Image>();
            weaponData.weapon = weapon;
            slot.group = itemToggleGroup;
        } else slot.interactable = false;
        slot.onValueChanged.AddListener(delegate { ViewItem(weaponData); });
    }
    private void SetupSlot(Toggle slot, HeadgearData headgearData, ToggleGroup itemToggleGroup) {
        Headgear headgear = (Headgear)library.items[headgearData.name];
        #region SetActiveParts
        if (headgear.ItemSprite != null) {
            slot.transform.GetChild(1).GetComponent<Image>().sprite = headgear.ItemSprite;
            slot.transform.GetChild(1).gameObject.SetActive(true);
        }
        slot.transform.GetChild(2).GetComponent<Image>().sprite = library.heroClassIcons[(int)headgear.HeroClasses[0]];
        if (headgear.HeroClasses.Length < 2) {
            SetActiveComponents(slot.transform, true, false, true, true);
        } else {
            transform.GetChild(3).GetComponent<Image>().sprite = library.heroClassIcons[(int)headgear.HeroClasses[1]];
            SetActiveComponents(slot.transform, true, true, true, true);
        }

        if (headgearData.character != null) slot.transform.GetChild(7).gameObject.SetActive(true);
        #endregion

        if (headgearData.character != null) {
            CharacterData characterData = headgearData.character as CharacterData;
            characterData.headgearData = headgearData;
        }

        if (itemToggleGroup != null) {
            headgearData.slot = slot;
            headgearData.equip = slot.transform.GetChild(7).GetComponent<Image>();
            headgearData.headgear = headgear;
            slot.group = itemToggleGroup;
        } else slot.interactable = false;
        slot.onValueChanged.AddListener(delegate { ViewItem(headgearData); });
    }
    private void SetupSlot(Toggle slot, UtilityData utilityData, ToggleGroup itemToggleGroup) {
        Utility utility = (Utility)library.items[utilityData.name];
        #region SetActiveParts
        if (utility.ItemSprite != null) {
            slot.transform.GetChild(1).GetComponent<Image>().sprite = utility.ItemSprite;
            slot.transform.GetChild(1).gameObject.SetActive(true);
        }
        slot.transform.GetChild(2).GetComponent<Image>().sprite = library.heroClassIcons[(int)utility.HeroClass];
        SetActiveComponents(slot.transform, true, false, false, false);

        if (utilityData.character != null) slot.transform.GetChild(7).gameObject.SetActive(true);
        #endregion

        if (utilityData.character != null) {
            CharacterData characterData = utilityData.character as CharacterData;
            characterData.utilityData = utilityData;
        }

        if (itemToggleGroup != null) {
            utilityData.slot = slot;
            utilityData.equip = slot.transform.GetChild(7).GetComponent<Image>();
            utilityData.utility = utility;
            slot.group = itemToggleGroup;
        } else slot.interactable = false;
        slot.onValueChanged.AddListener(delegate { ViewItem(utilityData); });
    }
    private void SetActiveComponents(Transform slot, bool heroClass0, bool heroClass1, bool element, bool limitBreak) {
        slot.GetChild(2).gameObject.SetActive(heroClass0);
        slot.GetChild(3).gameObject.SetActive(heroClass1);
        slot.GetChild(4).gameObject.SetActive(element);
        slot.GetChild(5).gameObject.SetActive(limitBreak);
    }
    #endregion

    #region ViewItem
    private void ViewItem(Toggle slot, Character character, CharacterData characterData) {
        if (slot.isOn) {
            //Left
            ViewRefresh(characterData);
            if (!statusButton.activeSelf) {
                statusButton.SetActive(true);
                statusPanel.gameObject.SetActive(true);
                statsPanel.gameObject.SetActive(false);
                lorePanel.gameObject.SetActive(false);
            }
            //Right
            slot.transform.GetChild(0).gameObject.SetActive(true);
            slot.transform.GetChild(8).gameObject.SetActive(true);

        } else {
            slot.transform.GetChild(0).gameObject.SetActive(false);
            slot.transform.GetChild(8).gameObject.SetActive(false);
        }
    }
    private void ViewRefresh(CharacterData characterData) {
        //display model
        AnimatorOverrideController anim = Instantiate(characterData.character.animator);
        //Weapon
        if (characterData.weaponData != null) anim["Template_Sword_0-0"] = characterData.weaponData.weapon.animations[0];
        else {
            Weapon weapon = library.items["Default sword"] as Weapon;
            anim["Template_Sword_0-0"] = weapon.animations[0];
        }
        //Headgear
        SpriteRenderer playerHead = displayModel.transform.Find("Head").GetComponent<SpriteRenderer>();
        Transform playerHeadgear = playerHead.transform.GetChild(0);
        if (characterData.headgearData != null) {
            playerHeadgear.gameObject.SetActive(true);
            if (characterData.headgearData.headgear.useMask) {
                playerHead.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                playerHeadgear.GetComponent<SpriteMask>().sprite = characterData.headgearData.headgear.spriteMask;
            } else playerHead.maskInteraction = SpriteMaskInteraction.None;
            anim["Template_Headgear_0"] = characterData.headgearData.headgear.animation;
        } else {
            playerHeadgear.gameObject.SetActive(false);
            playerHead.maskInteraction = SpriteMaskInteraction.None;
        }
        //Utility
        Transform playerUtility = displayModel.transform.Find("Arm_1").GetChild(0);
        if (characterData.utilityData != null) {
            anim["Template_Shield_0-0"] = characterData.utilityData.utility.animations[0];
            playerUtility.gameObject.SetActive(true);
        } else {
            playerUtility.gameObject.SetActive(false);
        }

        displayModel.runtimeAnimatorController = anim;

        #region Icon&Slot
        SetupSlot(status_Character, characterData, null);
        if (characterData.weaponData != null) { //Weapon
            status_WeaponIcon.sprite = characterData.weaponData.weapon.ItemSprite;
            SetupSlot(status_Weapon, characterData.weaponData, null);
        } else {
            status_WeaponIcon.sprite = library.items["Default sword"].ItemSprite;
            SetupSlot(status_Weapon, new WeaponData("Default sword"), null); 
        }
        if (characterData.headgearData != null) { //Headgear
            status_HeadgearIcon.sprite = characterData.headgearData.headgear.ItemSprite;
            status_HeadgearIcon.gameObject.SetActive(true);
            SetupSlot(status_Headgear, characterData.headgearData, null);
            status_Headgear.transform.parent.gameObject.SetActive(true);
        } else {
            status_HeadgearIcon.gameObject.SetActive(false);
            status_Headgear.transform.parent.gameObject.SetActive(false);
        }
        if (characterData.utilityData != null) { //Utility
            status_UtilityIcon.sprite = characterData.utilityData.utility.ItemSprite;
            status_UtilityIcon.gameObject.SetActive(true);
            SetupSlot(status_Utility, characterData.utilityData, null);
            status_Utility.transform.parent.gameObject.SetActive(true);
        } else {
            status_UtilityIcon.gameObject.SetActive(false);
            status_Utility.transform.parent.gameObject.SetActive(false);
        }
        #endregion

        statsSelect_Icon.sprite = characterData.character.ItemSprite;
        statsSelect_Name.text = characterData.character.Name;

        lore_Icon.sprite = characterData.character.ItemSprite;
        lore_Name.text = characterData.character.Name;
        lore_Description.text = characterData.character.Description;
    }
    private void ViewItem(WeaponData weaponData) {
        if (weaponData.slot.isOn) {
            //Left
            ViewRefresh(weaponData);

            //Right
            weaponData.slot.transform.GetChild(0).gameObject.SetActive(true);
        } else {
            weaponData.slot.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    private void ViewRefresh(WeaponData weaponData) {
        if (characterSelected == null) {
            if (statusPanel.gameObject.activeSelf) statsPanel.gameObject.SetActive(true);
            statusPanel.gameObject.SetActive(false);
            statusButton.SetActive(false);
            if (equipButton.gameObject.activeSelf) equipButton.gameObject.SetActive(false);
        } else {
            if (!equipButton.gameObject.activeSelf) equipButton.gameObject.SetActive(true);
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(delegate { EquipItem(weaponData); });
            if (characterSelected.weaponData != weaponData) equipButtonText.text = "EQUIP";
            else equipButtonText.text = "UNEQUIP";
        }

        statsSelect_Icon.sprite = weaponData.weapon.ItemSprite;
        statsSelect_Name.text = weaponData.weapon.Name;

        lore_Icon.sprite = weaponData.weapon.ItemSprite;
        lore_Name.text = weaponData.weapon.Name;
        lore_Description.text = weaponData.weapon.Description;
    }
    private void ViewItem(HeadgearData headgearData) {
        if (headgearData.slot.isOn) {
            ViewRefresh(headgearData);

            headgearData.slot.transform.GetChild(0).gameObject.SetActive(true);
        } else {
            headgearData.slot.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    private void ViewRefresh(HeadgearData headgearData) {
        if (characterSelected == null) {
            if (statusPanel.gameObject.activeSelf) statsPanel.gameObject.SetActive(true);
            statusPanel.gameObject.SetActive(false);
            statusButton.SetActive(false);
            if (equipButton.gameObject.activeSelf) equipButton.gameObject.SetActive(false);
        } else {
            if (!equipButton.gameObject.activeSelf) equipButton.gameObject.SetActive(true);
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(delegate { EquipItem(headgearData); });
            if (characterSelected.headgearData != headgearData) equipButtonText.text = "EQUIP";
            else equipButtonText.text = "UNEQUIP";
        }

        statsSelect_Icon.sprite = headgearData.headgear.ItemSprite;
        statsSelect_Name.text = headgearData.headgear.Name;

        lore_Icon.sprite = headgearData.headgear.ItemSprite;
        lore_Name.text = headgearData.headgear.Name;
        lore_Description.text = headgearData.headgear.Description;
    }
    private void ViewItem(UtilityData utilityData) {
        if (utilityData.slot.isOn) {
            ViewRefresh(utilityData);

            utilityData.slot.transform.GetChild(0).gameObject.SetActive(true);
        } else {
            utilityData.slot.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    private void ViewRefresh(UtilityData utilityData) {
        if (characterSelected == null) {
            if (statusPanel.gameObject.activeSelf) statsPanel.gameObject.SetActive(true);
            statusPanel.gameObject.SetActive(false);
            statusButton.SetActive(false);
            if (equipButton.gameObject.activeSelf) equipButton.gameObject.SetActive(false);
        } else {
            if (!equipButton.gameObject.activeSelf) equipButton.gameObject.SetActive(true);
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(delegate { EquipItem(utilityData); });
            if (characterSelected.utilityData != utilityData) equipButtonText.text = "EQUIP";
            else equipButtonText.text = "UNEQUIP";
        }

        statsSelect_Icon.sprite = utilityData.utility.ItemSprite;
        statsSelect_Name.text = utilityData.utility.Name;

        lore_Icon.sprite = utilityData.utility.ItemSprite;
        lore_Name.text = utilityData.utility.Name;
        lore_Description.text = utilityData.utility.Description;
    }
    #endregion

    private void EditCharacter(CharacterData characterData) {
        characterSelected = characterData;
        characterButton.SetActive(false);
        itemPanel.transform.GetChild(0).gameObject.SetActive(false);
        itemPanel.transform.GetChild(1).gameObject.SetActive(true);
        returnButton.gameObject.SetActive(true);

        if (characterData.weaponData != null) characterData.weaponData.equip.sprite = library.equipIcons[1];
        if (characterData.headgearData != null) characterData.headgearData.equip.sprite = library.equipIcons[1];
        if (characterData.utilityData != null) characterData.utilityData.equip.sprite = library.equipIcons[1];
    }
    public void QuitEditCharacter() {
        if (characterSelected.weaponData != null) characterSelected.weaponData.equip.sprite = library.equipIcons[0];
        if (characterSelected.headgearData != null) characterSelected.headgearData.equip.sprite = library.equipIcons[0];
        if (characterSelected.utilityData != null) characterSelected.utilityData.equip.sprite = library.equipIcons[0];

        characterSelected = null;
        characterButton.SetActive(true);
        foreach (Transform panel in itemPanel.transform)
            panel.gameObject.SetActive(false);
        characterPanel.gameObject.SetActive(true);
        equipButton.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);
    }

    #region Equip
    private void EquipItem(WeaponData weaponData) {
        if (weaponData.character == null) {//Equip
            if (characterSelected.weaponData != null) {
                characterSelected.weaponData.character = null;
                characterSelected.weaponData.slot.transform.GetChild(7).gameObject.SetActive(false);
            }
            weaponData.character = characterSelected;
            characterSelected.weaponData = weaponData;
            weaponData.slot.transform.GetChild(7).gameObject.SetActive(true);
            weaponData.equip.sprite = library.equipIcons[1];
            ViewRefresh(characterSelected);
            ViewRefresh(weaponData);
            SaveInventory();
        } else if (weaponData.character == characterSelected) {//Unequip
            weaponData.character = null;
            characterSelected.weaponData = null;
            weaponData.slot.transform.GetChild(7).gameObject.SetActive(false);
            ViewRefresh(characterSelected);
            ViewRefresh(weaponData);
            SaveInventory();
        } else {
            //pop warning msg
            Debug.Log("item currently equipped by " + weaponData.character.name);
        }
    }
    private void EquipItem(HeadgearData headgearData) {
        if (headgearData.character == null) {//Equip
            if (characterSelected.headgearData != null) {
                characterSelected.headgearData.character = null;
                characterSelected.headgearData.slot.transform.GetChild(7).gameObject.SetActive(false);
            }
            headgearData.character = characterSelected;
            characterSelected.headgearData = headgearData;
            headgearData.slot.transform.GetChild(7).gameObject.SetActive(true);
            headgearData.equip.sprite = library.equipIcons[1];
            ViewRefresh(characterSelected);
            ViewRefresh(headgearData);
            SaveInventory();
        } else if (headgearData.character == characterSelected) {
            headgearData.character = null;
            characterSelected.headgearData = null;
            headgearData.slot.transform.GetChild(7).gameObject.SetActive(false);
            ViewRefresh(characterSelected);
            ViewRefresh(headgearData);
            SaveInventory();
        } else {
            //pop warning msg
            Debug.Log("item currently equipped by " + headgearData.character.name);
        }
    }
    private void EquipItem(UtilityData utilityData) {
        if (utilityData.character == null) {//Equip
            if (characterSelected.utilityData != null) {
                characterSelected.utilityData.character = null;
                characterSelected.utilityData.slot.transform.GetChild(7).gameObject.SetActive(false);
            }
            utilityData.character = characterSelected;
            characterSelected.utilityData = utilityData;
            utilityData.slot.transform.GetChild(7).gameObject.SetActive(true);
            utilityData.equip.sprite = library.equipIcons[1];
            ViewRefresh(characterSelected);
            ViewRefresh(utilityData);
            SaveInventory();
        } else if (utilityData.character == characterSelected) {//Unequip
            utilityData.character = null;
            characterSelected.utilityData = null;
            utilityData.slot.transform.GetChild(7).gameObject.SetActive(false);
            ViewRefresh(characterSelected);
            ViewRefresh(utilityData);
            SaveInventory();
        } else {
            //pop warning msg
            Debug.Log("item currently equipped by " + utilityData.character.name);
        }
    }
    #endregion
    //levelUp.onClick.RemoveAllListeners();//USE DELEGATE!!
    //levelUp.onClick.AddListener(delegate { LevelUp(characterData, slot.transform.GetChild(5).GetComponent<Text>()); });
    //limitBreak.onClick.RemoveAllListeners();
    //limitBreak.onClick.AddListener(delegate { LimitBreak(characterData, slot.transform.GetChild(4).GetComponent<Image>()); });

    #region Enhance&LimitBreak
    private void LevelUp(CharacterData characterData, Text level) {
        characterData.exp += 5;
        level.text = characterData.exp.ToString();
    }
    private void LevelUp(WeaponData weaponData, Text level) {
        weaponData.level += 5;
        level.text = weaponData.level.ToString();
    }
    private void LevelUp(HeadgearData headgearData, Text level) {
        headgearData.level += 5;
        level.text = headgearData.level.ToString();
    }

    private void LimitBreak(CharacterData characterData, Image limit) {
        if (characterData.limitBreakState < 4) { 
            characterData.limitBreakState += 1;
            limit.sprite = library.limitBreakIcons[characterData.limitBreakState];
        }
    }
    #endregion

    public void AddItem(Item item) {
        if (item is Character)
            charactersOwned.Add(new CharacterData(item.Name));
        else if (item is Weapon)
            weaponsOwned.Add(new WeaponData(item.Name));
        else if (item is Headgear)
            headgearsOwned.Add(new HeadgearData(item.Name));
        else if (item is Utility)
            utilitiesOwned.Add(new UtilityData(item.Name));

        RefreshInventory();
    }

    #region SaveLoadFormat
    private void SaveInventory() {
        List<object> itemsOwned = new List<object> {
            charactersOwned,
            weaponsOwned,
            headgearsOwned,
            utilitiesOwned
        };
        SaveLoadHandler.Save(itemsOwned, "carton.egg");
    }
    private void LoadInventory() {
        List<object> itemsOwned = new List<object>();
        if (SaveLoadHandler.Load("carton.egg", ref itemsOwned)) {
            charactersOwned = (List<CharacterData>)itemsOwned[0];
            weaponsOwned = (List<WeaponData>)itemsOwned[1];
            headgearsOwned = (List<HeadgearData>)itemsOwned[2];
            utilitiesOwned = (List<UtilityData>)itemsOwned[3];
        } else {
            FormatInventory();
        }
    }
    private void FormatInventory() {
        charactersOwned = new List<CharacterData>();
        weaponsOwned = new List<WeaponData>();
        headgearsOwned = new List<HeadgearData>();
        utilitiesOwned = new List<UtilityData>();
        SaveInventory();
        Debug.Log("File formatted");
    }
    #endregion
}

#region ItemData
//Name is used to reference library as string
[System.Serializable]
internal class CharacterData : BaseData {
    internal int exp;
    internal int limitBreakState;
    [System.NonSerialized] internal Character character;
    [System.NonSerialized] internal WeaponData weaponData;
    [System.NonSerialized] internal HeadgearData headgearData;
    [System.NonSerialized] internal ApparelData apparelData;
    [System.NonSerialized] internal UtilityData utilityData;

    internal CharacterData(string name) {
        this.name = name;
    }
}
[System.Serializable]
internal class WeaponData : BaseData {
    internal int level;
    internal int limitBreakState;
    internal BaseData character;

    [System.NonSerialized] internal Weapon weapon;

    internal WeaponData(string name) {
        this.name = name;
        level = 1;
    }
}
[System.Serializable]
internal class HeadgearData : BaseData {
    internal int level;
    internal int limitBreakState;
    internal BaseData character;

    [System.NonSerialized] internal Headgear headgear;

    internal HeadgearData(string name) {
        this.name = name;
        level = 1;
    }
}
[System.Serializable]
internal class ApparelData : BaseData { 

}
[System.Serializable]
internal class UtilityData : BaseData {
    internal BaseData character;

    [System.NonSerialized] internal Utility utility;

    internal UtilityData(string name) {
        this.name = name;
    }
}
[System.Serializable]
internal class BaseData {
    internal string name;
    [System.NonSerialized] internal Toggle slot;
    [System.NonSerialized] internal Image equip;
}
#endregion