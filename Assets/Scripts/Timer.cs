using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text textField;
    private int timeRemaining;
    private Coroutine coroutine;

    [Inject] private GameController gameController;

    public void StartTimer(int value)
    {
        timeRemaining = value;
        if (coroutine != null) 
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(CountdownTimer());
    }

    private IEnumerator CountdownTimer()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f); // ∆дЄм секунду
            timeRemaining--;
            textField.text = timeRemaining.ToString();
        }

        textField.text = "0";
        gameController.LossGame(GameController.LossGameType.Timer);
    }
    public void ClearTimer()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        timeRemaining = 0;
        textField.text = "-";
    }
}
