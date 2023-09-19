using UnityEngine;
using Universal;

namespace GameEvent.Events
{
    public class Event2 : Event0
    {
        #region methods
        protected override void OnEventEnd(int finalVaritationID, VariationAnswer answer)
        {
            switch (finalVaritationID)
            {
                case 1: //leave
                    break;
                case 2: //trap
                    foreach (var el in GameDataInit.deskCards)
                    {
                        if (CustomMath.GetRandomChance(60))
                        {
                            el.hp -= 2;
                            el.hp = Mathf.Max(el.hp, 1);
                        }
                    }
                    break;
                case 3: //reward
                    GetRandomTreasure();
                    break;
                default: throw new System.NotImplementedException();
            }
            Invoke(nameof(Leave), timeToLoad);
        }
        #endregion methods
    }
}