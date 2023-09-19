using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;
using GameMenu.Inventory.Cards;
using System.Linq;

namespace GameMenu.House
{
    public sealed class HouseCardMenuInit : CardMenuInit
    {
        private int silverHealPrice = 0;
        private int silverHealPriceOne = 0;
        private bool isHouseTimeUpdating;
        private float hpGot = 0f;
        private float dmgGot = 0f;
        private float defGot = 0f;
        [SerializeField] private Text trashTimerText;
        [SerializeField] private Text eventText1;
        [SerializeField] private Text eventText2;
        protected override void Start()
        {
            try
            {
                if (transform.parent != null && transform.parent.name.Remove(12).Equals("HouseHealPos"))
                    UpdateHouseTime();
            }
            catch { }
            base.Start();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            isHouseTimeUpdating = false;
        }
        protected override void UpdateValues(List<CardData> cardDatas)
        {
            if (id == -1) return;
            base.UpdateValues(cardDatas);
            if (transform.Find("EventText_1") != null)
                UpdateHouseHealPrice();
        }
        private void UpdateHeal()
        {
            isHouseTimeUpdating = true;
            CardData currentCardData = GameDataInit.data.cardsData[listPosition];
            if (IsCardHealed(currentCardData))
            {
                currentCardData.hp = currentCardData.maxHP;
                currentCardData.damage = currentCardData.maxDamage;
                currentCardData.defense = currentCardData.maxDefense;
                UpdateValues();
                SetTimerUI(currentCardData, 0, 0);
                StopUpdateHouseTime();
                return;
            }
            int currentTime = GameDataInit.data.houseTime;
            float hpPerSec = HouseTier.hpPerMin[PrefabsData.instance.cardPrefabs[id].rareTier] / 60f;
            float dmgPerSec = hpPerSec / 3f;
            float defPerSec = hpPerSec / 4f;
            CheckHPByTime(currentCardData, currentTime, hpPerSec);
            CheckDamageByTime(currentCardData, currentTime, dmgPerSec);
            CheckDefenseByTime(currentCardData, currentTime, defPerSec);

            float remainingTime = GetRemainingTime(currentCardData, hpPerSec, dmgPerSec, defPerSec);
            int maxRemainingTime = GetMaxRemainingTime(currentCardData, hpPerSec, dmgPerSec, defPerSec);

            if (trashTimerText != null)
                SetTimerUI(currentCardData, remainingTime, maxRemainingTime);
            else
            {
                StopUpdateHouseTime();
                return;
            }

            if (!IsCardHealed(currentCardData))
                Invoke(nameof(UpdateHeal), 1f);
        }
        private bool IsCardHealed(CardData currentCardData) => currentCardData.hp >= currentCardData.maxHP && currentCardData.damage >= currentCardData.maxDamage && currentCardData.defense >= currentCardData.maxDefense;
        private float GetRemainingTime(CardData currentCardData, float hpPerSec, float dmgPerSec, float defPerSec)
        {
            float remainingTime = Mathf.Max(((currentCardData.maxHP - currentCardData.hp - hpGot) / hpPerSec), 0);
            remainingTime = Mathf.Max(remainingTime, ((currentCardData.maxDamage - currentCardData.damage - dmgGot) / dmgPerSec), 0);
            remainingTime = Mathf.Max(remainingTime, ((currentCardData.maxDefense - currentCardData.defense - defGot) / defPerSec), 0);
            return remainingTime;
        }
        private int GetMaxRemainingTime(CardData currentCardData, float hpPerSec, float dmgPerSec, float defPerSec)
        {
            int maxRemainingTime = (int)(currentCardData.maxHP / hpPerSec);
            maxRemainingTime = Mathf.Max(maxRemainingTime, (int)(currentCardData.maxDamage / dmgPerSec));
            maxRemainingTime = Mathf.Max(maxRemainingTime, (int)(currentCardData.maxDefense / defPerSec));
            return maxRemainingTime;
        }
        private void CheckHPByTime(CardData currentCardData, int currentTime, float hpPerSec)
        {
            int hpWaitedTime = currentTime - currentCardData.houseStartTimeHP;
            hpGot = hpWaitedTime * hpPerSec;
            int hpInt = (int)System.Math.Floor(hpGot);
            if (hpInt > 0 && currentCardData.hp < currentCardData.maxHP)
            {
                hpGot -= hpInt;
                currentCardData.hp = Mathf.Min(currentCardData.hp + hpInt, currentCardData.maxHP);
                currentCardData.houseStartTimeHP = currentTime;
                UpdateValues();
            }
        }
        private void CheckDamageByTime(CardData currentCardData, int currentTime, float dmgPerSec)
        {
            int dmgWaitedTime = currentTime - currentCardData.houseStartTimeDMG;
            dmgGot = dmgWaitedTime * dmgPerSec;
            int dmgInt = (int)System.Math.Floor(dmgGot);
            if (dmgInt > 0 && currentCardData.damage < currentCardData.maxDamage)
            {
                dmgGot -= dmgInt;
                currentCardData.damage = Mathf.Min(currentCardData.damage + dmgInt, currentCardData.maxDamage);
                currentCardData.houseStartTimeDMG = currentTime;
                UpdateValues();
            }
        }
        private void CheckDefenseByTime(CardData currentCardData, int currentTime, float defPerSec)
        {
            int defWaitedTime = currentTime - currentCardData.houseStartTimeDEF;
            defGot = defWaitedTime * defPerSec;
            int defInt = (int)System.Math.Floor(defGot);

            if (defInt > 0 && currentCardData.defense < currentCardData.maxDefense)
            {
                defGot -= defInt;
                currentCardData.defense = Mathf.Min(currentCardData.defense + defInt, currentCardData.maxDefense);
                currentCardData.houseStartTimeDEF = currentTime;
                UpdateValues();
            }
        }
        private void SetTimerUI(CardData currentCardData, float remainingTime, int maxRemainingTime)
        {
            int min = (int)(remainingTime / 60f);
            float sec = (remainingTime % 60f);
            if (IsCardHealed(currentCardData))
            {
                trashTimerText.text = ":)";
                eventText1.enabled = false;
                eventText2.enabled = false;
                trashTimerText.color = Color.white;
            }
            else
            {
                trashTimerText.text = min.ToString("00") + ":" + sec.ToString("00");
                trashTimerText.color = new Color(1f, 1f - ((float)remainingTime / maxRemainingTime), 1f - ((float)remainingTime / maxRemainingTime), 1f);
            }
        }
        private void StopUpdateHouseTime()
        {
            isHouseTimeUpdating = false;
            CancelInvoke(nameof(UpdateHeal));
        }

