using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Summons.Other
{
    class Armagrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("A grenade that releases little mothrons upon exploding" +
                "\nThese mothrons take up no summon slots but disappear after a short amount of time" +
                "\nYour minions will target enemies hit by the main grenade"); */
        }
        public override void SetDefaults()
        {
            Item.damage = 36;
            Item.DamageType = DamageClass.Summon;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 65);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item106;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 22;
            Item.mana = 10;
            Item.shoot = ModContent.ProjectileType<ArmagrenadeProj>();
        }
    }

    class ArmagrenadeProj : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Summons/Other/Armagrenade";
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage *= 0.75f;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 16;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);
            Projectile.velocity.Y += 0.18f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
        }
        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.NPCDeath46, Projectile.Center);
            SoundExtensions.PlaySoundOld(SoundID.Item14, Projectile.Center);
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                float Speed = Main.rand.Next(7, 10);
                Vector2 ProjectileSpeed = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * Speed;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, ProjectileSpeed, ModContent.ProjectileType<LittleMothron>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner);
            }
        }
    }
    class LittleMothron : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 16;
            Projectile.tileCollide = true;
            Projectile.friendly = false;
            Projectile.penetrate = 4;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 1000;
        }

        void FindFrame(int FrameHeight)
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frame++;
                Projectile.frameCounter = 3;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }

        int timeUntilDeadly = 60;
        int trueTimeLeft = 300;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            timeUntilDeadly = 45;
        }

        public override void AI()
        {
            if (trueTimeLeft > 0)
            {
                trueTimeLeft--;
            }
            else
            {
                Projectile.alpha += 255 / 60;
                if (Projectile.alpha >= 255)
                {
                    Projectile.timeLeft = 0;
                }
                timeUntilDeadly = 30;
            }

            FindFrame(Projectile.height);
            if (Projectile.velocity.X > 0)
            {
                Projectile.spriteDirection = 1;
            }
            else
            {
                Projectile.spriteDirection = -1;
            }

            if (timeUntilDeadly > 0)
            {
                Projectile.velocity *= 0.98f;
                Projectile.friendly = false;
                timeUntilDeadly--;
            }
            else
            {
                Projectile.friendly = true;
                NPC targetNPC = Main.npc[0];
                float Distance = 1000; //max distance away
                bool Targeted = false;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].Distance(Projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy() && Projectile.CanHitWithOwnBody(Main.npc[i]) && Projectile.localNPCImmunity[i] == 0)
                    {
                        targetNPC = Main.npc[i];
                        Distance = Main.npc[i].Distance(Projectile.Center);
                        Targeted = true;
                    }
                }
                if (Targeted)
                {
                    //HOME IN
                    float speed = .6f;
                    Vector2 direction = Projectile.DirectionTo(targetNPC.Center);
                    Projectile.velocity += speed * direction;
                    Projectile.velocity *= 0.96f;
                }
            }
        }
    }
}

