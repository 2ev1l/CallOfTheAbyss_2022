using System.Collections.Generic;
using System.Linq;
using Universal;

namespace GameMenu.Inventory
{
    public class InventoryStateMachine : StateMachine
    {
        private List<InventoryTab> inventoryTabs => new List<InventoryTab>()
    {
     new InventoryTab("Cards",      false                                 ),
     new InventoryTab("Chests",     GameDataInit.data.chestsData.Count == 0),
     new InventoryTab("Potions",    GameDataInit.data.potionsData.Count == 0),
     new InventoryTab("Artifacts",  GameDataInit.data.artifactsData.Count == 0),
     new InventoryTab("Preview",    false                                    ),
     new InventoryTab("Trash",      GameDataInit.data.cardsOnTrash.Count == 0),
    };
        public override void SetStatesAvailability()
        {
            foreach (InventoryPanelStates state in states.Cast<InventoryPanelStates>())
                state.gameObject.SetActive(!inventoryTabs.Find(tab => tab.name.Equals(state.stateNameNormalized)).isUnAvailable);
        }

        [System.Serializable]
        private class InventoryTab
        {
            public string name;
            public bool isUnAvailable;
            public InventoryTab(string name, bool isUnAvailable)
            {
                this.name = name;
                this.isUnAvailable = isUnAvailable;
            }
        }
    }
}