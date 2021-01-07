using System.Collections;
using System.Linq;
using UnityEngine;

public class TetrisManager : MonoBehaviour
{
    #region 欄位

    [Header("掉落時間"), Range(0.1f, 3)]
    public float iSpeed = 1.5f;

    [Header("目前分數")]
    public int iNowScore;

    [Header("最高分數")]
    public int iHighestScore;

    [Header("等級"), Range(1, 999)]
    public int iLevel = 1;

    [Header("結束畫面")]
    public GameObject gGameOver;

    [Header("音效：掉落、移動、清除、結束")]
    public AudioClip aDownAudio;
    public AudioClip aLeftRightAudio;
    public AudioClip aClearAudio;
    public AudioClip aGameOverAudio;

    [Header("下一顆")]
    public Transform traNext;
    [Header("方塊的父物件")]
    public Transform traBlockParent;
    [Header("分數判定區塊")]
    public Transform traScore;
    [Header("方塊陣列")]
    public RectTransform[] rtraBlock_L;

    public int iNextIndex;

    [Header("生成的方塊")]
    public RectTransform RTFInstant;
    [Header("生成的方塊位置")]
    public Vector2[] V2Block_L = {
        new Vector2(15,360),
        new Vector2(15,360),
        new Vector2(15,360),
        new Vector2(15,360),
        new Vector2(0,375),
        new Vector2(15,360),
        new Vector2(0,360)
    };

    public float timer;
    private int iMinCtrl = 30;
    private bool bFastDown;
    #endregion

    #region 事件

    private void Start()
    {
        GenerateBlock();
    }

    private void Update()
    {
        CtrlBlock();
        if (RTFInstant && !bFastDown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                iSpeed = 0.017f;
                bFastDown = true;
            }
        }
    }

    #endregion

    #region 方法

    public void CtrlBlock()
    {
        timer += Time.deltaTime;

        if (RTFInstant)
        {
            if (timer >= iSpeed)
            {
                timer = 0;
                RTFInstant.anchoredPosition -= new Vector2(0, iMinCtrl);
            }

            #region 控制按鍵
            Tetris TetBlock = RTFInstant.GetComponent<Tetris>();

            if (TetBlock.bHitWallRight == false && !TetBlock.bRightBlock)
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    RTFInstant.anchoredPosition += new Vector2(iMinCtrl, 0);
                }

            if (TetBlock.bHitWallLeft == false && !TetBlock.bLeftBlock)
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    RTFInstant.anchoredPosition -= new Vector2(iMinCtrl, 0);
                }

            if (TetBlock.bCanCtrl)
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    RTFInstant.eulerAngles += new Vector3(0, 0, 90);

                    TetBlock.offset();
                }
            if (!bFastDown)
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    iSpeed = 0.2f;
                }
                else
                {
                    iSpeed = 1.5f;
                }
            #endregion

            //if (RTFInstant.anchoredPosition.y <= -300)
            if (TetBlock.bHitFloor || TetBlock.bDownBlock)
            {
                SetGround();
                CheckBlock();
                StartCoroutine(ShockScreen());
                ResetGame();
            }
        }
    }

    /// <summary>
    /// 設為地板
    /// </summary>
    public void SetGround()
    {
        int iChildCount = RTFInstant.childCount;

        for (int x = 0; x < iChildCount; x++)
        {
            var Loop = RTFInstant.GetChild(x);
            Loop.name = "方塊";
            Loop.gameObject.layer = 13;
        }

        for (int x = 0; x < iChildCount; x++)
        {
            RTFInstant.GetChild(0).SetParent(traScore);
        }
        Destroy(RTFInstant.gameObject);
    }

    public void CheckBlock()
    {
        int iCount = traScore.childCount;
        rtraBlock_L = new RectTransform[iCount];

        for (int i = 0; i < iCount; i++)
        {
            rtraBlock_L[i] = traScore.GetChild(i).GetComponent<RectTransform>();
        }
        foreach (var Loop in rtraBlock_L)
        {
        print(Loop.anchoredPosition.y);

        }
        print("315 "+rtraBlock_L.Where(t => t.anchoredPosition.y == -315f).Count());
        //print("285 "+rtraBlock_L.Where(t => t.anchoredPosition.y == -285).Count());
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void ResetGame()
    {
        bFastDown = false;
        GameObject GOIndex = traNext.GetChild(iNextIndex).gameObject;
        GameObject GOInstant = Instantiate(GOIndex, traBlockParent);
        GOInstant.GetComponent<RectTransform>().anchoredPosition = V2Block_L[iNextIndex];
        GOIndex.SetActive(false);
        GenerateBlock();

        RTFInstant = GOInstant.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 生成俄羅斯方塊
    /// </summary>
    private void GenerateBlock()
    {
        iNextIndex = Random.Range(0, 7);
        iNextIndex = 0;
        traNext.GetChild(iNextIndex).gameObject.SetActive(true);
    }

    /// <summary>
    /// 添加分數
    /// </summary>
    /// <param name="iScore">增加的分數</param>
    public void AddScore(int iScore)
    {

    }

    /// <summary>
    /// 遊戲時間
    /// </summary>
    private void GameTime()
    {

    }

    /// <summary>
    /// 遊戲結束
    /// </summary>
    private void GameOver()
    {

    }

    /// <summary>
    /// 重新遊戲
    /// </summary>
    public void ReplayGame()
    {

    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void LeaveGame()
    {

    }



    private IEnumerator ShockScreen()
    {
        RectTransform rect = traBlockParent.GetComponent<RectTransform>();

        rect.anchoredPosition += Vector2.one * 30;
        yield return new WaitForSeconds(0.09f);
        rect.anchoredPosition = Vector2.zero;
        yield return new WaitForSeconds(0.09f);
        rect.anchoredPosition += Vector2.one * 20;
        yield return new WaitForSeconds(0.09f);
        rect.anchoredPosition = Vector2.zero;
    }
    #endregion
}
