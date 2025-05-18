using UnityEngine;
using Zenject;

public class InputController : MonoBehaviour
{
    [Inject] private BagController bagController;

    private bool processed = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (processed) return;
            processed = true;
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out FigureTemplate figureTemplate))
            {
                if (figureTemplate.figure.Status == Figure.FigureStatus.Spawned)
                    bagController.Add(figureTemplate.figure);
            }
        }
        else
        {
            processed = false;
        }
    }
}
