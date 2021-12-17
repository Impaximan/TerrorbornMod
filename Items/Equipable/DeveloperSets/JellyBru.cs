using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
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
            item.width = 26;
            item.height = 25;
            item.rare = ItemRarityID.Cyan;
            item.vanity = true;
            item.value = Item.sellPrice(0, 3, 0, 0);
        }
        public override bool DrawBody()
        {
            return false;
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
            item.width = 20;
            item.height = 12;
            item.rare = ItemRarityID.Cyan;
            item.vanity = true;
            item.value = Item.sellPrice(0, 3, 0, 0);
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
            item.width = 18;
            item.height = 18;
            item.rare = ItemRarityID.Cyan;
            item.vanity = true;
            item.value = Item.sellPrice(0, 3, 0, 0);
        }
    }
}
