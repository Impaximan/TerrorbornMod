using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;
using Terraria.DataStructures;

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
            Item.mana = 10;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 22;
            Item.width = 44;
            Item.height = 44;
            Item.sentry = true;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<IncendiaryGuardianSummon>();
            Item.shootSpeed = 10f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
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
                Projectile.NewProjectile(source, new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim(false);
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
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 62;
            Projectile.height = 70;
            Projectile.friendly = false;
            Projectile.sentry = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
        }

        void FindFrame(int FrameHeight)
        {
            Projectile.frame = 3;
        }

        int ProjectileWait = 0;
        public override void AI()
        {
            FindFrame(Projectile.height);
            Projectile.timeLeft = 10;
            bool Targeted = false;

            Player player = Main.player[Projectile.owner];
            NPC target = Main.npc[0];
            if (player.HasMinionAttackTargetNPC && Main.npc[player.MinionAttackTargetNPC].Distance(Projectile.Center) < 1500)
            {
                target = Main.npc[player.MinionAttackTargetNPC];
                Targeted = true;
            }
            else
            {
                float Distance = 1000;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].Distance(Projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                    {
                        target = Main.npc[i];
                        Distance = Main.npc[i].Distance(Projectile.Center);
                        Targeted = true;
                    }
                }
            }

            if (Targeted)
            {
                ProjectileWait++;
                if (ProjectileWait > 25)
                {
                    ProjectileWait = 0;
                    Vector2 position = Projectile.Center + new Vector2(0, -10);
                    Vector2 velocity = target.DirectionFrom(position);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<GuardianSummonLaser>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    SoundExtensions.PlaySoundOld(SoundID.Item33, position);
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
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.hide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.timeLeft = timeLeft;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
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
            Projectile.velocity.Normalize();
        }
    }
}