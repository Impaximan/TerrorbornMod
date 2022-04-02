using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.TBUtils;

namespace TerrorbornMod.WeaponPossession
{
    class PossessedProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile Projectile)
        {

        }

        public override void OnHitNPC(Projectile Projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (originalItem != null)
            {
                if (!originalItem.IsAir)
                {
                    PossessedItem pItem = PossessedItem.modItem(originalItem);
                    if (pItem.possessType == PossessType.Fright)
                    {
                        TerrorbornPlayer.modPlayer(Main.player[Projectile.owner]).terrorDrainCounter = 30;
                    }

                    if (pItem.possessType == PossessType.Light && crit)
                    {
                        int proj = Projectile.NewProjectile(Projectile.GetProjectileSource_OnHit(target, Projectile.whoAmI), target.Center, Vector2.Zero, ModContent.ProjectileType<Lightsplosion>(), damage / 2, 0, Projectile.owner);
                        Main.projectile[proj].ai[0] = target.whoAmI;
                        TerrorbornSystem.ScreenShake(5f);
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item68, target.Center);
                    }

                    if (pItem.possessType == PossessType.Night && crit)
                    {
                        Main.player[Projectile.owner].HealEffect(1);
                        Main.player[Projectile.owner].statLife++;
                    }
                }
            }
        }

        Item originalItem = null;
        bool start = true;
        public override bool PreAI(Projectile Projectile)
        {
            if (Projectile.owner == 255 || !Projectile.friendly || Projectile.damage == 0)
            {
                return base.PreAI(Projectile);
            }
            if (Main.player[Projectile.owner].HeldItem.IsAir)
            {
                return base.PreAI(Projectile);
            }
            if (start)
            {
                start = false;
                originalItem = null;
                if (Main.player[Projectile.owner] != null && Main.player[Projectile.owner].HeldItem != null && Projectile.friendly && !Projectile.hostile && !Main.gameMenu)
                {
                    Player player = Main.player[Projectile.owner];
                    TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
                    if (player.HeldItem == null)
                    {
                        return base.PreAI(Projectile);
                    }
                    Item item = player.HeldItem;
                    if (PossessedItem.modItem(item) == null)
                    {
                        return base.PreAI(Projectile);
                    }
                    originalItem = item;
                    PossessedItem pItem = PossessedItem.modItem(item);

                    if (pItem.possessType == PossessType.Might)
                    {
                        Projectile.extraUpdates = Projectile.extraUpdates * 2 + 1;
                    }

                    if (pItem.possessType == PossessType.Flight)
                    {
                        Projectile.tileCollide = false;
                        Projectile.velocity *= 0.75f;
                    }

                    if (pItem.possessType == PossessType.Sight)
                    {
                        Projectile.velocity *= 0.65f;
                    }
                }
            }
            return base.PreAI(Projectile);
        }

        public override void AI(Projectile Projectile)
        {
            base.AI(Projectile);

            if (Projectile.owner == 255 || !Projectile.friendly || Projectile.damage == 0)
            {
                return;
            }

            if (Main.player[Projectile.owner].HeldItem.IsAir)
            {
                return;
            }

            if (Main.player[Projectile.owner] != null && Projectile.friendly && !Projectile.hostile && !Main.gameMenu && !Projectile.minion && !Projectile.sentry && originalItem != null && Main.player[Projectile.owner].HeldItem != null)
            {
                Player player = Main.player[Projectile.owner];
                TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
                if (originalItem.IsAir)
                {
                    return;
                }
                Item item = originalItem;
                PossessedItem pItem = PossessedItem.modItem(item);

                if (pItem.possessType == PossessType.Flight)
                {
                    Projectile.tileCollide = false;
                    if (Projectile.Distance(player.Center) >= Main.screenWidth * 1.5f)
                    {
                        Projectile.timeLeft = 0;
                    }
                }

                if (pItem.possessType == PossessType.Sight)
                {
                    NPC targetNPC = Main.npc[0];
                    float Distance = 750; //max distance away
                    bool Targeted = false;
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].Distance(Projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                        {
                            targetNPC = Main.npc[i];
                            Distance = Main.npc[i].Distance(Projectile.Center);
                            Targeted = true;
                        }
                    }
                    if (Targeted)
                    {
                        //HOME IN
                        Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(1f * (Projectile.velocity.Length() / 20))).ToRotationVector2() * Projectile.velocity.Length();
                    }
                }
            }
        }
    }

    class Lightsplosion : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Glow_2";

        int timeLeft = 10;
        const int defaultSize = 500;
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

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == (int)Projectile.ai[0])
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, currentSize, new Color(255, 212, 255));
            return base.PreDraw(ref lightColor);
        }

        public override void AI()
        {
            currentSize -= defaultSize / timeLeft;
        }
    }
}
