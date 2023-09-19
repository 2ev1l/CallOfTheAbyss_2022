using Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameAdventure
{
    public class PointUIUpdater : MonoBehaviour
    {
        #region fields
        [SerializeField] private FightPoint fightPoint;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer blackLayout;
        [SerializeField] private Image mainImage;
        [SerializeField] private ShowHelp showHelp;

        private static readonly int fightTextId = 8;
        private static readonly int randomTextId = 70;
        private static readonly int defeatedTextId = 9;
        private static readonly int downTextId = 20;
        #endregion fields

        #region methods
        public void UpdatePointStatus(PointStatus status, int arg = 0)
        {
            fightPoint.fightParameters.status = status;
            switch (status)
            {
                case PointStatus.Normal:
                    spriteRenderer.sprite = fightPoint.fightParameters.isLastLocation ? GameAdventureIconsStorage.instance.iconFightBoss : GameAdventureIconsStorage.instance.iconFight;
                    showHelp.id = fightTextId;
                    if (fightPoint.CanGenerateMobs())
                    {
                        fightPoint.GenerateMobs();
                        if (TryGetHelpAdditionalText(out string txt))
                            showHelp.additionalText = $" - {txt}";
                        else
                            showHelp.additionalText = "";
                    }
                    break;
                case PointStatus.Respawned:
                    if (!fightPoint.isRandomEvent)
                    {
                        spriteRenderer.sprite = GameAdventureIconsStorage.instance.iconRespawned;
                        showHelp.id = fightTextId;
                        fightPoint.GenerateMobs();
                    }
                    else
                    {
                        spriteRenderer.sprite = GameAdventureIconsStorage.instance.iconRandom;
                        showHelp.id = randomTextId;
                    }
                    break;
                case PointStatus.Defeated:
                    spriteRenderer.sprite = fightPoint.fightParameters.isLastLocation ? GameAdventureIconsStorage.instance.iconDefeatedBoss : GameAdventureIconsStorage.instance.iconDefeated;
                    showHelp.id = fightPoint.fightParameters.isLastLocation ? downTextId : defeatedTextId;
                    if (arg == 1)
                    {
                        GetComponent<Button>().enabled = GameAdventurePointsInit.isInvisibleFlowerApplied;
                        showHelp.id = GameAdventurePointsInit.isInvisibleFlowerApplied ? downTextId : 97;
                    }
                    break;
                default: throw new System.NotImplementedException();
            }
        }
        private bool TryGetHelpAdditionalText(out string txt)
        {
            txt = "";
            if (!GameDataInit.data.canSeeWinChance) return false;
            txt = TextOutline.languageData.helpData[83];
            int index = txt.IndexOf("[X]");
            txt = txt.Remove(index, 3);
            txt = txt.Insert(index, GetWinningChance().ToString());
            return true;
        }
        private int GetWinningChance()
        {
            float totalEnemiesValue = 0;
            float totalAlliesValue = 0;
            List<ShortCardData> enemyCards = new List<ShortCardData>();
            List<ShortCardData> allyCards = new List<ShortCardData>();
            foreach (var el in fightPoint.MobsIdGenerated)
            {
                CardInfoSO cardInfo = PrefabsData.instance.cardPrefabs[el];
                enemyCards.Add(new ShortCardData(
                    cardInfo.id, cardInfo.hp, cardInfo.damage, cardInfo.defense, cardInfo.defensePriority, cardInfo.attackPriority, cardInfo.creatureType));
                totalEnemiesValue += GetValueFromCard(GameDataInit.GetCardDataFromPrefab(cardInfo, 0));
            }
            foreach (var el in GameDataInit.deskCards)
            {
                totalAlliesValue += GetValueFromCard(el);
                CardInfoSO cardInfo = PrefabsData.instance.cardPrefabs[el.id];
                allyCards.Add(new ShortCardData(
                    el.id, el.hp, el.damage, el.defense, cardInfo.defensePriority, cardInfo.attackPriority, cardInfo.creatureType));
            }
            bool isAllyWin = SimulateFight(enemyCards, allyCards, out List<ShortCardData> winningCards);
            float chanceMult = GetChanceMultiplier(isAllyWin, winningCards);
            float chance = (totalAlliesValue / totalEnemiesValue) * chanceMult;
            chance = Mathf.Clamp(chance, 0, 1);
            return Mathf.RoundToInt(chance * 100);
        }
        private float GetChanceMultiplier(bool isAllyWin, List<ShortCardData> winningCards)
        {
            float chanceMult = 1f;
            if (isAllyWin) chanceMult *= 1.2f;
            try
            {
                if (isAllyWin && winningCards.Count == 1)
                {
                    switch (winningCards.First().hp)
                    {
                        case int j when j <= 3: chanceMult *= 0.5f; break;
                        case int j when j <= 1: chanceMult *= 0.2f; break;
                        case int j when j <= 10: chanceMult *= 0.8f; break;
                    }
                }
                if (!isAllyWin && winningCards.Count == 1)
                {
                    switch (winningCards.First().hp)
                    {
                        case int y when y >= 10: chanceMult *= 0.2f; break;
                        case int y when y >= 6: chanceMult *= 0.4f; break;
                        case int y when y >= 4: chanceMult *= 0.7f; break;
                    }
                }
            }
            catch { }

            if (!isAllyWin && winningCards.Count > 1) chanceMult *= Mathf.Pow(0.4f, winningCards.Count);
            return chanceMult;
        }
        private float GetValueFromCard(CardData cardData)
        {
            float currentValue = 0;
            currentValue += cardData.hp * 2f;
            currentValue += cardData.damage * 1.8f;
            currentValue += cardData.defense * 2.1f;
            CardInfoSO cardInfo = PrefabsData.instance.cardPrefabs[cardData.id];
            currentValue *= cardInfo.statusEffect.effect != StatusType.None ? 1.2f : 1;
            currentValue *= cardInfo.specialAbility.type != AbilityType.None ? 1.2f : 1;
            return currentValue;
        }
        private bool SimulateFight(List<ShortCardData> enemyCards, List<ShortCardData> allyCards, out List<ShortCardData> winningCards)
        {
            if (allyCards.Count == 0)
            {
                winningCards = new List<ShortCardData>();
                print("ALLY CARDS COUNT IS ZERO --- MAY CAUSE ERRORS");
                return false;
            }
            while (true)
            {
                int rndEnemyCard = CustomMath.FindIndexByMax(enemyCards, x => x.defPriority);
                int rndAllyCard = CustomMath.FindIndexByMax(allyCards, x => x.atkPriority);
                float mult = allyCards[rndAllyCard].hasEffect ? 1.1f : 1f;
                mult = IsCreatureTypeStronger(enemyCards[rndEnemyCard].creatureType, allyCards[rndAllyCard].creatureType) ? 1.2f : 1f;
                enemyCards[rndEnemyCard].hp -= Mathf.RoundToInt(allyCards[rndAllyCard].damage * mult) - enemyCards[rndEnemyCard].defense;
                if (enemyCards[rndEnemyCard].hp < 0)
                    enemyCards.RemoveAt(rndEnemyCard);
                if (enemyCards.Count == 0) break;

                rndEnemyCard = CustomMath.FindIndexByMax(enemyCards, x => x.atkPriority);
                rndAllyCard = CustomMath.FindIndexByMax(allyCards, x => x.defPriority);
                mult = allyCards[rndAllyCard].hasEffect ? 1.1f : 1f;
                mult = IsCreatureTypeStronger(allyCards[rndAllyCard].creatureType, enemyCards[rndEnemyCard].creatureType) ? 1.2f : 1f;
                allyCards[rndAllyCard].hp -= Mathf.RoundToInt(enemyCards[rndEnemyCard].damage * mult) - allyCards[rndAllyCard].defense;
                if (allyCards[rndAllyCard].hp < 0)
                    allyCards.RemoveAt(rndAllyCard);
                if (allyCards.Count == 0) break;
            }
            if (allyCards.Count > 0)
            {
                winningCards = allyCards;
                return true;
            }
            else
            {
                winningCards = enemyCards;
                return false;
            }
        }
        private bool IsCreatureTypeStronger(CreatureType enemyType, CreatureType allyType)
        => ((enemyType == CreatureType.Ground && allyType == CreatureType.Underwater) ||
            (enemyType == CreatureType.Underwater && allyType == CreatureType.Flying) ||
            (enemyType == CreatureType.Flying && allyType == CreatureType.Ground));
        private class ShortCardData
        {
            public int id;
            public int hp;
            public int damage;
            public int defense;
            public int defPriority;
            public int atkPriority;
            public bool hasEffect;
            public CreatureType creatureType;
            public ShortCardData(int id, int hp, int damage, int defense, int defPriority, int atkPriority, CreatureType creatureType)
            {
                this.id = id;
                this.hp = hp;
                this.damage = damage;
                this.defense = defense;
                CardInfoSO cardInfo = PrefabsData.instance.cardPrefabs[id];
                hasEffect = cardInfo.statusEffect.effect != StatusType.None || cardInfo.specialAbility.type != AbilityType.None;
                this.defPriority = defPriority;
                this.atkPriority = atkPriority;
                this.creatureType = creatureType;
            }
        }
        public void EnableTrigger(bool enabled) => mainImage.enabled = enabled;
        public void ChangeSprite(Sprite newSprite) => spriteRenderer.sprite = newSprite;
        public void ChangeSpriteColor(Color newColor) => spriteRenderer.color = newColor;
        public void ChangeLayoutSpriteColor(Color newColor) => blackLayout.color = newColor;
        #endregion methods
    }
}