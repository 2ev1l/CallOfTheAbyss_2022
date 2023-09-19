using System.Collections;
using UnityEngine;
using Universal;

namespace GameAdventure
{
    public class ArrowAnimation : MonoBehaviour
    {
        #region fields
        [SerializeField] private GameObject startObject;
        [SerializeField] private GameObject nextObject;
        #endregion fields

        #region methods
        private void OnEnable()
        {
            transform.localPosition = startObject.transform.localPosition;
            StartCoroutine(Moving());
        }
        private IEnumerator Moving()
        {
            Vector3 startPosition = startObject.transform.localPosition;
            Vector3 finalPosition = nextObject.transform.localPosition;
            Vector3 direction = finalPosition - startPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
            transform.localPosition = startObject.transform.localPosition;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            yield return CustomAnimation.MoveToLocal(finalPosition, gameObject.transform, 1f);
            yield return RotateToPoint(gameObject.transform, -direction, Vector3.forward, 1.5f, FunctionType.Parabolic);
            yield return CustomAnimation.MoveToLocal(startPosition, gameObject.transform, 1f);
            yield return RotateToPoint(gameObject.transform, direction, Vector3.forward, 1.5f, FunctionType.Parabolic);
            StartCoroutine(Moving());
        }

        private IEnumerator RotateToPoint(Transform transform, Vector3 direction, Vector3 axis, float speed = 1f, FunctionType functionType = FunctionType.Constant)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
            float fixedDeltaTime = Time.fixedDeltaTime;
            Vector3 localEulerAngles = transform.localEulerAngles;
            float startAngle = (localEulerAngles * axis.x).x + (localEulerAngles * axis.y).y + (localEulerAngles * axis.z).z;
            float maxDegreesDelta = (startAngle - angle) * fixedDeltaTime * speed;
            float duration = 1f / speed;
            float lerp = fixedDeltaTime;
            float squareLerp = 0;
            float linearLerp = 0;
            float hyperbolicLerp = 0;
            Quaternion angleAxis = Quaternion.AngleAxis(angle, axis);
            while (angleAxis != transform.rotation && duration > lerp)
            {
                linearLerp = lerp * speed * 2f;
                if (functionType == FunctionType.Parabolic)
                {
                    squareLerp = lerp <= duration / 2.63f ?
                        linearLerp * speed * 2f :
                        squareLerp - fixedDeltaTime * speed;

                    squareLerp = Mathf.Clamp(squareLerp, fixedDeltaTime * speed * 2, linearLerp * speed * 2f);
                }
                if (functionType == FunctionType.Hyperbolic)
                {
                    hyperbolicLerp = 1.5f - linearLerp + lerp * speed;
                }
                float maxDegreesDeltaFunc = functionType switch
                {
                    FunctionType.Constant => maxDegreesDelta,
                    FunctionType.Parabolic => maxDegreesDelta * squareLerp,
                    FunctionType.Hyperbolic => maxDegreesDelta * hyperbolicLerp,
                    FunctionType.Linear => maxDegreesDelta * linearLerp,
                    _ => throw new System.NotImplementedException()
                };
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angleAxis, maxDegreesDeltaFunc);

                lerp += fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, angleAxis, 180);
        }
        #endregion methods
    }
}