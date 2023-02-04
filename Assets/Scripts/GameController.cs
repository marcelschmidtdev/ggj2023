using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private int matchDurationSeconds = 10;

    private float matchTimeElapsed;

    private void Awake()
    {
        StartCoroutine(StartMatch());
    }

    private IEnumerator StartMatch()
    {
        Debug.Log($"Time elapsed: {matchTimeElapsed} / {matchDurationSeconds} s");
        
        matchTimeElapsed += Time.deltaTime;

        if (matchTimeElapsed > matchDurationSeconds)
        {
            //TODO
            var hasWon = false;
            gameOverScreen.Show(hasWon);
            
            yield break;
        }

        yield return null;
    }
}