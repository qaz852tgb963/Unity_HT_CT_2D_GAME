using UnityEngine;

public class Tetris : MonoBehaviour
{
    [Header("各角度，長度")]
    public float length0;
    public float length90;

    [Header("目前，長度")]
    public float length_Wall_NOW;
    public float length_Floor_NOW;

    [Header("是否碰到牆壁_右")]
    public bool bHitWallRight;
    [Header("是否碰到牆壁_左")]
    public bool bHitWallLeft;
    [Header("是否碰到地板")]
    public bool bHitFloor;

    private void OnDrawGizmos()
    {

        #region 畫出輔助線

        int iAngles = (int)transform.eulerAngles.z;

        if (iAngles == 0 || iAngles == 180)
        {
            length_Wall_NOW = length0;
            length_Floor_NOW = length90;
        }
        else if (iAngles == 90 || iAngles == 270)
        {
            length_Wall_NOW = length90;
            length_Floor_NOW = length0;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.right * length_Wall_NOW);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.left * length_Wall_NOW);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down * length_Floor_NOW);

        #endregion


    }

    public void CheckWall()
    {
        RaycastHit2D HitRgiht = Physics2D.Raycast(transform.position, Vector3.right, length_Wall_NOW, 1 << 11);
        RaycastHit2D HitLeft = Physics2D.Raycast(transform.position, Vector3.left, length_Wall_NOW, 1 << 11);
        RaycastHit2D HitFloor = Physics2D.Raycast(transform.position, Vector3.down, length_Floor_NOW, 1 << 12);

        bHitWallRight = false;
        if (HitRgiht && HitRgiht.transform.name == "牆壁_右")
        {
            bHitWallRight = true;
        }

        bHitWallLeft = false;
        if (HitLeft && HitLeft.transform.name == "牆壁_左")
        {
            bHitWallLeft = true;
        }

        bHitFloor = false;
        if (HitFloor && HitFloor.transform.name == "地板")
        {
            bHitFloor = true;
        }

    }

    private void Start()
    {
        length_Wall_NOW = length0;
        length_Floor_NOW = length90;
    }

    private void Update()
    {
        CheckWall();
    }
}
