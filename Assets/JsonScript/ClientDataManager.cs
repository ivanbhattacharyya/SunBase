using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ClientDataManager : MonoBehaviour
{
    public string apiUrl = "https://qa.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";
    public UIManager uiManager;

    void Start()
    {
        StartCoroutine(FetchClientData());
    }

    IEnumerator FetchClientData()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("Raw JSON: " + json);

            var jObj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

// Deserialize clients list
var clients = jObj["clients"].ToObject<List<ClientHeader>>();

// Deserialize dictionary without custom converters
var safeSettings = new JsonSerializerSettings { Converters = new List<JsonConverter>() };
var data = JsonConvert.DeserializeObject<Dictionary<string, ClientDetails>>(jObj["data"].ToString(), safeSettings);

// Combine into full list
List<FullClient> fullClients = new List<FullClient>();
foreach (var clientHeader in clients)
{
    if (data.TryGetValue(clientHeader.id.ToString(), out var details))
    {
        fullClients.Add(new FullClient
        {
            id = clientHeader.id,
            label = clientHeader.label,
            isManager = clientHeader.isManager,
            name = details.name,
            address = details.address,
            points = details.points
        });
    }
}



            uiManager.PopulateClientList(fullClients);
        }
        else
        {
            Debug.LogError("API Error: " + request.error);
        }
    }
}


[System.Serializable]
public class RootObject
{
    public List<ClientHeader> clients;
    public Dictionary<string, ClientDetails> data;
    public string label;
}

[System.Serializable]
public class ClientHeader
{
    public int id;
    public string label;
    public bool isManager;
}

[System.Serializable]
public class ClientDetails
{
    public string name;
    public string address;
    public int points;
}

[System.Serializable]
public class FullClient
{
    public int id;
    public string label;
    public bool isManager;
    public string name;
    public string address;
    public int points;
}
