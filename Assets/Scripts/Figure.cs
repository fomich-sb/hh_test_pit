using UnityEngine;

public class Figure
{
    public GameObject prefab;
    public Color color;
    public Sprite animal;
    public GameObject instance;
    private Vector3 spawnPosition;
    private FiguresController figuresController;

    public FigureStatus Status { get; private set; } = FigureStatus.NotSpawned;

    static private int num=0;

    public Figure(GameObject _prefab, Color _color, Sprite _animal, Vector3 _spawnPosition, FiguresController _figuresController)
    {
        prefab = _prefab;
        color = _color;
        animal = _animal;
        spawnPosition = _spawnPosition;
        figuresController = _figuresController;
    }

    public GameObject Spawn()
    {
        if (Status != FigureStatus.NotSpawned) return null;

        instance = Object.Instantiate(prefab, spawnPosition, Quaternion.identity);
        if (instance.TryGetComponent(out FigureTemplate figureTemplate))
        {
            figureTemplate.Fill(color, animal);
            figureTemplate.figure = this;
        }
        num++; 
        instance.name = "Figure"+num.ToString();

        Status = FigureStatus.Spawned;

        ApplyRandomStartImpulse(instance.GetComponent<Rigidbody2D>());

        return instance;
    }
    
    public void SetStatusNotSpawned()
    {
        if(instance)
            Object.Destroy(instance);
        Status = FigureStatus.NotSpawned;
    }

    public void SetStatusSelected()
    {
        instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        if (instance.TryGetComponent(out PolygonCollider2D col))
            col.enabled = false;

        instance.transform.rotation = Quaternion.identity;
        instance.transform.localPosition = Vector2.zero;
        Status = FigureStatus.Selected;
    }

    public void SetStatusExploded()
    {
        Object.Destroy(instance);
        Status = FigureStatus.Exploded;
    }

    public bool Equals(Figure obj)
    {
        if(prefab == obj.prefab && color == obj.color && animal == obj.animal) 
            return true;

        return false;
    }

    public void ApplyRandomStartImpulse(Rigidbody2D gameObject)
    {
        float randomAngle = Random.Range(0f, -180f);
        Vector2 randomDirection = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad),
            Mathf.Sin(randomAngle * Mathf.Deg2Rad)
        );

        // Применяем импульс
        gameObject.AddForce(randomDirection * figuresController.SpawnImpulseStrength, ForceMode2D.Impulse);
    }
    public enum FigureStatus
    {
        NotSpawned,
        Spawned,
        Selected,
        Exploded,
    }
}
