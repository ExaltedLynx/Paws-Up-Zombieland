using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRefs : MonoBehaviour
{
    public static GameObject MainMenu { get; private set; }
    [SerializeField] private GameObject mainMenu;
    public static GameObject LoadGameMenu { get; private set; }
    [SerializeField] private GameObject loadGame;

    private void Awake()
    {
        MainMenu = mainMenu;
        LoadGameMenu = loadGame;
    }

    public static void SwitchMenus(GameObject menuOn, GameObject menuOff)
    {
        menuOff.SetActive(false);
        menuOn.SetActive(true);
    }
}
