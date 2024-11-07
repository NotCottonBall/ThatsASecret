using System.Numerics;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Analytics;

public class CharacterController : MonoBehaviour
{
    // Serialized Variables
    [SerializeField] Camera m_playerCamera;
    [SerializeField] float m_cameraSensitivity = 2.0f;
    // Class Variabels
    Rigidbody m_playerRB;
    float m_cameraPitch = 0.0f;

    // Public Variables
    public float PlayerSpeed = 600.0f;


    void Awake()
    {
        m_playerRB = GetComponent<Rigidbody>();
        if(m_playerRB == null)
            Debug.LogError("No Rigidbody Attached To The Player Character");
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        m_playerRB.linearDamping = 2.0f;

        m_playerCamera.transform.SetParent(transform);
    }

    void FixedUpdate()
    {
        float vMove = Input.GetAxisRaw("Vertical");
        float hMove = Input.GetAxisRaw("Horizontal");

        UnityEngine.Vector3 direction = UnityEngine.Vector3.zero;
        direction = (m_playerCamera.transform.forward * vMove) +
                    (m_playerCamera.transform.right * hMove);
        direction.y = 0;
        direction.Normalize();

        m_playerRB.linearVelocity = new UnityEngine.Vector3(
                                        direction.x * PlayerSpeed,
                                        m_playerRB.linearVelocity.y,
                                        direction.z * PlayerSpeed) * Time.deltaTime;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        // @TODO: Does this work in unity? Does this only run in Debug Mode?
        #if DEBUG
        if(Input.GetKeyDown(KeyCode.P))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endif

        if(Cursor.lockState == CursorLockMode.Locked) CameraMovement();
    }


    void CameraMovement()
    {
        float xMouse = Input.GetAxis("Mouse X") * m_cameraSensitivity;
        float yMouse = Input.GetAxis("Mouse Y") * m_cameraSensitivity;

        transform.Rotate(UnityEngine.Vector3.up * xMouse);
        m_playerCamera.transform.rotation = transform.rotation;

        m_cameraPitch -= yMouse;
        Mathf.Clamp(m_cameraPitch, -90.0f, 90.0f);
        m_playerCamera.transform.localRotation =
            UnityEngine.Quaternion.Euler(m_cameraPitch, 0.0f, 0.0f);
    }
}
