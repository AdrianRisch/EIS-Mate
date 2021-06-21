using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour
{

    public static BoardHighlights Instance { set; get; }

    public GameObject highlightPrefab;
    private List<GameObject> highlights;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    private GameObject GetHighLightObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);

        if (go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }

    public void HighLightAllowedMoves(bool[,] moves)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    GameObject go = GetHighLightObject();

                    GameObject arSession = GameObject.Find("AR Session Origin");
                    ARTapToPlaceObject arObj = arSession.GetComponent<ARTapToPlaceObject>();
                    Vector3 origin = arObj.boardPos;
                    Quaternion rotation = arObj.boardRot;
                    Vector3 scale = arObj.boardScale;

                    go.SetActive(true);

                    Vector3 position = new Vector3(i + 0.5f, 0.0001f, j + 0.5f);
                    position /= 10;

                    go.transform.position = origin + position;
                    //go.transform.rotation = rotation;
                    //go.transform.localScale = scale;
                }
            }

        }
    }

    public void HideHighlights()
    {
        foreach (GameObject go in highlights)
            go.SetActive(false);
    }
}
