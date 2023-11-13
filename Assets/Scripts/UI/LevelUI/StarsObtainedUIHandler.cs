using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarsObtainedUIHandler : MonoBehaviour
{
    [SerializeField] private RawImage[] starImages = new RawImage[3];

    public void UpdateStarImages(int stars)
    {
        switch (stars)
        {
            case 1:
                starImages[1].gameObject.SetActive(true);
                break;

            case 2:
                starImages[0].transform.position += new Vector3(2f, 0);
                starImages[0].gameObject.SetActive(true);
                starImages[2].transform.position -= new Vector3(2f, 0);
                starImages[2].gameObject.SetActive(true);
                break;

            case 3:
                starImages[0].gameObject.SetActive(true);
                starImages[1].gameObject.SetActive(true);
                starImages[2].gameObject.SetActive(true);
                break;
        }
    }
}
