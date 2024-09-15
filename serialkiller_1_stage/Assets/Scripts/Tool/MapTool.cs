using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapTool : MonoBehaviour {

    public PlaceManager m_PlaceManager;

    public float m_XValue;
    public float m_YValue;
    public float m_XIntervalValue;
    public float m_YIntervalValue;

    public TextAsset m_TextAsset;
    public GameObject m_Map;
    public GameObject m_Home;
    public GameObject m_Company;
    public GameObject m_Extra;
    public GameObject m_Case;
    public GameObject m_Road;
    public GameObject m_Default;

    public PlaceItem m_Original;

    public List<GameObject> m_List;
    private List<PlaceItem> m_HomeList;
    private List<PlaceItem> m_CompanyList;
    private List<PlaceItem> m_ExtraList;
    private List<PlaceItem> m_CaseList;
    private List<PlaceItem> m_RoadList;

    public void Setting()
    {
        print("Setting Start");
        m_PlaceManager = PlaceManager.instance;
        m_Map = GameObject.Find("GameMap").transform.Find("Map").gameObject;
        m_Home = m_Map.transform.Find("Home").gameObject;
        m_Company = m_Map.transform.Find("Company").gameObject;
        m_Extra = m_Map.transform.Find("Extra").gameObject;
        m_Case = m_Map.transform.Find("Case").gameObject;
        m_Road = m_Map.transform.Find("Road").gameObject;
        m_Default = m_Map.transform.Find("Default").gameObject;

        print("Setting End");
    }

    public void Draw()
    {
        Delete();

        print("Draw Start");
        m_List = new List<GameObject>();
        m_HomeList = new List<PlaceItem>();
        m_CompanyList = new List<PlaceItem>();
        m_ExtraList = new List<PlaceItem>();
        m_CaseList = new List<PlaceItem>();
        m_RoadList = new List<PlaceItem>();

        TextReader tr = new StringReader(m_TextAsset.text);

        string[] str;
        string line;
        PlaceItem go;

       for (int i = 0; i < m_TextAsset.text.Length - 1; i++)
        {
            line = tr.ReadLine();
            if (line == null)
                break;

            str = line.Split(',');
            for (int k = 0; k < str.Length; k++)
            {
                go = Instantiate(m_Original);
                go.transform.parent = m_Map.transform;
                go.transform.localScale = Vector3.one;
                //go.transform.localPosition = new Vector2(((k/ 2f) * (m_XIntervalValue + m_XValue)) - (i * m_XValue)/2f, -(k / 2f) * (m_YValue + m_YIntervalValue) - (i * m_YValue)/2f );
               

                /*if (str[k] == null || str[k] == "")
                {
                    DestroyImmediate(go.m_Sprite.gameObject.GetComponent<BoxCollider>());
                    go.m_Sprite.enabled = false;
                }
                else */if (str[k].Contains("-"))
                {
                    string[] file = str[k].Split('-');
                    go.m_Frame.spriteName = go.m_Sprite.spriteName = file[0];
                    int p;
                    switch (file[1])
                    {
                        case "Home":
                            go.m_Type = PlaceType.Home;
                            go.DataInitialize(int.Parse(file[2]));
                            m_HomeList.Add(go);
                            go.transform.parent = m_Home.transform;
                            go.name = "Home_" + file[2];

                            p = (int.Parse(file[0])) / 100;
                            if (p > 0)
                                p--;
                            go.m_Coliider[p].gameObject.SetActive(true);

                            break;
                        case "Company":
                            go.m_Type = PlaceType.Company;
                            go.DataInitialize(int.Parse(file[2]));
                            m_CompanyList.Add(go);
                            go.transform.parent = m_Company.transform;
                            go.name = "Company_" + file[2];

                            p = (int.Parse(file[0])) / 100;
                            if (p > 0)
                                p--;
                            go.m_Coliider[p].gameObject.SetActive(true);
                            break;

                        case "Extra":
                            go.m_Type = PlaceType.Extra;
                            go.DataInitialize(int.Parse(file[2]));
                            m_ExtraList.Add(go);
                            go.transform.parent = m_Extra.transform;
                            go.name = "Extra_" + file[2];

                            p = (int.Parse(file[0])) / 100;
                            if (p > 0)
                                p--;
                            go.m_Coliider[p].gameObject.SetActive(true);
                            break;

                        case "Case":
                            go.m_Type = PlaceType.Case;
                            go.m_CaseIndex = file[2];
                            m_CaseList.Add(go);
                            go.m_Frame.gameObject.SetActive(false);
                            go.transform.parent = m_Case.transform;
                            go.name = "Case_" + file[2];

                            p = (int.Parse(file[0])) / 100;
                            if (p > 0)
                                p--;
                            go.m_Coliider[p].gameObject.SetActive(true);
                            break;

                        case "Road":
                            go.m_Type = PlaceType.Road;
                            go.m_Sprite.depth = -1;
                            go.DataInitialize(int.Parse(file[2]));
                            m_RoadList.Add(go);
                            go.transform.parent = m_Road.transform;
                            go.name = "Road_" + file[2];
                            break;
                        default:
                            go.m_Type = PlaceType.Default;
                            go.transform.parent = m_Default.transform;
                            go.name = "Default_";
                            for (int w = 0; w < go.m_Coliider.Length; w++)
                            {
                                go.m_Coliider[w].enabled = false;
                            }
                         //   go.m_Coliider[ gameObject.GetComponent<BoxCollider>().enabled = false;
                         //   go.m_Touch.gameObject.GetComponent<BoxCollider>().enabled = false;

                            break;
                    }


                }
                else
                {
                    go.m_Sprite.spriteName = str[k];
                    go.m_Type = PlaceType.Default;
                    go.m_Sprite.depth = -1;
                    go.transform.parent = m_Default.transform;
                    go.name = "Default_";
                    //DestroyImmediate(go.m_Sprite);
                    DestroyImmediate(go.m_NameLabel);
                    DestroyImmediate(go.m_Frame.gameObject);

                    for (int p = 0; p < go.m_Coliider.Length; p++)
                    {
                        DestroyImmediate(go.m_Coliider[p].gameObject);
                    }
                    //DestroyImmediate(go.m_Sprite.gameObject.GetComponent<BoxCollider>());
                    //    DestroyImmediate(go.GetComponent<PlaceItem>());

                }
                go.transform.localPosition = new Vector2((k * m_XValue / 2f ) - (i * m_XValue  / 2f) , -((k * m_YValue/2f) + (i * m_YValue / 2f)));// - (m_YIntervalValue * k));//+ (i * m_YValue - m_YIntervalValue));
                //go.transform.localPosition = new Vector2(((k / 2f) * m_XValue)  + (m_XIntervalValue * k ) + (i * m_XValue) / 2f, ((k / 2f) * m_YValue ) - (m_YIntervalValue * k) - (i * m_YValue) / 2f);

                if (go.m_Frame != null)
                {
                    go.m_Sprite.MakePixelPerfect();
                    go.m_Frame.MakePixelPerfect();
                    go.m_Frame.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                }
                else
                {
                    go.m_Sprite.width = (int)m_XValue;
                    go.m_Sprite.height = (int)m_YValue;
                }

                m_List.Add(go.gameObject);

                if (str[k].Contains("-") == false)
                {
                    DestroyImmediate(go.GetComponent<PlaceItem>());
                }
            }
        }

        for (int i = 0; i < m_HomeList.Count; i++)
        {
            for (int k = 0; k < m_HomeList.Count; k++)
            {
                if (m_HomeList[i].name == m_HomeList[k].name && i != k)
                {
                    DestroyImmediate(m_HomeList[k].gameObject);
                    m_HomeList.RemoveAt(k);
                    k--;
                }
            }
        }

        for (int i = 0; i < m_HomeList.Count; i++)
        {
            for (int k = 0; k < m_HomeList.Count; k++)
            {
                if (i == m_HomeList[k].ReturnIndex())
                {
                    m_PlaceManager.m_HomeList.Add(m_HomeList[k]);
                }
            }
        }

        for (int i = 0; i < m_CompanyList.Count; i++)
        {
            for (int k = 0; k < m_CompanyList.Count; k++)
            {
                if (m_CompanyList[i].name == m_CompanyList[k].name && i != k)
                {
                    DestroyImmediate(m_CompanyList[k].gameObject);
                    m_CompanyList.RemoveAt(k);
                    k--;
                }
            }
        }

        for (int i = 0; i < m_CompanyList.Count; i++)
        {
            for (int k = 0; k < m_CompanyList.Count; k++)
            {
                if (i == m_CompanyList[k].ReturnIndex())
                {
                    m_PlaceManager.m_CompanyList.Add(m_CompanyList[k]);
                }
            }
        }

        for (int i = 0; i < m_CaseList.Count; i++)
        {
            for (int k = 0; k < m_CaseList.Count; k++)
            {
                if (m_CaseList[i].name == m_CaseList[k].name && i != k)
                {
                    DestroyImmediate(m_CaseList[k].gameObject);
                    m_CaseList.RemoveAt(k);
                    k--;
                }
            }
        }

        for (int i = 0; i < m_CaseList.Count; i++)
        {
            for (int k = 0; k < m_CaseList.Count; k++)
            {
                if (i == int.Parse(m_CaseList[k].m_CaseIndex))
                {
                    m_PlaceManager.m_CasePlaceList.Add(m_CaseList[k]);
                }
            }
        }

        for (int i = 0; i < m_ExtraList.Count; i++)
        {
            for (int k = 0; k < m_ExtraList.Count; k++)
            {
                if (m_ExtraList[i].name == m_ExtraList[k].name && i != k)
                {
                    DestroyImmediate(m_ExtraList[k].gameObject);
                    m_ExtraList.RemoveAt(k);
                    k--;
                }
            }
        }

        for (int i = 0; i < m_ExtraList.Count; i++)
        {
            for (int k = 0; k < m_ExtraList.Count; k++)
            {
                if (i == m_ExtraList[k].ReturnIndex())
                {
                    m_PlaceManager.m_ExtraPlaceList.Add(m_ExtraList[k]);
                }
            }
        }


        print("Draw End");
    }

    public void Delete()
    {
        print("Delete Start");
        if (m_Map == null)
            return;
        if (m_Home == null)
            return;
        if (m_Company == null)
            return;
        if (m_Extra == null)
            return;
        if (m_Case == null)
            return;
        if (m_Road == null)
            return;

        m_PlaceManager.m_HomeList = new List<PlaceItem>();
        m_PlaceManager.m_CompanyList = new List<PlaceItem>();
        m_PlaceManager.m_ExtraPlaceList = new List<PlaceItem>();
        m_PlaceManager.m_CasePlaceList = new List<PlaceItem>();

        for (int i = 0; i < m_List.Count; ++i)
        {
            DestroyImmediate(m_List[i]);
        }

        print("Delete End");
    }
}
