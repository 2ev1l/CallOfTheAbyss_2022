using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Universal;
using GameMenu.Inventory.Storages;

namespace GameMenu.Inventory.Chests
{
    public class ChestImageUpdater : DefaultUpdater
    {
        #region fields
        [SerializeField] private ChestInit chestInit;
        [SerializeField] private Image mainImage;
        private Text mainImageText;
        [SerializeField] private Image spawnedChestImage;
        #endregion fields

        protected override void OnEnable()
        {
            chestInit.OnInfoChange += SetImage;
        }
        protected override void OnDisable()
        {
            chestInit.OnInfoChange -= SetImage;
        }
        private void SetImage()
        {
            Sprite chestSprite = InventoryChestStorage.instance.GetChestSprite(chestInit.chestInfo);
            spawnedChestImage.sprite = chestSprite;
            mainImage.sprite = chestSprite;
        }
        public void ChangeSpawnedChestImage(Image newImage) => spawnedChestImage = newImage;
        public IEnumerator ShowChest()
        {
            float inc = 0f;
            while (spawnedChestImage.color.a < 1f)
            {
                inc += Time.fixedDeltaTime;
                spawnedChestImage.color = new Color(1f, 1f, 1f, inc);
                yield return new WaitForFixedUpdate();
            }
            spawnedChestImage.color = Color.clear;
        }
        public void EnableMainImage(bool isEnabled)
        {
            if (!isEnabled)
                mainImageText = mainImage.transform.Find("Text").GetComponent<Text>();
            mainImageText.enabled = isEnabled;
            mainImage.enabled = isEnabled;
        }
    }
}