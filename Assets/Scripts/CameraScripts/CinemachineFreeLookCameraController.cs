using UnityEngine;
using Cinemachine;
//This script allows you to modify Cinemachine through code

public class CinemachineFreelookCameraControl : MonoBehaviour
{
    [Tooltip("The cinemachine virtual camera script")]
    [SerializeField] CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] Transform focusObjectTransform; //The transform of the object the camera is focusing on

    void Awake()
    {
        Camera.main.gameObject.TryGetComponent<CinemachineBrain>(out var brain); //This will output a variable called brain
        if (brain == null) //If there is no brain
        {
            brain = Camera.main.gameObject.AddComponent<CinemachineBrain>(); //Add a brain
        }
        brain.m_DefaultBlend.m_Time = 1; //Controls the blend time between cameras

        cinemachineFreeLook = gameObject.AddComponent<CinemachineFreeLook>();
        cinemachineFreeLook.Follow = focusObjectTransform;
        cinemachineFreeLook.LookAt = focusObjectTransform;
        cinemachineFreeLook.Priority = 2;
        cinemachineFreeLook.m_SplineCurvature = 3;

        CinemachineVirtualCamera toprig = cinemachineFreeLook.GetRig(0);
        CinemachineVirtualCamera middlerig = cinemachineFreeLook.GetRig(1);
        CinemachineVirtualCamera bottomrig = cinemachineFreeLook.GetRig(2);

        toprig.AddCinemachineComponent<CinemachineTransposer>();

        CinemachineBasicMultiChannelPerlin noise = toprig.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;

        CinemachineCollider cinemachineCollider = gameObject.AddComponent<CinemachineCollider>();
        cinemachineCollider.m_Damping = 1;
    }

}
