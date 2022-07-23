using Entangle;
using UnityEngine;
using FigNet.Entangle;

public class AgentView : MonoBehaviour
{
    private NetworkEntity networkEntity;

    private Vector3 nextTargetPosition;
    private Vector3 prevTargetPosition;
    private float timer;

    private void Awake()
    {
        EN.OnMasterClientUpdate += OnMasterClientUpdated;
    }


    private void OnMasterClientUpdated(bool isMaster) 
    {
        if (isMaster)
        {
           
        }
    }

    void Start()
    {
        prevTargetPosition = transform.position;
        nextTargetPosition = new Vector3(Random.Range(-6f, 6f), 0.0f, Random.Range(-6f, 6f));
        networkEntity = GetComponent<EntangleView>().NetworkEntity;
    }

    void Update() 
    {
        timer += Time.deltaTime * 0.3f;

        if (timer > 1)
        {
            prevTargetPosition = nextTargetPosition;
            nextTargetPosition = new Vector3(Random.Range(-9f, 9f), 0.0f, Random.Range(-9f, 9f));
            timer = 0;
        }

        if (networkEntity != null && EN.IsMasterClient)
        {
            transform.position = Vector3.Lerp(prevTargetPosition, nextTargetPosition, timer);
        }
    }
  
}
