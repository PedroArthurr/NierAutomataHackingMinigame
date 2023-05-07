using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeButton : MonoBehaviour
{
    [SerializeField] private bool selected;
    [SerializeField] private bool isNew;

    [Space]

    [SerializeField] private GameObject[] selectedObjects;
    [SerializeField] private TMP_Text challengeName;
    [SerializeField] private GameObject newTag;
    [SerializeField] private Image squareIcon;
    [SerializeField] private Color red, white, black;

}
