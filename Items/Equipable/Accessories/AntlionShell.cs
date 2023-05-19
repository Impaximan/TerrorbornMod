using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class AntlionShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Generates a shield that rotates around you" +
                "\nThis shield can block a Projectile once every 5 seconds" +
                "\nAdditionally, the shield will hit enemies it passes through dealing 50 damage" +
                "\nBlocking a Projectile or hitting an enemy with the shield grants you the 'panic' buff for 5 seconds");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 34;
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.AntlionShell = true;

            bool AntlionShellShieldActive = true;
            for (int i = 0; i < 1000; i++)
            {
                Projectile Projectile = Main.projectile[i];
                if (Projectile.type == ModContent.ProjectileType<AntlionShellShield>() && Projectile.active)
                {
                    AntlionShellShieldActive = false;
                }
            }
            if (AntlionShellShieldActive)
            {
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<AntlionShellShield>(), 50, 0, player.whoAmI);
            }
        }
    }
    
    class AntlionShellShield : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.width = 20;
            Projectile.height = 38;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 120;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            player.AddBuff(BuffID.Panic, 60 * 5);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return target.type != NPCID.TruffleWorm && !target.friendly;
        }

        float direction = 0f;
        int cooldown = 60 * 5;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer superiorPlayer = TerrorbornPlayer.modPlayer(player);
            if (!superiorPlayer.AntlionShell)
            {
                Projectile.active = false;
            }
            else
            {
                Projectile.timeLeft = 60;
            }
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 5;
            }
            Projectile.velocity = Vector2.Zero;

            int distance = 100;
            Projectile.position = direction.ToRotationVector2() * distance + player.Center;
            Projectile.position.X -= Projectile.width / 2;
            Projectile.position.Y -= Projectile.height / 2;
            Projectile.rotation = direction + MathHelper.ToRadians(180);

            direction += MathHelper.ToRadians(1f);

            if (cooldown <= 0)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Projectile target = Main.projectile[i];
                    if (target.hostile && target.getRect().Intersects(Projectile.getRect()))
                    {
                        target.penetrate = 0;
                        player.AddBuff(BuffID.Panic, 60 * 5);
                        CombatText.NewText(Projectile.getRect(), Color.LightYellow, "Projectile blocked", true, true);
                        cooldown = 60 * 5;
                        SoundExtensions.PlaySoundOld(SoundID.DD2_SkeletonHurt);
                    }
                }
            }
            else
            {
                cooldown--;
                Projectile.alpha = 125;
            }
        }
    }
}

