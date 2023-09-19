using Universal;

namespace GameMenu.Inventory.Cards
{
    public sealed class InventoryCardMenuInit : CardMenuInit
    {
        public void AddToDesk()
        {
            if (GameDataInit.deskCards.Count >= GameDataInit.data.maxDeskSize) return;
            GameDataInit.data.cardsData[listPosition].onDesk = true;
            GameDataInit.data.cardsData[listPosition].deskPosition = GameDataInit.MaxDeskCardPosition() + 1;
            ItemList centerIL = InventoryPanelInit.instance.inventoryCardsCenter;
            ItemList rightIL = InventoryPanelInit.instance.inventoryDeskCards;
            centerIL.RemoveAtListParam(listParam, true, true);
            rightIL.GetUpdatedObject(GameDataInit.data.cardsData[listPosition].deskPosition, out ItemList.IListUpdater listUpdater);
            rightIL.Add(listUpdater, true, true);
            InventoryPanelInit.instance.OnDeskSizeChanged?.Invoke();
        }
        public void CheckTutorialEquip()
        {
            if (GameDataInit.data.isTutorialCompleted) return;
            IncTutorial();
        }
    }
}