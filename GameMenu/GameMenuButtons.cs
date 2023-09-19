using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Universal;
using GameMenu.House;
using GameMenu.Inventory;
using GameMenu.Inventory.Cards;

namespace GameMenu
{
    public class GameMenuButtons : Buttons
    {
        #region fields
        private static List<string> gameMenuPanelsName => new List<string> { "Shop", "TrainingCamp", "House", "Inventory", "Loot", "Blacksmith" };
        #endregion fields

        #region methods
        public void GameMenuPressedOpen(string what)
        {
            if (what == "Inventory")
            {
                ShowCoins(GameMenuCoinsInit.instance.silverCoins, false);
                ShowCoins(GameMenuCoinsInit.instance.goldCoins, false);

                foreach (string el in gameMenuPanelsName.Where(str => !str.Equals(what)))
                {
                    GameMenuPressedClose(el);
                }
                GameObject backPack = GameObject.Find("BackPackV2");

                if (backPack != null)
                    backPack.GetComponent<Image>().enabled = false;
                else
                    GameObject.Find("BackPackV2_T").GetComponent<Image>().enabled = false;
            }
            GameMenuPressedPanels(what, true);
        }
        private void GameMenuPressedPanels(string what, bool isOpen)
        {
            Animator panelAnimator = GameObject.Find(what).GetComponent<Animator>();
            panelAnimator.SetBool("Down0", isOpen);
            panelAnimator.SetBool("Up0", !isOpen);
            panelAnimator = GameObject.Find("Menu").GetComponent<Animator>();
            panelAnimator.SetBool("Up0", !isOpen);
            panelAnimator.SetBool("Down0", isOpen);
            GameMenuInit.instance.upperPanels[gameMenuPanelsName.IndexOf(what)].SetActive(isOpen);
        }
        public void GameMenuPressedClose(string what)
        {
            if (what == "Inventory")
            {
                ShowCoins(GameMenuCoinsInit.instance.silverCoins, true);
                ShowCoins(GameMenuCoinsInit.instance.goldCoins, true);
            }
            GameObject backPack = GameObject.Find("BackPackV2");
            GameObject backPackT = GameObject.Find("BackPackV2_T");
            if (backPack != null)
            {
                backPack.GetComponent<SpriteRenderer>().enabled = true;
                backPack.GetComponent<Image>().enabled = true;
            }
            else if (backPackT != null)
            {
                backPackT.GetComponent<SpriteRenderer>().enabled = true;
                backPackT.GetComponent<Image>().enabled = true;
            }
            GameMenuPressedPanels(what, false);
        }
        private void ShowCoins(GameObject coin, bool show)
        {
            if (coin != null)
            {
                coin.GetComponent<Image>().raycastTarget = show;
                coin.transform.GetChild(1).gameObject.GetComponent<Text>().raycastTarget = show;
            }
        }
        public void GameMenuPressedStart()
        {
            foreach (string el in gameMenuPanelsName)
                GameMenuPressedClose(el);

            Animator menuAnimator = GameObject.Find("Menu").GetComponent<Animator>();
            menuAnimator.SetBool("Up1", true);
            menuAnimator.SetBool("Up0", true);
            menuAnimator.SetBool("Down0", false);
            GameObject.Find("Gradient_2").transform.SetParent(menuAnimator.gameObject.transform);
            GameObject.Find("Gradient_3").transform.SetParent(menuAnimator.gameObject.transform);
            GameDataInit.ResetAdventureProgress();
            SceneLoader.instance.LoadSceneFade("GameAdventure", SceneLoader.screenFadeTime + 0.3f);
        }
        public void GameMenuHousePressedCardsPanel()
        {
            HousePanelInit housePanelInit = HousePanelInit.instance;
            housePanelInit.houseChoosePanel.SetActive(true);
            housePanelInit.UpdateHouseList(false);
            housePanelInit.OnHouseSizeChanged?.Invoke();
        }
        public void GameMenuHousePressedCardsPanelBack()
        {
            HousePanelInit housePanelInit = HousePanelInit.instance;
            housePanelInit.houseChoosePanel.SetActive(false);
            housePanelInit.UpdateHouseList(true);
            housePanelInit.OnHouseSizeChanged?.Invoke();
        }
        public void PressedRemoveAllCards()
        {
            GameDataInit.RemoveCardsFromDesk();
            InventoryPanelInit.instance.inventoryCardsCenter.UpdateListData();
            InventoryPanelInit.instance.inventoryDeskCards.UpdateListData();
            InventoryPanelInit.instance.OnDeskSizeChanged?.Invoke();
        }
        public void SpawnCardChooseParticles(GameObject objPosition) => InventoryCardsParticles.instance.SpawnChooseParticlesAt(objPosition.transform.position);
        public void PressedMoveAllCardsToInventory(GameObject gameObject)
        {
            GameDataInit.data.cardsData.Where(x => x.onHeal).ToList().ForEach(x => x.onHeal = false);
            HousePanelInit.instance.UpdateHouseList(true);
            SpawnCardChooseParticles(gameObject);
            HousePanelInit.instance.OnHouseSizeChanged?.Invoke();
        }
        #endregion methods
    }
}