using Entangle;
using FigNet.Core;
using UnityEngine;
using FigNet.Entangle;
using FigNetCommon.Data;
using FigNet.Entangle.Operations;

public interface IEntangleSync
{
    void Init(NetworkEntity networkEntity, System.Numerics.Vector3 position = default, System.Numerics.Vector4 rotation = default, System.Numerics.Vector3 scale = default);
}

public class EntangleManager : MonoBehaviour
{
    private EntangleConfiguration Configuration;

    public bool IsAutoConnect;
    private void Start()
    {
        ENetProvider.Module.Load();

        Configuration = Resources.Load<EntangleConfiguration>("ServerConfig");
        EN.OnConnected += Entangle_OnConnected;
        EN.OnDisconnected += Entangle_OnDisconnected;

        Room.SYNC_RATE = Configuration.Config.SyncRate;

        //var settings = Resources.Load<FigNetConfiguration>("FigNet_Configuration").Config;
        //FN.Initilize(settings);

        EN.Initialize();

        //FigNet.Entangle.Entangle.Room.BindListner

        EN.SetConfig(Configuration.Config);
        if (IsAutoConnect) Connect();
    }

    private void Entangle_OnDisconnected()
    {
        FN.Logger.Info("#EN Client Disconnected");
    }

    private void Entangle_OnConnected()
    {
        FN.Connections[0].SendMessage(new RegisterAppIdOperation().GetOperation(Configuration.Config.AppId), (response) => {

            var sucess = (bool)((response.Payload as OperationData).Parameters[0]);
            if (!sucess)
            {
                FN.Connections[0].Disconnect();
                FN.Logger.Info("#EN Invalid AppKey");
            }
            else
            {
                FN.Logger.Info("#EN valid AppKey");
            }
        });
    }

    private void OnApplicationQuit()
    {
        FN.Deinitialize();
    }

    private void Update()
    {
        Process();
    }

    public void Process()
    {
        try
        {
            for (int i = 0; i < FN.Connections?.Count; i++)
            {
                FN.Connections[i].Process();
            }
            TimeScheduler.Tick(Time.deltaTime);
        }
        catch (System.Exception ex)
        {
            FN.Logger.Exception(ex, ex.Message);
        }
    }

    [ContextMenu("Connect")]
    public void Connect()
    {
        FigNet.Entangle.EN.Connect();
    }

    [ContextMenu("Disconnect")]
    public void Disconnect()
    {
        EN.Disconnect();
    }
}
