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
		ChangeScene(0);
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
