using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private int LevelTimer = 30;

    [SerializeField] private Canvas StartPanel;
    [SerializeField] private Canvas WinPanel;
    [SerializeField] private Canvas LossPanel;
    [SerializeField] private Text StartPanelLevelText;
    [SerializeField] private Text LossText;

    public GameStatus Status { get; private set; } = GameStatus.NotStarted;

    [Inject] private FiguresController figuresController;
    [Inject] private BagController bagController;
    [Inject] private Timer timer;

    public int Level { get; private set; } = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        InitLevel();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void InitLevel()
    {
        WinPanel.enabled = false;
        LossPanel.enabled = false;
        StartPanelLevelText.text = "Уровень " + Level;
        StartPanel.enabled = true;
        Status = GameStatus.NotStarted;
        bagController.Clear();
        figuresController.ClearLevel();
        figuresController.GenerateSequence();
        timer.ClearTimer();
        Invoke("StartLevel", 1f);
    }

    private void StartLevel()
    {
        WinPanel.enabled = false;
        LossPanel.enabled = false;
        StartPanel.enabled = false;
        Status = GameStatus.Started;
        timer.StartTimer(LevelTimer);
    }

    public void InitNextLevel()
    {
        Level++;
        InitLevel();
    }

    public void RestartLevel()
    {
        InitLevel();
    }

    public void RestartGame()
    {
        Level=1;
        InitLevel();
    }

    public void LossGame(LossGameType type)
    {
        timer.ClearTimer();
        if (type == LossGameType.Timer)
            LossText.text = "Время вышло...";
        else if (type == LossGameType.BagOverflow)
            LossText.text = "Мешок заполнен...";

        WinPanel.enabled = false;
        LossPanel.enabled = true;
        StartPanel.enabled = false;
        Status = GameStatus.Ended;
    }

    public void WinGame()
    {
        timer.ClearTimer();
        WinPanel.enabled = true;
        LossPanel.enabled = false;
        StartPanel.enabled = false;
        Status = GameStatus.Ended;
    }

    public enum GameStatus
    {
        NotStarted,
        Started,
        Ended,
    }

    public enum LossGameType
    {
        Timer,
        BagOverflow,
    }
}
