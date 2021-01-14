using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("音效：掉落、移動、旋轉、清除、結束")]
    public AudioClip aDownAudio;
    public AudioClip aLeftRightAudio;
    public AudioClip aUpAudio;
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
    private const int iMaxRow = 23;
    private int top = 375;
    private int bottom = -315;
    private int iSmallRL = 5;//正負範圍
    private bool bFastDown;
    private bool[] destryRow = new bool[iMaxRow];
    public float[] downHight;

    /// <summary>
    /// 是否遊戲結束
    /// </summary>
    private bool bGameOver;

    private AudioSource Aud;

    public Text tNowScore;
    public Text tHightScore;

    #endregion

    #region 事件

    private void Start()
    {
        Aud = GetComponent<AudioSource>();
        GenerateBlock();
    }

    private void Update()
    {
        if (bGameOver) return;
        CtrlBlock();
        FastDown();
    }

    #endregion

    #region 方法

    public void FastDown()
    {
        if (RTFInstant && !bFastDown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                iSpeed = 0.018f;
                bFastDown = true;
            }
        }
    }

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
                    Aud.PlayOneShot(aLeftRightAudio, Random.Range(0.8f, 1.2f));
                    RTFInstant.anchoredPosition += new Vector2(iMinCtrl, 0);
                }

            if (TetBlock.bHitWallLeft == false && !TetBlock.bLeftBlock)
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Aud.PlayOneShot(aLeftRightAudio, Random.Range(0.8f, 1.2f));
                    RTFInstant.anchoredPosition -= new Vector2(iMinCtrl, 0);
                }

            if (TetBlock.bCanCtrl)
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    Aud.PlayOneShot(aUpAudio, Random.Range(0.8f, 1.2f));
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
                    iSpeed = fallSpeedMax;
                }
            #endregion

            //if (RTFInstant.anchoredPosition.y <= -300)
            if (TetBlock.bHitFloor || TetBlock.bDownBlock)
            {
                SetGround();
                StartCoroutine(CheckBlock());
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
        Aud.PlayOneShot(aDownAudio, Random.Range(0.8f, 1.2f));

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

    public IEnumerator CheckBlock()
    {
        int iCount = traScore.childCount;
        rtraBlock_L = new RectTransform[iCount];

        for (int i = 0; i < iCount; i++)
        {
            rtraBlock_L[i] = traScore.GetChild(i).GetComponent<RectTransform>();

            float y = rtraBlock_L[i].anchoredPosition.y;
            if (y <= top + 20 && y >= top - 20) GameOver();
        }

        for (int i = 0; i < iMaxRow; i++)
        {
            var CheckRow = rtraBlock_L.Where(t => t.anchoredPosition.y >= (bottom + iMinCtrl * i - iSmallRL) && t.anchoredPosition.y <= (bottom + iMinCtrl * i + iSmallRL)).ToArray();
            if (CheckRow.Count() == 18)
            {
                Aud.PlayOneShot(aClearAudio, Random.Range(0.8f, 1.2f));

                yield return StartCoroutine(Shine(CheckRow));
                destryRow[i] = true;
                AddScore(100);
            }
            else
                destryRow[i] = false;
        }

        downHight = new float[traScore.childCount];
        for (int i = 0; i < downHight.Length; i++) downHight[i] = 0;


        for (int i = 0; i < destryRow.Length; i++)
        {
            if (!destryRow[i]) continue;

            for (int j = 0; j < rtraBlock_L.Length; j++)
            {
                if (rtraBlock_L[j].anchoredPosition.y >= (bottom + iMinCtrl * i - iSmallRL))
                {
                    downHight[j] -= iMinCtrl;
                }
            }

            destryRow[i] = false;
        }

        for (int i = 0; i < rtraBlock_L.Length; i++)
        {
            rtraBlock_L[i].anchoredPosition += Vector2.up * downHight[i];
        }

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
        traNext.GetChild(iNextIndex).gameObject.SetActive(true);
    }

    public Text sScoreText;
    public Text sLevelText;
    public float fallSpeedMax = 1.5f;

    /// <summary>
    /// 添加分數
    /// </summary>
    /// <param name="iScore">增加的分數</param>
    public void AddScore(int iScore)
    {
        iNowScore += iScore;
        sScoreText.text = "分數：" + iNowScore;

        iLevel = 1 + iNowScore / 1000;
        sLevelText.text = "等級：" + iLevel;

        fallSpeedMax = 1.5f - iLevel / 10;
        fallSpeedMax = Mathf.Clamp(fallSpeedMax, 0.2f, 99f);
        iSpeed = fallSpeedMax;
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
        if (!bGameOver)
        {
            Aud.PlayOneShot(aGameOverAudio, Random.Range(0.8f, 1.2f));

            bGameOver = true;
            StopAllCoroutines();
            gGameOver.SetActive(true);

            tNowScore.text = "目前分數：" + iNowScore;

            if (iNowScore > PlayerPrefs.GetInt("最高分數"))
            {
                PlayerPrefs.SetInt("最高分數", iNowScore);
                tHightScore.text = "最高分數：" + iNowScore;
            }
            else tHightScore.text = "最高分數：" + PlayerPrefs.GetInt("最高分數");
        }
    }

    /// <summary>
    /// 重新遊戲
    /// </summary>
    public void ReplayGame()
    {
        SceneManager.LoadScene("遊戲");
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void LeaveGame()
    {
        Application.Quit();
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

    private IEnumerator Shine(RectTransform[] smalls)
    {
        float interval = 0.1f;
        for (int i = 0; i < 18; i++) smalls[i].GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(interval);
        for (int i = 0; i < 18; i++) smalls[i].GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(interval);
        for (int i = 0; i < 18; i++) smalls[i].GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(interval);
        for (int i = 0; i < 18; i++) smalls[i].GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(interval);

        for (int i = 0; i < 18; i++) Destroy(smalls[i].gameObject);
        yield return new WaitForSeconds(interval);

        int iCount = traScore.childCount;
        rtraBlock_L = new RectTransform[iCount];
        for (int i = 0; i < iCount; i++)
        {
            rtraBlock_L[i] = traScore.GetChild(i).GetComponent<RectTransform>();
        }
    }
    #endregion
}
