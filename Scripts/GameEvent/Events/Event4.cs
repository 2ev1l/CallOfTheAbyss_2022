using Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace GameEvent.Events
{
    public class Event4 : Event0
    {
        #region fields
        private int cardAtLocationRemoved;
        #endregion fields

        #region methods
        protected override void OnEventChanged(int currentVaritationID, VariationAnswer answer)
        {
            base.OnEventChanged(currentVaritationID, answer);
            if (currentVaritationID == 3)
            {
                //try reward
                if (GameDataInit.IsArtifactEffectApplied(ArtifactEffect.AncientTeleport) && GameDataInit.data.artifactsData.FindIndex(x => x.effect == ArtifactEffect.InvisibleFlower) < 0)
                {
                    ArtifactData artifactData = GameDataInit.data.artifactsData.Find(x => x.effect == ArtifactEffect.AncientTeleport && x.onDesk);
                    GameDataInit.RemoveArtifactFromDesk(artifactData.listPosition);
                    GameDataInit.RemoveArtifact(artifactData.listPosition);
                    SendAnswer(VariationAnswer.Left);
                    return;
                }
                List<CardData> possibleCardsRemoved = GameDataInit.data.cardsData.FindAll(x => !x.onDesk && !x.onHeal);
                if (possibleCardsRemoved.Count == 0)
                {
                    SendAnswer(VariationAnswer.Right);
                    return;
                }
                CardData cardData = possibleCardsRemoved[Random.Range(0, possibleCardsRemoved.Count)];
                cardAtLocationRemoved = PrefabsData.instance.cardPrefabs.ToList().Find(x => x.id == cardData.id).cardLocation;
                GameDataInit.RemoveCard(cardData.listPosition, GameMenu.Inventory.Cards.CardPlaceType.Inventory);
                if (cardAtLocationRemoved < GameDataInit.data.currentLocation)
                    SendAnswer(VariationAnswer.Right);
                else
                    SendAnswer(VariationAnswer.Straight);
            }
        }
        protected override void OnEventEnd(int finalVaritationID, VariationAnswer answer)
        {
            switch (finalVaritationID)
            {
                case 1: //nothing
                    break;
                case 2: //curse
                    GetCurse(1);
                    break;
                case 4: //altar artifact
                    GameDataInit.AddArtifact(9, true);
                    break;
                case 5: //altar reward
                    List<CardInfoSO> possibleCards = PrefabsData.instance.cardPrefabs.ToList().FindAll(x => x.cardLocation == cardAtLocationRemoved);
                    CardInfoSO choosedCard = possibleCards[Random.Range(0, possibleCards.Count)];
                    GameDataInit.AddCard(choosedCard.id, true);
                    break;
                case 6: //altar curse
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
                    el.hp -= 2 * mult;
                    el.defense -= 1 * mult;
                    el.damage = Mathf.Max(0, el.damage);
                    el.hp = Mathf.Max(1, el.hp);
                    el.defense = Mathf.Max(0, el.defense);
                }
        }
        #endregion methods
    }
}