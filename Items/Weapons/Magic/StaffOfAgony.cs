using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using System.Collections.Generic;
using TerrorbornMod.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class StaffOfAgony : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Rains deathrays from the sky");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 56;
            item.noMelee = true;
            item.width = 48;
            item.height = 48;
            item.useTime = 5;
            item.useAnimation = 5;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.crit = 14;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item12;
            item.shootSpeed = 1f;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<AgonyLazer>();
            item.mana = 3;
            item.magic = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = new Vector2(Main.MouseWorld.X + Main.rand.Next(-200, 200), player.Center.Y - 1000);
            Vector2 shootSpeed = Main.MouseWorld - position;
            shootSpeed.Normalize();
            speedX = shootSpeed.X;
            speedY = shootSpeed.Y;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 12);
            recipe.AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class AgonyLazer : Deathray
    {
        int timeLeft = 15;
        Vector2 offsetFromPlayer;
        public override string Texture => "TerrorbornMod/Projectiles/IncendiaryDeathray";
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 22;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.hide = false;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = timeLeft;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            offsetFromPlayer = projectile.position - Main.player[projectile.owner].Center;
            bodyRect = new Rectangle(0, 24, 18, 22);
            headRect = new Rectangle(0, 0, 18, 22);
            tailRect = new Rectangle(0, 46, 18, 22);
        }

        public override Vector2 Position()
        {
            return projectile.position;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
        }
    }
}

