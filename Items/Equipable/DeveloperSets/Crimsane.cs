using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.DeveloperSets
{
    [AutoloadEquip(EquipType.Body)]
    public class CrimsanesRobes : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sir Pogsalots's Robes");
            Tooltip.SetDefault("[c/8A9B98:Great for impersonating Terrorborn devs!]");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class CrimsanesLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sir Pogsalots's Leg Robes");
            Tooltip.SetDefault("[c/8A9B98:Great for impersonating Terrorborn devs!]");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }
    }
    [AutoloadEquip(EquipType.Head)]
    public class CrimsanesHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sir Pogsalots's Hood");
            Tooltip.SetDefault("[c/8A9B98:Great for impersonating Terrorborn devs!]");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }
    }
}

