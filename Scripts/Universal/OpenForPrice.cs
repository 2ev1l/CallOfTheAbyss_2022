using GameMenu.Shop;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Universal
{
    public class OpenForPrice : ShowHelp, IPointerClickHandler
    {
        #region fields
        [Header("Pricing")]
        [SerializeField] private Button mainButton;
        [SerializeField] private int priceSilver;
        [SerializeField] private int interfaceID;
        private bool isOpened;
        private int defaultId;
        #endregion fields

        #region methods
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || isOpened || priceSilver > GameDataInit.data.coinsSilver) return;
            OpenLocation();
        }
        private void OnEnable()
        {
            GameDataInit.instance.OnCoinsChanged += Init;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            GameDataInit.instance.OnCoinsChanged -= Init;
        }

        private IEnumerator Start()
        {
            defaultId = id;
            yield return CustomMath.WaitAFrame();
            Init();
        }
        public void Init()
        {
            isOpened = GameDataInit.data.interfacesOpened.IndexOf(interfaceID) >= 0;
            mainButton.enabled = isOpened;
            if (!isOpened)
            {
                id = 58;
                additionalText = $" ({langData[defaultId]}): ";
                additionalText = PriceParser.InsertSilverPrice(additionalText, priceSilver, true);
            }
            else
            {
                id = defaultId;
                additionalText = "";
            }
        }
        public void OpenLocation()
        {
            GameDataInit.data.interfacesOpened.Add(interfaceID);
            GameDataInit.AddSilver(-priceSilver, false);
            Init();

            helpText.SetActive(true);
            txt.text = langData[id] + additionalText;
            StartCoroutine(UpdateLine());
        }
        #endregion methods
    }
}