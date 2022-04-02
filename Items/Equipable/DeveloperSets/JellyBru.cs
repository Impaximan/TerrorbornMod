using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.DeveloperSets
{
    [AutoloadEquip(EquipType.Body)]
    public class JellybrusFeatherwear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("JellyBru's Featherwear");
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
    public class JellybrusTalons : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("JellyBru's Talons");
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
    public class JellybrusBeak : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("JellyBru's Beak");
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
