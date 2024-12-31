using UnityEngine;
using FirstPersonMobileTools.Utility;

namespace FirstPersonMobileTools.DynamicFirstPerson
{

    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(CameraLook))]

    public class MovementController : MonoBehaviour
    {
        // animation
        [SerializeField] private Animator m_Animator;

        // GUN
        [SerializeField] public Transform cameraTransform;
        [SerializeField] public int rayHittingRange = 20;
        [SerializeField] public AudioClip gunShotSound;
        [SerializeField] private Ray forwardRay;

        [SerializeField] public ParticleSystem muzzleFlash;

        [SerializeField] public GameObject hitEffect;

        // Movement

        #region Class accessible field
        [HideInInspector] public bool Input_Sprint { get; set; }    // Accessed through [Sprint button] in the scene
        [HideInInspector] public bool Input_Jump { get; set; }    // Accessed through [Jump button] in the scene
        [HideInInspector] public bool Input_Crouch { get; set; }    // Accessed through [Crouch button] in the scene
        [HideInInspector] public bool Input_Shoot { get; set; } // Accessed through "Shoot button"

        [HideInInspector] public float Walk_Speed { private get { return m_WalkSpeed; } set { m_WalkSpeed = value; } }          // Accessed through [Walk speed] slider in the settings
        [HideInInspector] public float Run_Speed { private get { return m_RunSpeed; } set { m_RunSpeed = value; } }             // Accessed through [Run speed] slider in the settings
        [HideInInspector] public float Crouch_Speed { private get { return m_CrouchSpeed; } set { m_CrouchSpeed = value; } }    // Accessed through [Crouch speed] slider in the settings
        [HideInInspector] public float Jump_Force { private get { return m_JumpForce; } set { m_JumpForce = value; } }          // Accessed through [Jump Force] slider in the settings
        [HideInInspector] public float Acceleration { private get { return m_Acceleration; } set { m_Acceleration = value; } }  // Accessed through [Acceleration] slider in the settings
        [HideInInspector] public float Land_Momentum { private get { return m_LandMomentum; } set { m_LandMomentum = value; } } // Accessed through [Landing Momentum] slider in the settings
        #endregion

        #region Editor accessible field
        // Input Settings
        [SerializeField] private Joystick m_Joystick;   // Available joystick mobile in the scene

        // Ground Movement Settings
        [SerializeField] private float m_Acceleration = 1.0f;
        [SerializeField] private float m_WalkSpeed = 1.0f;
        [SerializeField] private float m_RunSpeed = 3.0f;
        [SerializeField] private float m_CrouchSpeed = 0.5f;
        [SerializeField] private float m_CrouchDelay = 0.5f;    // Crouch transition time
        [SerializeField] private float m_CrouchHeight = 1.0f;   // Crouch target height

        // Air Movement Settings
        [SerializeField] private float m_JumpForce = 1.0f;      // y-axis force for jumping
        [SerializeField] private float m_Gravity = 10.0f;       // Gravity force
        [SerializeField] private float m_LandMomentum = 2.0f;   // Movement momentum strength after landed

        // Audio Settings
        [SerializeField] private AudioClip[] m_FootStepSounds;  // list of foot step sfx
        [SerializeField] private AudioClip m_JumpSound;         // Jumping sfx
        [SerializeField] private AudioClip m_LandSound;         // Landing sfx

        // Advanced Settings
        [SerializeField] private Bobbing m_WalkBob = new Bobbing(); // Bobbing for walking
        [SerializeField] private Bobbing m_IdleBob = new Bobbing(); // Bobbing for idling

        #endregion

        // Main reference class
        private Camera m_Camera;
        private CharacterController m_CharacterController;
        private CameraLook m_CameraLook;
        private AudioSource m_AudioSource;

        // Main global value
        private Vector3 m_MovementDirection;                        // Vector3 value for CharacterController.Move()
        private Vector3 m_HeadMovement;                             // Used for calculating all the head movement before applying to the camera position
        private Vector3 m_LandBobRange_FinalImpact;                 // Dynamic bob range based on falling velocity
        private Vector3 m_OriginalScale;                            // Original scale for crouching
        private const float m_StickToGround = -1f;                  // force character controller into the ground
        private float m_MinFallLand = -10f;                         // Minimum falling velocity to be considered as landed
        private float m_CrouchTimeElapse = 0.0f;                    // Time beofre 
        private float m_OriginalLandMomentum;                       // Slowdown momentum when landing
        private bool m_IsFloating = false;                          // Player state if is in the air




        private float m_MovementVelocity
        {
            get { return new Vector2(m_CharacterController.velocity.x, m_CharacterController.velocity.z).magnitude; }
        }

        private Vector2 Input_Movement
        {
            get { if (m_Joystick != null) return new Vector2(m_Joystick.Horizontal, m_Joystick.Vertical); else return Vector2.zero; }
        }

        private bool m_IsWalking
        {
            get { return m_MovementVelocity > 0.0f; }
        }

        private bool m_OnLanded
        {
            get { return m_IsFloating && m_MovementDirection.y < m_MinFallLand && m_CharacterController.isGrounded; }
        }

        private bool m_OnJump
        {
            get { return !m_CharacterController.isGrounded && !m_IsFloating; }
        }

        private float m_speed
        {
            get
            {
#if UNITY_EDITOR
                return Input_Crouch? m_CrouchSpeed : Input_Sprint? m_RunSpeed : 
                    Input_Movement.magnitude != 0 || External_Input_Movement.magnitude != 0? m_WalkSpeed : 0.0f; 
#elif UNITY_ANDROID
                return Input_Crouch? m_CrouchSpeed : Input_Sprint? m_RunSpeed : Input_Movement.magnitude != 0? m_WalkSpeed : 0.0f; 
#endif
            }
        }

#if UNITY_EDITOR
        public Vector2 External_Input_Movement;
#endif


        private void Start()
        {
            //m_OriginalHeight = m_CharacterController.height;  // Save the original height


            m_Camera = GetComponentInChildren<Camera>();
            m_AudioSource = GetComponent<AudioSource>();
            m_CharacterController = GetComponent<CharacterController>();
            m_CameraLook = GetComponent<CameraLook>();

            m_Animator = GetComponentInChildren<Animator>();

            if (m_Animator == null)
            {
                Debug.LogError("Animator component not found! Assign it in the inspector or ensure it exists.");
            }

            m_OriginalScale = transform.localScale;
            m_OriginalLandMomentum = m_LandMomentum;

            m_WalkBob.SetUp();
            m_IdleBob.SetUp();

        }

        private void Update()
        {
            forwardRay = new Ray(cameraTransform.position, cameraTransform.forward);
            Debug.DrawRay(forwardRay.origin, forwardRay.direction * rayHittingRange, Color.red);

            Handle_InputMovement();
            Handle_AirMovement();
            Handle_Crouch();
            Handle_Step();
            Shoot();
            UpdateWalkBob();
            // HandleShooting();
            m_CharacterController.Move(m_MovementDirection * Time.deltaTime);

            m_Camera.transform.localPosition += m_HeadMovement;
            m_HeadMovement = Vector3.zero;
        }
       
        private void Shoot()
        {
            RaycastHit hit;
            // Check if the shoot button is pressed
            if (Input_Shoot)
            {
                m_Animator.SetBool("firing", true);
                //m_Animator.SetBool("IsShooting", true); // Trigger the animation

                //GetComponent<Animator>().SetTrigger("IsShooting");
                if (Physics.Raycast(forwardRay, out hit, rayHittingRange))
                {
                    Debug.Log("Hit: " + hit.transform.name);
                    EnemyScript enemy = hit.transform.GetComponent<EnemyScript>();
                    GameObject hitPoint = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(hitPoint, 0.5f);
                    if (enemy != null)
                    {
                        enemy.TakeDamage(1);
                        enemy.OnBulletHit();
                    }
                    Debug.Log("Destroy called for hitPoint.");
                }
                else
                {
                    Debug.Log("No hit detected.");

                }
                muzzleFlash.Play();
                AudioSource.PlayClipAtPoint(gunShotSound, transform.position);
                Input_Shoot = false;

            }
            //m_Animator.SetBool("firing", false);
            //m_Animator.SetBool("IsShooting", false);
        }




        private void Handle_InputMovement()
        {

            Vector2 Input;
#if UNITY_EDITOR
            Input.x = Input_Movement.x == 0? External_Input_Movement.x : Input_Movement.x;
            Input.y = Input_Movement.y == 0? External_Input_Movement.y : Input_Movement.y;
#elif UNITY_ANDROID
            Input.x = Input_Movement.x;
            Input.y = Input_Movement.y;
#endif
            Vector3 WalkTargetDIrection =
                Input.y * transform.forward * m_speed +
                Input.x * transform.right * m_speed;

            WalkTargetDIrection = Input_Sprint && WalkTargetDIrection == Vector3.zero ? transform.forward * m_speed : WalkTargetDIrection;

            m_MovementDirection.x = Mathf.MoveTowards(m_MovementDirection.x, WalkTargetDIrection.x, m_Acceleration * Time.deltaTime);
            m_MovementDirection.z = Mathf.MoveTowards(m_MovementDirection.z, WalkTargetDIrection.z, m_Acceleration * Time.deltaTime);

            if (m_LandMomentum != m_OriginalLandMomentum)
            {
                m_LandMomentum = Mathf.Clamp(m_LandMomentum + Time.deltaTime, 0, m_OriginalLandMomentum);
                m_MovementDirection.x *= m_LandMomentum / m_OriginalLandMomentum;
                m_MovementDirection.z *= m_LandMomentum / m_OriginalLandMomentum;
            }

            // Determine if the character is walking
            bool isWalking = WalkTargetDIrection.magnitude > 0.3f;

            // Update Animator with triggers
            if (m_Animator != null)
            {
                if (isWalking)
                {
                    m_Animator.SetTrigger("StartWalking");
                    Debug.Log("Trigger: StartWalking");
                }
                else
                {
                    m_Animator.SetTrigger("StopWalking");
                }
            }
            // Detect Forward or Backward Movement
        }



        private void Handle_AirMovement()
        {

            if (m_OnLanded)
            {

                m_LandMomentum = 0;
                PlaySound(m_LandSound);

            }

            if (m_CharacterController.isGrounded)
            {

                if (m_IsFloating) m_IsFloating = false;

                // force player to stick to ground or else CharacterController.IsGrounded will return true
                m_MovementDirection.y = m_StickToGround;

                if (Input_Jump)
                {
                    PlaySound(m_JumpSound);
                    m_MovementDirection.y = m_JumpForce;
                    if (m_JumpSound != null) PlaySound(m_JumpSound);
                }

            }
            else
            {

                if (!m_IsFloating) m_IsFloating = true;

                // Prevent floating if jumping is blocked 
                if (m_CharacterController.collisionFlags == CollisionFlags.Above)
                {
                    m_MovementDirection.y = 0.0f;
                }

                m_MovementDirection.y -= m_Gravity * Time.deltaTime;

            }

        }


        private void Handle_Crouch()
        {

            //Crouching State
            if (Input_Crouch && transform.localScale.y != (m_CrouchHeight / m_CharacterController.height) * m_OriginalScale.y)
            {

                CrouchTransition(m_CrouchHeight, Time.deltaTime);
                m_Animator.SetBool("crouching", true);

            }

            // Standing State
            if (!Input_Crouch && transform.localScale.y != m_OriginalScale.y)
            {

                CrouchTransition(m_CharacterController.height, -Time.deltaTime);
                m_Animator.SetBool("crouching", false);

            }

            void CrouchTransition(float TargetHeight, float value)
            {

                // Origin is on top of head to avoid any collision with the player it self
                Vector3 Origin = transform.position + (transform.localScale.y / m_OriginalScale.y) * m_CharacterController.height * Vector3.up;
                if (Physics.Raycast(Origin, Vector3.up, m_CharacterController.height - Origin.y))
                {
                    Input_Crouch = true;
                    return;
                }

                m_CrouchTimeElapse += value;

                m_CrouchTimeElapse = Mathf.Clamp(m_CrouchTimeElapse, 0, m_CrouchDelay);

                transform.localScale = new Vector3(
                    transform.localScale.x,
                    Mathf.Lerp(m_OriginalScale.y, (m_CrouchHeight / m_CharacterController.height) * m_OriginalScale.y, m_CrouchTimeElapse / m_CrouchDelay),
                    transform.localScale.z);

            }

        }


        /*
        private void Handle_Crouch()
        {

            // Crouching State
            if (Input_Crouch && !isCrouching )
            {
                CrouchTransition(m_CrouchHeight, crouchCenterY, Time.deltaTime);
                m_Animator.SetBool("crouching", true);
                Debug.Log("Player is crouching");
            }

            // Standing State
            if (!Input_Crouch && isCrouching)
            {
                CrouchTransition(m_OriginalHeight, standCenterY, -Time.deltaTime);
                m_Animator.SetBool("crouching", false);
                Debug.Log("No crouching");
            }
        }

        private void CrouchTransition(float targetHeight, float targetCenterY, float value)
        {
            // Smoothly adjust the CharacterController's height and center
            m_CrouchTimeElapse += value;

            // Clamp the time elapsed to avoid going beyond the crouch transition delay
            m_CrouchTimeElapse = Mathf.Clamp(m_CrouchTimeElapse, 0, m_CrouchDelay);

            // Update the CharacterController height and center smoothly
            m_CharacterController.height = Mathf.Lerp(m_CharacterController.height, targetHeight, m_CrouchTimeElapse / m_CrouchDelay);
            m_CharacterController.center = new Vector3(0, Mathf.Lerp(m_CharacterController.center.y, targetCenterY, m_CrouchTimeElapse / m_CrouchDelay), 0);

            // Update the player's Y position so that the feet stay on the ground
            if (m_CharacterController.height == m_OriginalHeight || m_CharacterController.height == m_CrouchHeight)
            {
                Vector3 currentPosition = transform.position;
                transform.position = new Vector3(currentPosition.x, m_CharacterController.height / 2, currentPosition.z);
            }

            // Check if crouch transition is complete
            if (Mathf.Approximately(m_CharacterController.height, targetHeight))
            {
                isCrouching = targetHeight == m_CrouchHeight;
            }
        }

        // Helper function to check if the player can crouch (e.g., no obstacle above them)
        private bool CanCrouch()
        {
            Vector3 crouchOrigin = transform.position + Vector3.up * m_CharacterController.height;
            return !Physics.Raycast(crouchOrigin, Vector3.up, 0.1f);  // Check small distance above the player
        }
        */

        private void Handle_Step()
        {

            if (m_FootStepSounds.Length == 0) return;
            if (m_WalkBob.OnStep) PlaySound(m_FootStepSounds[UnityEngine.Random.Range(0, m_FootStepSounds.Length - 1)]);

        }

        private void UpdateWalkBob()
        {

            if ((m_IsWalking && !m_IsFloating) || !m_WalkBob.BackToOriginalPosition)
            {
                float speed = m_MovementVelocity == 0 ? m_WalkSpeed : m_MovementVelocity;
                m_HeadMovement += m_WalkBob.UpdateBobValue(speed, m_WalkBob.BobRange);
            }
            else if (!m_IsWalking || !m_IdleBob.BackToOriginalPosition)
            {
                m_HeadMovement += m_IdleBob.UpdateBobValue(1, m_IdleBob.BobRange);
            }

        }

        // Utility function
        private void PlaySound(AudioClip audioClip)
        {
            m_AudioSource.clip = audioClip;
            if (m_AudioSource.clip != null) m_AudioSource.PlayOneShot(m_AudioSource.clip);
        }


    }

}

