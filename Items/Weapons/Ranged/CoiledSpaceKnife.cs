using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class CoiledSpaceKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Throws a space knife that fires lasers after hitting enemies");
        }
        public override void SetDefaults()
        {
            item.damage = 14;
            item.ranged = true;
            item.width = 26;
            item.height = 26;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 17;
            item.useAnimation = 17;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3.5f;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
            item.shootSpeed = 15;
            item.shoot = ModContent.ProjectileType<CoiledSpaceKnifeProjectile>();
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 75);
            recipe.AddRecipe();
        }
    }

    class CoiledSpaceKnifeProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/CoiledSpaceKnife";
        int FallWait = 60;
        int leaveWait = 30;
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 26;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ranged = true;
            projectile.hide = false;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.timeLeft = 3000;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (FallWait > 0) return base.CanHitNPC(target);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (FallWait <= 0)
            {
                return;
            }

            FallWait = 0;
            projectile.velocity /= 6;
        }

        public override void AI()
        {
            if (FallWait > 0)
            {
                FallWait--;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            }
            else
            {
                projectile.velocity *= 0.95f;
                if (leaveWait > 0)
                {
                    leaveWait--;
                    if (leaveWait <= 0)
                    {
                        if (Main.rand.NextBool()) Main.PlaySound(SoundID.Item114, projectile.Center);
                        else Main.PlaySound(SoundID.Item115, projectile.Center);

                        for (int i = 0; i < Main.rand.Next(2, 4); i++)
                        {
                            Vector2 velocity = projectile.velocity.ToRotation().ToRotationVector2().RotatedByRandom(MathHelper.ToRadians(25));
                            Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<SpaceDeathray>(), (int)(projectile.damage * 0.75f), projectile.knockBack / 2f, projectile.owner);
                        }
                    }
                }
                else
                {
                    projectile.alpha += 15;
                    if (projectile.alpha >= 255)
                    {
                        projectile.active = false;
                    }
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(SoundID.Dig, projectile.position);
        }
    }

    class SpaceDeathray : Deathray
    {
        int timeLeft = 10;
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/LightBlast";
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.hide = false;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.timeLeft = timeLeft;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.arrow = true;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            FollowPosition = false;
            drawColor = Color.LightGreen;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
            projectile.velocity.Normalize();
        }
    }
}

