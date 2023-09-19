using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace GameEvent.Events
{
    public class Event1 : Event0
    {
        #region methods
        protected override void OnEventChanged(int currentVaritationID, VariationAnswer answer)
        {
            base.OnEventChanged(currentVaritationID, answer);
            switch (currentVaritationID)
            {
                case 2:
                    foreach (var el in GameDataInit.data.cardsData)
                    {
                        if (CustomMath.GetRandomChance(50))
                        {
                            el.hp -= 4;
                            el.damage--;
                            el.defense--;
                            el.hp = Mathf.Max(el.hp, 1);
                            el.damage = Mathf.Max(el.damage, 0);
                            el.defense = Mathf.Max(el.defense, 0);
                        }
                    }
                    break;
            }
        }
        protected override void OnEventEnd(int finalVaritationID, VariationAnswer answer)
        {
            switch (finalVaritationID)
            {
                case 1: //leave
                    break;
                case 3: //reward
                    if (GameDataInit.data.artifactsData.FindIndex(x => x.id == 5) < 0 && GameDataInit.data.earnedArtifacts.FindIndex(x => x.id == 5) < 0)
                        GameDataInit.AddArtifact(5, true);
                    else
                    {
                        ChangeVariationID(2);
                        return;
                    }
                    break;
                default: throw new System.NotImplementedException();
            }
            Invoke(nameof(Leave), timeToLoad);
        }
        #endregion methods
    }
}