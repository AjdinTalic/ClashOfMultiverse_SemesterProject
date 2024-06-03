using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField] private GameObject credits;
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Credits()
    {
        StartCoroutine(ShowCredits(5));
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator ShowCredits(float num)
    {
        credits.SetActive(true);
        yield return new WaitForSeconds(num);
        credits.SetActive(false);
    }
}