        public void UpdateHouseTime()
        {
            if (!isHouseTimeUpdating)
            {
                CancelInvoke(nameof(UpdateHeal));
                UpdateHeal();
            }
        }
        private void UpdateHouseHealPrice()
        {
            GameObject event1 = transform.Find("EventText_1").gameObject;
            GameObject event2 = transform.Find("EventText_2").gameObject;
            event1.GetComponent<LanguageLoad>().Load();
            event2.GetComponent<LanguageLoad>().Load();
            CardData cardData = GameDataInit.data.cardsData[listPosition];
            int mult = 0;
            mult += Mathf.Clamp(cardData.maxHP - cardData.hp, 0, 1);
            mult += Mathf.Clamp(cardData.maxDamage - cardData.damage, 0, 1);
            mult += Mathf.Clamp(cardData.maxDefense - cardData.defense, 0, 1);
            int pricePerHP = HouseTier.silverPricePerHP[PrefabsData.instance.cardPrefabs[id].rareTier];
            silverHealPriceOne = pricePerHP * mult;

            silverHealPrice = (cardData.maxHP - cardData.hp) * pricePerHP;
            silverHealPrice += (cardData.maxDamage - cardData.damage) * pricePerHP;
            silverHealPrice += (cardData.maxDefense - cardData.defense) * pricePerHP;

            Text eventText = event1.GetComponent<Text>();
            eventText.text += ": ";
            GameMenu.Shop.PriceParser.InsertSilverPrice(eventText, silverHealPrice, true);

            eventText = event2.GetComponent<Text>();
            eventText.text += " (+1): ";
            GameMenu.Shop.PriceParser.InsertSilverPrice(eventText, silverHealPriceOne, true);
        }
        private void DefaultHeal()
        {
            UpdateHouseHealPrice();
            hpGot = 0;
            dmgGot = 0;
            defGot = 0;
        }
        public void HealCard()
        {
            if (GameDataInit.data.coinsSilver < silverHealPrice) return;
            DefaultHeal();
            CardData currentCard = GameDataInit.data.cardsData[listPosition];
            GameDataInit.AddSilver(-silverHealPrice, false);
            GameDataInit.data.cardsData[listPosition].hp = currentCard.maxHP;
            GameDataInit.data.cardsData[listPosition].damage = currentCard.maxDamage;
            GameDataInit.data.cardsData[listPosition].defense = currentCard.maxDefense;
            CancelInvoke(nameof(UpdateHeal));
            UpdateValues();
            RemoveFromHouse();
        }
        public void HealCardOne()
        {
            if (GameDataInit.data.coinsSilver < silverHealPriceOne) return;
            DefaultHeal();
            CardData currentCard = GameDataInit.data.cardsData[listPosition];
            GameDataInit.AddSilver(-silverHealPriceOne, false);
            List<CardData> cardsData = GameDataInit.data.cardsData;
            cardsData[listPosition].hp += Mathf.Clamp(currentCard.maxHP - currentCard.hp, 0, 1);
            cardsData[listPosition].damage += Mathf.Clamp(currentCard.maxDamage - currentCard.damage, 0, 1);
            cardsData[listPosition].defense += Mathf.Clamp(currentCard.maxDefense - currentCard.defense, 0, 1);
            int currentTime = GameDataInit.data.houseTime;
            cardsData[listPosition].houseStartTimeHP = currentTime;
            cardsData[listPosition].houseStartTimeDMG = currentTime;
            cardsData[listPosition].houseStartTimeDEF = currentTime;
            UpdateValues();
            CancelInvoke(nameof(UpdateHeal));
            UpdateHeal();
            HousePanelInit.instance.OnHouseSizeChanged?.Invoke();
        }
        public void AddToHouse()
        {
            HousePanelInit housePanelInit = HousePanelInit.instance;
            housePanelInit.lastHouseIndexPressed = housePanelInit.houseCardsHeal.items.ToList().FindIndex(obj => !obj.rootObject.GetComponent<CardMenuInit>());

            int currentTime = GameDataInit.data.houseTime;
            List<CardData> cardsData = GameDataInit.data.cardsData;
            cardsData[listPosition].onHeal = true;
            cardsData[listPosition].houseStartTimeHP = currentTime;
            cardsData[listPosition].houseStartTimeDMG = currentTime;
            cardsData[listPosition].houseStartTimeDEF = currentTime;
            housePanelInit.houseCardsChoose.RemoveAtListParam(listParam, true, true);
            housePanelInit.houseCardsHeal.RemoveAt(housePanelInit.lastHouseIndexPressed);
            housePanelInit.houseCardsHeal.GetUpdatedObject(listPosition, out ItemList.IListUpdater listUpdater);
            housePanelInit.houseCardsHeal.AddAt(listUpdater, housePanelInit.lastHouseIndexPressed, true, true);
            housePanelInit.houseCardsHeal.TryAddHealPrefab();
            housePanelInit.UpdateHouseTimers();
            housePanelInit.OnHouseSizeChanged?.Invoke();
        }
        public void RemoveFromHouse()
        {
            GameDataInit.data.cardsData[listPosition].onHeal = false;
            CancelInvoke(nameof(UpdateHeal));
            isHouseTimeUpdating = false;
            HouseCardsHealList houseHealItemList = HousePanelInit.instance.houseCardsHeal;
            houseHealItemList.RemoveAtListParam(listParam, true, true);
            houseHealItemList.TryAddHealPrefab();
            HousePanelInit.instance.UpdateHouseTimers();
            HousePanelInit.instance.OnHouseSizeChanged?.Invoke();
        }
        public void CheckTutorial()
        {
            if (GameDataInit.data.isTutorialCompleted || GameDataInit.data.cardsData[listPosition].hp != GameDataInit.data.cardsData[listPosition].maxHP) return;
            IncTutorial();
        }
    }
}