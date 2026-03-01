using UnityEngine;

namespace FurnitureCodex.Utility
{
    internal class InputHandler : MonoBehaviour
    {
        bool IsMenu => GameUIController.instance?.IsMenuVisible ?? false;
        bool IsInventory => GameUIController.instance?.InventoryIsVisible() ?? false;
        bool IsBuildMenu => BuildingSystemMenu.instance?.menuIsOpen ?? false;
        bool IsPaused => GameController.instance?.GameIsPaused ?? false;

        void Update()
        {
            if (!GameUIController.instance || !BuildingSystemMenu.instance) return;
            if ((IsMenu && !IsBuildMenu) || IsInventory || IsPaused) return;

            if (Input.GetKeyDown(KeyCode.K))
            {
                if (Plugin.codex.enabled) CloseCodex();
                else OpenCodex();
                return;
            }

            if (InputManager.instance.GetKeyDown("Pause Menu") || InputManager.instance.GetKeyDown("Menu"))
            {
                if (Plugin.codex.enabled) CloseCodex();
            }
        }

        System.Collections.IEnumerator ControlsEnabled(bool enable)
        {
            yield return new WaitForEndOfFrame();
            if (enable)
                GameController.instance.ControlsEnabled(this.gameObject);
            else
                GameController.instance.ControlsDisabled(gameObject: this.gameObject, ShowCursor: true);
        }

        void OpenCodex()
        {
            if (GameController.instance != null && GameController.instance.keysDisabled)
                return;

            Plugin.codex.enabled = true;
            Plugin.tooltip.enabled = true;
            StartCoroutine(ControlsEnabled(false));
        }

        void CloseCodex()
        {
            Plugin.codex.enabled = false;
            Plugin.tooltip.enabled = false;
            StartCoroutine(ControlsEnabled(true));
        }
    }
}
