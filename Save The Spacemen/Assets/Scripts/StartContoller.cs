using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartContoller : MonoBehaviour
{
    #region Save Data

    int[] scores = new int[11];
    string savePath;

    #endregion

    #region AudioFields

    [SerializeField] private AudioClip Click;
    [SerializeField] private AudioClip badClick;

    #endregion

    private int currentLevel = 0;
    private int maxLevel = 10;
    public Text levelName;
    public Image[] stars;

    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.dataPath + "/Saves/save.txt";
        LoadGameData();
        UpdateLevelPanel();
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

    #region  Functions Main UI

    public void UpdateLevelPanelOnClick(bool increase)
    {
        if (increase)
        {
            if (currentLevel < maxLevel) currentLevel += 1;
            else currentLevel = 0;
        }

        else
        {
            if (currentLevel > 0) currentLevel -= 1;
            else currentLevel = maxLevel;
        }

        UpdateLevelPanel();
    }

    public void UpdateLevelPanel()
    {
        levelName.text = currentLevel.ToString();

        // Set all stars to inactive and then bring them back to active if the saved score is high enough.
        stars[0].gameObject.SetActive(false);
        stars[1].gameObject.SetActive(false);
        stars[2].gameObject.SetActive(false);

        for (int starNum = 0; starNum < 3; starNum++)
        {
            if (starNum < scores[currentLevel]) stars[starNum].gameObject.SetActive(true);
        }
    }

    public void ClickGo()
    {
        SceneManager.LoadScene(currentLevel + 1);
    }



    #endregion

    #region Functions Sounds

    public void PlayClickButton(bool goodClick)
    {
        if (goodClick) SoundManager.Instance.PlaySound(Click);
        else SoundManager.Instance.PlaySound(badClick);
    }

    #endregion
}
