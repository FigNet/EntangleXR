using UnityEngine;
using FigNet.Core;
using FigNetCommon;
using FigNet.Entangle;

public class RoomManager : MonoBehaviour, IEntangleRoomListener
{
    //private GameObject players;
    #region IEntangleRoomListener
    public void OnAgentCreated(NetAgent agent, System.Numerics.Vector3 position, System.Numerics.Vector4 rotation, System.Numerics.Vector3 scale)
    {
        if (EntangleEntityManager.AlreadyExists(agent.NetworkId)) return;
        var entity = Container.Entities.Find(e => e.EntityId == agent.EntityId);
        if (entity == null)
        {
            FN.Logger.Error($"#EN Agent {agent.EntityId} prefab is not register in Settings");
            return;
        }
        var agentView = GameObject.Instantiate<EntangleView>(entity, EntitiesContainer.transform);
        agentView.name = entity.name + $" - {agent.EntityId} - {agent.NetworkId}";
        agentView.transform.position = new Vector3(position.X, position.Y, position.Z);
        agentView.transform.eulerAngles = new Vector3(rotation.X, rotation.Y, rotation.Z);
        //        agentView.transform.localScale = new Vector3(scale.X, scale.Y, scale.Z);
        agentView.SetNetworkEntity(agent, position, rotation);


        EntangleEntityManager.AddEntity(agent.NetworkId, agentView);
    }

    public void OnAgentDeleted(NetAgent agent)
    {
        EntangleEntityManager.RemoveEntity(agent.NetworkId);
    }

    public void OnEventReceived(uint sender, RoomEventData eventData)
    {
    }

    public void OnItemCreated(NetItem item, System.Numerics.Vector3 position, System.Numerics.Vector4 rotation, System.Numerics.Vector3 scale)
    {
        if (EntangleEntityManager.AlreadyExists(item.NetworkId)) return;
        var entity = Container.Entities.Find(e => e.EntityId == item.EntityId);
        if (entity == null)
        {
            FN.Logger.Error($"#EN Item {item.EntityId} prefab is not register in Settings");
            return;
        }

        if (position == System.Numerics.Vector3.Zero) 
            position = new System.Numerics.Vector3(entity.transform.position.x,entity.transform.position.y,entity.transform.position.z);
        if (rotation == System.Numerics.Vector4.Zero)
            rotation = new System.Numerics.Vector4(entity.transform.rotation.x, entity.transform.rotation.y, entity.transform.rotation.z, transform.rotation.w);

        var itemView = GameObject.Instantiate<EntangleView>(entity, EntitiesContainer.transform);
        itemView.name = entity.name + $" - {item.EntityId} - {item.NetworkId}";
        itemView.transform.position = new Vector3(position.X, position.Y, position.Z);
        itemView.transform.rotation = new Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W);
        //       itemView.transform.localScale = new Vector3(scale.X, scale.Y, scale.Z);
        itemView.SetNetworkEntity(item, position, rotation);

