using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryCardManager : MonoBehaviour
{
    public static MemoryCardManager Instance;

    public GameObject cardPrefab;
    public Transform cardParent;
    public Material[] frontMaterials; // 10 adet
    public Material backMaterial;

    private List<Card> revealedCards = new List<Card>();
    private List<int> cardIDs = new List<int>();

    private int currentPlayer = 1;
    private int player1Score = 0;
    private int player2Score = 0;

    private int totalPairs = 10;

    public TextMeshProUGUI playerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultText;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        GenerateCardIDs();
        SpawnCards();
        UpdateUI();
    }

    void GenerateCardIDs()
    {
        // 10 çifti iki kez ekle (0-9)
        for (int i = 0; i < totalPairs; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }

        // Karýþtýr
        for (int i = 0; i < cardIDs.Count; i++)
        {
            int rnd = Random.Range(0, cardIDs.Count);
            (cardIDs[i], cardIDs[rnd]) = (cardIDs[rnd], cardIDs[i]);
        }
    }

    void SpawnCards()
    {
        int index = 0;
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                Vector3 position = new Vector3(col * 2f, 0, row * 2f);
                GameObject cardObj = Instantiate(cardPrefab, position, Quaternion.identity, cardParent);
                Card card = cardObj.GetComponent<Card>();
                int cardId = cardIDs[index];
                card.SetupCard(cardId, frontMaterials[cardId], backMaterial);
                index++;
            }
        }
    }

    public void OnCardClicked(Card clickedCard)
    {
        if (revealedCards.Count >= 2) return;

        clickedCard.ShowFront();
        revealedCards.Add(clickedCard);

        if (revealedCards.Count == 2)
        {
            StartCoroutine(CheckMatch());
        }
    }

    System.Collections.IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1f);

        Card card1 = revealedCards[0];
        Card card2 = revealedCards[1];

        if (card1.cardId == card2.cardId)
        {
            card1.SetMatched();
            card2.SetMatched();

            if (currentPlayer == 1)
                player1Score++;
            else
                player2Score++;
        }
        else
        {
            card1.ShowBack();
            card2.ShowBack();
            currentPlayer = (currentPlayer == 1) ? 2 : 1;
        }

        revealedCards.Clear();
        CheckGameOver();
        UpdateUI();
    }

    void UpdateUI()
    {
        playerText.text = $"{currentPlayer}. Oyuncunun Sırası";
        scoreText.text = $"1: {player1Score} | 2: {player2Score}";
    }

    void CheckGameOver()
    {
        if (player1Score + player2Score == totalPairs)
        {
            resultText.gameObject.SetActive(true); // yazýyý göster

            if (player1Score > player2Score)
                resultText.text = "1. Oyuncu Kazandı!";
            else if (player2Score > player1Score)
                resultText.text = "2. Oyuncu Kazandı!";
            else
                resultText.text = "Beraberlik!";
        }
    }
}
