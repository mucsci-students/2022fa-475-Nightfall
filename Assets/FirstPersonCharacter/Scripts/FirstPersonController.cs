using System;
using System.Linq;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;
//using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour, ICustomMessenger
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip[] m_JumpSounds;        // An array of jump sounds that will be randomly selected when the player jumps.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
        [SerializeField] private static float _furnaceReachDistance = 10;
        [SerializeField] private static GameObject plr;
        [SerializeField] private List<GameObject> _spawnWhenKilled = new List<GameObject>();

        private Camera m_Camera;
        private Camera m_Other_Camera;
        private SkinnedMeshRenderer m_Right_Arm;
        private SkinnedMeshRenderer m_Left_Arm;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
        private FogEffect fogEffect;
        private UnderWaterEffect underwater;

        public MouseLook GetMouseLook() => m_MouseLook;

        private bool cursorLock = true;
        public void ToggleMenuMessage()
        {
            cursorLock = !cursorLock;
            m_MouseLook.SetCursorLock(cursorLock);
        }
        private bool benderMode = false;
        public void BenderModeMessage()
        {
            benderMode = !benderMode;
            if (benderMode)
            {
                cursorLock = true;
                m_MouseLook.SetCursorLock(true);
                m_Other_Camera.fieldOfView = 120f;
                m_Left_Arm.enabled = false;
                m_Right_Arm.enabled = false;
            }
            else
            {
                cursorLock = false;
                m_MouseLook.SetCursorLock(false);
                m_Other_Camera.fieldOfView = 45f;
                m_Right_Arm.enabled = true;
                m_Left_Arm.enabled = true;
            }
        }

        public static bool IsNearFurnace() => FindObjectsOfType<IFurnace>()
           .Where(furnace => Vector3.Distance(plr.transform.position, furnace.transform.position) <= _furnaceReachDistance)
           .FirstOrDefault() != null;

        public static bool IsNearWorkbench() => FindObjectsOfType<IWorkbench>()
           .Where(workbench => Vector3.Distance(plr.transform.position, workbench.transform.position) <= _furnaceReachDistance)
           .FirstOrDefault() != null;

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Other_Camera = GameObject.Find("Player/CameraHolder").GetComponent<Camera>();
            m_Left_Arm = GameObject.Find("Player/CameraHolder/Male/fp_male_hand_left/fp_male_hand").GetComponent<SkinnedMeshRenderer>();
            m_Right_Arm = GameObject.Find("Player/CameraHolder/Male/fp_male_hand_right/fp_male_hand").GetComponent<SkinnedMeshRenderer>();
            
            // m_Camera = GetComponent<Camera>();
            m_Camera = gameObject.transform.GetChild(0).GetComponent<Camera>();
            underwater = gameObject.transform.GetChild(0).GetComponent<UnderWaterEffect>();
            fogEffect = gameObject.transform.GetChild(0).GetComponent<FogEffect>();
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);

            // Attach death handler
            PlayerHandler.OnPlayerKilled += HandlePlayerKilled;

            plr = GameObject.Find("Player");
        }

        private void HandlePlayerKilled(object sender, EventArgs args)
        {

            // Spawn item(s) when player dies
            foreach (var itemToSpawn in _spawnWhenKilled)
            {
                var spawned = Instantiate(itemToSpawn, gameObject.transform);
                if (spawned.transform.parent != null) { spawned.transform.parent = null; }
            }

            PlayerHandler.OnPlayerKilled -= HandlePlayerKilled;
            Destroy(gameObject);

        }

        // Update is called once per frame
        private void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump && !m_Jumping)
            {
                m_Jump = Input.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }

        private float sprintTimer = 0;
        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;

            sprintTimer += Time.deltaTime;
            if (sprintTimer >= 1)
            {
                sprintTimer = 0;
                if (m_IsWalking)
                {
                    PlayerHandler.AddValue("stamina", 3);
                }
                else if (speed >= 1)
                {
                    PlayerHandler.AddValue("stamina", -4);
                }
            }

            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);

            m_MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound()
        {

            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_JumpSounds.Length);
            m_AudioSource.clip = m_JumpSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_JumpSounds[n] = m_JumpSounds[0];
            m_JumpSounds[0] = m_AudioSource.clip;

            // m_AudioSource.clip = m_JumpSound;
            // m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }

        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = PlayerHandler.GetValue("stamina") > 4 ? !Input.GetKey(KeyCode.LeftShift) : true;
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
