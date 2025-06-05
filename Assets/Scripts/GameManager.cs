using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//PUZZLE

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;
    [SerializeField] private GameObject originalImage, arMuseum; // Tam resim ve buton
    [SerializeField] private TextMeshProUGUI infoText; // UI Text component    
    [TextArea][SerializeField] private string paintingInfo;

    private List<Transform> pieces;
    private int emptyLocation;
    private int size;
    private bool shuffling = false;
    private bool gameCompleted = false;

    private void Start()
    {
        pieces = new List<Transform>();
        size = 2;

        CreateGamePieces(0.01f);

        if (originalImage != null) originalImage.SetActive(false);
        if (infoText != null) infoText.gameObject.SetActive(false);
        arMuseum.SetActive(false);

        StartCoroutine(Shuffle());
    }

    private void CreateGamePieces(float gapThickness)
    {
        float width = 1f / (float)size;
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);
                piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
                                                  +1 - (2 * width * row) - width,
                                                  0);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";

                if ((row == size - 1) && (col == size - 1))
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];
                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));
                    mesh.uv = uv;
                }
            }
        }
    }

    private void Update()
    {
        if (!shuffling && !gameCompleted && CheckCompletion())
        {
            gameCompleted = true;
            ShowOriginalImageAndText();
            return;
        }

        if (Input.GetMouseButtonDown(0) && !gameCompleted)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i] == hit.transform)
                    {
                        if (SwapIfValid(i, -size, size)) break;
                        if (SwapIfValid(i, +size, size)) break;
                        if (SwapIfValid(i, -1, 0)) break;
                        if (SwapIfValid(i, +1, size - 1)) break;
                    }
                }
            }
        }
    }

    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            (pieces[i].localPosition, pieces[i + offset].localPosition) =
            (pieces[i + offset].localPosition, pieces[i].localPosition);
            emptyLocation = i;
            return true;
        }
        return false;
    }

    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}") return false;
        }
        return true;
    }

    private IEnumerator Shuffle()
    {
        shuffling = true;
        int shuffleCount = 100;
        for (int i = 0; i < shuffleCount; i++)
        {
            int[] directions = new int[] { -size, +size, -1, +1 };
            List<int> validMoves = new List<int>();
            foreach (int dir in directions)
            {
                int target = emptyLocation + dir;
                if (target >= 0 && target < size * size)
                {
                    if ((dir == -1 && emptyLocation % size == 0) ||
                        (dir == 1 && emptyLocation % size == size - 1))
                        continue;

                    validMoves.Add(target);
                }
            }

            if (validMoves.Count > 0)
            {
                int swapWith = validMoves[Random.Range(0, validMoves.Count)];
                (pieces[emptyLocation], pieces[swapWith]) = (pieces[swapWith], pieces[emptyLocation]);
                (pieces[emptyLocation].localPosition, pieces[swapWith].localPosition) =
                (pieces[swapWith].localPosition, pieces[emptyLocation].localPosition);
                emptyLocation = swapWith;
            }

            yield return null;
        }

        shuffling = false;
        gameCompleted = false;
    }

    private void ShowOriginalImageAndText()
    {
        foreach (var piece in pieces)
        {
            piece.gameObject.SetActive(false);
        }

        if (originalImage != null)
            originalImage.SetActive(true);

        if (infoText != null)
        {
            infoText.gameObject.SetActive(true);
            infoText.text = paintingInfo;
        }

        arMuseum.SetActive(true);
    }
}
