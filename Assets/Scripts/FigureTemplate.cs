using UnityEngine;

public class FigureTemplate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer Background;
    [SerializeField] private SpriteRenderer Animal;
    [HideInInspector] public Figure figure;

    public int Status { get; private set; } = 0;


    public void Fill(Color color, Sprite animal)
    {
        Background.color = color;
        Animal.sprite = animal;
    }

}
