using UnityEngine;
using UnityEngine.AI;

namespace Script
{
    public class EnemyController : MonoBehaviour
    {
        public NavMeshAgent nevMeshAgent;
        public GameObject player;
        public PlayerController playerController;
        
        // Start is called before the first frame update
        private void Start()
        {
            nevMeshAgent = GetComponent<NavMeshAgent>();
            playerController = player.GetComponent<PlayerController>();
        }

        // Update is called once per frame
        private void Update()
        {
            var b = player.transform.position;
            if (playerController.move != Vector2.zero)
            {
                Vector3 a = new Vector3(2,0,0);
                var c = b - a;
                nevMeshAgent.SetDestination(c);
                Debug.Log($"player {b}");
                Debug.Log(nevMeshAgent.SetDestination(c));
            }
        }
    }
}
