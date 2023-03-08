using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class getHandPos : MonoBehaviour
{
    [SerializeField]
    private Transform handRight, handLeft;

    [SerializeField]
    private float offsetY = 0.07f;
   
    IMixedRealityHandJointService handJointService;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.DontDestroyOnLoad();
        handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (handJointService != null)
        {
            try
            {
                Transform jointTransformRight = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
                Transform jointTransformLeft = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left);
               
                handRight.position = jointTransformRight.position - new Vector3(0,offsetY,0);
                handRight.rotation = jointTransformRight.rotation;
                handLeft.position = jointTransformLeft.position - new Vector3(0,offsetY,0);
                handLeft.rotation = jointTransformLeft.rotation;
                
            }
            catch
            {
                Debug.Log("cannot find hands");
            }
        }
   
    }
}
