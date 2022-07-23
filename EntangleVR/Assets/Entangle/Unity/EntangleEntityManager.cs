using FigNet.Core;
using System.Linq;
using FigNetCommon;
using FigNet.Entangle;
using System.Collections.Generic;

public class EntangleEntityManager
{
    private static Dictionary<uint, EntangleView> entities = new Dictionary<uint, EntangleView>();

    public static bool AlreadyExists(uint networkId)
    {
        return entities.ContainsKey(networkId);
    }

    public static void AddEntity(uint networkId, EntangleView entangleView)
    {
        if (!entities.ContainsKey(networkId))
        {
            entities.Add(networkId, entangleView);
        }
        else
        {
            FN.Logger.Info($"Entity with Id {networkId} type {entangleView.EntityType} already exists");
        }
    }

    public static EntangleView GetEntangleViewById(uint id)
    {
        EntangleView view = null;

        if (entities.ContainsKey(id))
        {
            return entities[id];
        }

        return view;
    }

    public static List<EntangleView> GetEntangleViewEntitiesByType(EntityType type)
    {
        var _entities = new List<EntangleView>();

        foreach (var entity in entities)
        {
            if (entity.Value.EntityType == type)
            {
                _entities.Add(entity.Value);
            }
        }

        return _entities;
    }

    public static void RemoveEntity(uint networkId, bool deleteView = true)
    {
        if (entities.ContainsKey(networkId))
        {
            var entity = entities[networkId];
            entity.NetworkEntity.ClearStates();
            entities.Remove(networkId);
            if (deleteView) UnityEngine.GameObject.Destroy(entity.gameObject);
        }
        else
        {
            FN.Logger.Info($"Trying to remove Entity with Id {networkId} that does not exists");
        }
    }

    public static uint MyPlayerId()
    {
        uint id = uint.MaxValue;
        foreach (var entity in entities)
        {
            var player = entity.Value.NetworkEntity as NetPlayer;
            if (player != null)
            {
                if (player.IsMine) id = player.NetworkId;
            }
        }
        return id;
    }

    public static void ClearEntitiesByType(EntityType type)
    {
        var _entities = entities.Values.ToList();
        foreach (var item in _entities)
        {
            if (item.EntityType == EntityType.Player)
            {
                RemoveEntity(item.NetworkEntity.NetworkId);
            }
        }
    }

    public static void ClearEntities()
    {
        try
        {
            var _entities = entities.Values.ToList();

            foreach (var item in _entities)
            {
                RemoveEntity(item.NetworkEntity.NetworkId);
            }

            entities.Clear();
        }
        catch (System.Exception ex)
        {
            FN.Logger.Warning(ex.Message);
        }

    }
}