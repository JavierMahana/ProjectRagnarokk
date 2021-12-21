using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    public TextMeshProUGUI FirstText;
    public TextMeshProUGUI SecondText;
    public TextMeshProUGUI ThirdText;
    public TextMeshProUGUI FourthText;

    public TextMeshProUGUI Credits;

    public Image VictoryBackground;
    public Image BlankScreen;

    public Button SkipButton;

    private void Start()
    {
        AudioManager.instance.CheckMusic();
        StartCoroutine(Ending());
        GameManager.Instance.RestartGame();
    }

    public IEnumerator Ending()
    {

        VictoryBackground.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2);

        FirstText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(7);

        FirstText.gameObject.SetActive(false);
        SecondText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(7);

        SecondText.gameObject.SetActive(false);
        ThirdText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(7);

        ThirdText.gameObject.SetActive(false);
        BlankScreen.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3);

        FourthText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(6);

        FourthText.gameObject.SetActive(false);
        Credits.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(6);
        SkipButton.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(52);
        SceneChanger.Instance.End();
    }     
}


