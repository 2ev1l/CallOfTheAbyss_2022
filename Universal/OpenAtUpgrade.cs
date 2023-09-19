using Data;
using System.Collections;
using UnityEngine;

namespace Universal
{
    public class OpenAtUpgrade : MonoBehaviour
    {
        #region fields
        [SerializeField] private GameObject obj;
        [SerializeField] private int upgradeID;
        [SerializeField] private int upgradeTier;
        #endregion fields

        #region methods
        private IEnumerator Start()
        {
            yield return CustomMath.WaitAFrame();
            CheckUpgrade();
        }
        public bool CheckUpgrade()
        {
            UpgradeData upgrade = GameDataInit.data.upgradeData.Find(x => x.id == upgradeID && x.upgradeType == UpgradeType.Other);
            bool isActive = upgrade.tier >= upgradeTier;
            obj.SetActive(isActive);
            return isActive;
        }
        #endregion methods
    }
}