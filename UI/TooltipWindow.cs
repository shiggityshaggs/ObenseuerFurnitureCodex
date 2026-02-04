using UnityEngine;

namespace FurnitureCodex.UI
{
    internal class TooltipWindow : MonoBehaviour
    {
        readonly int windowId = "FurnitureCodexTooltipWindow".GetHashCode();
        Rect windowRect;

        GUIStyle windowStyle;
        GUIStyle labelStyle;

        void Init()
        {
            windowRect.x = Input.mousePosition.x + 15;
            windowRect.y = Screen.height - Input.mousePosition.y;

            if (windowStyle == null)
            {
                windowStyle = new();
            }

            if (labelStyle == null)
            {
                labelStyle = new(GUI.skin.label);
                labelStyle.wordWrap = false;
                labelStyle.normal.background = Texture2D.linearGrayTexture;
            }
        }

        internal void OnGUICustom()
        {
            if (!Codex.HoverItem) return;
            Init();
            windowRect = GUILayout.Window(id: windowId, screenRect: windowRect, func: WindowFunction, text: "", style: windowStyle);
        }

        private void WindowFunction(int id)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUI.backgroundColor = Color.black;
                GUILayout.Label($"{Codex.HoverItem?.title} {Codex.HoverSkin?.id}", labelStyle);
                GUI.backgroundColor = Color.white;

                GUILayout.FlexibleSpace();
            }
        }
    }
}
