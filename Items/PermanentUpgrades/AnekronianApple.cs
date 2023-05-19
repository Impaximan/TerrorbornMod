using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.PermanentUpgrades
{
    class AnekronianApple : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Permanently increases Shriek of Horror movement speed by 20%" +
                "\nCan only be used once");
        }
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.consumable = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.maxStack = 9999;
            Item.UseSound = SoundID.Item2;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!modPlayer.AnekronianApple)
            {
                CombatText.NewText(new Rectangle((int)(player.Center.X - 50), (int)(player.Center.Y - 50), 100, 10), Color.Red, "Shriek of Horror movement speed increased by 20%", true, false);
                modPlayer.AnekronianApple = true;
                Item.stack--;
                return true;
            }
            return false;
        }
    }
}

