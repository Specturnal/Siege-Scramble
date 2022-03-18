using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour {
    private ItemLibrary library;
    private ItemInventory inventory;

    private void Start() {
        library = ItemLibrary.instance;
        inventory = ItemInventory.instance;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            GachaLOL();
        }
    }

    public void GachaLOL() {
        inventory.AddItem(library.characters[Random.Range(0, library.characters.Count)]);
        inventory.AddItem(library.weapons[Random.Range(0, library.weapons.Count)]);
        inventory.AddItem(library.headgears[Random.Range(0, library.headgears.Count)]);
        inventory.AddItem(library.utilities[Random.Range(0, library.utilities.Count)]);
    }
}
