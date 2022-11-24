using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class LunarHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a powerful Tide Spirit to fight for you" +
                "\nThis Tide Spirit will consume terror in order to attack enemies" +
                "\nIt won't be able to attack enemies if you don't have enough terror");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 5));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 34;
            Item.accessory = true;
            Item.noMelee = true;
            Item.expert = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.TideSpirit = true;

            bool TidalSpiritActive = true;
            for (int i = 0; i < 1000; i++)
            {
                Projectile Projectile = Main.projectile[i];
                if (Projectile.type == ModContent.ProjectileType<TideSpirit>() && Projectile.active)
                {
                    TidalSpiritActive = false;
                }
            }
            if (TidalSpiritActive)
            {
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<TideSpirit>(), 24, 0, player.whoAmI);
            }
        }
    }

    class TideSpirit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.width = 36;
            Projectile.height = 168 / 4;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 120;
            Projectile.timeLeft = 60;
        }

        void FindFrame(int FrameHeight)
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frame++;
                Projectile.frameCounter = 4;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }

        float direction = 0f;
        int mode = 0;
        int azuriteCounter = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer superiorPlayer = TerrorbornPlayer.modPlayer(player);
            if (!superiorPlayer.TideSpirit)
            {
                Projectile.active = false;
            }
            else
            {
                Projectile.timeLeft = 60;
            }

            if (Projectile.velocity.X > 0)
            {
                Projectile.spriteDirection = -1;
            }
            else
            {
                Projectile.spriteDirection = 1;
            }

            FindFrame(Projectile.height);

            bool Targeted = false;
            NPC target = Main.npc[0];

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

            if (!Projectile.CanHitWithOwnBody(player) || !Targeted || !Projectile.CanHitWithOwnBody(target))
            {
                mode = 0;
            }
            else
            {
                mode = 1;
            }

            if (mode == 0)
            {
                if (Projectile.Distance(player.Center) > 100)
                {
                    float speed = 1f;
                    Projectile.velocity += Projectile.DirectionTo(player.Center) * speed;
                    Projectile.velocity *= 0.96f;
                }
                if (Projectile.Distance(player.Center) > 5000)
                {
                    Projectile.position = player.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
                }
            }

            if (mode == 1)
            {
                if (Projectile.Distance(target.Center) > 100)
                {
                    float speed = 1f;
                    Projectile.velocity += Projectile.DirectionTo(target.Center) * speed;
                    Projectile.velocity *= 0.96f;
                }

                if (azuriteCounter > 0)
                {
                    azuriteCounter--;
                }
                else if (superiorPlayer.TerrorPercent >= 3f)
                {
                    superiorPlayer.LoseTerror(3f, false);
                    azuriteCounter = 40;
                    SoundExtensions.PlaySoundOld(SoundID.Item110, Projectile.Center);
                    for (int i = 0; i < Main.rand.Next(3, 5); i++)
                    {
                        float speed = Main.rand.Next(25, 40);
                        Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * speed;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Projectiles.AzuriteShard>(), Projectile.damage, 1, Projectile.owner);
                    }
                }
            }
        }
    }
}


