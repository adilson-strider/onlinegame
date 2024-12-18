using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TMP_InputField loginUserName;
    public TMP_InputField loginPassword;

    public TMP_InputField registerUsername;
    public TMP_InputField registerPassword;
    public TMP_InputField registerEmail;

    public TMP_Text usernameText;
    public TMP_Text xpText;

    public TMP_Text winsText;
    public TMP_Text lossesText;

    public Transform charSelectRoster;
    public GameObject charSelectPrefab;

    public string jwtToken;
    public GameObject authScreen;
    public GameObject selectScreen;
    public GameObject battleScreen;

    public CharacterData[] characters;

    public int chosenCharId;
    public TMP_Text chosenCharIdText;

    public void Awake(){
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenAuthScreen(){
        authScreen.SetActive(true);
        selectScreen.SetActive(false);
        battleScreen.SetActive(false);
    }

    public void OpenSelectScreen(){
        authScreen.SetActive(false);
        selectScreen.SetActive(true);
        battleScreen.SetActive(false);
    }

    public void OpenBattleScreen(){
        authScreen.SetActive(false);
        selectScreen.SetActive(false);
        battleScreen.SetActive(true);
    }

    public void Login()
    {
        StartCoroutine(SendLoginRequest());
    }

    private IEnumerator SendLoginRequest(){
        WWWForm form = new WWWForm();
        form.AddField("identifier", loginUserName.text);
        form.AddField("password", loginPassword.text);

        var newRequest = UnityWebRequest.Post(SocketManager.Instance.connectionURL+"/api/auth/local", form);

        yield return newRequest.SendWebRequest();

        if(newRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(newRequest.error);
            yield break;
        }

        var res = Encoding.UTF8.GetString(newRequest.downloadHandler.data);
        var resObj = JObject.Parse(res);
        Debug.Log(resObj);

        jwtToken = resObj["jwt"].ToString();  

        var charactersRequest = UnityWebRequest.Get(SocketManager.Instance.connectionURL+"/api/characters");

        yield return charactersRequest.SendWebRequest();    

        if(charactersRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(charactersRequest.error);
            yield break;
        }

        var charactersRes = Encoding.UTF8.GetString(charactersRequest.downloadHandler.data);

        var charactersResObj = JObject.Parse(charactersRes);
        characters = charactersResObj["data"].ToObject<CharacterData[]>(); 

        Debug.Log(charactersResObj);

        usernameText.text = resObj["user"]["username"].ToString();
        xpText.text = resObj["user"]["experience"].ToString();
        winsText.text = resObj["user"]["wins"].ToString();
        lossesText.text = resObj["user"]["losses"].ToString();

        OpenSelectScreen(); 
        PopulateCharactersSelectScreen();
    }

    public void PopulateCharactersSelectScreen(){
        foreach (CharacterData character in characters) {
            var newChar = Instantiate(charSelectPrefab, charSelectRoster);
            Debug.Log(characters.Length);
            newChar.GetComponent<Character>().id = character.Id;
            newChar.GetComponent<Character>().charName.text = character.Name;
            newChar.GetComponent<Character>().charDescription.text = character.Description;

            StartCoroutine(GetTexture(character.ImageUrl, newChar.GetComponent<Character>().facePicture));

        }  
    }

    public void Register()
    {
        // going to call a strapi endpoint for register a user

        var request = new UnityWebRequest(SocketManager.Instance.connectionURL+"/api/auth/local/register", "POST");

        string json = "{\"username\": \"" + registerUsername.text + 
              "\", \"email\": \"" + registerEmail.text + 
              "\", \"password\": \"" + registerPassword.text + "\"}";

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SendWebRequest();

    }

    

    public IEnumerator GetTexture(string imageUrl, Image facePic)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download image: " + request.error);
            yield break;
        }

        Texture2D texture = ((DownloadHandlerTexture) request.downloadHandler).texture;

        if (texture != null)
        {
            facePic.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogError("Downloaded texture is null");
        }
    }

}
