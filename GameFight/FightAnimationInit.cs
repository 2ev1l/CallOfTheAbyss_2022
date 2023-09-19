using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Runtime.InteropServices;
using GameFight.Card;
using Data;
using Universal;

namespace GameFight
{
    public class FightAnimationInit : SingleSceneInstance
    {
        #region fields & properties
        public static FightAnimationInit instance { get; private set; }
        public UnityAction OnSkipFightAnimation;
        public UnityAction<bool> OnChangeAnimationState;
        public UnityAction<bool, bool> OnAnimationEnd;

        [SerializeField] private GameObject isAnimationIcon;
        [SerializeField] private CanvasGroup afterBattlePanel;
        [SerializeField] private GameObject completedPanel;
        [SerializeField] private GameObject deathPanel;

        private static bool isEnemyFightAnimation = true;
        private static bool isAllyFightAnimation = true;

        public static bool isFightAnimation { get; private set; }
        public static bool skipAnimation { get; private set; }
        public static float animationSpeed = 3f;
        #endregion fields & properties

        #region methods
        private void Start()
        {
            isEnemyFightAnimation = true;
            isAllyFightAnimation = true;
            bool enableSkipAnimation = GameDataInit.IsArtifactEffectApplied(ArtifactEffect.Hourglass);
            isAnimationIcon.GetComponent<Image>().raycastTarget = enableSkipAnimation;
        }
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        private void OnEnable()
        {
            OnSkipFightAnimation += SkipFightAnimation;
            OnChangeAnimationState += ChangeFightAnimationState;
            OnAnimationEnd += EndAnimation;
            StageEndInit.instance.OnStageEnded += SetAlphaAfterBattle;
            StageEndInit.instance.OnStageEnded += SetAfterBattlePanel;
        }
        private void OnDisable()
        {
            OnSkipFightAnimation -= SkipFightAnimation;
            OnChangeAnimationState -= ChangeFightAnimationState;
            OnAnimationEnd -= EndAnimation;
            StageEndInit.instance.OnStageEnded -= SetAlphaAfterBattle;
            StageEndInit.instance.OnStageEnded -= SetAfterBattlePanel;
        }
        private void SetAfterBattlePanel(bool isCompleted)
        {
            if (isCompleted)
                completedPanel.SetActive(true);
            else
                deathPanel.SetActive(true);
        }
        private void EndAnimation(bool isEnemy, bool changeAnimationState)
        {
            if (isEnemy)
                isEnemyFightAnimation = false;
            else
                isAllyFightAnimation = false;

            if (!isEnemyFightAnimation && !isAllyFightAnimation && changeAnimationState)
                OnChangeAnimationState?.Invoke(false);
        }
        private static void ChangeFightAnimationState(bool state)
        {
            isFightAnimation = state;
            instance.isAnimationIcon.SetActive(state);
        }
        private void SkipFightAnimation()
        {
            skipAnimation = true;
            StartCoroutine(StopSkipFightAnimation());
        }
        private IEnumerator StopSkipFightAnimation()
        {
            while (isFightAnimation)
                yield return CustomMath.WaitAFrame();
            skipAnimation = false;
        }
        private void SetAlphaAfterBattle(bool isCompleted) => StartCoroutine(SetAfterBattleAlpha());
        private IEnumerator SetAfterBattleAlpha()
        {
            while (afterBattlePanel.alpha < 0.99f)
            {
                afterBattlePanel.alpha += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            afterBattlePanel.alpha = 1f;
        }
        public void UpdateIntCounterSmoothByText(Text txt, int toCount, float startOffset, bool isCharFromEnd, Color startColor, Color finalColor,
            [Optional] bool checkOnFinalValue, [Optional] CardFightInit cardInit, [Optional] UpdateValueType type)
        {
            if (checkOnFinalValue)
                StartCoroutine(UpdateIntCounterSmooth(txt, toCount, startOffset, isCharFromEnd, startColor, finalColor, checkOnFinalValue, cardInit, type));
            else
                StartCoroutine(UpdateIntCounterSmooth(txt, toCount, startOffset, isCharFromEnd, startColor, finalColor));
        }
        private IEnumerator UpdateIntCounterSmooth(Text txt, int toCount, float lerp, bool isCharFromEnd, Color startColor, Color finalColor,
            [Optional] bool checkOnFinalValue, [Optional] CardFightInit cardInit, [Optional] UpdateValueType type)
        {
            yield return CustomMath.WaitAFrame();
            float duration = 4f;
            lerp += Time.deltaTime / duration;

            int txtCount = 0;
            if (isCharFromEnd)
                txtCount = System.Convert.ToInt32(txt.text.Remove(txt.text.Length - 1).ToString());
            else
                txtCount = System.Convert.ToInt32(txt.text.Remove(0, 1).ToString());

            int inc = 0;
            if (txtCount > toCount)
                inc = -1;
            else if (txtCount < toCount)
                inc = 1;
            else
            {
                StartCoroutine(UpdateIntCounterSmoothEnd(txt, 0f, toCount, isCharFromEnd, finalColor));
                yield break;
            }

            txtCount = (int)Mathf.Lerp(txtCount, toCount, lerp);
            if (isCharFromEnd)
                txt.text = $"{txtCount}:";
            else
                txt.text = $":{txtCount}";
            txt.color = startColor;

            if (toCount == txtCount + inc)
            {
                if (checkOnFinalValue)
                    StartCoroutine(UpdateIntCounterSmoothEnd(txt, 0.3f, toCount, isCharFromEnd, finalColor, checkOnFinalValue, cardInit, type));
                else
                    StartCoroutine(UpdateIntCounterSmoothEnd(txt, 0.3f, toCount, isCharFromEnd, finalColor));
            }
            else
            {
                if (checkOnFinalValue)
                {
                    if (skipAnimation)
                        StartCoroutine(UpdateIntCounterSmoothEnd(txt, 0f, toCount, isCharFromEnd, finalColor, checkOnFinalValue, cardInit, type));
                    else
                        StartCoroutine(UpdateIntCounterSmooth(txt, toCount, lerp, isCharFromEnd, startColor, finalColor, checkOnFinalValue, cardInit, type));
                }
                else
                    StartCoroutine(UpdateIntCounterSmooth(txt, toCount, lerp, isCharFromEnd, startColor, finalColor));
            }
        }
        private IEnumerator UpdateIntCounterSmoothEnd(Text txt, float sec, int toCount, bool isCharFromEnd, Color finalColor,
            [Optional] bool checkOnFinalValue, [Optional] CardFightInit cardInit, [Optional] UpdateValueType type)
        {
            yield return new WaitForSeconds(sec);

            int txtCount = toCount;
            if (isCharFromEnd)
                txt.text = $"{txtCount}:";
            else
                txt.text = $":{txtCount}";
            txt.color = finalColor;
            if (checkOnFinalValue)
                switch (type)
                {
                    case UpdateValueType.HP:
                        cardInit.OnHPChanged?.Invoke(cardInit.hp, true);
                        break;
                    case UpdateValueType.DEF:
                        cardInit.OnDefenseChanged?.Invoke(cardInit.defense, true);
                        break;
                    case UpdateValueType.ATK:
                        cardInit.OnDamageChanged?.Invoke(cardInit.damage, true);
                        break;
                }
        }
        #endregion methods
    }
}
