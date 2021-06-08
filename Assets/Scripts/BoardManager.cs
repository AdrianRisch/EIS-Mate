using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    private bool [,] allowedMoves { set; get; }

    public Chessman [,] Chessmans { set; get; }
    private Chessman selectedChessman;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chesspiecesPrefabs;
    private List<GameObject> activeChesspiece;

    private Quaternion orientation = Quaternion.Euler(0, 90, 0);

    public bool isWhiteTurn = true;

    private void Start()
    {
        Instance = this;
        SpawnAllChesspieces();
    }

    private void Update()
    {
        UpdateSelection();
        DrawChessboard();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if(selectedChessman== null)
                {
                    //select the chessman
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    //move the Chessman
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
    }

    private void SelectChessman(int x, int y)
    {
        if(Chessmans[x,y]== null)
            return;
     
        if (Chessmans[x, y].isWhite != isWhiteTurn)
            return;

        allowedMoves = Chessmans[x, y].PossibleMove();
        selectedChessman = Chessmans[x, y];
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
        
    }

    private void MoveChessman(int x, int y)
    {
        if (allowedMoves[x,y])
        {
            Chessman c = Chessmans[x, y];

            if(c != null && c.isWhite != isWhiteTurn)
            {
                //Capture a piece

                //If it is the king
                if(c.GetType() == typeof(King))
                {
                    //End the game
                    return;
                }
                activeChesspiece.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            Chessmans [selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;
            isWhiteTurn = !isWhiteTurn;
        }
        BoardHighlights.Instance.Hidehighlights();
        selectedChessman = null;
    }
    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        } 
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }
    private void SpawnChesspiece(int index, int x, int y )
    {
        GameObject go = Instantiate(chesspiecesPrefabs[index], GetTileCenter(x,y), orientation) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChesspiece.Add(go);
    }
    private void SpawnAllChesspieces()
    {
        activeChesspiece = new List<GameObject>();
        Chessmans = new Chessman[8, 8];
        //Spawn White 
        //King
        SpawnChesspiece(0, 3, 0);
        //Queen
        SpawnChesspiece(1, 4, 0);
        //Rooks
        SpawnChesspiece(2, 0, 0);
        SpawnChesspiece(2, 7, 0);
        //Bishops
        SpawnChesspiece(3, 2, 0);
        SpawnChesspiece(3, 5, 0);
        //Knights
        SpawnChesspiece(4, 1, 0); 
        SpawnChesspiece(4, 6, 0);
        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChesspiece(5, i, 1);
        }
        //Spawn Black
        //King
        SpawnChesspiece(6, 4, 7);
        //Queen
        SpawnChesspiece(7, 3, 7);
        //Rooks
        SpawnChesspiece(8, 0, 7);
        SpawnChesspiece(8, 7, 7);
        //Bishops
        SpawnChesspiece(9, 2, 7);
        SpawnChesspiece(9, 5, 7);
        //Knights
        SpawnChesspiece(10, 1, 7);
        SpawnChesspiece(10, 6, 7);
        //Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChesspiece(11, i, 6);
        }
    }
    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }
    private void DrawChessboard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for(int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }
        //Draw the selection
        if(selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }
}
