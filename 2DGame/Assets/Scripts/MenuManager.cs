using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void DelayPlayGame()
    {
        Invoke("PlayGame", 0.8f);
    }

    public void DelayLeaveGame()
    {
        Invoke("LeaveGame", 0.8f);
    }
    
    /// <summary>
    /// 開始遊戲
    /// </summary>
    private void PlayGame()
    {
        SceneManager.LoadScene("遊戲");
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    private void LeaveGame()
    {
        Application.Quit();
    }
}
