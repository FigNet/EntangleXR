using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntangleContainer", menuName = "Entangle/EntangleContainer", order = 1)]
public class EntangleContainer : ScriptableObject
{
    public EntangleView MyPlayer;
    public EntangleView RemotePlayer;
    public List<EntangleView> Entities;
}

