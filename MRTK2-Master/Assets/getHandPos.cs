using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class getHandPos : MonoBehaviour
{

    Transform sphereRight;
    public float offsetY = 0.07f;
    Transform sphereLeft;
    // Start is called before the first frame update
    void Start()
    {
        sphereRight = this.gameObject.transform.GetChild(0);
        sphereLeft = this.gameObject.transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        var handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        if (handJointService != null)
        {
            try
            {
                Transform jointTransformRight = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
                Transform jointTransformLeft = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left);
               
                sphereRight.position = jointTransformRight.position - new Vector3(0,offsetY,0);
                sphereRight.rotation = jointTransformRight.rotation;
                sphereLeft.position = jointTransformLeft.position - new Vector3(0,offsetY,0);
                sphereLeft.rotation = jointTransformLeft.rotation;
                
            }
            catch
            {
                
            }
        }
    }
}
