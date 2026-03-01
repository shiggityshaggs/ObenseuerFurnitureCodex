using BepInEx;
using FurnitureCodex.UI;
using FurnitureCodex.Utility;
using System;

namespace FurnitureCodex;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    static internal Codex codex;
    static internal TooltipWindow tooltip;
    static internal InputHandler inputHandler;

    private void Awake()
    {
        Console.WriteLine($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        codex = gameObject.AddComponent<UI.Codex>();
        codex.enabled = false;

        tooltip = gameObject.AddComponent<UI.TooltipWindow>();
        tooltip.enabled = false;

        inputHandler = gameObject.AddComponent<InputHandler>();
    }
}
