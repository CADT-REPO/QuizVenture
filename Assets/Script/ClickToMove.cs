using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Unity.AI.Navigation.Samples
{
    [RequireComponent(typeof(NavMesh))]
    public class ClickToMove : MonoBehaviour
    {
        private NavMeshAgent agent;
        private RaycastHit hitInfo = new RaycastHit();
        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();


        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
                {
                    agent.destination = hitInfo.point;
                }
            }
            if (agent.velocity.magnitude != 0f)
            {
                animator.SetBool("running", true);
            }
            else
            {
                animator.SetBool("running", false);
            }

        }
        void OnAnimatorMove()
        {
            if (animator.GetBool("running"))
            {
                agent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
            }
        }
    }

}

