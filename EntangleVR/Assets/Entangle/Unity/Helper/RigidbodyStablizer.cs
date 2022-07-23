using Entangle;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EntangleView))]
public class RigidbodyStablizer : MonoBehaviour
{
    private NetworkEntity networkEntity;
    private Rigidbody rbody;
    void Start()
    {
        networkEntity = GetComponent<EntangleView>().NetworkEntity;
        rbody = GetComponent<Rigidbody>();
        networkEntity.OnOWnershipChange += NetworkEntity_OnOWnershipChange;
        InvokeRepeating(nameof(CheckForOwnerShip), 1f, 0.20f);
    }

    private void NetworkEntity_OnOWnershipChange(bool obj)
    {
        Debug.Log($"OnOnwershipChanged Entity: {networkEntity.NetworkId}|{networkEntity.EntityId} IsMine: {networkEntity.IsMine}");
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(CheckForOwnerShip));
    }

    private void CheckForOwnerShip()
    {
        if (!networkEntity.IsMine)
        {
            rbody.isKinematic = true;
        }
    }

}