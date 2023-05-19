using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class IncendiaryVisor : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Underworldly Visor");
            Tooltip.SetDefault("'It feels otherwordly... but also incendiary'");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.vanity = true;
        }

    }

    [AutoloadEquip(EquipType.Body)]
    public class IncendiaryBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Underwordly Breastplate");
            Tooltip.SetDefault("'It feels otherwordly... but also incendiary'");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.vanity = true;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class IncendiaryLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Underworldly Leggings");
            Tooltip.SetDefault("'It feels otherwordly... but also incendiary'");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 12;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.vanity = true;
        }
    }
}

