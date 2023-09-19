using UnityEngine;

namespace Universal
{
    public class CursorSettings : MonoBehaviour
    {
        public static CursorSettings instance { get; private set; }

        [SerializeField] private Texture2D cursorDefault;
        [SerializeField] private Texture2D cursorPoint;
        [SerializeField] private Vector2 cursorDefaultOffset;

        [SerializeField] private AudioClip onClickSound;
        [SerializeField] private AudioClip onSelectSound;
        public void Init()
        {
            instance = this;
            Cursor.SetCursor(cursorDefault, cursorDefaultOffset, CursorMode.Auto);
        }
        public void SetDefaultCursor() => Cursor.SetCursor(cursorDefault, cursorDefaultOffset, CursorMode.Auto);
        public void SetPointCursor() => Cursor.SetCursor(cursorPoint, cursorDefaultOffset, CursorMode.Auto);

        public void DoClickSound() => AudioManager.PlayClip(onClickSound, SoundType.Sound);
    }
}