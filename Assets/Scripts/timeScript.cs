using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class timeScript : MonoBehaviour
{
    public Text counterText;
    public bool timeCounter = true;
    public float seconds, minutes;
    public GameObject GameoverPanel;

    // Use this for initialization
    void Start()
    {
        counterText = GetComponent<Text>() as Text;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCounter)
        {
            // Calculate minutes and seconds
            minutes = Mathf.Floor(Time.timeSinceLevelLoad / 60f); // Total minutes
            seconds = Time.timeSinceLevelLoad % 60f; // Remaining seconds

            // Update the text with minutes and seconds
            counterText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
            if(minutes == 2)
            {
                AudioManager.Instance.PlayGameOverSOund();
                timeCounter = false;
                counterText.color = Color.red;
                GameoverPanel.SetActive(true);
            }
        }
    }

    public void endGame()
    {
        timeCounter = false;
        counterText.color = Color.yellow;
    }
    

}
