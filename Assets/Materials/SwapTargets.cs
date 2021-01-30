using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SwapTargets : MonoBehaviour
{
    Transform temp;
    [SerializeField] DampedTransform[] dampedTransforms;
    [SerializeField] Transform[] tentacleParts;
    [SerializeField] Transform rigParent;
    [SerializeField] Vector3 posConstraintOffset;
    [SerializeField] bool maintainOffset;
    [SerializeField][Range(0,1)] float posDamp;
    [SerializeField][Range(0,1)] float rotDamp;
    [SerializeField] MultiReferentialConstraint multiReferentialConstraint;
    [ContextMenu("Populate multi")]
    public void Populate()
    {
        multiReferentialConstraint.data.sourceObjects = tentacleParts.ToList();
    }    
    [ContextMenu("Swap")]
    public void Swap()
    {
        for (int i = 0; i < dampedTransforms.Length; i++)
        {
            var dampedTransform = dampedTransforms[i];
            temp = dampedTransform.data.constrainedObject;
            dampedTransform.data.constrainedObject = dampedTransform.data.sourceObject;
            dampedTransform.data.sourceObject = temp;
        }
    } 
    [ContextMenu("AdjustDampStrength")]
    public void AdjustDampStrength()
    {
        for (int i = 0; i < dampedTransforms.Length; i++)
        {
            var dampedTransform = dampedTransforms[i];
            dampedTransform.data.dampPosition = posDamp;
            dampedTransform.data.dampRotation = rotDamp;
        }
    }
    [ContextMenu("SwapPosConstraints")]
    public void SwapPosConstraints()
    {
        for (int i = 0; i < dampedTransforms.Length; i++)
        {
            var gameObject = dampedTransforms[i].gameObject;
            var multiPositionConstraint = gameObject.GetComponent<MultiPositionConstraint>();
            WeightedTransform weighted = new WeightedTransform(multiPositionConstraint.data.constrainedObject, 1); ;
            multiPositionConstraint.data.constrainedObject = multiPositionConstraint.data.sourceObjects[0].transform;
            multiPositionConstraint.data.sourceObjects = new WeightedTransformArray { weighted };
        }
    }

    [ContextMenu("CreateRig")]
    public void CreateRig()
    {
        for (int i = 1; i < tentacleParts.Length; i++)
        {
            GameObject damp = new GameObject("damping" + i);
            damp.transform.parent = rigParent;
            var dampedTrans = damp.AddComponent<DampedTransform>();
            dampedTrans.data.sourceObject = tentacleParts[i - 1];
            dampedTrans.data.constrainedObject = tentacleParts[i];
            dampedTrans.data.dampPosition = 0;
        }
        dampedTransforms = rigParent.GetComponentsInChildren<DampedTransform>();
    }
    [ContextMenu("AddPositionConstraint")]
    public void AddPositionConstraint()
    {
        for (int i = 0; i < dampedTransforms.Length; i++)
        {
            var gameObject = dampedTransforms[i].gameObject;
            var multiPositionConstraint = gameObject.AddComponent<MultiPositionConstraint>();
            multiPositionConstraint.data.constrainedObject = dampedTransforms[i].data.sourceObject;
            WeightedTransform weighted = new WeightedTransform(dampedTransforms[i].data.constrainedObject, 1);

            multiPositionConstraint.data.sourceObjects = new WeightedTransformArray { weighted };
            //   multiPositionConstraint.data.offset = Vector3.one;
        }
    }
    [ContextMenu("EditPositionConstaint")]
    public void PositionConstraintOffset()
    {
        for (int i = 0; i < dampedTransforms.Length; i++)
        {
            var gameObject = dampedTransforms[i].gameObject;
            var multiPositionConstraint = gameObject.GetComponent<MultiPositionConstraint>();
            multiPositionConstraint.data.offset = posConstraintOffset;
            multiPositionConstraint.data.maintainOffset = maintainOffset;
        }
    }
    [ContextMenu("RescaleParts")]
    public void RescaleParts()
    {
        for (int i = 0; i < tentacleParts.Length; i++)
        {
            tentacleParts[i].localScale = (((i + ((0.1f * tentacleParts.Length) / 1.9f))) / (tentacleParts.Length / 1.9f)) * Vector3.one;
        }
        for (int i = 1; i < tentacleParts.Length; i++)
        {
            Vector3 pos =  tentacleParts[i-1].transform.position;
            Vector3 offset= (tentacleParts[i - 1].localScale + tentacleParts[i].localScale *0.6f);
            pos.y = pos.y - offset.y;
            tentacleParts[i].transform.position = pos;
        }
    }
}