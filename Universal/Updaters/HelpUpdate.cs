using UnityEngine;

namespace Universal
{
    public class HelpUpdate : SingleSceneInstance
    {
        #region fields
        public static float offsetHelpX = -1.34f;
        public static float offsetHelpY = -0.24f;
        #endregion fields

        #region methods
        protected override void Awake()
        {
            CheckInstances(GetType());
        }
        protected virtual void Update()
        {
            if (ShowHelp.helpText != null && ShowHelp.helpText.activeSelf)
            {
                Vector3 pos = ShowHelp.helpText.transform.position;
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float offX = offsetHelpX * CustomMath.GetScreenSquare().x;
                float offY = offsetHelpY * CustomMath.GetScreenSquare().y;
                pos.x += offX;
                pos.y += offY;
                pos.z = 1;
                ShowHelp.helpText.transform.position = pos;
            }
        }
        #endregion methods
    }
}