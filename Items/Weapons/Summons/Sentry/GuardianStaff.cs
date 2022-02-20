using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Items.Weapons.Summons.Sentry
{
    class GuardianStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a friendly incendiary guardian to fire deathrays at enemies");
        }

        public override void SetDefaults()
        {
            item.mana = 10;
            item.summon = true;
            item.damage = 22;
            item.width = 44;
            item.height = 44;
            item.sentry = true;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 1.5f;
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item44;
            item.shoot = ModContent.ProjectileType<IncendiaryGuardianSummon>();
            item.shootSpeed = 10f;
            item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse != 2)
            {
                int turretAmount = 0;
                for (int i = 999; i >= 0; i--)
                {
                    if (Main.projectile[i].sentry && Main.projectile[i].active)
                    {
                        turretAmount++;
                        if (turretAmount >= player.maxTurrets)
                        {
                            Main.projectile[i].active = false;
                        }
                    }
                }
                Projectile.NewProjectile(new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, type, damage, knockBack, item.owner);
            }
            return false;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
    }

    class IncendiaryGuardianSummon : ModProjectile
    {
        public override string Texture => "TerrorbornMod/NPCs/Incendiary/IncendiaryGuardian";

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 62;
            projectile.height = 70;
            projectile.friendly = false;
            projectile.sentry = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 10;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale = 1f;
        }

        void FindFrame(int FrameHeight)
        {
            projectile.frame = 3;
        }

        int projectileWait = 0;
        public override void AI()
        {
            FindFrame(projectile.height);
            projectile.timeLeft = 10;
            bool Targeted = false;

            Player player = Main.player[projectile.owner];
            NPC target = Main.npc[0];
            if (player.HasMinionAttackTargetNPC && Main.npc[player.MinionAttackTargetNPC].Distance(projectile.Center) < 1500)
            {
                target = Main.npc[player.MinionAttackTargetNPC];
                Targeted = true;
            }
            else
            {
                float Distance = 1000;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                    {
                        target = Main.npc[i];
                        Distance = Main.npc[i].Distance(projectile.Center);
                        Targeted = true;
                    }
                }
            }

            if (Targeted)
            {
                projectileWait++;
                if (projectileWait > 25)
                {
                    projectileWait = 0;
                    Vector2 position = projectile.Center + new Vector2(0, -10);
                    Vector2 velocity = target.DirectionFrom(position);
                    Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<GuardianSummonLaser>(), projectile.damage, projectile.knockBack, projectile.owner);
                    Main.PlaySound(SoundID.Item33, position);
                }
            }
        }
    }

    class GuardianSummonLaser : Deathray
    {
        int timeLeft = 25;
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
            projectile.timeLeft = timeLeft;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 5;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            FollowPosition = false;
            drawColor = new Color(255, 228, 200);
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
            projectile.velocity.Normalize();
        }
    }
}