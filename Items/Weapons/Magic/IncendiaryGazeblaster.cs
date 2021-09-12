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
    class IncendiaryGazeblaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gazeblaster");
            Tooltip.SetDefault("Fires mini deathrays at your cursor that can hit enemies multiple times");
        }

        public override void SetDefaults()
        {
            item.damage = 17;
            item.noMelee = true;
            item.width = 46;
            item.height = 26;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = 5;
            item.crit = 14;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item33;
            item.shootSpeed = 1f;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<IncendiaryGazeblast>();
            item.mana = 6;
            item.magic = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 shootSpeed = new Vector2(speedX, speedY);
            shootSpeed.Normalize();
            speedX = shootSpeed.X;
            speedY = shootSpeed.Y;
            return true;
        }
    }

    class IncendiaryGazeblast : Deathray
    {
        int timeLeft = 30;
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
            projectile.localNPCHitCooldown = 10;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            bodyRect = new Rectangle(0, 24, 18, 22);
            headRect = new Rectangle(0, 0, 18, 22);
            tailRect = new Rectangle(0, 46, 18, 22);
        }

        public override Vector2 Position()
        {
            return Main.player[projectile.owner].Center;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
        }
    }
}
