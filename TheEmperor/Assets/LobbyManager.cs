using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text ChatText;
    [SerializeField] TMP_InputField InputText;
    [SerializeField] TMP_Text PlayersText;
    [SerializeField] GameObject startButton;


    void Start()
    {
        RefreshPlayers();
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(false);
        }

        if (PlayerPrefs.HasKey("Winner") && PhotonNetwork.IsMasterClient)
        {
            //создаем временную переменную, в которую кладем сохраненное имя игрока 
            string winner = PlayerPrefs.GetString("Winner");
            //вызываем метод отображения сообщений и выводим в чат имя игрока, который победил в прошлом матче
            photonView.RPC("ShowMessage", RpcTarget.All, "The last match was won: " + winner);
            //удаляем все ключи, чтобы при перезапуске игры оно не вывелось в чат 
            PlayerPrefs.DeleteAll();
        }
    }

    void Log(string message)
    {
        ChatText.text += "\n";
        ChatText.text += message;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //выводим сообщение о том, что игрок вышел из комнаты и его никнейм
        Log(otherPlayer.NickName + " left the room");
        RefreshPlayers();
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //выводим сообщение о том, что игрок вошел в комнату и его никнейм
        Log(newPlayer.NickName + " entered the room");
        RefreshPlayers();
    }

    [PunRPC]
    public void ShowMessage(string message)
    {
        ChatText.text += "\n";
        ChatText.text += message;
    }
    public void Send()
    {
        //если в поле ввода нет символов, то ничего не делаем
        if (string.IsNullOrWhiteSpace(InputText.text)) { return; }
        //если мы нажали на клавишу Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //вызываем метод ShowMessage для всех игроков на сервере
            // и записываем никнейм того, кто отправил сообщение + текст, который игрок написал в поле InputField
            photonView.RPC("ShowMessage", RpcTarget.All, PhotonNetwork.NickName + ": " + InputText.text);
            //очищаем строку в InputField
            InputText.text = string.Empty;
        }
    }

    [PunRPC]
    public void ShowPlayers()
    {
        //Обнуляем список игроков и оставляем только надпись Players: 
        PlayersText.text = "Players: ";
        //Запускаем цикл, который перебирает всех игроков на сервере
        foreach (Photon.Realtime.Player otherPlayer in PhotonNetwork.PlayerList)
        {
            PlayersText.text += "\n";

            PlayersText.text += otherPlayer.NickName;
        }
    }

    void RefreshPlayers()
    {
        //управлять вызов будет только Мастер Клиент(игрок, который создал сервер)
        if (PhotonNetwork.IsMasterClient)
        {
            //Вызываем метод ShowPlayers для всех игроков в Лобби
            photonView.RPC("ShowPlayers", RpcTarget.All);
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }


}