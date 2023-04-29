using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged.Thrown
{
    class MindPiercer : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Hitting an enemy causes a spearhead to form above them" +
                "\nThis spearhead will loom above them for 3 seconds before falling and dealing 50 damage" +
                "\nOnly one spearhead can form at once per enemy"); */
        }
        public override void SetDefaults()
        {
            Item.damage = 2;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 38;
            Item.height = 38;
            Item.useTime = 17;
            Item.noUseGraphic = true;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 15;
            Item.shoot = ModContent.ProjectileType<MindPiercerProjectile>();
        }
    }
    class MindPiercerProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/Thrown/MindPiercer";
        int FallWait = 40;
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = false;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 3000;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            FallWait = 0;
            TerrorbornNPC modTarget = TerrorbornNPC.modNPC(target);
            if (modTarget.mindSpearheadTime <= 0)
            {
                modTarget.mindSpearheadTime = 60 * 3;
                int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<MindSpearhead>(), 50, 0, Projectile.owner);
                Main.projectile[proj].ai[0] = target.whoAmI;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (FallWait <= 0)
            {
                damage = (int)(damage * 0.75f);
            }
        }

        public override void AI()
        {
            if (FallWait > 0)
            {
                FallWait--;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);

                if (Projectile.velocity.X > 0)
                {
                    Projectile.spriteDirection = 1;
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
                }
                else
                {
                    Projectile.spriteDirection = -1;
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
                }
            }
            else
            {
                Projectile.velocity *= 0.9f;
                Projectile.alpha += 255 / 20;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
        }
    }

    class MindSpearhead : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 3000;
        }

        bool fallen = false;
        int distance = 50;

        public override bool? CanHitNPC(NPC target)
        {
            TerrorbornNPC modTarget = TerrorbornNPC.modNPC(target);
            return target.whoAmI == Projectile.ai[0] && fallen;
        }

        public override void AI()
        {
            NPC target = Main.npc[(int)Projectile.ai[0]];
            TerrorbornNPC modTarget = TerrorbornNPC.modNPC(target);

            if (modTarget.mindSpearheadTime <= 0)
            {
                fallen = true;
            }

            if (!target.active)
            {
                Projectile.active = false;
            }

            if (fallen)
            {
                distance -= 10;
                if (distance < 0)
                {
                    distance = 0;
                }
            }
            else
            {
                Projectile.rotation = Projectile.DirectionTo(target.Center).ToRotation() + MathHelper.ToRadians(45);
            }

            Projectile.position = new Vector2(target.Center.X, target.position.Y - distance);
            Projectile.position -= new Vector2(Projectile.width, Projectile.height) / 2;
        }
    }
}