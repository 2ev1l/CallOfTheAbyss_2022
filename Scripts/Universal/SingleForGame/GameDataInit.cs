using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using GameAdventure;
using Data;
using GameMenu.Inventory.Cards;

namespace Universal
{
    public class GameDataInit : MonoBehaviour
    {
        #region fields & properties
        public static GameDataInit instance { get; private set; }
        public static GameData data;

        public UnityAction OnCoinsChanged;
        public UnityAction OnTrashCardsChanged;
        public UnityAction OnArtifactEffectsChanged;
        public UnityAction OnCardRemoved;

        public static bool earnReward;
        public static readonly string saveName = "save";
        private static readonly List<string> scenesToNotSave = new List<string> { "Menu", "GameFight", "GameEvent" };
        [field: SerializeField] public List<int> adventureLayersMaxPointsID { get; private set; } = new List<int>();
        #endregion fields & properties

        public void Init()
        {
            instance = this;
        }
        public void Start()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (!scenesToNotSave.Contains(currentSceneName))
                data.sceneName = currentSceneName;
        }

        #region Data (<SaveData>)
        public static void AddCard(int id, bool isTemp)
        {
            CardInfoSO cardInit = PrefabsData.instance.cardPrefabs[id];
            CardData cardData = new CardData()
            {
                maxHP = cardInit.hp,
                maxDamage = cardInit.damage,
                maxDefense = cardInit.defense,
                houseStartTimeHP = 0,
                houseStartTimeDMG = 0,
                houseStartTimeDEF = 0,
                trashStartTime = 0,
                hp = cardInit.hp,
                damage = cardInit.damage,
                defense = cardInit.defense,
                id = id,
                listPosition = data.cardsData.Count
            };

            if (isTemp)
            {
                cardData.listPosition += data.earnedCards.Count;
                data.earnedCards.Add(cardData);
            }
            else
            {
                if (data.maxInventorySize <= data.cardsData.Count)
                {
                    cardData.trashStartTime = data.trashTime;
                    cardData.listPosition = data.cardsOnTrash.Count;
                    data.cardsOnTrash.Add(cardData);
                    instance.OnTrashCardsChanged?.Invoke();
                }
                else
                    data.cardsData.Add(cardData);
            }
        }
        public static int AddCard(int id, out CardPlaceType placeType)
        {
            CardInfoSO cardInit = PrefabsData.instance.cardPrefabs[id];
            CardData cardData = GetCardDataFromPrefab(cardInit, data.cardsData.Count);
            if (data.maxInventorySize <= data.cardsData.Count)
            {
                cardData.trashStartTime = data.trashTime;
                cardData.listPosition = data.cardsOnTrash.Count;
                data.cardsOnTrash.Add(cardData);
                placeType = CardPlaceType.Trash;
            }
            else
            {
                data.cardsData.Add(cardData);
                placeType = CardPlaceType.Inventory;
            }
            return cardData.listPosition;
        }
        public static CardData GetCardDataFromPrefab(CardInfoSO cardInit, int listPosition)
        {
            CardData cardData = new CardData()
            {
                maxHP = cardInit.hp,
                maxDamage = cardInit.damage,
                maxDefense = cardInit.defense,
                houseStartTimeHP = 0,
                houseStartTimeDMG = 0,
                houseStartTimeDEF = 0,
                trashStartTime = 0,
                hp = cardInit.hp,
                damage = cardInit.damage,
                defense = cardInit.defense,
                id = cardInit.id,
                listPosition = listPosition
            };
            return cardData;
        }
        public static void RemoveCard(int listPosition, CardPlaceType placeType)
        {
            switch (placeType)
            {
                case CardPlaceType.Inventory:
                    if (data.cardsData[listPosition].onDesk || data.cardsData[listPosition].onHeal)
                        throw new NotSupportedException();
                    data.cardsData.RemoveAt(listPosition);
                    UpdateDataListPositions(LootType.Card);
                    instance.OnCardRemoved?.Invoke();
                    break;
                case CardPlaceType.Trash:
                    data.cardsOnTrash.RemoveAt(listPosition);
                    UpdateDataListPositions(LootType.TrashCard);
                    instance.OnTrashCardsChanged?.Invoke();
                    break;
                default:
                    Debug.LogError("not implemented");
                    break;
            }
        }
        public static void AddArtifact(int id, bool isTemp)
        {
            ArtifactData artifactData = new ArtifactData()
            {
                id = id,
                listPosition = data.artifactsData.Count,
                effect = PrefabsData.instance.artifactPrefabs[id].effect
            };
            if (isTemp)
            {
                artifactData.listPosition += data.earnedArtifacts.Count;
                data.earnedArtifacts.Add(artifactData);
            }
            else
                data.artifactsData.Add(artifactData);
        }
        public static int AddArtifact(int id)
        {
            ArtifactData artifactData = new ArtifactData()
            {
                id = id,
                listPosition = data.artifactsData.Count,
                effect = PrefabsData.instance.artifactPrefabs[id].effect
            };
            data.artifactsData.Add(artifactData);
            return artifactData.listPosition;
        }
        public static void AddPotion(int id, bool isTemp)
        {
            var potionPrefab = PrefabsData.instance.potionPrefabs[id];
            PotionData potionData = new PotionData()
            {
                id = id,
                listPosition = data.potionsData.Count,
                effect = potionPrefab.effect,
                value = potionPrefab.value,
            };
            if (isTemp)
            {
                potionData.listPosition += data.earnedPotions.Count;
                data.earnedPotions.Add(potionData);
            }
            else
                data.potionsData.Add(potionData);
        }
        public static int AddPotion(int id)
        {
            var potionPrefab = PrefabsData.instance.potionPrefabs[id];
            PotionData potionData = new PotionData()
            {
                id = id,
                listPosition = data.potionsData.Count,
                effect = potionPrefab.effect,
                value = potionPrefab.value
            };
            data.potionsData.Add(potionData);
            return potionData.listPosition;
        }
        public static void AddChest(int id, bool isTemp)
        {
            if (isTemp)
                data.earnedChests.Add(id);
            else
                data.chestsData.Add(id);
        }
        public static int AddChest(int id)
        {
            data.chestsData.Add(id);
            return id;
        }
        public static void RemoveChest(int id)
        {
            data.chestsData.RemoveAt(data.chestsData.FindLastIndex(value => value == id));
        }
        public static void RemovePotion(int listPosition)
        {
            data.potionsData.RemoveAt(listPosition);
            UpdateDataListPositions(LootType.Potion);
        }
        public static void RemoveArtifact(int listPosition)
        {
            data.artifactsData.RemoveAt(listPosition);
            UpdateDataListPositions(LootType.Artifact);
        }
        public static void AddGold(int amount, bool isTemp)
        {
            if (amount == 0) return;
            if (isTemp)
                data.earnedGold += amount;
            else
                data.coinsGold += amount;
        }
        public static void AddSilver(int amount, bool isTemp)
        {
            if (amount == 0) return;
            if (isTemp)
                data.earnedSilver += amount;
            else
                data.coinsSilver += amount;
        }
        public static void AddAnyLoot(int id, LootType lootType, bool isTemp)
        {
            switch (lootType)
            {
                case LootType.Artifact: AddArtifact(id, isTemp); break;
                case LootType.Card: AddCard(id, isTemp); break;
                case LootType.Chest: AddChest(id, isTemp); break;
                case LootType.Potion: AddPotion(id, isTemp); break;
                case LootType.Gold: AddGold(id, isTemp); break;
                case LootType.Silver: AddSilver(id, isTemp); break;
                default: throw new NotImplementedException();
            };
        }
        public static void AddAllEarnedLoot()
        {
            AddSilver(data.earnedSilver, false);
            AddGold(data.earnedGold, false);
            foreach (var el in data.earnedArtifacts)
                AddArtifact(el.id, false);
            foreach (var el in data.earnedPotions)
                AddPotion(el.id, false);
            foreach (var el in data.earnedChests)
                AddChest(el, false);
            foreach (var el in data.earnedCards)
                AddCard(el.id, false);
        }
        public static CardData deskCard(int deskPosition) => data.cardsData.Find(x => x.onDesk && x.deskPosition == deskPosition);
        public static PotionData deskPotion(int deskPosition) => data.potionsData.Find(x => x.onDesk && x.deskPosition == deskPosition);
        public static ArtifactData deskArtifact(int deskPosition) => data.artifactsData.Find(x => x.onDesk && x.deskPosition == deskPosition);
        public static List<CardData> deskCards => data.cardsData.Where(x => x.onDesk).ToList();
        public static List<PotionData> deskPotions => data.potionsData.Where(x => x.onDesk).ToList();
        public static List<ArtifactData> deskArtifacts => data.artifactsData.Where(x => x.onDesk).ToList();
        public static int MaxDeskCardPosition() => CustomMath.FindMax(deskCards, x => x.deskPosition);
        public static int MaxDeskPotionPosition() => CustomMath.FindMax(deskPotions, x => x.deskPosition);
        public static int MaxDeskArtifactPosition() => CustomMath.FindMax(deskArtifacts, x => x.deskPosition);
        public static int CopiesCount(int cardId) => data.cardsCopyData.Where(x => x == cardId).Count();

