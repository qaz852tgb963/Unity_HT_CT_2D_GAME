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
    [Header("畫布")]
    public Transform traCanvas;

    public int iNextIndex;

    [Header("生成的方塊")]
    public RectTransform RTFInstant;
    [Header("生成的方塊位置")]
    public Vector2[] V2Block_L = {
        new Vector2(0,375),
        new Vector2(0,375),
        new Vector2(15,360),
        new Vector2(15,360),
        new Vector2(0,375),
        new Vector2(15,360),
        new Vector2(0,360)
    };

    public float timer;
    int iMinCtrl = 30;
    #endregion

    #region 事件

    private void Start()
    {
        GenerateBlock();
    }

    private void Update()
    {
        CtrlBlock();
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

            //if (RTFInstant.anchoredPosition.x < 225)
            if (TetBlock.bHitWallRight == false)
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    RTFInstant.anchoredPosition += new Vector2(iMinCtrl, 0);
                }

            //if (RTFInstant.anchoredPosition.x > -225)
            if (TetBlock.bHitWallLeft == false)
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    RTFInstant.anchoredPosition -= new Vector2(iMinCtrl, 0);
                }

            if (Input.GetKeyDown(KeyCode.W))
            {
                RTFInstant.eulerAngles += new Vector3(0, 0, 90);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                iSpeed = 0.2f;
            }
            else
            {
                iSpeed = 1.5f;
            }
            #endregion

            if (RTFInstant.anchoredPosition.y <= -270)
            {
                StartGame();
            }
        }
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void StartGame()
    {
        GameObject GOIndex = traNext.GetChild(iNextIndex).gameObject;
        GameObject GOInstant = Instantiate(GOIndex, traCanvas);
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
    #endregion
}
