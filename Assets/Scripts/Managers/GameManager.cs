using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private LevelGenerator levelGenerator;
    public EnemiesController enemiesController;

    private PlayerController playerController;

    private void Awake() => instance = this;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        GetReferences();
    }

    public void GetReferences()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void OnGameOver()
    {
        playerController.CanMove = false;
        StartCoroutine(LevelManager.instance.NextLevel());
    }
}