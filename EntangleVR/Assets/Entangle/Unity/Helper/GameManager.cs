using Entangle;
using FigNet.Core;
using UnityEngine;
using FigNetCommon;
using UnityEngine.UI;
using FigNet.Entangle;
using System.Collections.Generic;

// spawn Network Entities
public class GameManager : MonoBehaviour
{
    [SerializeField] private Button SpawnCube;
    [SerializeField] private Button SpawnNPC;
    [SerializeField] private Toggle OpenDoor;
    [SerializeField] private Door Door;

    private void OnSpawnCubeButtonClicked() 
    {
        NetworkEntity.Instantiate(FigNetCommon.EntityType.Item, 3, new FigNetCommon.FNVec3(9.376f, 6f, 6.707f));
    }
    private void OnSpawnNPCButtonClicked() 
    {
        NetworkEntity.Instantiate(FigNetCommon.EntityType.Agent, 10);
    }
    private void OnToggleButtonClicked(bool val) 
    {
        var data = new Dictionary<byte, object>()
            {
                { 1, val }
            };
        Room.SendEvent(new RoomEventData() { EventCode = 6, Data = data }, DeliveryMethod.Reliable);
    }


    private void Awake()
    {
        SpawnCube.onClick.AddListener(OnSpawnCubeButtonClicked);
        SpawnNPC.onClick.AddListener(OnSpawnNPCButtonClicked);
        OpenDoor.onValueChanged.AddListener(OnToggleButtonClicked);

        FigNet.Entangle.Room.OnEventReceived += Room_OnEventReceived;
    }

    private void Room_OnEventReceived(uint sender, RoomEventData data)
    {
        if (data.EventCode == 6)
        {
            bool doorState = (bool)data.Data[1];
            Door.DoorState = doorState;
        }
    }

    void OnDestroy() 
    {
        SpawnCube.onClick.RemoveListener(OnSpawnCubeButtonClicked);
        SpawnNPC.onClick.RemoveListener(OnSpawnNPCButtonClicked);
        OpenDoor.onValueChanged.RemoveListener(OnToggleButtonClicked);
    }
   
}