        public static void RemoveCardFromDesk(int listPosition)
        {
            List<ItemData> itemsData = new List<ItemData>();
            itemsData.AddRange(data.cardsData);
            RemoveAnyFromDesk(listPosition, itemsData, MaxDeskCardPosition());
        }
        public static void RemovePotionFromDesk(int listPosition)
        {
            List<ItemData> itemsData = new List<ItemData>();
            itemsData.AddRange(data.potionsData);
            RemoveAnyFromDesk(listPosition, itemsData, MaxDeskPotionPosition());
        }
        public static void RemoveArtifactFromDesk(int listPosition)
        {
            List<ItemData> itemsData = new List<ItemData>();
            itemsData.AddRange(data.artifactsData);
            RemoveAnyFromDesk(listPosition, itemsData, MaxDeskArtifactPosition());
        }
        private static void RemoveAnyFromDesk(int listPosition, List<ItemData> itemsData, int maxPosition)
        {
            itemsData[listPosition].onDesk = false;
            for (int i = itemsData[listPosition].deskPosition + 1; i <= maxPosition; i++)
            {
                int index = itemsData.FindIndex(x => x.deskPosition == i);
                itemsData[index].deskPosition--;
            }
            itemsData[listPosition].deskPosition = -1;
        }
        public static void RemoveCardsFromDesk()
        {
            List<CardData> deskCardsc = deskCards;
            for (int i = 0; i < deskCardsc.Count; i++)
                RemoveCardFromDesk(deskCardsc[i].listPosition);
        }
        public static bool IsArtifactEffectApplied(ArtifactEffect artifactEffect) => data.artifactsData.FindIndex(x => x.effect == artifactEffect && x.onDesk) >= 0;
        private static void UpdateDataListPositions(LootType type)
        {
            if (type == LootType.Chest) return;
            int size = type switch
            {
                LootType.Card => data.cardsData.Count,
                LootType.Artifact => data.artifactsData.Count,
                LootType.Potion => data.potionsData.Count,
                LootType.TrashCard => data.cardsOnTrash.Count,
                _ => throw new System.NotImplementedException()
            };

            for (int i = 0; i < size; i++)
                switch (type)
                {
                    case LootType.Card: data.cardsData[i].listPosition = -1; break;
                    case LootType.TrashCard: data.cardsOnTrash[i].listPosition = -1; break;
                    case LootType.Artifact: data.artifactsData[i].listPosition = -1; break;
                    case LootType.Potion: data.potionsData[i].listPosition = -1; break;
                }

            for (int i = 0; i < size; i++)
                switch (type)
                {
                    case LootType.Card: if (data.cardsData[i].listPosition == -1) data.cardsData[i].listPosition = i; break;
                    case LootType.TrashCard: if (data.cardsOnTrash[i].listPosition == -1) data.cardsOnTrash[i].listPosition = i; break;
                    case LootType.Artifact: if (data.artifactsData[i].listPosition == -1) data.artifactsData[i].listPosition = i; break;
                    case LootType.Potion: if (data.potionsData[i].listPosition == -1) data.potionsData[i].listPosition = i; break;
                }
        }

