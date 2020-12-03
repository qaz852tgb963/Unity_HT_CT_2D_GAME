using UnityEngine;

public class Car : MonoBehaviour
{
    string name { get; set; }
    float size { get; set; }

    new Car(string name,float size)
    {
        name = name,
        size = size
    }
}
