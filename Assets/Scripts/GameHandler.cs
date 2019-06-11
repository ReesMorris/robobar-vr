using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHandler : MonoBehaviour {

    public enum GameModes { Sandbox, Normal }
    public GameModes gameMode;
    public TMP_Text orderText;
    public TMP_Text timerText;
    public TMP_Text xtraText;
    public TMP_Text recipeText;

    Recipe targetRecipe;
    public Recipe TargetRecipe {
        get {
            return targetRecipe;
        }
    }
    float targetSetTime;

    Recipes recipes;

    void Start() {
        recipes = GetComponent<Recipes>();

        switch (gameMode) {
            case GameModes.Normal:
                break;
            case GameModes.Sandbox:
                break;
        }
    }

    public Recipe SetTargetRecipe(Recipe recipe) {
        StopCoroutine("RecipeTime");
        targetSetTime = Time.time;
        if (recipe == null) {
            timerText.text = "-";
            orderText.text = "-";
            xtraText.text = "-";
            recipeText.text = "";
            targetRecipe = null;
        } else {
            orderText.text = recipe.name;
            targetRecipe = recipe;
            SetRecipeText(recipe);
            SetExtras(recipe);
            StartCoroutine("RecipeTime");
        }
        return targetRecipe;
    }

    public string GetTargetCompletionTime() {
        return (Time.time - targetSetTime).ToString("F2");
    }

    public Recipe SetRandomRecipe() {
        Recipe recipe = SetTargetRecipe(recipes.recipes[Random.Range(0, recipes.recipes.Length)]);
        return recipe;
    }

    void SetRecipeText(Recipe recipe) {
        foreach(Ingredient ingredient in recipe.ingredients) {
            recipeText.text += ingredient.name + " (" + ingredient.amount + "ml)\n";
        }
    }

    void SetExtras(Recipe recipe) {
        recipe.ice = (Random.value > 0.5f);
        recipe.lime = (Random.value > 0.5f);
        recipe.lemon = (Random.value > 0.5f);
        xtraText.text = "";
        if (recipe.ice)
            xtraText.text += "Ice\n";
        if (recipe.lime)
            xtraText.text += "Lime\n";
        if (recipe.lemon)
            xtraText.text += "Lemon\n";
        if (xtraText.text == "")
            xtraText.text = "-";
    }

    IEnumerator RecipeTime() {
        timerText.text = "0s";
        while (true) {
            yield return new WaitForSeconds(0f);
            float time = Mathf.Floor(Time.time - targetSetTime);
            timerText.text = time.ToString() + "s";
        }
    }
}
