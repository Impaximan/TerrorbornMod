using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class HorrificCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Spawns a horrific eye above you that cries damaging tears when enemies are nearby");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.DamageType = DamageClass.Summon;
            Item.rare = ItemRarityID.Blue;
            Item.damage = 10;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.HorrificCharm = true;

            bool AntlionShellShieldActive = true;
            for (int i = 0; i < 1000; i++)
            {
                Projectile Projectile = Main.projectile[i];
                if (Projectile.type == ModContent.ProjectileType<HorrificEye>() && Projectile.active)
                {
                    AntlionShellShieldActive = false;
                }
            }
            if (AntlionShellShieldActive)
            {
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<HorrificEye>(), Item.damage, 0, player.whoAmI);
            }
        }
    }

    class HorrificEye : ModProjectile
    {
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            position.Y += 4;
            Main.spriteBatch.Draw(texture, position, new Rectangle(0, 0, Projectile.width, Projectile.height), Color.White, Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.width = 52;
            Projectile.height = 30;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 120;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        int scaleDirection = 1;
        int ProjectileCounter = 60;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer superiorPlayer = TerrorbornPlayer.modPlayer(player);
            if (!superiorPlayer.HorrificCharm)
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

            Projectile.position = player.Center + new Vector2(0, -55);
            Projectile.position.X -= Projectile.width / 2;
            Projectile.position.Y -= Projectile.height / 2;

            if (ProjectileCounter > 0)
            {
                ProjectileCounter--;
            }

            NPC targetNPC = Main.npc[0];
            float Distance = 1000; //max distance away
            bool Targeted = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(Projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy() && Projectile.CanHitWithOwnBody(Main.npc[i]))
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(Projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                if (ProjectileCounter <= 0)
                {
                    SoundExtensions.PlaySoundOld(SoundID.Item8, Projectile.Center);
                    ProjectileCounter = 40;
                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.Next(5, 8);
                    Vector2 velocity = direction * speed;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<HorrificTear>(), Projectile.damage, 1f, Projectile.owner);
                }
            }

            //pulse
            if (scaleDirection == 1)
            {
                Projectile.scale += 0.3f / 60;
                if (Projectile.scale >= 1.1f)
                {
                    scaleDirection = -1;
                }
            }
            else
            {
                Projectile.scale -= 0.3f / 60;
                if (Projectile.scale <= 0.9f)
                {
                    scaleDirection = 1;
                }
            }
        }
    }

    class HorrificTear : ModProjectile
    {
        public override bool? CanHitNPC(NPC target)
        {
            return target.type != NPCID.TruffleWorm && !target.friendly;
        }

        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/MagicGuns/TarSwarm";
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.timeLeft = 180;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, Scale: 1.35f, newColor: Color.White);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity / 3;
            Main.dust[dust].noLight = true;

            Projectile.velocity.Y += 0.15f;
        }

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
                Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
            }
            return false;
        }
    }
}


