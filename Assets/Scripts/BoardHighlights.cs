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
                    float factor = arObj.currentFactor;
                    Vector3 currentScale = (new Vector3(0.01f, 0.01f, 0.01f) * (float) factor);

                    float i_2 = (float)(i / 10);
                    float j_2 = (float)(j / 10);
                    Vector3 position = new Vector3(i_2 + 0.05f, 0.0001f, j_2 + 0.05f);

                    go.SetActive(true);

                    go.transform.position = origin + position;
                    go.transform.rotation = rotation;
                    go.transform.localScale = currentScale;
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
