using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Summons.Minions
{
    class OpticCane : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons an optic demon that swarms enemies and spews blood");
        }
        public override void SetDefaults()
        {
            Item.mana = 5;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 12;
            Item.width = 38;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item44;
            Item.shoot = mod.ProjectileType("OpticDemon");
            Item.shootSpeed = 10f;
            Item.value = Item.sellPrice(0, 0, 50, 0);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim(false);
            }
            return base.UseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].minion && player.slotsMinions > player.maxMinions)
                    {
                        Main.projectile[i].active = false;
                    }
                }
                Projectile.NewProjectile(source, new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, type, damage, knockback, player.whoAmI);
                if (player.slotsMinions <= player.maxMinions)
                {
                    player.AddBuff(ModContent.BuffType<OpticDemonBuff>(), 60);
                }
            }
            return false;
        }
    }

    class OpticDemon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 7;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 60;
            Projectile.height = 42;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 360;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.minion = true;
Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1;
        }
        void FindFrame(int FrameHeight)
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frame++;
                Projectile.frameCounter = 5;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 0, 25, 7, DustID.Blood, DustScale: 1f, NoGravity: true);
        }


        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, Color.Red, DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }

        int mode = 0;
        int bloodCounter = 10;
        public override void AI()
        {
            Projectile.timeLeft = 500;
            if (Projectile.velocity.X > 0)
            {
                Projectile.spriteDirection = -1;
            }
            else
            {
                Projectile.spriteDirection = 1;
            }

            FindFrame(Projectile.height);

            Player player = Main.player[Projectile.owner];
            if (!player.HasBuff(ModContent.BuffType<OpticDemonBuff>()))
            {
                Projectile.active = false;
            }

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

            if (player.HasMinionAttackTargetNPC)
            {
                target = Main.npc[player.MinionAttackTargetNPC];
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
                    float speed = 0.6f;
                    Projectile.velocity += Projectile.DirectionTo(player.Center) * speed;
                    Projectile.velocity *= 0.98f;
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
                    float speed = 0.6f;
                    Projectile.velocity += Projectile.DirectionTo(target.Center) * speed;
                    Projectile.velocity *= 0.985f;
                }

                bloodCounter--;
                if (bloodCounter <= 0)
                {
                    bloodCounter = 25;
                    float speed = 25;
                    Vector2 velocity = Projectile.DirectionTo(target.Center) * speed;
                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<OpticBlood>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
        }
    }
    public class OpticBlood : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.hostile = false;
            Projectile.hide = true;
            Projectile.timeLeft = 100;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }


        int tileCollideCounter = 25;
        public override void AI()
        {
            if (tileCollideCounter <= 0)
            {
                Projectile.tileCollide = true;
            }
            else
            {
                tileCollideCounter--;
            }

            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity;
        }
    }

    class OpticDemonBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Optic Demon");
            Description.SetDefault("An optic demon is fighting for you!");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<OpticDemon>() && Main.projectile[i].active)
                {
                    player.buffTime[buffIndex] = 60;
                }
            }
        }
    }
}
