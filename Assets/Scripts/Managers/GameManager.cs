using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private LevelGenerator levelGenerator;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        levelGenerator.GenerateLevel();
    }
}
