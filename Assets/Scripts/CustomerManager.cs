using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour {

    public GameObject customerPrefab;
    public Transform queuePoints;
    public Transform leavePoint;

    List<Transform> queue;
    List<Customer> customers;
    GameHandler gameHandler;

    void Start () {
        customers = new List<Customer>();
        gameHandler = GameObject.Find("GameManager").GetComponent<GameHandler>();
        if (gameHandler.gameMode != GameHandler.GameModes.Sandbox) {
            SetupQueue();
            SpawnCustomer();
            StartCoroutine(Spawner());
        }
    }
	
	void Update () {
        CheckForSpaces();
    }

    void SetupQueue() {
        queue = new List<Transform>();
        foreach (Transform transform in queuePoints)
            queue.Add(transform);
    }

    void SpawnCustomer() {
        GameObject go = Instantiate(customerPrefab);
        Customer customer = go.GetComponent<Customer>();
        customer.queuePos = queue.Count;
        customers.Add(customer);
    }

    void CheckForSpaces() {
        for(int i = 0; i < customers.Count; i++) {
            if(customers[i].queuePos > i && !customers[i].HasOrdered) {
                customers[i].SetQueuePosition(queue[customers[i].queuePos-1].position, customers[i].queuePos - 1);
            }
        }
    }

    public Customer CurrentCustomer() {
        if (customers.Count == 0)
            return null;

        Customer customer = customers[0];
        if(customer.HasOrdered)
            return customer;
        return null;
    }

    IEnumerator Spawner() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(10f, 30f));
            if (customers.Count < queue.Count) {
                SpawnCustomer();
            }
        }
    }

    public void OnDrinkServed(float quality) {
        Customer customer = CurrentCustomer();
        if (customer != null) {
            gameHandler.SetTargetRecipe(null);
            customer.OnDrinkReceived(quality);
        }
    }

    public void OnServingComplete() {
        if (CurrentCustomer() != null) {
            gameHandler.SetTargetRecipe(null);
            customers[0].SetQueuePosition(leavePoint.position, -1);
            customers.RemoveAt(0);
        }
    }
}
