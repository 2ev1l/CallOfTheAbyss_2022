using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameFight;
using static UnityEngine.ParticleSystem;

namespace Universal
{
    public class CustomAnimation : MonoBehaviour
    {
        public static CustomAnimation instance { get; private set; }

        #region methods
        public void Init()
        {
            instance = this;
        }
        public static IEnumerator MoveTo(Vector3 finalPosition, GameObject movingObj, float speed = 0.4f, float maxWaitingTime = Mathf.Infinity)
        {
            bool isCurrentSceneFight = SceneManager.GetActiveScene().name.Equals("GameFight");
            Transform objTransform = movingObj.transform;
            Vector3 startPosition = objTransform.position;
            Vector3 direction = finalPosition - startPosition;
            float distance = Vector3.Distance(finalPosition, startPosition);
            Vector3 finalPositionX = startPosition + direction * FightAnimationInit.animationSpeed;
            float deadZone = 0.01f;
            float waitedTime = 0f;
            while (distance > deadZone && maxWaitingTime > waitedTime)
            {
                if (FightAnimationInit.skipAnimation && isCurrentSceneFight) break;
                objTransform.position = Vector3.Lerp(startPosition, finalPositionX, speed * Time.deltaTime);
                distance = Vector3.Distance(finalPosition, startPosition);
                float deadZoneMax = Vector3.Distance(startPosition, objTransform.position) * 1.04f;
                if (deadZone < deadZoneMax)
                    deadZone = deadZoneMax;
                startPosition = objTransform.position;
                waitedTime += Time.deltaTime;
                yield return CustomMath.WaitAFrame();
            }
            movingObj.transform.position = finalPosition;
        }
        public static IEnumerator MoveToLocal(Vector3 finalLocalPosition, Transform currentObject, float speed = 1f)
        {
            float duration = 1f / speed;
            float lerp = Time.deltaTime;
            float squareLerp = 0f;
            float deadZone = 0f;
            float distance = Vector3.Distance(currentObject.localPosition, finalLocalPosition);
            float startDistance = Vector3.Distance(currentObject.localPosition, finalLocalPosition);
            Vector3 lastPosition = currentObject.localPosition;
            while (duration >= lerp && distance > deadZone)
            {
                float currentProgress = lerp / duration;
                if (lerp < duration / 1.67f)
                    squareLerp = currentProgress * 2.4f;
                else
                    squareLerp = currentProgress * 2.4f * Mathf.Pow((1.7f - currentProgress), 2);

                float step = startDistance * Time.deltaTime * speed;
                currentObject.localPosition += currentObject.right * step * squareLerp;
                float maxDeadZone = Vector3.Distance(currentObject.localPosition, lastPosition) * 1.04f;
                if (maxDeadZone > deadZone) deadZone = maxDeadZone;
                lerp += Time.deltaTime;
                lastPosition = currentObject.localPosition;
                distance = Vector3.Distance(currentObject.localPosition, finalLocalPosition);
                yield return CustomMath.WaitAFrame();
            }
            currentObject.localPosition = finalLocalPosition;
        }
        private IEnumerator UpdateIntCounterSmooth(string objectName, int toCount, float lerp, bool isCharFromEnd, float timeWaited = 0f)
        {
            yield return CustomMath.WaitAFrame();
            if (GameObject.Find(objectName) == null) yield break;

            float duration = 5f;
            lerp += Time.deltaTime / duration;
            timeWaited += Time.deltaTime;
            Text txt = GameObject.Find(objectName).transform.Find("Text").GetComponent<Text>();
            int txtCount = 0;
            if (isCharFromEnd)
                txtCount = System.Convert.ToInt32(txt.text.Remove(txt.text.Length - 1).ToString());
            else
                txtCount = System.Convert.ToInt32(txt.text.Remove(0, 1).ToString());

            int inc = 0;
            if (txtCount > toCount)
            {
                inc = -1;
            }
            else if (txtCount < toCount)
            {
                inc = 1;
            }
            txtCount = (int)Mathf.Lerp(txtCount, toCount, lerp);


            if (isCharFromEnd)
                txt.text = $"{txtCount}:";
            else
                txt.text = $":{txtCount}";

            if (toCount != txtCount)
                //txt.color = Color.white;
                //else
                txt.color = new Color(0f, (float)toCount / txtCount, (float)toCount / txtCount, (float)toCount / txtCount);

            if (toCount == txtCount + inc && timeWaited >= duration - 2f)
            {
                StartCoroutine(UpdateIntCounterSmoothEnd(objectName, 0.5f, toCount, isCharFromEnd));
            }
            else
            {
                if (toCount == txtCount && timeWaited >= duration - 2f)
                    StartCoroutine(UpdateIntCounterSmoothEnd(objectName, 0f, toCount, isCharFromEnd));
                else
                    StartCoroutine(UpdateIntCounterSmooth(objectName, toCount, lerp, isCharFromEnd, timeWaited));
            }
        }
        private IEnumerator UpdateIntCounterFast(string objectName, int toCount, bool isCharFromEnd)
        {
            yield return CustomMath.WaitAFrame();
            if (GameObject.Find(objectName) == null) yield break;
            Text txt = GameObject.Find(objectName).transform.Find("Text").GetComponent<Text>();
            if (isCharFromEnd)
                txt.text = $"{toCount}:";
            else
                txt.text = $":{toCount}";
            txt.color = Color.white;
        }
        private IEnumerator UpdateIntCounterSmoothEnd(string objectName, float sec, int toCount, bool isCharFromEnd)
        {
            yield return new WaitForSeconds(sec);
            if (GameObject.Find(objectName) == null)
                yield break;
            Text txt = GameObject.Find(objectName).transform.Find("Text").GetComponent<Text>();
            int txtCount = toCount;
            if (isCharFromEnd)
                txt.text = $"{txtCount}:";
            else
                txt.text = $":{txtCount}";
            txt.color = Color.white;
        }
        public Vector3 UpdateIntCounterSmooth(string objectName, int toCount, float lerp, bool fromEnd, float offset, Vector3 toPosition)
        {
            GameObject obj = GameObject.Find(objectName);
            if (obj == null) return toPosition;
            if (toCount == 0)
            {
                obj.SetActive(false);
                return toPosition;
            }
            obj.transform.localPosition = toPosition;
            toPosition.y -= offset;
            StartCoroutine(UpdateIntCounterSmooth(objectName, toCount, lerp, fromEnd));
            return toPosition;
        }
        public Vector3 UpdateIntCounterFast(string objectName, int toCount, bool fromEnd, float offset, Vector3 toPosition)
        {
            GameObject obj = GameObject.Find(objectName);
            if (obj == null) return toPosition;
            if (toCount == 0)
            {
                obj.SetActive(false);
                return toPosition;
            }
            obj.transform.localPosition = toPosition;
            toPosition.y -= offset;
            StartCoroutine(UpdateIntCounterFast(objectName, toCount, fromEnd));
            return toPosition;
        }
        public static IEnumerator SetTextAlpha(Text txt, float from, float to, float sec)
        {
            Color cols = txt.color;
            cols.a = from;
            txt.color = cols;
            float lerp = 0;
            float colDistance = Mathf.Abs(from - to);
            float colPerSec = (float)colDistance / sec;
            if (from < to)
                while (txt.color.a < to)
                {
                    try
                    {
                        Color col = txt.color;
                        col.a += Time.deltaTime * colPerSec;
                        txt.color = col;
                        lerp += Time.deltaTime;
                    }
                    catch { yield break; }
                    yield return CustomMath.WaitAFrame();
                    if (lerp > sec) break;
                }
            else
                while (txt.color.a > to)
                {
                    try
                    {
                        Color col = txt.color;
                        col.a -= Time.deltaTime * colPerSec;
                        txt.color = col;
                        lerp += Time.deltaTime;
                    }
                    catch { yield break; }
                    yield return CustomMath.WaitAFrame();
                    if (lerp > sec) break;
                }
            cols = txt.color;
            cols.a = to;
            txt.color = cols;
        }
        public static void BurstParticlesAt(Vector3 position, ParticleSystem particleSystem)
        {
            particleSystem.transform.position = position;
            Burst burst = particleSystem.emission.GetBurst(0);
            int particlesCount = Random.Range(burst.minCount, burst.maxCount);
            Vector3 startScale = particleSystem.transform.localScale;
            float optimalScale = CustomMath.GetOptimalScreenScale();
            particleSystem.transform.localScale = new Vector3(startScale.x * optimalScale, startScale.y * optimalScale, startScale.z * optimalScale);
            particleSystem.Emit(particlesCount);
            particleSystem.transform.localScale = startScale;
        }
        #endregion methods
    }
}