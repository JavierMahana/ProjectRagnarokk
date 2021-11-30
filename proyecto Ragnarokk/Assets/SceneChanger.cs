using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : Singleton<SceneChanger>
{

	public void LoadCombatScene(CombatEncounter encounter)
	{
		GameManager.Instance.currentEncounter = encounter;
		ChangeScene("CombatScene");
	}
	public void LoadMenuScene()
	{
		GameManager.Instance.currentEncounter = null;
		GameManager.Instance.ConfirmationClick = false;

		ChangeScene("SelectFightersScene");
	}

	public void LoadExplorationScene()
	{
		ChangeScene("Exploration");
	}

	public void LoadGeneralMenuScene()
	{
		ChangeScene("GeneralMenu");
	}

	public void End()
    {
		ChangeScene("MainMenu");
		GameManager.Instance.RestartGame();
    }

	public void ChangeScene(int sceneBuildNumber)
	{
		SceneManager.LoadScene(sceneBuildNumber);
	}
	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName); 
	}
	public void Exit()
	{
		Application.Quit();
	}

}
