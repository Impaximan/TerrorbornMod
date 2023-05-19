using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.BurstJumps
{
    class BottledWeeping : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.DreadfulEssence>(5)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(Utils.Accessories.GetBurstJumpString((int)(60 * 1f)) + "\nGrants you immunity frames upon use, with a cooldown of 10 seconds");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<Rarities.Golden>();
            Item.defense = 10;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.useAnimation = 5;
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.burstJump = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Utils.Accessories.UpdateBurstJump((int)(60 * 1f), 45, Item, player, new Vector2(25, -5), Color.LightGoldenrodYellow, SoundID.Item14);
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.JustBurstJumped)
            {
                if (!player.HasBuff(ModContent.BuffType<BottledWeepingCooldown>()))
                {
                    modPlayer.iFrames = 60 * 2;
                    player.AddBuff(ModContent.BuffType<BottledWeepingCooldown>(), 60 * 10);
                }
            }
            if (modPlayer.BurstJumpTime > 0)
            {
                Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, DustID.GoldFlame)];
                dust.noGravity = true;
                dust.noLight = false;
                dust.velocity = Vector2.Zero;
            }
        }
    }

    class BottledWeepingCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Released Weeping");
            // Description.SetDefault("Bottled weeping immunity on cooldown");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
    }
}