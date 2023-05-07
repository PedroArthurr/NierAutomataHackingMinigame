using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialization : MonoBehaviour
{
    void Start()
    {
        SceneLoader.instance.LoadScene(Consts.MENU);
    }
}
