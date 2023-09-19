using GameMenu.Potions;
using UnityEngine;
using Universal;

namespace GameFight.Equipment
{
    public class PotionAnimation : DefaultUpdater
    {
        #region fields & properties
        [SerializeField] private FightPotion fightPotion;
        private Vector3 defaultScale;
        #endregion fields & properties

        #region methods
        private void Start()
        {
            defaultScale = fightPotion.transform.localScale;
        }
        protected override void OnEnable()
        {
            FightPotion.OnPotionDeselect += OnDeselect;
            FightPotion.OnPotionChoosed += DoPotionScale;
        }

        protected override void OnDisable()
        {
            FightPotion.OnPotionDeselect -= OnDeselect;
            FightPotion.OnPotionChoosed -= DoPotionScale;
        }

        private void DoPotionScale(FightPotion choosedPotion)
        {
            if (choosedPotion == fightPotion)
                transform.localScale += Vector3.one * 0.2f;
            else
                transform.localScale = defaultScale;
        }
        private void OnDeselect(FightPotion dechoosedPotion)
        {
            transform.localScale = defaultScale;
        }
        #endregion methods
    }
}