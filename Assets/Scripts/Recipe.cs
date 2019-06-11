using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe {

    public string name;
    [HideInInspector] public bool ice;
    [HideInInspector] public bool lime;
    [HideInInspector] public bool lemon;
    public Ingredient[] ingredients;
}
