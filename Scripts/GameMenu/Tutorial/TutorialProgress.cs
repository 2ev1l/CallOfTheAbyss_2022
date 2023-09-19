using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Tutorial
{
    public class TutorialProgress : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private List<GameObject> disableObjects;
        [SerializeField] private List<GameObject> enableObjects;
        [SerializeField] private List<Button> buttons;
        [SerializeField] private List<GameObject> disableAfterButtons;
        [SerializeField] private bool disableImageRaycast;
        [field: SerializeField] public int id { get; private set; }
        #endregion fields & properties

        #region methods
        public void Init()
        {
            GameDataInit.data.tutorialProgress = id;
            SavingUtils.SaveGameData();
            foreach (var el in disableObjects.Where(obj => obj != gameObject))
                el.SetActive(false);
            Invoke(nameof(OnClickButton), 0f);
        }
        private void OnClickButton()
        {
            foreach (var el in enableObjects)
                el.SetActive(true);

            foreach (Button el in buttons)
                el.onClick.Invoke();

            foreach (var el in disableAfterButtons)
                el.SetActive(false);

            GameObject.Find("TutorialPanel").GetComponent<Image>().enabled = !disableImageRaycast;

            GameObject go = disableObjects.Find(obj => obj == gameObject);
            if (go != null)
            {
                go.SetActive(false);
            }
        }
        #endregion methods
    }
}