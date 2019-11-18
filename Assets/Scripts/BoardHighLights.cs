using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighLights : MonoBehaviour
{
    public static BoardHighLights Instance { get; set; }

    public GameObject canGoPrefab;
    public GameObject underAttackPrefab;
    private List<GameObject> highlights;


    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    private GameObject GetHighLightObject(GameObject highLight)
    {
        GameObject go = highlights.Find(g => !g.activeSelf);

        go = Instantiate(highLight);
        highlights.Add(go);
        
        return go;
    }

    public void HighlightAllowedMoves(bool[,] moves)
    {
        GameObject go;
        for (int i=0; i<8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    Debug.ClearDeveloperConsole();
                    Chessman c = BoardManager.Instance.Chessmans[i, j];
                    if (c != null)
                    {
                        go = GetHighLightObject(underAttackPrefab);
                    }
                    else 
                    { 
                        go = GetHighLightObject(canGoPrefab);
                    }

                    go.SetActive(true);
                    go.transform.position = new Vector3(i+0.5f, 0, j+0.5f);

                    Debug.Log(go.transform.position.ToString());
                }
            }
        }
    }


    public void Hidehighlights()
    {
        foreach(var go in highlights)
        {
            go.SetActive(false);
        }
    }

}
