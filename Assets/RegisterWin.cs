using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegisterWin : MonoBehaviour
{

    public GameObject victoryScreen;
    public TextMeshProUGUI timerText;

    float playertime;
    bool victory;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GameObject.FindGameObjectWithTag("VictoryScreen").SetActive(false);
        victory = false;
        playertime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!victory)
        {
            playertime += Time.deltaTime;
        }

        // Debug.Log(playertime);

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Berry")
        {
            Debug.Log("ACTIVATING VICTORY SCREEN");
            victory = true;
            victoryScreen.SetActive(true);
            timerText.text = playertime.ToString();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

}
