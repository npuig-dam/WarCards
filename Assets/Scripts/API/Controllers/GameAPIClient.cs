using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.API.Model;
using System;

public class GameAPIClient : MonoBehaviour
{
    private string baseUrl = "https://api-projectgame.onrender.com/cgw/";

    // API: [HttpGet("{id}")]
    public IEnumerator GetLoginById(int id)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{baseUrl}login/{id}"))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Id trobada");
            }
        }
    }

    // API: [HttpGet] PasswdVer(int Id, string Passwd)
    public IEnumerator VerifyPassword(int id, string password, System.Action<bool> callback)
    {
        string url = $"{baseUrl}login/PasswdVer?Id={id}&Passwd={UnityWebRequest.EscapeURL(password)}";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
        
                bool isCorrect = bool.Parse(www.downloadHandler.text);
                callback?.Invoke(isCorrect);
            }
        }
    }

    // API: [HttpGet] (GetAllLogins)
    public IEnumerator GetAllLogins(System.Action<List<MLogin>> callback)
    {
 
        using (UnityWebRequest www = UnityWebRequest.Get(baseUrl + "login"))
        {
            Debug.Log("Fetching from: " + www.url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                Debug.Log("JSON Received: " + json);

                try
                {
                    string newJson = "{ \"logins\": " + json + " }";
                    MLoginList wrapper = JsonUtility.FromJson<MLoginList>(newJson);
                    callback?.Invoke(wrapper.logins);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("JSON Parsing Error: " + e.Message);
                    callback?.Invoke(null);
                }
            }
            else
            {
                Debug.LogError($"Web Error: {www.result} - {www.error}");
                callback?.Invoke(null);
            }
        }
    }

    // API: [HttpPost] Insert([FromBody] Login login)
    public IEnumerator CreateLogin(MLogin newLogin, Action onSuccess = null)
    {
        string json = JsonUtility.ToJson(newLogin);

        UnityWebRequest www = new UnityWebRequest(baseUrl + "login", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Login creat");
            onSuccess?.Invoke();
        }
        else
        {
            Debug.LogError("La creacio del login a fallat");
        }
    }


    // API: [HttpGet("{id}")]
    public IEnumerator GetPlayer(int id, System.Action<MPlayer> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{baseUrl}player/{id}"))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                callback?.Invoke(JsonUtility.FromJson<MPlayer>(www.downloadHandler.text));
            }
        }
    }

    // API: [HttpPost] Insert([FromBody] Player player)
    public IEnumerator CreatePlayer(MPlayer newPlayer)
    {
        string json = JsonUtility.ToJson(newPlayer);

        Debug.Log("Enviando JSON al servidor: " + json);

        using (UnityWebRequest www = new UnityWebRequest($"{baseUrl}player/", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Perfil creat");
            }
        }
    }

    // API: [HttpPut("{id}")] Update(int id, [FromBody] Player player)
    public IEnumerator UpdatePlayer(int id, MPlayer updatedData)
    {
        string json = JsonUtility.ToJson(updatedData);
        using (UnityWebRequest www = UnityWebRequest.Put($"{baseUrl}player/{id}", json))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Update Failed: " + www.error);
            }
        }
    }

    // API: [HttpDelete("{id}")]
    public IEnumerator DeletePlayer(int id)
    {
        using (UnityWebRequest www = UnityWebRequest.Delete($"{baseUrl}player/{id}"))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Player Deleted");
            }
        }
    }
}