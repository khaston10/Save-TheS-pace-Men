using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainContoller : MonoBehaviour
{
    #region Save Data

    int[] scores = new int[11];
    string savePath;

    #endregion

    // Fields that need to be updated for each level.
    public int sceneIndex;
    public GameObject[] astronauts;

    public bool gameIsStarted = false;
    public GameObject player;
    public int numberOfAstrosSaved;
    public int numberOfAstrosLeft;
    public GameObject mainCamera;
    public GameObject startPanel;
    public GameObject endPanel;
    public Text astroLeftText;

    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.dataPath + "/Saves/save.txt";
        LoadGameData();
        numberOfAstrosSaved = 0;
        numberOfAstrosLeft = astronauts.Length;
        astroLeftText.text = numberOfAstrosLeft.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadGameData()
    {
        StreamReader reader = new StreamReader(savePath);
        //Print the text from the file
        string text = reader.ReadToEnd();
        print(text);
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = System.Int32.Parse(text[i].ToString());
        }
        reader.Close();
    }

    void SaveGameData()
    {
        // Update the scores digit.
        scores[sceneIndex - 1] = StarsEarned();

        string textToWrite = "";

        for (int i=0; i < scores.Length; i++){
            textToWrite += scores[i].ToString();
        }
        

        StreamWriter writer = new StreamWriter(savePath, false);
        writer.WriteLine(textToWrite);
        writer.Close();
    }

    public int StarsEarned()
    {
        return 3;
    }

    public void PressStart()
    {
        player.GetComponent<PlayerContoller>().gameIsStarted = true;
        mainCamera.GetComponent<CameraFollow>().gameIsStarted = true;

        for (int i = 0; i < astronauts.Length; i++)
        {
            astronauts[i].GetComponentInChildren<AstronautController>().gameIsStarted = true;
        }

        startPanel.SetActive(false);
    }

    public void PressRetry()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void PressMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void AstroSavedOrDead(bool saved)
    {
        if (saved)
        {
            numberOfAstrosLeft -= 1;
            numberOfAstrosSaved += 1;
        }

        else numberOfAstrosLeft -= 1;

        astroLeftText.text = numberOfAstrosLeft.ToString();
    }

    public void CheckToSeeIfLevelIsComplete()
    {
        if (numberOfAstrosLeft <= 0)
        {
            endPanel.SetActive(true);
            gameIsStarted = false;
            SaveGameData();
            SceneManager.LoadScene(2);
        }
    }

    public void EndGame()
    {
        gameIsStarted = false;
        endPanel.SetActive(true);
    }
}
