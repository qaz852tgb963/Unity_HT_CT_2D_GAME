using UnityEngine;

public class APIStatic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //print("allCamerasCount：" + Camera.allCamerasCount);
        //Cursor.visible = false;
        //print("visible：" + Cursor.visible);

        //Application.OpenURL("https://docs.unity3d.com/2019.2/Documentation/ScriptReference/Application.html");
        float AAA = 1.5f;
        print("CeilToInt：" + Mathf.FloorToInt(AAA));
    }

    // Update is called once per frame
    void Update()
    {
        //print("anyKey：" + Input.anyKey);
        //print("time：" + Time.time);

        print("GetKeyDown：" + Input.GetKeyDown(KeyCode.Space));
    }
}