        EntangleEntityManager.AddEntity(item.NetworkId, itemView);
        itemView.gameObject.SetActive(true);
    }

    public void OnItemDeleted(NetItem item)
    {
        EntangleEntityManager.RemoveEntity(item.NetworkId);
    }

    public void OnPlayerJoined(FigNet.Entangle.NetPlayer player)
    {
        FN.Logger.Info($"EN: onplayer join {player.IsMine} | {player.NetworkId}");

        var alradyExists = EntangleEntityManager.AlreadyExists(player.NetworkId);
        if (alradyExists)
        {
            Debug.LogError($"Player Already Exist {player.NetworkId} | {player.IsMine}");
            return;
        }

        if (Container.MyPlayer == null) return;


        if (player.IsMine)
        {
            var myPlayer = GameObject.Instantiate<EntangleView>(Container.MyPlayer, EntitiesContainer.transform);
            if (myPlayer == null)
            {
                FN.Logger.Error("My Player prefab is not register in Settings");
                return;
            }
            myPlayer.SetNetworkEntity(player, System.Numerics.Vector3.Zero, System.Numerics.Vector4.Zero);
            EntangleEntityManager.AddEntity(player.NetworkId, myPlayer);

        }
        else
        {
            if (Container.RemotePlayer == null) return;
            var remotePlayer = GameObject.Instantiate<EntangleView>(Container.RemotePlayer, EntitiesContainer.transform);

            if (remotePlayer == null)
            {
                FN.Logger.Error("Remote Player prefab is not register in Settings");
                return;
            }
            remotePlayer.SetNetworkEntity(
                player,
                new System.Numerics.Vector3(remotePlayer.transform.position.x, remotePlayer.transform.position.y, remotePlayer.transform.position.z),
                new System.Numerics.Vector4(remotePlayer.transform.rotation.x, remotePlayer.transform.rotation.y, remotePlayer.transform.rotation.z, remotePlayer.transform.rotation.w));

            EntangleEntityManager.AddEntity(player.NetworkId, remotePlayer);
        }

        //if (player.IsMine)
        //{



        //    var id = EntangleEntityManager.MyPlayerId();
        //    if (id != uint.MaxValue)
        //    {
        //        var mPlayer = EntangleEntityManager.GetEntangleViewById(id);
        //        if (mPlayer != null)
        //        {
        //            mPlayer.SetNetworkEntity(
        //                player,
        //                new System.Numerics.Vector3(mPlayer.transform.position.x, mPlayer.transform.position.y, mPlayer.transform.position.z),
        //                new System.Numerics.Vector3(mPlayer.transform.rotation.eulerAngles.x, mPlayer.transform.rotation.eulerAngles.y, mPlayer.transform.rotation.eulerAngles.z));
        //        }

        //        EntangleEntityManager.RemoveEntity(id, false);


        //        EntangleEntityManager.AddEntity(player.NetworkId, mPlayer);

        //        FN.Logger.Error("Returning player");
        //    }
        //    else
        //    {
        //        FN.Logger.Error("fresh join");
        //        var myPlayer = GameObject.Instantiate<EntangleView>(Container.MyPlayer);
        //        if (myPlayer == null)
        //        {
        //            FN.Logger.Error("My Player prefab is not register in Settings");
        //            return;
        //        }
        //        myPlayer.SetNetworkEntity(player, System.Numerics.Vector3.Zero, System.Numerics.Vector3.Zero);
        //        EntangleEntityManager.AddEntity(player.NetworkId, myPlayer);
        //    }
        //}
        //else
        //{
        //    EntangleView remotePlayer = null;
        //    var alradyExists = EntangleEntityManager.AlreadyExists(player.NetworkId);

        //    if (!alradyExists)
        //    {
        //        remotePlayer = GameObject.Instantiate<EntangleView>(Container.RemotePlayer);
        //        if (remotePlayer == null)
        //        {
        //            FN.Logger.Error("Remote Player prefab is not register in Settings");
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        remotePlayer = EntangleEntityManager.GetEntangleViewById(player.NetworkId);
        //        EntangleEntityManager.RemoveEntity(player.NetworkId, false);

        //    }
        //    FN.Logger.Error($"Remote Player alreadyPresent {alradyExists} | {player.NetworkId}");


        //    remotePlayer.SetNetworkEntity(
        //        player,
        //        new System.Numerics.Vector3(remotePlayer.transform.position.x, remotePlayer.transform.position.y, remotePlayer.transform.position.z),
        //        new System.Numerics.Vector3(remotePlayer.transform.rotation.eulerAngles.x, remotePlayer.transform.rotation.eulerAngles.y, remotePlayer.transform.rotation.eulerAngles.z));

        //    EntangleEntityManager.AddEntity(player.NetworkId, remotePlayer);
        //}
    }

    public void OnPlayerLeft(FigNet.Entangle.NetPlayer player)
    {
        FN.Logger.Info($"On PlayerLeft {player.NetworkId}");
        EntangleEntityManager.RemoveEntity(player.NetworkId);
    }

    public void OnRoomCreated()
    {
        //FN.Logger.Info("EN: On Room Created!");
    }

    public void OnRoomDispose()
    {
        //FN.Logger.Info("EN: On Room Disposed!");
    }

    public void OnRoomStateChange(int status)
    {
        //FN.Logger.Info($"EN: On RoomState Change {status}");
    }

    #endregion

    private EntangleContainer Container;
    private static GameObject EntitiesContainer;

    // todo: remove it if not needed
    //public void OnMasterClientUpdate(bool isMaster)
    //{
    //    if (!isMaster) return;

    //    var entities = EntangleEntityManager.GetEntangleViewEntitiesByType(EntityType.Item);
    //    foreach (var entity in entities)
    //    {
    //        if (!entity.NetworkEntity.IsLocked)
    //        {
    //            var rb = entity.GetComponent<Rigidbody>();
    //            rb.isKinematic = false;
    //        }
    //    }
    //}


    private void Room_OnRoomDispose()
    {
        EntangleEntityManager.ClearEntities();
    }


    private static RoomManager instance = null;
    void OnEnable() 
    {
        Room.OnRoomDispose += Room_OnRoomDispose;
    }

    void OnDisable() 
    {
        Room.OnRoomDispose -= Room_OnRoomDispose;
    }
    void Awake() 
    {
        FigNet.Entangle.Room.BindListener(this);

        // If the instance reference has not been set, yet, 
        if (instance == null)
        {
            // Set this instance as the instance reference.
            instance = this;
        }
        else if (instance != this)
        {
            // If the instance reference has already been set, and this is not the
            // the instance reference, destroy this game object.
            Destroy(gameObject);
        }

        // Do not destroy this object, when we load a new scene.
        DontDestroyOnLoad(gameObject);

    }

    void Start()
    {
        Container = Resources.Load<EntangleContainer>("EntangleContainer");
        //EN.Room.Init();
        //EN.OnMasterClientUpdate = OnMasterClientUpdate;
        
        if (EntitiesContainer == null)
        {
            EntitiesContainer = new GameObject("__NETWORK_ENTITIES__");
            EntitiesContainer.transform.SetParent(transform);
            EntitiesContainer.transform.position = Vector3.zero;
        }
        
    }

    void Update()
    {
        EN.Room.Tick(Time.deltaTime);
    }

    private void OnDestroy()
    {
        FigNet.Entangle.Room.UnBindListener(this);
        EN.Room.Deinit();
    }
}
