using Entangle;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _objectToSpawn;
    [SerializeField]
    private Transform _output;

    [ContextMenu("Spawn Cube")]
    public void OnHoverEnter() 
    {
        if (_objectToSpawn && _output)
        {
            NetworkEntity.Instantiate(FigNetCommon.EntityType.Item, 1,
                new FigNetCommon.FNVec3(_output.position.x, _output.position.y, _output.position.z),
                new FigNetCommon.FNVec3(_output.rotation.eulerAngles.x, _output.rotation.eulerAngles.y, _output.rotation.eulerAngles.z));
        }
        else Debug.Log("No object to spawn specified");
    }
}
