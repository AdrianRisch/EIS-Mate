using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour
{

    public static BoardHighlights Instance { set; get; }

    public GameObject highlightPrefab;
    private List<GameObject> highlights;
    private float factor = 1.0f;
    private Vector3 scale;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
        scale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    private GameObject GetHighLightObject()
    {
        //spawn highlight object
        GameObject go = highlights.Find(g => !g.activeSelf);

        if (go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }

    // highlight each possible move 
    public void HighLightAllowedMoves(bool[,] moves)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                // if possible move
                if (moves[i, j])
                {
                    // make highlight
                    GameObject go = GetHighLightObject();

                    // get current position of chessboard
                    GameObject arSession = GameObject.Find("AR Session Origin");
                    ARTapToPlaceObject arObj = arSession.GetComponent<ARTapToPlaceObject>();
                    Vector3 origin = arObj.boardPos;

                    go.SetActive(true);

                    // scale highlight prefab
                    go.transform.localScale = scale;

                    // get position on board 
                    Vector3 position = new Vector3(i + 0.5f, 0.0001f, j + 0.5f);
                    position /= 10;
                    //scale
                    position *= factor;
                    //set position om board
                    go.transform.position = origin + position;
                }
            }

        }
    }
    // to set current factor when scaled
    public void setFactor(float factor)
    {
        this.factor *= factor;
        scale *= factor;
    }
    // to set current scale when scaled
    public void setScale(Vector3 scale)
    {
        this.scale = scale;
    }
    // to hide the highlights
    public void HideHighlights()
    {
        foreach (GameObject go in highlights)
            go.SetActive(false);
    }
}
