using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameEvent.Events
{
    public class Event0 : EventVariations
    {
        #region fields & properties
        [SerializeField] private List<VariationState<Sprite>> variationStates = new List<VariationState<Sprite>>();
        [SerializeField] private Image background;
        [SerializeField] protected float timeToLoad = 2f;
        #endregion fields & properties

        #region methods
        protected override void OnInit(int defaultVariationID)
        {
            ChangeBG(defaultVariationID);
        }
        protected override void OnEventChanged(int currentVaritationID, VariationAnswer answer)
        {
            ChangeBG(currentVaritationID);
        }
        protected override void OnEventEnd(int finalVaritationID, VariationAnswer answer)
        {
            switch (finalVaritationID)
            {
                case 1: //cursed
                    foreach (var el in GameDataInit.deskCards)
                    {
                        if (CustomMath.GetRandomChance(70))
                        {
                            el.damage -= 2;
                            el.hp -= 4;
                            el.defense -= 1;
                            el.damage = Mathf.Max(0, el.damage);
                            el.hp = Mathf.Max(1, el.hp);
                            el.defense = Mathf.Max(0, el.defense);
                        }
                    }
                    break;
                case 2: //received
                    GetRandomTreasure();
                    break;
                case 3: break; //none
                default: print(finalVaritationID); throw new System.NotImplementedException();
            }
            Invoke(nameof(Leave), timeToLoad);
        }
        protected void ChangeBG(int variationID)
        {
            background.sprite = GetVariationState(variationStates, variationID).item1;
        }
        protected void GetRandomTreasure()
        {
            switch (CustomMath.GetRandomChance())
            {
                case float i when i <= 10: GameDataInit.AddCard(Random.Range(0, 25), true); break;
                case float i when i <= 30: GameDataInit.AddGold((GameDataInit.data.currentLocation + 1) * 2, true); break;
                case float i when i <= 70: GameDataInit.AddSilver((GameDataInit.data.currentLocation + 1) * 7, true); break;
                case float i when i <= 100: GameDataInit.AddChest(Random.Range(0, 10), true); break;
            }
        }
        #endregion methods
    }
}