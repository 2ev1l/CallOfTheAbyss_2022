using UnityEngine;

namespace Universal
{
    public class ObjectsUpdate : SingleSceneInstance
    {
        #region fields
        public static ObjectsUpdate instance { get; private set; }
        private static new GameObject gameObject;
        public static Vector2 offset = new Vector2(-1.34f, -0.24f);
        public static bool ignoreCoordinates = false;
        public static Vector2 customOffset = new Vector2();
        #endregion fields

        #region methods
        protected override void Awake()
        {
            instance = this;
            gameObject = instance.transform.gameObject;
            CheckInstances(GetType());
        }
        protected virtual void Update()
        {
            if (gameObject != null && gameObject.activeSelf)
            {
                Vector2 mult = CustomMath.GetScreenSquare();
                if (ignoreCoordinates)
                {
                    mult = customOffset;
                }
                Vector3 pos = gameObject.transform.position;
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float offX = offset.x * mult.x;
                float offY = offset.y * mult.y;
                pos.x += offX;
                pos.y += offY;
                pos.z = 1;
                gameObject.transform.position = pos;
            }
        }

        #endregion methods
    }
}