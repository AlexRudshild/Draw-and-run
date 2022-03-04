using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Button RestartButton;
    public GameObject DrawText;
    public GameObject FinishText;

    private void OnEnable()
    {
        MinionsManager.OnMinionsRunOut += WaitForRestart;

        Drawer.OnDrawComplite += StartMove;

        PlayerMover.OnFinishReach += FinishLevel;
    }

    private void OnDisable()
    {
        MinionsManager.OnMinionsRunOut -= WaitForRestart;

        Drawer.OnDrawComplite -= StartMove;

        PlayerMover.OnFinishReach -= FinishLevel;
    }

    private void StartMove()
    {
        DrawText.SetActive(false);
    }

    private void FinishLevel()
    {
        FindObjectOfType<Drawer>().enabled = false;

        MinionsManager.OnMinionsRunOut -= WaitForRestart;

        FinishText.SetActive(true);
    }

    private void WaitForRestart()
    {
        FindObjectOfType<Drawer>().enabled = false;
        RestartButton.gameObject.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Level1");
    }
}
