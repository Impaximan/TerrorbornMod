using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Summons.Minions
{
    class FrigidStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Summons a frigid soul to fight for you that has a chance to inflict frostburn");
        }

        public override void SetDefaults()
        {
            Item.mana = 5;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 7;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<FrigidSoul>();
            Item.shootSpeed = 10f;
            Item.value = Item.sellPrice(0, 0, 20, 0);
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
                    player.AddBuff(ModContent.BuffType<FrigidSoulBuff>(), 60);
                }
            }
            return false;
        }
    }

    class FrigidSoul : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 52;
            Projectile.height = 52;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        void FindFrame(int FrameHeight)
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frame++;
                Projectile.frameCounter = 10;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 0, 25, 7, DustID.Ice, DustScale: 1f, NoGravity: true);
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

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextFloat() <= 0.2f)
            {
                target.AddBuff(BuffID.Frostburn, 60 * 3);
            }
        }

        int mode = 0;
        int rotationDirection = 0;
        int dashCounter = 30;
        public override void AI()
        {
            Projectile.timeLeft = 60;
            if (Projectile.velocity.X > 0)
            {
                rotationDirection = 1;
            }
            else
            {
                rotationDirection = -1;
            }

            FindFrame(Projectile.height);

            Player player = Main.player[Projectile.owner];
            if (!player.HasBuff(ModContent.BuffType<FrigidSoulBuff>()))
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
                if (Projectile.Distance(player.Center) > 80)
                {
                    float speed = 0.4f;
                    Projectile.velocity += Projectile.DirectionTo(player.Center) * speed;
                    Projectile.velocity *= 0.98f;
                }
                if (Projectile.Distance(player.Center) > 5000)
                {
                    Projectile.position = player.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
                }
                Projectile.friendly = false;
                Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length()) * rotationDirection;
            }

            if (mode == 1)
            {
                Projectile.friendly = true;
                float speed = 20f;
                dashCounter--;
                if (dashCounter <= 0)
                {
                    dashCounter = 30;
                    Projectile.velocity = Projectile.DirectionTo(target.Center) * speed;
                }

                speed = 0.2f;
                Projectile.velocity += Projectile.DirectionTo(target.Center) * speed;

                Projectile.velocity *= 0.96f;
                Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length()) * rotationDirection;
            }
        }
    }

    class FrigidSoulBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Frigid Soul");
            // Description.SetDefault("A spirit of the cold follows to ensure your safety");
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
                if (Main.projectile[i].type == ModContent.ProjectileType<FrigidSoul>() && Main.projectile[i].active)
                {
                    player.buffTime[buffIndex] = 60;
                }
            }
        }
    }
}

