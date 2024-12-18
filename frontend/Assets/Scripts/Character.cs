using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public TMP_Text charName;
    public TMP_Text charDescription;

    public int id;
    public Image facePicture;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseCharacter()
    {
        GameManager.Instance.chosenCharId = id;
        GameManager.Instance.chosenCharIdText.text = id.ToString();
    }
}
