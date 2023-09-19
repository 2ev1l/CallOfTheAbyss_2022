using System.Linq;
using Universal;

namespace GameMenu.Shop
{
    public class SellStateMachine : StateMachine
    {
        #region methods
        public override void SetStatesAvailability()
        {
            foreach (SellPanelStates state in states.Cast<SellPanelStates>())
            {
                bool disableElement = false;
                disableElement = (state.stateNameNormalized) switch
                {
                    "Cards" => false,
                    "Chests" => GameDataInit.data.chestsData.Count == 0,
                    "Potions" => GameDataInit.data.potionsData.Count == 0,
                    "Artifacts" => GameDataInit.data.artifactsData.Count == 0,
                    _ => throw new System.NotImplementedException()
                };
                state.gameObject.SetActive(!disableElement);
            }
        }
        #endregion methods
    }
}