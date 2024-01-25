using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UnNextLevel : MonoBehaviour
{
    public void OnClick()

    {
        if (Input.GetMouseButtonDown(0))
        {
            UnlockMission();
            SceneManager.LoadScene(0);
        }
    }


    public void UnlockMission()
    {
        int currentMission = SceneManager.GetActiveScene().buildIndex;

        if (currentMission >= PlayerPrefs.GetInt("mission"))
        {
            PlayerPrefs.SetInt("mission", currentMission + 1);
            SceneManager.LoadScene(0);
        }

    }
}
