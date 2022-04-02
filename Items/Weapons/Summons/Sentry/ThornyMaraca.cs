using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Summons.Sentry
{
    class ThornyMaraca : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a thorny companion to help you fight." +
                "\nSummoning it despawns ALL other sentries but its damage scales with sentry slots.");
        }
        public override void SetDefaults()
        {
            Item.mana = 10;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 22;
            Item.width = 38;
            Item.sentry = true;
            Item.height = 42;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<MaractusSentry>();
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
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].sentry)
                    {
                        Main.projectile[i].active = false;
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
    class PinMissle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 2;
            
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 10;
            Projectile.height = 12;
            Projectile.penetrate = 5;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.HasBuff(BuffID.Oiled))
            {
                damage = (int)(damage * 1.3f);
            }
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }
    }
    class MaractusSentry : ModProjectile
    {
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
            Main.projFrames[Projectile.type] = 12;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true; //This is necessary for right-click targeting
        }
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 66;
            Projectile.height = 66;
            Projectile.friendly = true;
            Projectile.sentry = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }
        void FindFrame(int FrameHeight)
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frame++;
                Projectile.frameCounter = 6;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }
        int PinWait = 60;
        int PinRoundsLeft = 2;
        public override void AI()
        {
            Vector2 position = new Vector2(Projectile.Center.X, Projectile.position.Y);
            while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
        new Conditions.IsSolid()
                }), out _))
            {
                position.Y++;
            }
            position.Y -= Projectile.height - 4;
            Projectile.position = new Vector2(position.X - Projectile.width / 2, position.Y);
            FindFrame(Projectile.height);
            Projectile.timeLeft = 10;
            bool Targeted = false;
            //Projectile.velocity.Y = 50;
            //Projectile.velocity.X = 0;
            Player player = Main.player[Projectile.owner];
            NPC target = Main.npc[0];
            if (player.HasMinionAttackTargetNPC && Main.npc[player.MinionAttackTargetNPC].Distance(Projectile.Center) < 1500)
            {
                target = Main.npc[player.MinionAttackTargetNPC];
                Targeted = true;
            }
            else
            {
                float Distance = 1500;
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

                PinWait--;
                if (PinWait <= 0)
                {
                    if (PinRoundsLeft > 0)
                    {
                        PinRoundsLeft--;
                        PinWait = 10;
                    }
                    else
                    {
                        PinWait = 100;
                        PinRoundsLeft = Main.rand.Next(1, 5);
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, Projectile.Center);
                    Vector2 Rotation = Projectile.DirectionTo(target.Center);
                    float Speed = 20;
                    Vector2 Velocity = Rotation * Speed;
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Velocity, ModContent.ProjectileType<PinMissle>(), Projectile.damage * player.maxTurrets, Projectile.knockBack, Projectile.owner);
                }
            }
        }
    }
}
