using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class FinishedDrink : MonoBehaviour {

    public Text titleLabel;
    public Text ingredientsLabel;

    [Header("Incorrect Orders")]
    public Vector2 minMaxMistakes;
    public bool leaveAfterIncorrect;

    int currentMistakes;
    GameObject gameManager;
    GameHandler gameHandler;
    CustomerManager customerManager;
    Recipes recipes;
    bool drinkWasCorrect;

    void Start() {
        gameManager = GameObject.Find("GameManager");
        gameHandler = gameManager.GetComponent<GameHandler>();
        recipes = gameManager.GetComponent<Recipes>();
        customerManager = gameManager.GetComponent<CustomerManager>();
    }

    void OnTriggerEnter(Collider other) {
        Glass glass = other.GetComponent<Glass>();
        if(glass != null) {
            if(glass.fillPercentage > 0f && customerManager.CurrentCustomer() != null) {
                Item item = glass.GetComponent<Item>();
                Recipe targetRecipe = gameHandler.TargetRecipe;

                SetLabels(targetRecipe, recipes.MatchingRecipe(glass), glass);
                StartCoroutine(RespawnAfterServing(item));
            }
        }
    }

    void SetLabels(Recipe targetRecipe, Recipe actualRecipe, Glass glass) {
        drinkWasCorrect = false;
        if (actualRecipe == null) {
            // No drink could not be found matching
            //titleLabel.text = "You made a custom drink!";
            if(targetRecipe != null) {
                //titleLabel.text += " Your customer is not impressed.";
                CustomerNotImpressed();
            }
        } else {
            float score = Mathf.Ceil(recipes.ClosenessToRecipe(actualRecipe, glass));
            if (targetRecipe == null) {
                // No target; so no backlash
                //titleLabel.text = "You made a " + actualRecipe.name + "!";
                //titleLabel.text += " Your accuracy was " + score + "%";
                drinkWasCorrect = true;
                customerManager.OnDrinkServed(score);
            } else {
                if(targetRecipe == actualRecipe) {
                    // Recipe matches what player was supposed to make
                    //titleLabel.text = "You made a " + actualRecipe.name + " in " + gameHandler.GetTargetCompletionTime() + " seconds!";
                    //titleLabel.text += " Your accuracy was " + score + "%";
                    drinkWasCorrect = true;
                    customerManager.OnDrinkServed(score);
                } else {
                    // Recipe does not match target
                    //titleLabel.text = "You made a " + actualRecipe.name + "! Your customer is not impressed.";
                    CustomerNotImpressed();
                }
            }
        }

        ingredientsLabel.text = "";
        foreach (KeyValuePair<string, float> content in glass.Contents) {
            ingredientsLabel.text += content.Key + ": " + (content.Value * glass.maxFillMeasurement).ToString("F2") + "ml\n";
        }
    }

    void CustomerNotImpressed() {
        Customer customer = customerManager.CurrentCustomer();
        if (leaveAfterIncorrect && customer != null) {
            currentMistakes++;
            if (currentMistakes > Random.Range(minMaxMistakes.x, minMaxMistakes.y)) {
                currentMistakes = 0;
                drinkWasCorrect = true;
                titleLabel.text = "";
                ingredientsLabel.text = "";
                customerManager.OnDrinkServed(0);
            } else {
                customer.OnWrongDrinkReceived();
            }
        }
    }

    IEnumerator RespawnAfterServing(Item item) {
        item.Respawn();
        if (drinkWasCorrect)
            drinkWasCorrect = false;
        yield return new WaitForSeconds(8f);
        titleLabel.text = "";
        ingredientsLabel.text = "";
    }
}
