using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum RoomType { Study, Game, Recovery }

public class RoomManager : MonoBehaviour
{
    public TextMeshProUGUI roomTitleText;
    public GameObject studyRoomPanel, gameRoomPanel, recoveryRoomPanel, textBookCategory, puzzleCategory;
    public GameObject gamePanel, mainMenuPanel;
    public Image background, topPanel, bottomPanel;
    public GameObject cube;

    private RoomType currentRoom = RoomType.Study;

    private void Start()
    {
        ShowRoom(currentRoom);
    }

    public void NextRoom()
    {
        currentRoom = (RoomType)(((int)currentRoom + 1) % 3);
        ShowRoom(currentRoom);
    }

    public void PreviousRoom()
    {
        currentRoom = (RoomType)(((int)currentRoom + 2) % 3); // +2 % 3 is like -1
        ShowRoom(currentRoom);
    }

    public void ShowRoom(RoomType room)
    {
        studyRoomPanel.SetActive(false);
        gameRoomPanel.SetActive(false);
        recoveryRoomPanel.SetActive(false);

        switch (room)
        {
            case RoomType.Study:
                roomTitleText.text = "Çalışma Odası";
                studyRoomPanel.SetActive(true); 
                background.color = new Color(221 / 255f, 246 / 255f, 210 / 255f);
                topPanel.color = new Color(176 / 255f, 219 / 255f, 156 / 255f);
                bottomPanel.color = new Color(176 / 255f, 219 / 255f, 156 / 255f);
                break;
            case RoomType.Game:
                roomTitleText.text = "Oyun Odası";
                gameRoomPanel.SetActive(true);
                background.color = new Color(255 / 255f, 192 / 255f, 217 / 255f);
                topPanel.color = new Color(255 / 255f, 144 / 255f, 188 / 255f);
                bottomPanel.color = new Color(255 / 255f, 144 / 255f, 188 / 255f);
                break;
            case RoomType.Recovery:
                roomTitleText.text = "Bakım Odası";
                recoveryRoomPanel.SetActive(true);
                background.color = new Color(219 / 255f, 223 / 255f, 234 / 255f);
                topPanel.color = new Color(172 / 255f, 177 / 255f, 214 / 255f);
                bottomPanel.color = new Color(172 / 255f, 177 / 255f, 214 / 255f);
                break;
        }
    }

    public void TextBookCategory()
    {
        gamePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        studyRoomPanel.SetActive(false);
        recoveryRoomPanel.SetActive(false);
        gameRoomPanel.SetActive(false);
        puzzleCategory.SetActive(false);
        textBookCategory.SetActive(true);        
    }

    public void PuzzleCategory()
    {
        gamePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        studyRoomPanel.SetActive(false);
        recoveryRoomPanel.SetActive(false);
        gameRoomPanel.SetActive(false);        
        textBookCategory.SetActive(false);
        puzzleCategory.SetActive(true);
    }

    public void ReturnStudyRoom()
    {
        gamePanel.SetActive(true);
        studyRoomPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        textBookCategory.SetActive(false);
    }

    public void ReturnGameRoom()
    {
        gamePanel.SetActive(true);
        gameRoomPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        puzzleCategory.SetActive(false);
    }

    public void OpenGame()
    {
        gamePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}