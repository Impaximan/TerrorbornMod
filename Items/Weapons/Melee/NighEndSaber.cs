using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TerrorbornMod.Items.Weapons.Melee
{
    public class NighEndSaber : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hitting enemies on left click grants you terror" +
                "\nRight click to consume 25% terror and throw the saber, swapping places with the enemy it hits");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.critDamageMult = 1.1f;
            item.damage = 16;
            item.melee = true;
            item.width = 34;
            item.height = 36;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 4f;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shootSpeed = 15f;
            item.shoot = ModContent.ProjectileType<NighEndThrown>();
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            TerrorbornPlayer.modPlayer(player).GainTerror(3f, false, false, true);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.noUseGraphic = true;
                item.autoReuse = false;
                item.noMelee = true;
                if (TerrorbornPlayer.modPlayer(player).TerrorPercent < 25f)
                {
                    return false;
                }
                TerrorbornPlayer.modPlayer(player).LoseTerror(25f);
            }
            else
            {
                item.noUseGraphic = false;
                item.autoReuse = true;
                item.noMelee = false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse != 2)
            {
                return false;
            }
            return true;
        }
    }

    public class NighEndThrown : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Melee/NighEndSaber";
        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 36;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.melee = true;
            projectile.hide = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.timeLeft = 3000;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            Vector2 originalPosition = player.Center;
            player.position = target.Center - player.Size / 2;
            target.position = originalPosition - target.Size / 2;
            Main.PlaySound(SoundID.Item6, player.Center);
        }

        public override void AI()
        {
            if (projectile.velocity.X > 0)
            {
                projectile.spriteDirection = 1;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            }
            else
            {
                projectile.spriteDirection = -1;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            }
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(SoundID.Tink, projectile.position, 0);
        }
    }
}
