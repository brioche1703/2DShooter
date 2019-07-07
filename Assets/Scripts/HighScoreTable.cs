using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highScoreEntryTransformList;
    HighScores highScores;

    private int playerPosition = -1;
    private int playerScore = -1;
        
    private float templateHeight = 30.0f;

    public GameObject inputName;
    // To not have to hit enter twice to confirm the score
    bool allowEnter; 

    private void Awake()
    {
        
//        ClearHighScoreEntry();

        playerScore = PersistentManagerScript.Instance.score;

        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("HighScoreTable");
        highScores = JsonUtility.FromJson<HighScores>(jsonString);

        if (highScores == null)
        {
            AddHighScoreEntry(300, "AAA");
            AddHighScoreEntry(200, "AAA");
            AddHighScoreEntry(210, "AAA");
            AddHighScoreEntry(270, "AAA");
            AddHighScoreEntry(240, "AAA");
            AddHighScoreEntry(100, "AAA");
            AddHighScoreEntry(50, "AAA");
            AddHighScoreEntry(50, "AAA");
            AddHighScoreEntry(10, "AAA");
            AddHighScoreEntry(10, "AAA");
            AddHighScoreEntry(10, "AAA");
            AddHighScoreEntry(10, "AAA");
            AddHighScoreEntry(10, "AAA");
            jsonString = PlayerPrefs.GetString("HighScoreTable");
            highScores = JsonUtility.FromJson<HighScores>(jsonString);
        }

        // Add Player score
        AddHighScoreEntry(playerScore, "");
        jsonString = PlayerPrefs.GetString("HighScoreTable");
        highScores = JsonUtility.FromJson<HighScores>(jsonString);

        HighScoreEntry playerHighScoreEntry = new HighScoreEntry {score = playerScore, name = "" };


        // Sorting list and getting player position
        for (int i = 0; i < highScores.highScoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highScores.highScoreEntryList.Count; j++)
            {
                if (highScores.highScoreEntryList[j].score > highScores.highScoreEntryList[i].score)
                {
                    HighScoreEntry tmp = highScores.highScoreEntryList[i];
                    highScores.highScoreEntryList[i] = highScores.highScoreEntryList[j];
                    highScores.highScoreEntryList[j] = tmp;
                    // Check if it's player score
                    if (highScores.highScoreEntryList[i].IsEqual(playerHighScoreEntry))
                    {
                        playerPosition = i;
                    }
                    if (highScores.highScoreEntryList[j].IsEqual(playerHighScoreEntry))
                    {
                        playerPosition = j;
                    }
                }
            }
        }

        highScoreEntryTransformList = new List<Transform>();
        for (int i = 0; i < 10 && i < highScores.highScoreEntryList.Count; i++)
        {
            HighScoreEntry highScoreEntry = highScores.highScoreEntryList[i];
            CreatingHighScoreEntryTransform(highScoreEntry, entryContainer, highScoreEntryTransformList);
        }
        // Clear scores not in the 10 firsts
        for (int i = 10; i < highScores.highScoreEntryList.Count; i++)
        {
            highScores.highScoreEntryList.RemoveAt(i);
        }
        if (playerPosition > 10 || playerPosition < 0)
        {
            inputName.gameObject.SetActive(false);
        }
        else
        {
            inputName.GetComponent<InputField>().ActivateInputField();
        }

        
    }

    void Update()
    {
        bool scoreSaved = false;
        inputName.GetComponent<InputField>().Select();
        inputName.GetComponent<InputField>().text = inputName.GetComponent<InputField>().text.ToUpper();
        if (!scoreSaved)
        {
            if (playerPosition < 10 || playerPosition >= 0)
            {
                inputName.GetComponent<InputField>().Select();
                inputName.GetComponent<InputField>().text = inputName.GetComponent<InputField>().text.ToUpper();
                if (allowEnter && inputName.GetComponent<InputField>().text.Length == 3 && Input.GetKey(KeyCode.Return))
                {
                    allowEnter = false;
                    highScores.highScoreEntryList[playerPosition].name = inputName.GetComponent<InputField>().text;
                    inputName.gameObject.SetActive(false);
                    string json = JsonUtility.ToJson(highScores);
                    PlayerPrefs.SetString("HighScoreTable", json);
                    PlayerPrefs.Save();
                    scoreSaved = true;

                    // Clear transform list to display with the new score name
                    highScoreEntryTransformList.Clear();
                    for (int i = 0; i < 10 && i < highScores.highScoreEntryList.Count; i++)
                    {
                        HighScoreEntry highScoreEntry = highScores.highScoreEntryList[i];
                        CreatingHighScoreEntryTransform(highScoreEntry, entryContainer, highScoreEntryTransformList);
                    }
                }
                else 
                {
                    allowEnter = inputName.GetComponent<InputField>().isFocused;
                }
            }
        }
    }

    private void CreatingHighScoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);


        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default: rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        Debug.Log(playerPosition);
        Debug.Log(playerScore);

        int score = highScoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highScoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        
        if (transformList.Count == playerPosition)
        {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;

            inputName.GetComponent<RectTransform>().position = new Vector2(
                    inputName.GetComponent<RectTransform>().position.x,
                    entryTransform.Find("nameText").GetComponent<Text>().transform.position.y);
        }

        transformList.Add(entryTransform);


    }

    private void EnterHighScoreEntry(int score)
    {
        // Create entry to be modified
        HighScoreEntry highScoreEntry = new HighScoreEntry {score = score, name = "AAA" };

        // Load saved highScores
        string jsonString = PlayerPrefs.GetString("HighScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);
        
        if (highScores == null)
        {
            highScores = new HighScores() {
                highScoreEntryList = new List<HighScoreEntry>()
            };
        }  


    }

    private void AddHighScoreEntry(int score, string name)
    {
        // Create entry
        HighScoreEntry highScoreEntry = new HighScoreEntry { score = score, name = name };

        // Load saved highscores
        string jsonString = PlayerPrefs.GetString("HighScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);
   
        // If no table create one 
        if (highScores == null)
        {
            highScores = new HighScores() {
                highScoreEntryList = new List<HighScoreEntry>()
            };
        }

        // Add new entry to highscores
        highScores.highScoreEntryList.Add(highScoreEntry);

        // Save updated highscores
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("HighScoreTable", json);
        PlayerPrefs.Save();

    }

    private void ClearHighScoreEntry()
    {
        PlayerPrefs.DeleteAll(); 
    }

    private class HighScores 
    {
        public List<HighScoreEntry> highScoreEntryList;
    }

    [System.Serializable]
    private class HighScoreEntry
    {
        public int score;
        public string name;

        public bool IsEqual(HighScoreEntry b)
        {
            return (score == b.score && name == b.name);
        }
    }
}
