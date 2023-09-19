using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameFight.Card
{
    public class ImageFightUpdater : DefaultUpdater
    {
        #region fields
        [SerializeField] private CardFightInit cardFightInit;
        [SerializeField] private Image mainImage;
        [SerializeField] private Image back;
        [SerializeField] private Image layout;
        [SerializeField] private Image typeIcon;
        [SerializeField] private Image downPanel;
        [SerializeField] private Image damageGainPanel;
        #endregion fields

        #region methods
        protected override void OnEnable()
        {
            cardFightInit.UpdateCardUI += OnCardUIUpdate;
            cardFightInit.OnCardDeath += OnCardDeath;
            cardFightInit.cardFight.OnDamageTakenByEnemy += OnDamageTakenByEnemy;
        }
        protected override void OnDisable()
        {
            cardFightInit.UpdateCardUI -= OnCardUIUpdate;
            cardFightInit.OnCardDeath -= OnCardDeath;
            cardFightInit.cardFight.OnDamageTakenByEnemy -= OnDamageTakenByEnemy;
        }

        private void OnCardUIUpdate(bool isEnemy) => StartCoroutine(updateCardUI(isEnemy));
        private IEnumerator updateCardUI(bool isEnemy)
        {
            back.sprite = isEnemy ? FightStorage.instance.backEnemyAliveSprite : FightStorage.instance.backAllyAliveSprites[cardFightInit.rareTier];
            layout.sprite = FightStorage.instance.layoutAllyAliveSprites[cardFightInit.rareTier];
            mainImage.sprite = cardFightInit.cardBG;

            yield return CustomMath.WaitAFrame();
            typeIcon.sprite = cardFightInit.creatureType switch
            {
                CreatureType.Ground => FightStorage.instance.groundType[0],
                CreatureType.Underwater => FightStorage.instance.waterType[0],
                CreatureType.Flying => FightStorage.instance.skyType[0],
                _ => throw new System.NotImplementedException()
            };
            typeIcon.GetComponent<ShowHelp>().id = cardFightInit.creatureType switch
            {
                CreatureType.Ground => 27,
                CreatureType.Underwater => 28,
                CreatureType.Flying => 29,
                _ => throw new System.NotImplementedException()
            };
            downPanel.sprite = cardFightInit.creatureType switch
            {
                CreatureType.Ground => FightStorage.instance.groundType[1],
                CreatureType.Underwater => FightStorage.instance.waterType[1],
                CreatureType.Flying => FightStorage.instance.skyType[1],
                _ => throw new System.NotImplementedException()
            };
        }
        private void OnCardDeath(bool isEnemy)
        {
            back.sprite = isEnemy ? FightStorage.instance.backEnemyDeadSprite : FightStorage.instance.backAllyDeadSprite;
        }
        private void OnDamageTakenByEnemy(int damage, AttackType attackType)
        {
            if (damage <= 0) return;
            damageGainPanel.sprite = CardFight.currentCard.cardInit.attackSprite;
            SetDamagePanelColorAlpha(1);
            StartCoroutine(ResetDamagePanel());
        }
        private IEnumerator ResetDamagePanel()
        {
            float lerp = 0f;
            float step = Time.fixedDeltaTime;
            while (damageGainPanel.color.a > 0f || step >= 1f)
            {
                if (FightAnimationInit.skipAnimation) break;
                SetDamagePanelColorAlpha(damageGainPanel.color.a - step);
                lerp += step;
                yield return new WaitForFixedUpdate();
            }
            SetDamagePanelColorAlpha(0);
        }
        private void SetDamagePanelColorAlpha(float alpha)
        {
            Color newCol = damageGainPanel.color;
            newCol.a = alpha;
            damageGainPanel.color = newCol;
        }
        #endregion methods
    }
}