using Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace GameEvent.Events
{
    public class Event5 : Event0
    {
        #region methods
        protected override void OnEventChanged(int currentVaritationID, VariationAnswer answer)
        {
            base.OnEventChanged(currentVaritationID, answer);
            if (currentVaritationID == 4) //after start clicked wait
            {
                bool expr = GameDataInit.IsArtifactEffectApplied(ArtifactEffect.LiveCube);
                expr = expr && GameDataInit.data.artifactsData.FindIndex(x => x.effect == ArtifactEffect.GodsFlesh) < 0;
                expr = expr && GameDataInit.data.earnedArtifacts.FindIndex(x => x.effect == ArtifactEffect.GodsFlesh) < 0;
                SendAnswer(expr ? VariationAnswer.Yes : VariationAnswer.No);
            }
        }
        protected override void OnEventEnd(int finalVaritationID, VariationAnswer answer)
        {
            switch (finalVaritationID)
            {
                case 1: //leave
                    break;
                case 2: //final wait
                    GameDataInit.AddArtifact(11, true);
                    break;
                case 3: //if live cube not exists
                    GetCurse(2);
                    break;
                default: throw new System.NotImplementedException();
            }
            Invoke(nameof(Leave), timeToLoad);
        }
        private void GetCurse(int mult)
        {
            foreach (var el in GameDataInit.deskCards)
                if (CustomMath.GetRandomChance(50))
                {
                    el.damage -= 1 * mult;
                    el.hp -= 3 * mult;
                    el.defense -= 1 * mult;
                    el.damage = Mathf.Max(0, el.damage);
                    el.hp = Mathf.Max(1, el.hp);
                    el.defense = Mathf.Max(0, el.defense);
                }
        }
        #endregion methods
    }
}