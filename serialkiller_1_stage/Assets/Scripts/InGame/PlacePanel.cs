using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePanel : MonoBehaviour
{
    public GameObject characterContent;
    public GameObject infoContent;

    public PlaceItem m_item;

    public UILabel title;
    public UILabel content;
    public UISprite profile;
    public UILabel profileContent;

    public GameObject table;
    public GameObject tabItem;

    public PlaceTabItem selected;

    private enum KeyType
    {
        PLACE_TITLE,
        PLACE_INFO,
        PLACE_BUTTON,
    }
    private string[] keyTitle = { "Place_{0}_{1}", "Place_{0}_{1}_Info"};
    private string GetKey(KeyType type)
    {
        string placeName = m_item.m_Type.ToString() + "_" + m_item.ReturnIndex();
        switch (type)
        {
            case KeyType.PLACE_TITLE:
                return string.Format(keyTitle[(int)KeyType.PLACE_TITLE]
                    , PlayDataManager.instance.m_StageName
                    , placeName);

            case KeyType.PLACE_INFO:
                return string.Format(keyTitle[(int)KeyType.PLACE_INFO]
                    , PlayDataManager.instance.m_StageName
                    , placeName);

            case KeyType.PLACE_BUTTON:
                return string.Format(keyTitle[(int)KeyType.PLACE_TITLE]
                    , PlayDataManager.instance.m_StageName
                    , placeName);

            default:
                return string.Empty;
        }
    }

    public void Init(PlaceItem data)
    {
        m_item = data;
        //string placeName = m_item.m_Type.ToString() + "_" + m_item.ReturnIndex();
        title.text = Localization.Get(GetKey(KeyType.PLACE_TITLE));
        content.text = Localization.Get(GetKey(KeyType.PLACE_INFO));
        int childCount = table.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(table.transform.GetChild(0).gameObject);
        }
        int index = 0;
        BuildTab(index++, 0, title.text).Select();
        foreach (var character in m_item.m_CharacterList)
        {
            BuildTab(index++, 1, character);
        }
    }

    public void Dialog()
    {
        Action("Dialog");
    }
    public void Suggest()
    {
        Action("Suggest");
    }
    public void Action(string command)
    {
        PlaceManager.instance.m_Mode = command;
        PlaceManager.instance.ClickFace(selected.content);
    }

    PlaceTabItem BuildTab(int index, int type, string data)
    {
        GameObject tab = Instantiate(tabItem, table.transform);
        tab.transform.localPosition = new Vector2(0, -1 * 100 * index);
        tab.SetActive(true);
        tab.GetComponent<PlaceTabItem>().Init(type, data);
        return tab.GetComponent<PlaceTabItem>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}