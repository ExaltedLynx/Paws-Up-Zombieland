using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public TextAsset dialogueTextAsset;
    public float textSpeed;
    public int maxCharactersPerLine = 85;
    public Button skipButton; // Reference to the skip button
    private int currentLineIndex;
    private List<string> textChunks = new List<string>();
    private bool alreadyRead = false;

     public List<GameObject> activateObjects; // List of objects to activate
    
    // Portraits
    [SerializeField] public CharacterPortraitManager portraitManager;


    void Start()
    {
        textComponent.text = string.Empty;
        Debug.Log("Initial Font Size: " + GlobalSettings.dialogueFontSize);
        if (dialogueTextAsset != null)
        {
            LoadTextFromAsset();
            SplitLinesIntoChunks();
            StartDialogue();
        }
        else
        {
            Debug.LogError("No dialogue text asset assigned.");
        }

        if (activateObjects == null)
        {
            activateObjects = new List<GameObject>();
        }

         if (skipButton != null)
        {
            // Attach the method to be called when the skip button is clicked
            skipButton.onClick.AddListener(SkipToEnd);
        }
        skipButton.gameObject.SetActive(CanSkipDialogue() || PersistDialogue.Instance.AlreadyReadDialogue);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentLineIndex < lines.Length)
            {
                if (textComponent.text == textChunks[currentLineIndex])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = textChunks[currentLineIndex];
                }
            }
            else
            {
                NextLine();
            }
        }
        if (skipButton != null && skipButton.interactable && skipButton.onClick != null && skipButton.onClick.GetPersistentEventCount() > 0 && skipButton.onClick.GetPersistentMethodName(0) == "SkipToEnd")
        {
            SkipToEnd();
            return;
        }

        // Check if the dialogue is finished
        if (currentLineIndex >= lines.Length)
        {
            EndDialogue();
            return;
        }

    }

    void SplitLinesIntoChunks()
    {
        textChunks.Clear();
        foreach (string line in lines)
        {
            int startIndex = 0;
            while (startIndex < line.Length)
            {
                int endIndex = Mathf.Min(startIndex + maxCharactersPerLine, line.Length);
                textChunks.Add(line.Substring(startIndex, endIndex - startIndex));
                startIndex = endIndex;
            }
        }
    }

   void StartDialogue()
{
    currentLineIndex = 0;
    textComponent.text = string.Empty;
    portraitManager.SetCharacterPortrait(currentLineIndex); // Set portrait for the current line
    StartCoroutine(TypeLine());
}

    IEnumerator TypeLine()
    {
        foreach (char c in textChunks[currentLineIndex].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

  void NextLine()
{
    if (currentLineIndex < lines.Length - 1)
    {
        currentLineIndex++;
        textComponent.text = string.Empty;
        portraitManager.SetCharacterPortrait(currentLineIndex); // Set portrait for the current line
        StartCoroutine(TypeLine());
    }
    else
    {
        EndDialogue();
    }
}

    void EndDialogue()
    {
        gameObject.SetActive(false);
        foreach (GameObject obj in activateObjects)
        {
            if (obj.TryGetComponent(out WaveSpawner spawner))
            {
                spawner.enabled = true;
            }
            else
            {
                obj.SetActive(true);
            }
        }
        // Disable the skip button
        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(false);
        }
        PersistDialogue.Instance.AlreadyReadDialogue = true;
    }
    void LoadTextFromAsset()
    {
        if (dialogueTextAsset != null)
        {
            // Debug.Log("Text Asset Content: " + dialogueTextAsset.text);

            lines = dialogueTextAsset.text.Split('\n');
            // foreach (string line in lines)
            //{
                // Debug.Log("Line: " + line);
           // }
        }
        else
        {
            Debug.LogError("No dialogue text asset assigned.");
        }
    }

    [ContextMenu("Skip Dialogue")]
    void SkipToEnd()
    {
        // Skip to the end of the dialogue
        currentLineIndex = lines.Length - 1;
        textComponent.text = textChunks[currentLineIndex];


        EndDialogue();
    }

    private bool CanSkipDialogue()
    {
        if(GameManager.unlockedLevels == 5)
            return true;

        if (GameManager.unlockedLevels > GameManager.currentLevel)
            return true;

        return false;
    }
}