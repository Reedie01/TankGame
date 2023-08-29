using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class EnemyTankMovement : MonoBehaviour
{
    // The tank will stop moving towards the player once it reaches this distance
    public float m_CloseDistance = 8f;
    // The tank's turret object
    public Transform m_Turret;
    // A reference to the player - this will be set when the enemy is loaded
    private GameObject m_Player;
    // A reference to the nav mesh agent component
    private NavMeshAgent m_NavAgent;
    // A reference to the rigidbody component
    private Rigidbody m_Rigidbody;
    // Will be set to true when this tank should follow the player
    private bool m_Follow;

    private bool m_moving;

    public Transform homeBase;
    // Use this for initialization
    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Follow = false;
        m_moving = false;
    }
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        // get distance from player to enemy tank
        float distance = (m_Player.transform.position - transform.position).magnitude;
        float homeBaseDistance = (homeBase.transform.position - transform.position).magnitude;

        if (!m_Follow && m_moving)
        {
            m_Rigidbody.isKinematic = false;
            m_NavAgent.SetDestination(homeBase.transform.position);
            m_NavAgent.isStopped = false;
            m_moving = true;
        }
        if (distance < m_CloseDistance && homeBaseDistance < 0.5f)
        {
            m_moving = false;
        }
        if (m_moving == false)
            return;

        // if distance is less than stop distance, then stop moving
        if (distance > m_CloseDistance)
        {
            m_NavAgent.SetDestination(m_Player.transform.position);
            m_NavAgent.isStopped = false;
        }
        else
        {
            m_NavAgent.isStopped = true;
        }
        if (m_Turret != null)
        {
            m_Turret.LookAt(m_Player.transform);
        }        
    }
    private void OnEnable()
    {
        // when the tank is turned on, make sure it is not kinematic
        m_Rigidbody.isKinematic = false;
    }
    private void OnDisable()
    {
        // when the tank is turned off, set it to kinematic so it stops moving
        m_Rigidbody.isKinematic = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Follow = true;
            m_moving = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Follow = false;
        }
    }
}
