using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
	[SerializeField] TMP_InputField usernameInput;

	void Start()
	{
		if(PlayerPrefs.HasKey("username"))
		{
			usernameInput.text = PlayerPrefs.GetString("username");
			PhotonNetwork.NickName = PlayerPrefs.GetString("username");
		}
		else
		{
			usernameInput.text = "Player " + Random.Range(0, 10000).ToString("0000");
			OnUsernameInputValueChanged();
		}
	}

	public void OnUsernameInputValueChanged()
	{
		PhotonNetwork.NickName = usernameInput.text;
		PlayerPrefs.SetString("username", usernameInput.text);
	}
}
