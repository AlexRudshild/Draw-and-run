using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Button RestartButton;
    public GameObject DrawText;
    public GameObject FinishText;

    private Move _mover;
    private Drawing _drawer;

    private void OnEnable()
    {
        if (_drawer == null)
            _drawer = FindObjectOfType<Drawing>();
        if (_mover == null)
            _mover = FindObjectOfType<Move>();

        _drawer.OnMInionsRunOut += WaitForRestart;
        _drawer.OnDrawComplite += StartMove;

        _mover.OnFinishReach += FinishLevel;
    }

    private void OnDisable()
    {
        _drawer.OnMInionsRunOut -= WaitForRestart;
        _drawer.OnDrawComplite -= StartMove;

        _mover.OnFinishReach -= FinishLevel;
    }
    private void StartMove()
    {
        DrawText.SetActive(false);
    }

    private void FinishLevel()
    {
        FindObjectOfType<Drawing>().enabled = false;
        FinishText.SetActive(true);
    }

    private void WaitForRestart()
    {
        FindObjectOfType<Drawing>().enabled = false;
        RestartButton.gameObject.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
