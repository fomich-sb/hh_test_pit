using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using static UnityEditor.Progress;

public class BagController : MonoBehaviour
{
    [SerializeField] private GameObject[] BagItems;
    public int FiguresCntToExplode = 3;

    [Inject] private GameController gameController;
    [Inject] private FiguresController figuresController;
    private Dictionary<GameObject, Figure> FigureItems = new();

    public void Add(Figure figure)
    {
        if (gameController.Status != GameController.GameStatus.Started) return;

        GameObject bagItem = GetEmptyItem();
        if (!bagItem) return;

        FigureItems.Add(bagItem, figure);
        figure.instance.transform.parent = bagItem.transform;
        figure.SetStatusSelected();
        if (!CheckExplode(figure))
            CheckFilled();
    }

    private GameObject GetEmptyItem()
    {
        foreach (var item in BagItems)
        {
            if (!FigureItems.ContainsKey(item))
                return item;
        }
        return null;
    }

    private bool CheckExplode(Figure figure)
    {
        int cntEquals = 0;
        foreach(var item in FigureItems)
        {
            if (item.Value.Equals(figure))
                cntEquals++;
        }

        if(cntEquals >= FiguresCntToExplode)
            Explode(figure);

        return false;
    }

    private void Explode(Figure figure)
    {
        foreach (var key in FigureItems.Keys.ToList()) 
        {
            if (FigureItems[key].Equals(figure))
            {
                FigureItems[key].SetStatusExploded();
                FigureItems.Remove(key);
            }
        }
        figuresController.CheckWin();
    }

    private bool CheckFilled()
    {
        if (FigureItems.Count >= BagItems.Length)
        {
            gameController.LossGame(GameController.LossGameType.BagOverflow);
            return true;
        }

        return false;
    }

    public void Clear()
    {
        FigureItems = new();
    }
}
