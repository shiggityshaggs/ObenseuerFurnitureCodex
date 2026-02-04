using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BuildingSystem.FurnitureInfo;
using static Furniture;

namespace FurnitureCodex.UI
{
    internal class Cell(Furniture furniture, Skin skin = null)
    {
        public Furniture Furniture { get; } = furniture;
        public Skin Skin { get; } = skin;
        public string Title { get; } = furniture.title + (skin != null ? $" {skin.id}" : string.Empty);
        public Sprite Image { get; } = (skin != null && skin.image != null) ? skin.image : furniture.image;
        public int SkinIndex { get; } = furniture.SkinIndex(skin);
        public int OwnedCount { get; set; }

        public Meta SkinMeta()
        {
            var meta = new BuildingSystem.FurnitureInfo.Meta() { currentSkin = SkinIndex };
            return meta;
        }

        public int UpdateOwnedCount()
        {
            OwnedCount = BuildingSystem.instance.availableFurnitures
                .Where(af => af.furniture.Id == Furniture.Id && af.meta.currentSkin == SkinIndex)
                .Sum(af => af.amount);
            return OwnedCount;
        }

        public bool AddOrRemove(bool canRemoveStorage = false)
        {
            if (Event.current.button == 0)
            {
                return BuildingSystem.instance.AddFurniture(Furniture, 1, SkinMeta());
            }

            if (Event.current.button == 1 && OwnedCount > 0)
            {
                var fi = BuildingSystem.instance.availableFurnitures
                    .Where(x => x.gameObject == null || canRemoveStorage)
                    .FirstOrDefault(x => x.furniture.Id == Furniture.Id && x.meta.currentSkin == SkinIndex);

                return (fi != null) && BuildingSystem.instance.RemoveFurniture(fi);
            }

            return false;
        }
    }

    internal static class CellExt
    {
        private static Cell Cell(this Furniture furniture) => new(furniture);
        public static List<Cell> Cells(this Furniture furniture)
        {
            List<Cell> cells = [furniture.Cell(), .. furniture.skins.Select(skin => new Cell(furniture, skin))];
            foreach (var cell in cells) cell.UpdateOwnedCount();
            return cells;
        }
    }
}
