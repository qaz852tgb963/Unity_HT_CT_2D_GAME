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
    #endregion

    #region 事件

    private void Start()
    {
        GenerateBlock();
    }

    #endregion

    #region 方法
    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void StartGame()
    {
        GameObject IndexGO = traNext.GetChild(iNextIndex).gameObject;
        GameObject InstantGO = Instantiate(IndexGO, traCanvas);
        InstantGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,380);
        IndexGO.SetActive(false);
        GenerateBlock();
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
