﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerrorbornMod.TBUtils;

namespace TerrorbornMod.Items.Equipable.Accessories.Shields
{
    [AutoloadEquip(EquipType.Shield)]
    class Aegis : ModItem
    {
        int cooldown = 10 * 60;
        float knockback = 10f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TBUtils.Accessories.GetParryShieldString(cooldown, knockback) + "\nParrying attacks will cause a holy burst of light that deals 1000 damage to nearby enemies");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.defense = 8;
            Item.knockBack = knockback;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            TerrorbornItem.modItem(Item).parryShield = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.parryColor = Color.LightGoldenrodYellow;
            if (modPlayer.JustParried)
            {
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<AegisLight>(), 1000, 0f, player.whoAmI);
                TerrorbornSystem.ScreenShake(25f);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item68, player.Center);
            }
            TBUtils.Accessories.UpdateParryShield(cooldown, Item, player);
        }
    }

    class AegisLight : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Glow_2";

        int timeLeft = 10;
        const int defaultSize = 1000;
        int currentSize = defaultSize;
        public override void SetDefaults()
        {
            Projectile.width = defaultSize;
            Projectile.height = defaultSize;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = timeLeft;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, currentSize, Color.LightYellow);
            return false;
        }

        public override void AI()
        {
            currentSize -= defaultSize / timeLeft;
        }
    }
}
