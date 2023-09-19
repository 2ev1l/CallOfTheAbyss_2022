using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Data;
using Universal;
using GameMenu.Inventory.Storages;
using GameMenu.Inventory.Cards;
using GameMenu.Potions;
using GameMenu.Artifacts;

namespace GameMenu.Inventory.Chests
{
    public class ChestLootGenerator : MonoBehaviour
    {
        #region fields & properties
        public UnityAction OnChestOpened;

        [SerializeField] private ChestMenuInit chestMenuInit;
        [SerializeField] private ChestImageUpdater chestImageUpdater;
        [SerializeField] private GameObject openedPanel;
        private ChestInfo chestInfo => chestMenuInit.chestInfo;
        private static ChestParticles chestEffects => ChestParticles.instance;
        public static bool isAnimation;
        private int currentLootCount = 0;
        #endregion fields & properties

        #region methods 
        private void Start()
        {
            isAnimation = false;
        }
        private void OnEnable()
        {
            OnChestOpened += OpenChest;
        }
        private void OnDisable()
        {
            OnChestOpened -= OpenChest;
        }
        private void OpenChest()
        {
            isAnimation = true;
            StartCoroutine(OnChestOpen());
        }
        private IEnumerator OnChestOpen()
        {
            bool isTutorial = GameDataInit.data.isTutorialCompleted;
            if (!isTutorial)
            {
                GameDataInit.data.isTutorialCompleted = true;
            }
            currentLootCount = 0;
            GameDataInit.RemoveChest(chestInfo.id);
            chestImageUpdater.EnableMainImage(false);

            List<bool> offObjectsStates = new List<bool>();
            foreach (GameObject el in InventoryChestStorage.instance.objectsToOff)
            {
                offObjectsStates.Add(el.activeSelf);
                el.SetActive(false);
            }

            openedPanel.SetActive(true);
            GameObject oldPanel = openedPanel;
            openedPanel = Instantiate(openedPanel);
            oldPanel.SetActive(false);

            Transform openedPanelTransform = openedPanel.transform;
            openedPanelTransform.SetParent(transform.parent.parent.parent.parent);
            openedPanelTransform.SetAsLastSibling();
            openedPanelTransform.position = Vector3.zero;
            openedPanelTransform.localScale = Vector3.one / 2f;
            chestImageUpdater.ChangeSpawnedChestImage(openedPanelTransform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>());

            InitAllChestLoot();
            GenerateLoot(out List<ChestSpawned> chestSpawned);
            chestEffects.StartParcticleSystem(chestInfo, chestSpawned);
            chestEffects.PlayChestOpenClip();
            yield return chestImageUpdater.ShowChest();
            chestEffects.StopParticleSystem();

            yield return ShowLoot(chestSpawned);

            for (int i = 0; i < InventoryChestStorage.instance.objectsToOff.Length; i++)
                InventoryChestStorage.instance.objectsToOff[i].SetActive(offObjectsStates[i]);
            chestImageUpdater.EnableMainImage(true);

            InventoryPanelInit.instance.inventoryChests.RemoveAtLastListParam(chestMenuInit.listParam, true, true);
            Destroy(openedPanel);
            openedPanel = oldPanel;
            isAnimation = false;
            if (!isTutorial)
                Tutorial.GameMenuTutorialInit.instance.tutorialProgresses[8].Init();
            SavingUtils.SaveGameData();
            InventoryPanelInit.instance.SetInventoryStatesAvailability();
            if (HasLootType(chestSpawned, LootType.Chest)) InventoryPanelInit.instance.inventoryChests.UpdateListData();
        }
        private bool HasLootType(List<ChestSpawned> chestSpawned, LootType lootType) => chestSpawned.FindIndex(x => x.lootType == lootType) > -1;
        private void InitAllChestLoot()
        {
            for (int i = 0; i < chestInfo.chestLoot.Count; i++)
            {
                ChestLoot currentLoot = chestInfo.chestLoot[i];
                switch (currentLoot.type)
                {
                    case LootType.Card:
                        currentLoot.location = PrefabsData.instance.cardPrefabs[currentLoot.id].cardLocation;
                        currentLoot.rare = PrefabsData.instance.cardPrefabs[currentLoot.id].rareTier;
                        currentLoot.tier = PrefabsData.instance.cardPrefabs[currentLoot.id].upgradedTier;
                        break;
                    case LootType.Potion:
                        currentLoot.location = PrefabsData.instance.potionPrefabs[currentLoot.id].potionLocation;
                        break;
                    case LootType.Artifact:
                        currentLoot.location = PrefabsData.instance.artifactPrefabs[currentLoot.id].artifactLocation;
                        break;
                    case LootType.Chest:
                        break;
                    default: throw new System.NotImplementedException();
                }
                chestInfo.chestLoot[i] = currentLoot;
            }
        }
        private void GenerateLoot(out List<ChestSpawned> chestSpawned)
        {
            chestSpawned = new List<ChestSpawned>();
            while (currentLootCount < chestInfo.maxLootCount)
            {
                RandomizeRare(out int choosedRare);
                RandomizeTier(choosedRare, out int choosedTier);
                RandomizeLootType(choosedRare, choosedTier, out List<ChestLoot> sortedLoot, out LootType choosedLootType);
                RandomizeLocation(choosedLootType, sortedLoot, out int choosedLocation, out sortedLoot);
                ChooseLootByLootType(choosedLootType, choosedRare, choosedTier, choosedLocation, sortedLoot, out sortedLoot);
                ChooseLoot(choosedLootType, choosedRare, choosedTier, choosedLocation, sortedLoot, out choosedLootType, out int finalID);
                chestSpawned.Add(GetFinalLoot(choosedLootType, finalID));
                currentLootCount++;
            }
        }
        private void RandomizeRare(out int choosedRare)
        {
            List<int> rareChance = new List<int>();
            foreach (ChestChances el in chestInfo.chestChances)
                rareChance.Add(el.rareChance);
            choosedRare = GetRandomFromArray(rareChance.ToArray());
        }
        private void RandomizeTier(int choosedRare, out int choosedTier)
        {
            choosedTier = GetRandomFromArray(chestInfo.chestChances[choosedRare].tiersChance.ToArray());
        }
        private void RandomizeLootType(int choosedRare, int choosedTier, out List<ChestLoot> sortedLoot, out LootType choosedLootType)
        {
            sortedLoot = chestInfo.chestLoot.Where(el => el.rare == choosedRare && el.tier == choosedTier).ToList();
            choosedLootType = sortedLoot[Random.Range(0, sortedLoot.Count)].type;
        }
        private void RandomizeLocation(LootType choosedLootType, List<ChestLoot> sortedLoot, out int choosedLocation, out List<ChestLoot> newSortedLoot)
        {
            sortedLoot = sortedLoot.Where(el => el.type == choosedLootType).OrderBy(el => el.location).ToList();
            choosedLocation = CustomMath.GetRandomChance() switch
            {
                float i when i < 10f => chestInfo.chestLocation - 1,
                float i when i <= 97f => chestInfo.chestLocation,
                float i when i <= 100f => chestInfo.chestLocation + 1,
                _ => sortedLoot.First().location
            };
            choosedLocation = Mathf.Clamp(choosedLocation, sortedLoot.First().location, sortedLoot.Last().location);
            newSortedLoot = sortedLoot;
            if (new int[] { 0, 1 }.Contains(chestInfo.id))
                choosedLocation = chestInfo.chestLocation;
        }
        private void ChooseLootByLootType(LootType choosedLootType, int choosedRare, int choosedTier, int choosedLocation, List<ChestLoot> sortedLoot, out List<ChestLoot> newSortedLoot)
        {
            sortedLoot = choosedLootType switch
            {
                LootType.Card => sortedLoot.Where(el =>
                                    el.rare == choosedRare &&
                                    el.tier == choosedTier &&
                                    el.type == choosedLootType &&
                                    el.location == choosedLocation).ToList(),
                LootType.Potion => sortedLoot.Where(el =>
                                    el.type == choosedLootType &&
                                    el.location == choosedLocation).ToList(),
                LootType.Artifact => sortedLoot.Where(el =>
                                    el.type == choosedLootType &&
                                    el.location == choosedLocation).ToList(),
                LootType.Chest => sortedLoot.Where(el =>
                                    el.type == choosedLootType).ToList(),
                _ => throw new System.NotImplementedException()
            };
            newSortedLoot = sortedLoot;
        }
        private void ChooseLoot(LootType choosedLootType, int choosedRare, int choosedTier, int choosedLocation, List<ChestLoot> sortedLoot, out LootType newChoosedLootType, out int finalID)
        {
            int finalid = 0;
            if (sortedLoot.Count == 0)
            {
                finalid = chestInfo.chestLoot[UnityEngine.Random.Range(0, chestInfo.chestLoot.Count)].id;
                choosedLootType = chestInfo.chestLoot[chestInfo.chestLoot.FindIndex(x => x.id == finalid)].type;
                print($"available loot count is zero => {choosedRare} rare + {choosedTier} tier + {choosedLootType} loot type + {choosedLocation} location + {finalid} id");
            }
            else
            {
                finalid = sortedLoot[UnityEngine.Random.Range(0, sortedLoot.Count)].id;
                print($"=> {sortedLoot.Count} count + {choosedRare} rare + {choosedTier} tier + {choosedLootType} loot type + {choosedLocation} location + {finalid} id");
            }
            finalID = finalid;
            newChoosedLootType = choosedLootType;
        }
        private ChestSpawned GetFinalLoot(LootType choosedLootType, int finalID)
        {
            ChestSpawned lastSpawned = new ChestSpawned();
            switch (choosedLootType)
            {
                case LootType.Card:
                    lastSpawned.listPosition = GameDataInit.AddCard(finalID, out CardPlaceType pt);
                    lastSpawned.cardPlaceType = pt;
                    if (pt == CardPlaceType.Trash)
                        GameDataInit.instance.OnTrashCardsChanged?.Invoke();
                    break;
                case LootType.Potion: lastSpawned.listPosition = GameDataInit.AddPotion(finalID); break;
                case LootType.Artifact: lastSpawned.listPosition = GameDataInit.AddArtifact(finalID); break;
                case LootType.Chest: lastSpawned.listPosition = GameDataInit.AddChest(finalID); break;
                default: throw new System.NotImplementedException();
            }
            lastSpawned.obj = InventoryChestStorage.instance.lootTypePrefabs.Find(x => x.lootType == choosedLootType).prefab;
            lastSpawned.lootType = choosedLootType;
            lastSpawned.id = finalID;
            return lastSpawned;
        }
        private IEnumerator ShowLoot(List<ChestSpawned> chestSpawned)
        {
            foreach (ChestSpawned el in chestSpawned)
            {
                chestEffects.StartParcticleSystem(el);
                chestEffects.PlayChestLootClip();

                GameObject spawned = Instantiate(el.obj, GameObject.Find("UpperPanel").transform);
                Transform spawnedTransform = spawned.transform;
                spawnedTransform.SetAsLastSibling();
                spawnedTransform.position = Vector3.zero;

                switch (el.lootType)
                {
                    case LootType.Card: spawned.GetComponent<ChestCardMenuInit>().ChangeListPosition(el.listPosition, el.cardPlaceType); break;
                    case LootType.Potion: spawned.GetComponent<PotionInit>().UpdateValues(el.listPosition); break;
                    case LootType.Artifact: spawned.GetComponent<ArtifactInit>().UpdateValues(el.listPosition); break;
                    case LootType.Chest: spawned.GetComponent<ChestInit>().OnListUpdate(el.listPosition); break;
                    default: throw new System.NotImplementedException();
                }
                yield return (chestEffects.SpawnedLootAnimation(spawnedTransform, Vector3.zero, Vector3.one * 2f));

                yield return new WaitForSecondsRealtime(2f);
                chestEffects.StopParticleSystem();

                yield return CustomAnimation.MoveTo(new Vector3(3, 0, 0), spawned, 1f);
                yield return CustomAnimation.MoveTo(new Vector3(-20, 0, 0), spawned, 1f);

                Destroy(spawned);
            }
        }
        private static int GetRandomFromArray(params int[] array)
        {
            int finalIndex = array.Length - 1;
            int maxChance = 100;
            for (int i = 0; i < array.Length; i++)
            {
                int rnd = UnityEngine.Random.Range(1, maxChance);
                if (rnd <= array[i])
                {
                    finalIndex = i;
                    break;
                }
                else
                {
                    maxChance -= array[i];
                }
            }
            return finalIndex;
        }
        #endregion methods 
    }
}