        public static void ResetAdventureProgress()
        {
            data.currentLocation = data.locationOffset;
            data.currentLocationMax = data.currentLocation;
            data.tempData = new List<TempData>();
            data.earnedArtifacts = new List<ArtifactData>();
            data.earnedChests = new List<int>();
            data.earnedPotions = new List<PotionData>();
            data.earnedCards = new List<CardData>();
            data.earnedGold = 0;
            data.earnedSilver = 0;

            for (int i = 0; i < data.reachedLocation; i++)
            {
                data.tempData.Add(new TempData());
                TempData tempData = data.tempData[i];
                float chance = Mathf.Sqrt((data.reachedLocation - i) * 500 / (i + 0.5f));
                try
                {
                    for (int j = 0; j <= instance.adventureLayersMaxPointsID[i]; j++)
                    {
                        if (CustomMath.GetRandomChance(chance))
                        {
                            tempData.defeatedPoints.Add(j);
                            data.tempData[i] = tempData;
                        }
                    }
                }
                catch
                {
                    Debug.LogError($"{i} location -> choose max points at GameDataInit");
                }
            }
        }
        public static void ResetEarnedLoot(int totalSilverGain = 0, int totalGoldGain = 0, bool enableScale = true)
        {
            float percentRewardOnDeath = enableScale switch
            {
                false => 0f,
                true => data.percentRewardOnDeath
            };
            data.earnedSilver = Mathf.RoundToInt((totalSilverGain / FightPoint.fightParametersCopy.rewardDivision / 2f) + data.earnedSilver * percentRewardOnDeath);
            data.earnedGold = Mathf.RoundToInt((totalGoldGain / FightPoint.fightParametersCopy.rewardDivision / 2f) + data.earnedGold * percentRewardOnDeath);
            AddSilver(data.earnedSilver, false);
            AddGold(data.earnedGold, false);
            if (enableScale)
            {
                data.earnedArtifacts = InitEarnedListOnReset(data.earnedArtifacts, el => el.id, AddArtifact);
                data.earnedPotions = InitEarnedListOnReset(data.earnedPotions, el => el.id, AddPotion);
                data.earnedChests = InitEarnedListOnReset(data.earnedChests, el => el, AddChest);
                data.earnedCards = InitEarnedListOnReset(data.earnedCards, el => el.id, AddCard);
            }
            else
            {
                data.earnedArtifacts = ResetEarnedList(data.earnedArtifacts, 0);
                data.earnedPotions = ResetEarnedList(data.earnedPotions, 0);
                data.earnedChests = ResetEarnedList(data.earnedChests, 0);
                data.earnedCards = ResetEarnedList(data.earnedCards, 0);
            }
            data.sceneName = "GameMenu";
            SavingUtils.SaveGameData();
            earnReward = enableScale;
        }
        private static List<T> InitEarnedListOnReset<T>(List<T> list, System.Func<T, int> addParam, System.Action<int, bool> addFunc)
        {
            AddValuesByRandom(list, addParam, addFunc, out int counter);
            return ResetEarnedList(list, counter);
        }
        private static void AddValuesByRandom<T>(List<T> list, System.Func<T, int> addParam, System.Action<int, bool> addFunc, out int counter)
        {
            counter = 0;
            foreach (T el in list)
            {
                if (CustomMath.GetRandomChance(data.percentRewardOnDeath * 100f))
                {
                    addFunc.Invoke(addParam.Invoke(el), false);
                    counter++;
                }
            }
        }
        private static List<T> ResetEarnedList<T>(List<T> list, int count)
        {
            list = new List<T>();
            for (int i = 0; i < count; i++)
                list.Add(default);
            return list;
        }
        public static void CheckReachedPoints()
        {
            int reachedPoint = data.tempData[data.currentLocation].reachedPoint;
            int currentPoint = data.tempData[data.currentLocation].currentPoint;
            if (currentPoint >= reachedPoint)
            {
                data.tempData[data.currentLocation].reachedPoint = currentPoint + 1;
                reachedPoint = currentPoint + 1;
                if (reachedPoint > data.reachedPoint && data.currentLocation == data.reachedLocation)
                    data.reachedPoint = reachedPoint;
            }
        }
        public static string[] GetCardsName()
        {
            List<string> newTextData = new List<string>();
            foreach (CardTextData el in TextOutline.languageData.cardsTextData)
                newTextData.Add(el.name);
            return newTextData.ToArray();
        }
        public static string[] GetCardsDescription()
        {
            List<string> newTextData = new List<string>();
            foreach (CardTextData el in TextOutline.languageData.cardsTextData)
                newTextData.Add(el.description);
            return newTextData.ToArray();
        }
        #endregion Data
    }
}