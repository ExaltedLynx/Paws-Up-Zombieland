using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject heldUnit;

    public static GameManager Instance
    {
        get => instance;
    }

    private static GameManager instance;

    void Start()
    {
        instance = this;
    }

    void Update()
    {

    }

    public void SetHeldUnit(GameObject unit)
    {
        heldUnit = Instantiate(unit, transform);
    }


}
