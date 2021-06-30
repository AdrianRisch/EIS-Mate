using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using System.Security.Cryptography;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; set; }
    private bool[,] allowedMoves { get; set; }

    private const float TILE_SIZE = 0.1f;
    private const float TILE_OFFSET = 0.05f;
    private float factor = 1.0f;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;

    private Quaternion whiteOrientation = Quaternion.Euler(0, 270, 0);
    private Quaternion blackOrientation = Quaternion.Euler(0, 90, 0);


    public Chessman[,] Chessmans { get; set; }
    private Chessman selectedChessman;

    public bool isWhiteTurn = true;

    private Material previousMat;
    public Material selectedMat;

    private GameOverScreen screen;

    public int[] EnPassantMove { set; get; }

    void Awake()
    {
        // Initialize all important parts
        // Boardmanager instance
        Instance = this;
        // spawn the chess pieces
        SpawnAllChessmans();
        // en passant move
        EnPassantMove = new int[2] { -1, -1 };
        // speech recognition
        SpeechToText.instance.onResultCallback = onResultCallback;

        screen = GameObject.Find("CanvasObject/Canvas/EndGameCanvas").GetComponent<GameOverScreen>();
        Debug.Log("Screen Object: " + screen.ToString());
    }

    // check for voice input
    void onResultCallback(string _data)
    {
        Debug.Log("Input: " + _data);
        int xPos = 10;

        // take first substring and check if its A-H 
        // -> set to corresponding index as xPos
        switch (_data.Substring(0, 1))
        {
            //Uppercase
            case "A":
                xPos = 0;
                break;
            case "B":
                xPos = 1;
                break;
            case "C":
                xPos = 2;
                break;
            case "D":
                xPos = 3;
                break;
            case "E":
                xPos = 4;
                break;
            case "F":
                xPos = 5;
                break;
            case "G":
                xPos = 6;
                break;
            case "H":
                xPos = 7;
                break;
                //Lowercase
            case "a":
                xPos = 0;
                break;
            case ("b"):
                xPos = 1;
                break;
            case "c":
                xPos = 2;
                break;
            case "d":
                xPos = 3;
                break;
            case "e":
                xPos = 4;
                break;
            case "f":
                xPos = 5;
                break;
            case "g":
                xPos = 6;
                break;
            case "h":
                xPos = 7;
                break;
            default:
                break;
        }

        // get 2nd substring and set as yPos
        int yPos = Int32.Parse(_data.Substring(1, 1));

        // check if xPos and yPos are valid numbers
        if (xPos >= 0 && xPos <= 7 && yPos >= 1 && yPos <= 8)
        {
            // if no chess piece is selected, select the one on that spot
            if (selectedChessman == null)
            {
                // Select the chessman
                SelectChessman(xPos, yPos - 1);
            }
            // if one is already selected, move that piece to that spot
            else
            {
                // Move the chessman
                MoveChessman(xPos, yPos - 1);
            }
        }
    }

    private void SelectChessman(int x, int y)
    {
        // if there is no chess piece return
        if (Chessmans[x, y] == null) return;

        // if the color of the chess piece is not the same as the person whos turn it is return
        if (Chessmans[x, y].isWhite != isWhiteTurn) return;

        bool hasAtLeastOneMove = false;

        // check for possible moves
        allowedMoves = Chessmans[x, y].PossibleMoves();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    hasAtLeastOneMove = true;
                    i = 8;
                    break;
                }
            }
        }

        // if no moves possible return
        if (!hasAtLeastOneMove)
            return;

        // select chess piece and highlight it 
        selectedChessman = Chessmans[x, y];
        previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;

        // highlight possible moves
        BoardHighlights.Instance.HighLightAllowedMoves(allowedMoves);
    }

    private void MoveChessman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            Chessman c = Chessmans[x, y];

            if (c != null && c.isWhite != isWhiteTurn)
            {
                // Capture a piece

                if (c.GetType() == typeof(King))
                {
                    // End the game
                    Debug.Log("EndGame Start");
                    EndGame();
                    // return;
                }
                Debug.Log("Remove piece");
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            // en passant move
            if (x == EnPassantMove[0] && y == EnPassantMove[1])
            {
                if (isWhiteTurn)
                    c = Chessmans[x, y - 1];
                else
                    c = Chessmans[x, y + 1];

                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;
            
            // Pawn Promotion
            if (selectedChessman.GetType() == typeof(Pawn))
            {
                if (y == 7) // White Promotion
                {
                    activeChessman.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnChessman(1, x, y, true);
                    selectedChessman = Chessmans[x, y];
                }
                else if (y == 0) // Black Promotion
                {
                    activeChessman.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnChessman(7, x, y, false);
                    selectedChessman = Chessmans[x, y];
                }
                EnPassantMove[0] = x;
                if (selectedChessman.CurrentY == 1 && y == 3)
                    EnPassantMove[1] = y - 1;
                else if (selectedChessman.CurrentY == 6 && y == 4)
                    EnPassantMove[1] = y + 1;
            }

            // move selected chess piece to new spot
            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;

            // change whos turn it is
            isWhiteTurn = !isWhiteTurn;
        }

        Deselect();
    }

    private void SpawnChessman(int index, int x, int y, bool isWhite)
    {
        Vector3 position = GetTileCenter(x, y);
        GameObject go;

        // get current position, rotation and scale of the chessboard
        GameObject arSession = GameObject.Find("AR Session Origin");
        ARTapToPlaceObject arObj = arSession.GetComponent<ARTapToPlaceObject>();
        Vector3 origin = arObj.boardPos;
        Quaternion rotation = arObj.boardRot;
        this.transform.SetPositionAndRotation(origin, rotation);

        // instantiate white pieces with white orientation and black pieces whit black orientation
        if (isWhite)
        {
            go = Instantiate(chessmanPrefabs[index], position, whiteOrientation) as GameObject;
        }
        else
        {
            go = Instantiate(chessmanPrefabs[index], position, blackOrientation) as GameObject;
        }

        //  set chessboard as parent of pieces and set position
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        // get current position of the chessboard
        Vector3 origin = Vector3.zero;
        GameObject arSession = GameObject.Find("AR Session Origin");
        ARTapToPlaceObject arObj = arSession.GetComponent<ARTapToPlaceObject>();
        Vector3 boardPos= arObj.boardPos;

        Debug.Log("Faktor: " + factor);

        // scale tile size and offset
        float scaledTileSize = TILE_SIZE * factor;
        float scaledTileOffset = TILE_OFFSET * factor;

        // set position with scaled offset and tile size
        origin.x = boardPos.x + (scaledTileSize * x) + scaledTileOffset;
        origin.z = boardPos.z + (scaledTileSize * y) + scaledTileOffset;
        origin.y = boardPos.y;

        return origin;
    }

    // to set scale factor from ARTapToPlaceObject
    public void setFactor(float factor)
    {
        this.factor *= factor;
    }

    // spawn all the pieces on the correct tile
    private void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];

        // White

        // King
        SpawnChessman(0, 4, 0, true);

        // Queen
        SpawnChessman(1, 3, 0, true);

        // Rooks
        SpawnChessman(2, 0, 0, true);
        SpawnChessman(2, 7, 0, true);

        // Bishops
        SpawnChessman(3, 2, 0, true);
        SpawnChessman(3, 5, 0, true);

        // Knights
        SpawnChessman(4, 1, 0, true);
        SpawnChessman(4, 6, 0, true);

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(5, i, 1, true);
        }


        // Black

        // King
        SpawnChessman(6, 4, 7, false);

        // Queen
        SpawnChessman(7, 3, 7, false);

        // Rooks
        SpawnChessman(8, 0, 7, false);
        SpawnChessman(8, 7, 7, false);

        // Bishops
        SpawnChessman(9, 2, 7, false);
        SpawnChessman(9, 5, 7, false);

        // Knights
        SpawnChessman(10, 1, 7, false);
        SpawnChessman(10, 6, 7, false);

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(11, i, 6, false);
        }
    }

    // End Game Screen
    private void EndGame()
    {
        Debug.Log("EndGame called");
        // Victory
        if (isWhiteTurn)
        {
            screen.Setup("WHITE TEAM WINS");
        } 
        // Defeat
        else
        {
            screen.Setup("BLACK TEAM WINS");
        }
    }
    public void Restart()
    {
        // Remove all Pieces
        foreach (GameObject go in activeChessman)
        {
            Destroy(go);
        }
        // Remove Chessboard
        GameObject chessboard = GameObject.Find("Chessboard");
        Destroy(chessboard);
        // Reset Spawned Object so it will spawn a new one on first tap
        GameObject arSession = GameObject.Find("AR Session Origin");
        ARTapToPlaceObject arObj = arSession.GetComponent<ARTapToPlaceObject>();
        arObj.resetSpawnedObject();

        // Reset Game
        isWhiteTurn = true;
        BoardHighlights.Instance.HideHighlights();
    }

    public void Deselect()
    {
        if(selectedChessman != null)
        {
            // deselect chess piece
            selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
            BoardHighlights.Instance.HideHighlights();
            selectedChessman = null;
        }
    }

}