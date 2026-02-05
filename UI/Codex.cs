using FurnitureCodex.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Furniture;

namespace FurnitureCodex.UI
{
    [RequireComponent(typeof(TooltipWindow), typeof(InputHandler))]
    internal class Codex : MonoBehaviour
    {
        Dictionary<Category, List<Cell>> furnitureCellsDict;

        readonly int windowId = "FurnitureCodexMainWindow".GetHashCode();
        Rect windowRect;
        Vector2 scrollPos;

        GUILayoutOption[] windowStyle;
        GUIStyle buttonStyle;
        readonly int itemsPerRow = 10;
        bool windowHover;
        bool canRemoveStorage = false;

        public static Furniture HoverItem { get; set; }

        public static Skin HoverSkin { get; set; }

        HashSet<Category> collapsedCategories = [];

        public string Filter
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;

                    FilterPredicate = cell =>
                    string.IsNullOrEmpty(field) ||
                    cell.Title.ToLowerInvariant().Contains(field.ToLowerInvariant());

                    StartCoroutine(Routine());
                }
            }
        } = string.Empty;

        int SelectionIndex
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;

                    OwnedPredicate = field switch
                    {
                        1 => cell => cell.OwnedCount > 0,
                        2 => cell => cell.OwnedCount == 0,
                        _ => cell => true
                    };

                    StartCoroutine(Routine());
                }
            }
        } = 0;

        GUIStyle selectionGridStyle;

        Predicate<Cell> OwnedPredicate = _ => true;
        Predicate<Cell> FilterPredicate = _ => true;

        void Init()
        {
            if (selectionGridStyle == null)
            {
                selectionGridStyle = new GUIStyle(GUI.skin.button);
                selectionGridStyle.onNormal.textColor = Color.cyan;
                selectionGridStyle.onHover.textColor = Color.cyan;
            }

            if (windowRect.Equals(default))
            {
                var x = Screen.width / 2 - (itemsPerRow * 70 / 2);
                var y = Screen.height / 3.5f;
                windowRect = new(x, y, 0, 0);
            }

            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    fixedWidth = 64,
                    fixedHeight = 64,
                    margin = new RectOffset(left: 2, right: 2, top: 2, bottom: 5)
                };
                buttonStyle.normal.textColor = Color.white;
            }

            windowStyle ??= [
                GUILayout.Width(itemsPerRow * 70 + 5),
                GUILayout.Height(Screen.height / 2),
            ];
        }

        void OnEnable()
        {
            if (DeveloperConsole.Instance?.furniture?.furnitures != null)
                StartCoroutine(Routine());
        }

        IEnumerator Routine()
        {
            while (DeveloperConsole.Instance?.furniture?.furnitures == null)
            {
                yield return new WaitForSecondsRealtime(1);
            }

            ReGroup();
        }

        void ReGroup()
        {
            furnitureCellsDict = [];

            var groupings = DeveloperConsole.Instance.furniture.furnitures
            .OrderBy(x => x.category.ToString())
            .ThenBy(x => x.name)
            .GroupBy(x => x.category)
            .ToList();

            foreach (var grouping in groupings)
            {
                var cells = grouping
                    .Where(x => x.category != Category.None)
                    .SelectMany(x => x.Cells())
                    .Where(OwnedPredicate.Invoke)
                    .Where(FilterPredicate.Invoke)
                    .ToList();

                if (cells.Any())
                    furnitureCellsDict.Add(grouping.Key, [.. cells]);
            }
        }

        void OnGUI()
        {
            if (furnitureCellsDict == null) return;
            Init();
            windowHover = windowRect.Contains(Event.current.mousePosition);
            GUI.backgroundColor = Color.black;
            windowRect = GUILayout.Window(windowId, windowRect, WindowFunction, "Furniture Codex", windowStyle);
            GUI.backgroundColor = Color.white;
            if (HoverItem && Plugin.tooltip) Plugin.tooltip.OnGUICustom();
        }

        void Release()
        {
            if (GUI.GetNameOfFocusedControl() == "Filter")
            {
                GUI.FocusControl(null);
                GUI.FocusWindow(windowId);
            }
        }

        private void WindowFunction(int id)
        {
            HoverItem = null;

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Filter:");

                GUI.SetNextControlName("Filter");
                Filter = GUILayout.TextField(Filter, GUILayout.MinWidth(100));

                if (!string.IsNullOrEmpty(Filter))
                {
                    if (GUILayout.Button("X"))
                    {
                        Filter = string.Empty;
                        Release();
                    }
                }

                GUILayout.FlexibleSpace();
                GUILayout.Label("Show:");
                SelectionIndex = GUILayout.SelectionGrid(selected: SelectionIndex, texts: ["All", "Have", "Missing"], xCount: 3, selectionGridStyle);

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Collapse")) collapsedCategories = [..Enum.GetValues(typeof(Category)).Cast<Category>()];
                if (GUILayout.Button("Expand")) collapsedCategories = [];

                GUILayout.FlexibleSpace();
                canRemoveStorage = GUILayout.Toggle(canRemoveStorage, "Can remove storage");
            }

            if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.KeyDown)
                Release();

            using (var scrollView = new GUILayout.ScrollViewScope(scrollPos))
            {
                scrollPos = scrollView.scrollPosition;

                foreach (var kvp in furnitureCellsDict)
                {
                    bool collapsed = collapsedCategories.Contains(kvp.Key);

                    GUI.color = collapsed ? Color.yellow : Color.white;
                    if (GUILayout.Button(kvp.Key.ToString(), GUI.skin.label))
                    {
                        if (collapsed) collapsedCategories.Remove(kvp.Key);
                        else collapsedCategories.Add(kvp.Key);
                        return;
                    }

                    if (collapsed) continue;

                    var filtered = kvp.Value;

                    for (int i = 0; i < filtered.Count(); i += itemsPerRow)
                    {
                        var cells = filtered.Skip(i).Take(itemsPerRow);

                        using (new GUILayout.HorizontalScope())
                        {
                            foreach (var cell in cells)
                            {
                                var content = new GUIContent() { image = cell.Image?.texture ?? Texture2D.redTexture };
                                if (GUILayout.Button(content, buttonStyle))
                                {
                                    if (cell.AddOrRemove(canRemoveStorage))
                                    {
                                        ReGroup();
                                    }
                                }

                                if (cell.OwnedCount > 0)
                                    GUI.Label(GUILayoutUtility.GetLastRect(), cell.OwnedCount.ToString());

                                if (Event.current.type == EventType.Repaint)
                                {
                                    var rect = GUILayoutUtility.GetLastRect();
                                    if (windowHover && rect.Contains(Event.current.mousePosition))
                                    {
                                        HoverItem = cell.Furniture;
                                        HoverSkin = cell.Skin;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!GUI.changed) GUI.DragWindow();
        }
    }

    internal static class FurnitureExt
    {
        public static int SkinIndex(this Furniture furniture, Skin skin) => (skin == null) ? 0 : furniture.skins.ToList().IndexOf(skin) + 1;
    }
}
