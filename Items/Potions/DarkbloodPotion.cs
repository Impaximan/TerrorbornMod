using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Potions
{
    public class DarkbloodPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows you to use items while using Shriek of Horror");
        }
        public override void SetDefaults()
        {
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item3;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<Buffs.Darkblood>();
            Item.buffTime = 3600 * 10;
        }
    }
}
