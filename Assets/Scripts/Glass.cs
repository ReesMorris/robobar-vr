using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Glass : MonoBehaviour {

    [Header("Liquids")]
    [Range(0, 1)] public float fillPercentage;
    public GameObject fillContainer;
    public GameObject fill;
    public float fillScale;
    public Renderer fillMaterial;
    public float fillAmount;
    public float maxFillMeasurement;

    [Header("Ice")]
    [Range(0, 1)] public float iceFillPercentage;
    public Transform iceContainer;
    public Canvas iceCanvas;
    public Image iceFill;

    [Header("Extras")]
    public GameObject lemonSlice;
    public GameObject limeSlice;

    [Header("Contents")]
    public Canvas contentCanvas;
    public Text containerText;

    List<Liquid> liquids;
    Dictionary<string, float> contents;
    List<GameObject> ice;
    Recipes recipes;
    float lastFilledTime;
    float lastIceTime;

    public Dictionary<string, float> Contents {
        get {
            return contents;
        }
    }

    void Start() {
        liquids = new List<Liquid>();
        contents = new Dictionary<string, float>();
        ice = new List<GameObject>();
        recipes = GameObject.Find("GameManager").GetComponent<Recipes>();
        SetContainerText();
        InitIce();
    }
    
	void Update () {
        ShowUI();

        fillContainer.transform.localScale = new Vector3(fillContainer.transform.localScale.x, Mathf.SmoothStep(0, fillScale, fillPercentage), fillContainer.transform.localScale.z);
        if (fillPercentage == 0f)
            fill.SetActive(false);
        else
            fill.SetActive(true);

        float rot = gameObject.transform.eulerAngles.z;

        // Pour glass left direction
        if (rot > 45f && rot <= 180f) {
            if (fillPercentage >= 0.85f || rot > 92f) {
                float spillAmount = Mathf.Abs(0.0002f * (100 - (Mathf.Abs(rot - 180f))));
                SetIce(iceFillPercentage - spillAmount);
                Empty(fillPercentage - spillAmount);
            }
        }

        // Pour glass right direction
        else if (rot >= 180f && rot < 315f) {
            if (fillPercentage >= 0.85f || rot < 268f) {
                float spillAmount = Mathf.Abs(0.0002f * (100 - (Mathf.Abs(180f - rot))));
                SetIce(iceFillPercentage - spillAmount);
                Empty(fillPercentage - spillAmount);
            }
        }
    }

    void ShowUI() {
        string parentName;
        if (transform.parent == null)
            parentName = "";
        else
            parentName = transform.parent.name;

        if (parentName == "LeftHand" || parentName == "RightHand") {
            iceCanvas.enabled = true;
            contentCanvas.enabled = true;
        } else {
            iceCanvas.enabled = false;
            contentCanvas.enabled = false;
        }

        // Special circumstances - glass has recently been filled
        if(lastFilledTime + 2 >= Time.time) {
            contentCanvas.enabled = true;
        }
        if (lastIceTime + 2 >= Time.time) {
            iceCanvas.enabled = true;
        }

    }

    void OnParticleCollision(GameObject other) {
        Liquid liquid = other.GetComponent<Liquid>();
        lastFilledTime = Time.time;
        if (liquid != null && fillPercentage < 1f) {
            fillPercentage += fillAmount;
            liquids.Add(liquid);

            // Source code from [https://answers.unity.com/questions/725895/best-way-to-mix-color-values.html]; accessed December 5, 2018
            Color result = new Color(0, 0, 0, 0);
            foreach (Liquid l in liquids)
                result += l.colour;
            result /= liquids.Count;
            // End source code from
            fillMaterial.material.color = new Color(result.r, result.g, result.b, 1f);

            // Set the fills
            if (contents.ContainsKey(liquid.name))
                contents[liquid.name] += fillAmount;
            else
                contents.Add(liquid.name, fillAmount);

            SetContainerText();
        }
    }

    void SetContainerText() {
        containerText.text = "";
        foreach(KeyValuePair<string, float> content in contents) {
            containerText.text += content.Key + ": " + (content.Value * maxFillMeasurement).ToString("F2") + "ml\n";
        }
    }

    public void Empty(float amount) {
        if (fillPercentage > 0f) {
            float oldAmount = fillPercentage;
            fillPercentage = amount;
            fillPercentage = Mathf.Max(0, fillPercentage);

            // Source code from [https://answers.unity.com/questions/409835/out-of-sync-error-when-iterating-over-a-dictionary.html]; accessed December 5, 2018
            List<string> keys = new List<string>(contents.Keys);
            foreach (string key in keys) {
                contents[key] -= (oldAmount - amount) / contents.Count;
                if (contents[key] <= 0f)
                    contents.Remove(key);
            }
            // End source code from

            if (fillPercentage <= 0f) {
                liquids.Clear();
                contents.Clear();
            }
            SetContainerText();
        }
    }

    void OnCollisionEnter(Collision collision) {
        Item item = collision.gameObject.GetComponent<Item>();
        if (collision.gameObject.name == "SpawnedIce") {
            lastIceTime = Time.time;
            Destroy(collision.gameObject);
            SetIce(iceFillPercentage + 0.1f);
        } else if(collision.gameObject.tag == "Lime") {
            limeSlice.SetActive(true);
            item.Respawn();
        } else if(collision.gameObject.tag == "Lemon") {
            lemonSlice.SetActive(true);
            item.Respawn();
        }
    }

    public void InitIce() {
        foreach (Transform child in iceContainer) {
            ice.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }

    public void SetIce(float amount) {
        iceFillPercentage = amount;
        if (iceFillPercentage < 0)
            iceFillPercentage = 0;
        if (iceFillPercentage > 1)
            iceFillPercentage = 1;
        float totalIce = iceContainer.childCount;
        float visibleIce = Mathf.Floor(totalIce * iceFillPercentage);

        iceFill.fillAmount = iceFillPercentage;

        for(int i = 0; i < totalIce; i++) {
            if (i < visibleIce)
                ice[i].SetActive(true);
            else
                ice[i].SetActive(false);
        }
    }
}
