﻿using System.Collections;
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
                    float i_2 = (float)i / 10;
                    float j_2 = (float)j / 10;
                    GameObject go = GetHighLightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i_2 + 0.05f, 0.0001f, j_2 + 0.05f);
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
