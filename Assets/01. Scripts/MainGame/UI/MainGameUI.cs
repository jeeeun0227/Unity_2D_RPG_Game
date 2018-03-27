using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour
{

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    // Play UI

    public GameObject HPGuagePrefabs;
    public GameObject CooltimeGuagePrefabs;

    public Slider CreateSlider(bool isHPGauge)
    {
        if (isHPGauge)
        {
            return CreateGameSlider(HPGuagePrefabs);
        }
        return CreateGameSlider(CooltimeGuagePrefabs);
    }
  
    Slider CreateGameSlider(GameObject gaugePrefabs)
    {
        GameObject guageObject = GameObject.Instantiate(gaugePrefabs);
        Slider slider = guageObject.GetComponent<Slider>();
        return slider;
    }

    // Button Actions

    // public Character TargetCharacter;

    /*
    public void OnAttack()
    {
        Character target = GameManager.Instance.TargetCharacter;

        MessageParam msgParam = new MessageParam();
        msgParam.sender = null;
        msgParam.receiver = target;
        msgParam.message = "Attack";
        msgParam.attackPoint = 1000;

        MessageSystem.Instance.Send(msgParam);
    }
    */
}