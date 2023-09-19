using UnityEngine;
using Universal;

namespace GameEvent.Events
{
    public class Event3 : Event0
    {
        #region methods
        protected override void OnEventEnd(int finalVaritationID, VariationAnswer answer)
        {
            switch (finalVaritationID)
            {
                case 1: //nothing
                    break;
                case 2: //curse
                    foreach (var el in GameDataInit.deskCards)
                        if (CustomMath.GetRandomChance(50))
                        {
                            el.damage -= 5;
                            el.hp -= 10;
                            el.defense -= 5;
                            el.damage = Mathf.Max(0, el.damage);
                            el.hp = Mathf.Max(1, el.hp);
                            el.defense = Mathf.Max(0, el.defense);
                        }
                    break;
                default: throw new System.NotImplementedException();
            }
            Invoke(nameof(Leave), timeToLoad);
        }
        #endregion methods
    }
}