using Entangle;
using UnityEngine;
using FigNet.Entangle;

public class DemoCube : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private NetworkEntity networkEntity;
    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        networkEntity = GetComponent<EntangleView>().NetworkEntity;
    }

    public void OnActivate()
    {
        Color randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        _meshRenderer.material.color = randomColor;
    }


    public void OnHoveEnter() 
    {
        (networkEntity as NetItem).RequestOwnership(true);

        (networkEntity as NetItem).ClearOwnership();
    }

}
