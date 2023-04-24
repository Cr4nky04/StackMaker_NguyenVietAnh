using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BrickCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowScore(int numberOfBrick)
    {
        scoreText.text = numberOfBrick.ToString();
    }
}
