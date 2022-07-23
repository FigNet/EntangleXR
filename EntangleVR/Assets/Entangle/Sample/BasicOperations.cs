using Entangle;
using FigNet.Core;
using UnityEngine;
using UnityEngine.UI;
using FigNet.Entangle;
using FigNetCommon;

public class BasicOperations : MonoBehaviour
{
    public Text ping;
    void Update()
    {
        //if(EN.ClientSocket != null)
        ping.text = $"Ping {EN.GetPing()}";
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateRoom();
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            JoinRoom();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CreateItem();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            CreateItem1();
        }

        else if (Input.GetKeyDown(KeyCode.M))
        {
            CreateAgent();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            REJoinRoom();
        }
    }

    [ContextMenu("CreateRoom")]
    public void CreateRoom()
    {
        Lobby.CreateRoom("Test", 5, "", (sucess, roomId) => {

        });
    }
    [ContextMenu("Join Room")]
    public void JoinRoom()
    {
        Lobby.GetRooms(RoomQuery.Avaliable, (result) => {

            Lobby.JoinRoom(result[0].Id, "", -1, (response) => {

                FN.Logger.Info($"Join Room Response {response}");
            });
        });
    }

    [ContextMenu("REJoin Room")]
    public void REJoinRoom()
    {
        EN.ReconnectAndJoin();
    }

    public void CreateItem()
    {
        NetworkEntity.Instantiate(EntityType.Item, 3, new FNVec3(0, 6, 0));
    }
    public void CreateItem1()
    {
        NetworkEntity.Instantiate(EntityType.Item, 6, new FNVec3(1, 25, -1), new FNVec3(0, 45, 0));
    }


    public void CreateAgent()
    {
        NetworkEntity.Instantiate(EntityType.Agent, 0, new FNVec3(0, 3, 0));
    }
}
