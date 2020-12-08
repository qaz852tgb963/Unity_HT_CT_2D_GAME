using UnityEngine;

public class Car : MonoBehaviour
{
    public int iSize;

    [Header("擁有"), Tooltip("是否擁有"),Range(float.MinValue, float.MaxValue)]
    public float iFloat = float.MinValue;

    public string sName;

    [Header("擁有"),Tooltip("是否擁有")]
    public bool bHave;

    public Color colorA = new Color(255, 255, 255, 0.5f);

    public Vector2 v2A = Vector2.one;
    public Vector2 v2B = Vector2.zero;

    public AudioClip AC1;
    public Sprite imgA;

    private void Start()
    {
        print(colorA);
    }

    
}
