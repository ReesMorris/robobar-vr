using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipes : MonoBehaviour {

    public Recipe[] recipes;

    // Returns a boolean of whether or not a glass matches a specific recipe (liquid only)
    public bool MatchesRecipe(Recipe recipe, Glass glass) {
        if (glass.Contents.Count == recipe.ingredients.Length) {
            foreach (Ingredient ingredient in recipe.ingredients) {
                if (!glass.Contents.ContainsKey(ingredient.name)) {
                    return false;
                }
            }
        } else {
            return false;
        }
        return true;
    }
    // Returns the recipe that a glass matches (liquid only)
    public Recipe MatchingRecipe(Glass glass) {
        foreach(Recipe recipe in recipes) {
            if (MatchesRecipe(recipe, glass))
                return recipe;
        }
        return null;
    }

    // Returns a value between 0 and 100 of how close the glass contents are to the recipe itself
    public float ClosenessToRecipe(Recipe recipe, Glass glass) {
        float points = 100;
        float maxReduction = 100 / recipe.ingredients.Length;

        // Liquid closeness
        foreach (Ingredient ingredient in recipe.ingredients) {
            if (glass.Contents.ContainsKey(ingredient.name)) {
                points -= Mathf.Min(maxReduction, Mathf.Abs(ingredient.amount - (glass.Contents[ingredient.name] * glass.maxFillMeasurement)));
            } else {
                points -= maxReduction;
            }
        }

        // Ice
        if (recipe.ice && glass.iceFillPercentage < 1f)
            points -= 10;
        // Lime
        if (recipe.lime != glass.limeSlice.activeSelf)
            points -= 10;
        // Lemon
        if (recipe.lemon != glass.lemonSlice.activeSelf)
            points -= 10;

        return Mathf.Max(0, points);
    }

    public Recipe GetRecipe(string name) {
        foreach(Recipe recipe in recipes) {
            if (recipe.name == name)
                return recipe;
        }
        return null;
    }
}
