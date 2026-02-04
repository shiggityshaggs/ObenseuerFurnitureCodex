using UnityEngine;
using HarmonyLib;

namespace FurnitureCodex.Utility
{
    internal class InputHandler : MonoBehaviour
    {
        bool IsMenu => GameUIController.instance?.IsMenuVisible ?? false;
        bool IsInventory => GameUIController.instance?.InventoryIsVisible() ?? false;
        bool IsBuildMenu => BuildingSystemMenu.instance?.menuIsOpen ?? false;
        bool IsPaused => GameController.instance?.GameIsPaused ?? false;

        internal static bool WasCodexClosedThisFrame;

        void Update()
        {
            WasCodexClosedThisFrame = false;

            if (!GameUIController.instance || !BuildingSystemMenu.instance) return;
            if (IsMenu || IsInventory || IsBuildMenu || IsPaused) return;

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

        void OpenCodex()
        {
            Plugin.codex.enabled = true;
            Plugin.tooltip.enabled = true;
            GameController.instance.ControlsDisabled(gameObject: this.gameObject, ShowCursor: true);
        }

        void CloseCodex()
        {
            Plugin.codex.enabled = false;
            Plugin.tooltip.enabled = false;
            GameController.instance.ControlsEnabled(this.gameObject);
            WasCodexClosedThisFrame = true;
        }
    }

    [HarmonyPatch]
    class Patch
    {
        [HarmonyPatch(typeof(PauseMenu), nameof(PauseMenu.Pause)), HarmonyPrefix]
        static bool PauseMenu_Pause() => !InputHandler.WasCodexClosedThisFrame;

        [HarmonyPatch(typeof(GameUIController), nameof(GameUIController.ToggleGameMenu)), HarmonyPrefix]
        static bool GameUIController_ToggleGameMenu() => !InputHandler.WasCodexClosedThisFrame;
    }
}
