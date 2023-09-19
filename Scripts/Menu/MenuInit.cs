using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace Menu
{
    public class MenuInit : HelpUpdate
    {
        #region fields & properties
        [SerializeField] private GameObject buttonContinue;
        [SerializeField] private GameObject buttonContinueLine;
        [SerializeField] private GameObject buttonAdventureExtra;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            CheckInstances(GetType());
        }
        private void Start()
        {
            buttonAdventureExtra.SetActive(GameDataInit.data.isGameCompleted);

            if (GameDataInit.data.sceneName != "Menu") return;

            buttonContinue.GetComponent<Button>().enabled = false;
            buttonContinue.GetComponent<Buttons>().enabled = false;
            buttonContinueLine.SetActive(true);
        }
        #endregion methods
    }
}