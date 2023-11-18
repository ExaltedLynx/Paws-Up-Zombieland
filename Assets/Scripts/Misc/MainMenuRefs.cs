using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRefs : MonoBehaviour
{
    public static GameObject MainMenu { get; private set; }
    [SerializeField] private GameObject mainMenu;

    public static GameObject LoadGameMenu { get; private set; }
    [SerializeField] private GameObject loadGame;

    public static GameObject LevelSelectMenu { get; private set; }
    [SerializeField] private GameObject levelSelect;


    private void Awake()
    {
        MainMenu = mainMenu;
        LoadGameMenu = loadGame;
        LevelSelectMenu = levelSelect;
    }

    public static void SwitchMenus(GameObject menuOn, GameObject menuOff)
    {
        menuOff.SetActive(false);
        menuOn.SetActive(true);
    }
}
