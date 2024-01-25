using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UnLevelManager : MonoBehaviour
{
    int missionUnLock;
    public Button[] buttons;

    void Start()
    {
        missionUnLock = PlayerPrefs.GetInt("mission", 1);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < missionUnLock; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void loadmission(int missionIndex)
    {
        SceneManager.LoadScene(missionIndex);
    }
}
