using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public delegate void RespawnItem();
    public static RespawnItem RespawnItems;

    public void RespawnAllItems() {
        RespawnItems();
    }
}