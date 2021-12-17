using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.vanity = true;
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
            item.width = 26;
            item.height = 24;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.vanity = true;
        }

        public override bool DrawBody()
        {
            return false;
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
            item.width = 20;
            item.height = 12;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.vanity = true;
        }
    }
}

