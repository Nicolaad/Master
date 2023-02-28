using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class getHandPos : MonoBehaviour
{

    Transform handRight;
    public float offsetY = 0.07f;
    Transform handLeft;
    // Start is called before the first frame update
    void Start()
    {
        handRight = this.gameObject.transform.GetChild(0);
        handLeft = this.gameObject.transform.GetChild(1);
        gameObject.DontDestroyOnLoad();
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
