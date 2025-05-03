using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Dropdown filterDropdown;
    public GameObject clientItemPrefab;
    public Transform contentPanel;
    public GameObject popupPanel;
    public Text popupName, popupPoints, popupAddress;

    private List<FullClient> allClients = new List<FullClient>();

    void Start()
    {
        filterDropdown.onValueChanged.AddListener(delegate { FilterList(); });
    }

    public void PopulateClientList(List<FullClient> clients)
    {
        allClients = clients;
        DisplayClients(clients);
    }

    void DisplayClients(List<FullClient> clients)
    {
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        foreach (var client in clients)
        {
            var item = Instantiate(clientItemPrefab, contentPanel);
            string label = client.label;
            if (client.isManager)
                label += " (Manager)";
            item.GetComponentInChildren<Text>().text = $"{label} - Points: {client.points}";
            item.GetComponent<Button>().onClick.AddListener(() => ShowPopup(client));
        }
    }

    void FilterList()
    {
        string filter = filterDropdown.options[filterDropdown.value].text;

        if (filter == "All")
            DisplayClients(allClients);
        else if (filter == "Managers only")
            DisplayClients(allClients.FindAll(c => c.isManager));
        else
            DisplayClients(allClients.FindAll(c => !c.isManager));
    }

    void ShowPopup(FullClient client)
    {
        popupName.text = client.name;
        popupPoints.text = "Points: " + client.points.ToString();
        popupAddress.text = "Address: " + client.address;

        popupPanel.transform.localScale = Vector3.zero;
        popupPanel.SetActive(true);
        popupPanel.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack);
    }

    public void ClosePopup()
    {
        popupPanel.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            popupPanel.SetActive(false);
        });
    }
}
