using UnityEngine;

public class Tetris : MonoBehaviour
{
    [Header("各角度，長度")]
    public float length0;
    public float length90;
    [Header("旋轉位移左右")]
    public int offsetX;
    [Header("旋轉位移上下")]
    public int offsetY;
    [Header("偵測旋轉")]
    public float lengthCtrl0R;
    public float lengthCtrl0L;
    public float lengthCtrl90R;
    public float lengthCtrl90L;

    [Header("目前，長度")]
    public float length_Wall_NOW;
    public float length_Floor_NOW;
    public float length_CtrlR_NOW;
    public float length_CtrlL_NOW;

    [Header("是否碰到牆壁_右")]
    public bool bHitWallRight;
    [Header("是否碰到牆壁_左")]
    public bool bHitWallLeft;
    [Header("是否碰到地板")]
    public bool bHitFloor;
    [Header("是否旋轉")]
    public bool bCanCtrl = true;

    private RectTransform rect;

    public void CheckWall()
    {
        RaycastHit2D HitRgiht = Physics2D.Raycast(transform.position, Vector3.right, length_Wall_NOW, 1 << 11);
        RaycastHit2D HitLeft = Physics2D.Raycast(transform.position, Vector3.left, length_Wall_NOW, 1 << 11);
        RaycastHit2D HitFloor = Physics2D.Raycast(transform.position, Vector3.down, length_Floor_NOW, 1 << 12);

        RaycastHit2D CtrlHitRgiht = Physics2D.Raycast(transform.position, Vector3.right, length_CtrlR_NOW, 1 << 11);
        RaycastHit2D CtrlHitLeft = Physics2D.Raycast(transform.position, Vector3.left, length_CtrlL_NOW, 1 << 11);
        
        if (HitRgiht && HitRgiht.transform.name == "牆壁_右")
        {
            bHitWallRight = true;
        }
        else
        {
            bHitWallRight = false;
        }

        if (HitLeft && HitLeft.transform.name == "牆壁_左")
        {
            bHitWallLeft = true;
        }
        else
        {
            bHitWallLeft = false;
        }


        if (HitFloor && HitFloor.transform.name == "地板")
        {
            bHitFloor = true;
        }
        else
        {
            bHitFloor = false;
        }
        
        if (CtrlHitRgiht && CtrlHitRgiht.transform.name == "牆壁_右"||
            CtrlHitLeft && CtrlHitLeft.transform.name == "牆壁_左")
        {
            bCanCtrl = false;
        }
        else
        {
            bCanCtrl = true;
        }
    }

    public void offset()
    {
        int iAngles = (int)transform.eulerAngles.z;

        if (iAngles == 90 || iAngles == 270)
        {
            rect.anchoredPosition -= new Vector2(offsetX, offsetY);
        }
        else if (iAngles == 0 || iAngles == 180)
        {
            rect.anchoredPosition += new Vector2(offsetX, offsetY);
        }

    }





    private void OnDrawGizmos()
    {

        #region 畫出輔助線

        int iAngles = (int)transform.eulerAngles.z;

        if (iAngles == 0 || iAngles == 180)
        {
            length_Wall_NOW = length0;
            length_Floor_NOW = length90;
            length_CtrlR_NOW = lengthCtrl0R;
            length_CtrlL_NOW = lengthCtrl0L;
        }
        else if (iAngles == 90 || iAngles == 270)
        {
            length_Wall_NOW = length90;
            length_Floor_NOW = length0;
            length_CtrlR_NOW = lengthCtrl90R;
            length_CtrlL_NOW = lengthCtrl90L;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.right * length_Wall_NOW);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.left * length_Wall_NOW);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down * length_Floor_NOW);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector3.right * length_CtrlR_NOW);
        Gizmos.DrawRay(transform.position, Vector3.left * length_CtrlL_NOW);

        #endregion


    }


    private void Start()
    {
        length_Wall_NOW = length0;
        length_Floor_NOW = length90;
        length_CtrlR_NOW = lengthCtrl0R;
        length_CtrlL_NOW = lengthCtrl0L;

        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        CheckWall();
    }
}
