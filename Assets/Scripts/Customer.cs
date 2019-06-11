using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Customer : MonoBehaviour {

    [Header("Conversational")]
    public string[] faces;
    public string[] messageState1;
    public string[] messageState2;
    public string[] messageState3;
    public string[] messageState4;

    [Header("Special Messages")]
    public string[] orderIncorrectMessages;
    public string[] hitMessages;
    public string[] giveUpMessages;
    public string[] drinkQualityTerrible;
    public string[] drinkQualityBad;
    public string[] drinkQualityGood;
    public string[] drinkQualityExcellent;

    public Text facesUI;
    public Text talkingUI;
    [HideInInspector] public int queuePos;
    [HideInInspector] public bool isMoving;
    int conversationStage;
    bool leaving;
    bool hasOrdered;
    float drinkQuality;
    public bool HasOrdered {
        get {
            return hasOrdered;
        }
    }

    Animator animator;
    NavMeshAgent navMeshAgent;
    GameHandler gameHandler;
    CustomerManager customerManager;
    Recipes recipes;
    Recipe recipe;
    bool complaining;

    void Start() {
        hasOrdered = false;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameHandler = GameObject.Find("GameManager").GetComponent<GameHandler>();
        recipes = GameObject.Find("GameManager").GetComponent<Recipes>();
        customerManager = GameObject.Find("GameManager").GetComponent<CustomerManager>();
        navMeshAgent.Warp(transform.position);
        StartCoroutine(EnterBar());
        StartCoroutine(Faces());
    }

    public void SetQueuePosition(Vector3 position, int queuePosition) {
        // CODE TAKEN FROM [https://answers.unity.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html]; ACCESSED 9 DECEMBER 2018
        if (!navMeshAgent.pathPending && !isMoving) {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
                    queuePos = queuePosition;
                    navMeshAgent.destination = position;
                    if (queuePos == -1)
                        StartCoroutine(LeaveBar());
                }
            }
        }
    }

    IEnumerator LeaveBar() {
        while(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) {
            yield return new WaitForSeconds(0.4f);
        }
        isMoving = true;
        animator.enabled = true;
        animator.Play("Customer_Leave_Bar");
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    IEnumerator EnterBar() {
        isMoving = true;
        animator.Play("Customer_Enter_Bar");
        yield return new WaitForSeconds(2.5f);
        animator.enabled = false;
        isMoving = false;
    }

    IEnumerator Faces() {
        while(queuePos > 0 || conversationStage == 3) {
            if (!complaining) {
                talkingUI.text = "";
                facesUI.text = faces[Random.Range(0, faces.Length)];
            }
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
        facesUI.text = "";
        while (conversationStage == 0 && complaining && queuePos == 0)
            yield return new WaitForSeconds(0.4f);
        Conversation();
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.GetComponent<Item>() != null) {
            if (queuePos != 0 || conversationStage == 3) {
                if (!complaining) {
                    complaining = true;
                    facesUI.text = "";
                    StartCoroutine(SayMessage(hitMessages[Random.Range(0, hitMessages.Length)], false));
                }
            }
        }
    }

    void Conversation() {
        if(conversationStage == 0) {
            StartCoroutine(SayMessage(messageState1[Random.Range(0, messageState1.Length)], true));
        } else if (conversationStage == 1) {
            StartCoroutine(SayMessage(messageState2[Random.Range(0, messageState2.Length)], true));
        } else if (conversationStage == 2) {
            recipe = recipes.recipes[Random.Range(0, recipes.recipes.Length)];
            StartCoroutine(SayMessage(messageState3[Random.Range(0, messageState3.Length)].Replace("{NAME}", recipe.name), true));
        } else if (conversationStage == 3) {
            gameHandler.SetTargetRecipe(recipe);
            talkingUI.text = "";
            hasOrdered = true;
            StartCoroutine(Faces());
        } else if (conversationStage == 4) {
            leaving = true;
            if (drinkQuality <= 0)
                StartCoroutine(SayMessage(giveUpMessages[Random.Range(0, giveUpMessages.Length)], true));
            else if(drinkQuality <= 25)
                StartCoroutine(SayMessage(drinkQualityTerrible[Random.Range(0, drinkQualityTerrible.Length)], true));
            else if (drinkQuality <= 50)
                StartCoroutine(SayMessage(drinkQualityBad[Random.Range(0, drinkQualityBad.Length)], true));
            else if (drinkQuality <= 75)
                StartCoroutine(SayMessage(drinkQualityGood[Random.Range(0, drinkQualityGood.Length)], true));
            else
                StartCoroutine(SayMessage(drinkQualityExcellent[Random.Range(0, drinkQualityExcellent.Length)], true));
            facesUI.text = "";
        } else if (conversationStage == 5) {
            customerManager.OnServingComplete();
        }
    }

    public void OnWrongDrinkReceived() {
        StartCoroutine(SayMessage(orderIncorrectMessages[Random.Range(0, orderIncorrectMessages.Length)], false));
    }

    public void OnDrinkReceived(float quality) {
        if (!leaving) {
            drinkQuality = quality;
            conversationStage++;
            Conversation();
        }
    }

    IEnumerator SayMessage(string message, bool proceedAfterMessage) {
        facesUI.text = "";
        for(int i = 0; i <= message.Length; i++) {
            talkingUI.text = message.Substring(0, i);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(1.3f);
        if (proceedAfterMessage) {
            conversationStage++;
            Conversation();
        }
        complaining = false;
    }
}
