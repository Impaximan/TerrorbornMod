using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.DeveloperSets
{
    [AutoloadEquip(EquipType.Body)]
    public class PenumbralGamingsChestwear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Penumbral's Chestwear");
            Tooltip.SetDefault("[c/8A9B98:Great for impersonating Terrorborn devs!]");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 25;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class PenumbralGamingsLegwear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Penumbral's Legwear");
            Tooltip.SetDefault("[c/8A9B98:Great for impersonating Terrorborn devs!]");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 12;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }
    }
    [AutoloadEquip(EquipType.Head)]
    public class PenumbralGamingsVisor : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Penumbral's Visor");
            Tooltip.SetDefault("[c/8A9B98:Great for impersonating Terrorborn devs!]");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ItemRarityID.Cyan;
            Item.vanity = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }
    }
}
