using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;

public class FiguresController : MonoBehaviour
{
    [SerializeField] private int FiguresCountBase = 30;
    [SerializeField] private int FiguresCountIncrementByLevel = 6;
    [SerializeField] private float GenerateDelay = 0.1f;
    public float SpawnImpulseStrength = 2f;
    [SerializeField] private GameObject[] FigurePrefabs;
    [SerializeField] private Color[] Colors;
    [SerializeField] private Sprite[] Animals;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private GameObject FiguresRoot;

    [Inject] private GameController gameController;
    [Inject] private BagController bagController;

    private List<Figure> FigureSequence = new();
    private List<Figure> FigureForSpawn = new();

    private float lastGenerateTime = 0;

    private void Update()
    {
        if(gameController.Status == GameController.GameStatus.Started && FigureForSpawn.Count > 0 && Time.time - lastGenerateTime > GenerateDelay)
        {
            GameObject instance = FigureForSpawn[0].Spawn();
            instance.transform.parent = FiguresRoot.transform;
            FigureForSpawn.RemoveAt(0);
            lastGenerateTime = Time.time;
        }
    }


    public void GenerateSequence()
    {
        int figuresCount = FiguresCountBase + FiguresCountIncrementByLevel* gameController.Level;
        while(FigureSequence.Count < figuresCount) {
            GameObject figurePrefab = FigurePrefabs[Random.Range(0, FigurePrefabs.Length)];
            Color color = Colors[Random.Range(0, Colors.Length)];
            Sprite animal = Animals[Random.Range(0, Animals.Length)];

            for(int i = 0; i < bagController.FiguresCntToExplode; i++)
                FigureSequence.Add(new Figure(figurePrefab, color, animal, SpawnPoint.position, this));
        }
        SequenceShuffle();
    }

    public void RespawnedFigures()
    {
        foreach(var figure in FigureSequence)
            if(figure.Status == Figure.FigureStatus.Spawned)
                figure.SetStatusNotSpawned();
        SequenceShuffle();
    }

    public void ClearLevel()
    {
        GameObject[] figuresToDestroy = GameObject.FindGameObjectsWithTag("Figure");
        foreach (GameObject obj in figuresToDestroy)
            Destroy(obj);

        FigureSequence = new();
        FigureForSpawn = new();
    }

    public bool CheckWin()
    {
        if (FigureSequence.Where(f => f.Status != Figure.FigureStatus.Exploded).Count<Figure>() == 0)
        {
            gameController.WinGame();
            return true;
        }
        return false;
    }

    private void SequenceShuffle()
    {
        for (int i = 1; i < FigureSequence.Count; i++)
        {
            int j = Random.Range(0, FigureSequence.Count);
            if(i != j)
                (FigureSequence[i], FigureSequence[j]) = (FigureSequence[j], FigureSequence[i]);
        }
        UpdateFigureLists();
    }


    private void UpdateFigureLists()
    {
        FigureForSpawn = FigureSequence.Where(f => f.Status == Figure.FigureStatus.NotSpawned).ToList();
    }
}
