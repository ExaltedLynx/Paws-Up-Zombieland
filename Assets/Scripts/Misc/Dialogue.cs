using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public TextAsset dialogueTextAsset;
    public float textSpeed;
    public int maxCharactersPerLine = 50;

    private int index;
    private List<string> textChunks = new List<string>();
    private int currentChunkIndex;

    public GameObject startObject;

    void Start()
    {
        textComponent.text = string.Empty;
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
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentChunkIndex < textChunks.Count)
            {
                if (textComponent.text == textChunks[currentChunkIndex])
                {
                    NextChunk();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = textChunks[currentChunkIndex];
                }
            }
            else
            {
                NextLine();
            }
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
        index = 0;
        currentChunkIndex = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in textChunks[currentChunkIndex].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextChunk()
    {
        if (currentChunkIndex < textChunks.Count - 1)
        {
            currentChunkIndex++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            NextLine();
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            currentChunkIndex = 0;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            if (startObject != null)
            {
                startObject.SetActive(true);
            }
        }
    }

    void LoadTextFromAsset()
    {
        lines = dialogueTextAsset.text.Split('\n');
    }
}