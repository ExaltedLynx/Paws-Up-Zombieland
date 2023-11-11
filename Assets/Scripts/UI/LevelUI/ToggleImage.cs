using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour
{
    [SerializeField] GameObject image1;
    [SerializeField] GameObject image2;

    public void SwapImage()
    {
        image1.gameObject.SetActive(!image1.activeSelf);
        image2.gameObject.SetActive(!image2.activeSelf);
    }
}
