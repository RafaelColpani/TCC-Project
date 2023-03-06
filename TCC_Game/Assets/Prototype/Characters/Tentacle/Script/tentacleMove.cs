using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tentacle
{
    public class tentacleMove : MonoBehaviour
    {
        //Eu preciso que o tentaculo pegue a distancia do player apra ele começar a fazer o procedural
        #region Variaveis
       private GameObject playerTag;

       //Change collider arc in real time
       public CircleCollider2D  Distance;
       [Range(0f, 5f)] [SerializeField] float DistanceToPlayer = 3.5f;
       
      [Range(0f, 5f)] [SerializeField] float AttackPlayer = 2.5f;
      
        #endregion
    
        private void Start() {
            playerTag = GameObject.FindWithTag("Player");
            
        }

        private void Update() {
            Distance.radius = DistanceToPlayer;
            
        }   

        private void OnTriggerEnter2D(Collider2D other) {
            //Localiza quando o Player entrou na area do obbjeto = distancia
            gameObject.GetComponent<tentacleProcedural>().enabled = false;
            if(other.tag == "Player")
            {
                Debug.Log("Player inside");
                if(AttackPlayer <= playerTag.transform.position.x || AttackPlayer >= playerTag.transform.position.x)
                {
                    Debug.Log("Procedural is start");
                    gameObject.GetComponent<tentacleProcedural>().enabled = true;
                }
            }
            
        }
        

        
        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, DistanceToPlayer);

            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackPlayer);
            
        }
        
    }    
}
