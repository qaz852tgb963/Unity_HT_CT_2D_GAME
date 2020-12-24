using UnityEngine;

public class Tetris : MonoBehaviour
{
    [Header("0度，長度")]
    public float length0;
    [Header("90度，長度")]
    public float length90;
    [Header("目前，長度")]
    public float length_NOW;
    [Header("是否碰到牆壁_右")]
    public bool bHitWallRight;
    [Header("是否碰到牆壁_左")]
    public bool bHitWallLeft;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        #region 畫出輔助線

        int iAngles = (int)transform.eulerAngles.z;

        if (iAngles == 0 || iAngles == 180)
        {
            length_NOW = length0;
        }
        else if (iAngles == 90 || iAngles == 270)
        {
            length_NOW = length90;
        }

        Gizmos.DrawRay(transform.position, Vector3.right * length_NOW);
        Gizmos.DrawRay(transform.position, Vector3.left * length_NOW);

        #endregion


    }

    public void CheckWall()
    {
        RaycastHit2D HitRgiht = Physics2D.Raycast(transform.position, Vector3.right, length_NOW, 1 << 11);
        RaycastHit2D HitLeft = Physics2D.Raycast(transform.position, Vector3.left, length_NOW, 1 << 11);

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
    }

    private void Start()
    {
        length_NOW = length0;
    }

    private void Update()
    {
        CheckWall();
    }
}
