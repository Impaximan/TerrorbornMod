using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Summons.Minions
{
    class MysteriousSkull : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 12)
                .AddIngredient(ItemID.SoulofFright, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Summons a skull to assault your foes");
        }
        public override void SetDefaults()
        {
            Item.mana = 10;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 87;
            Item.width = 26;
            Item.height = 28;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<MysteriousSkullMinion>();
            Item.shootSpeed = 10f;
            Item.value = Item.sellPrice(0, 10, 0, 0);
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
                    player.AddBuff(ModContent.BuffType<MysteriousSkullBuff>(), 60);
                }
            }
            return false;
        }
    }

    class MysteriousSkullMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 28;
            Projectile.height = 352 / 8;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 360;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1;
        }

        int frame = 0;
        int frameCounter = 0;
        bool charging = false;
        void FindFrame(int FrameHeight)
        {
            frameCounter++;
            if (frameCounter > 4)
            {
                frameCounter = 0;
                frame++;
                if (frame >= 4)
                {
                    frame = 0;
                }
            }

            if (charging)
            {
                Projectile.frame = frame + 4;
            }
            else
            {
                Projectile.frame = frame;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (charging)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                BezierCurve bezier = new BezierCurve();
                bezier.Controls.Clear();
                foreach (Vector2 pos in Projectile.oldPos)
                {
                    if (pos != Vector2.Zero && pos != null)
                    {
                        bezier.Controls.Add(pos);
                    }
                }

                if (bezier.Controls.Count > 1)
                {
                    List<Vector2> positions = bezier.GetPoints(45);
                    for (int i = 0; i < positions.Count; i++)
                    {
                        float mult = (float)(positions.Count - i) / (float)positions.Count;
                        Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                        Color color = Projectile.GetAlpha(Color.Lerp(new Color(255, 194, 177), new Color(255, 194, 177), mult)) * mult;
                        Utils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
                    }
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            }
            return base.PreDraw(ref lightColor);
        }


        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 0, 25, 7, 6, DustScale: 1f, NoGravity: true);
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
        int chargeTime = 0;
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
            if (!player.HasBuff(ModContent.BuffType<MysteriousSkullBuff>()))
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
                if (Projectile.Distance(target.Center) > 200)
                {
                    float speed = 0.6f;
                    Projectile.velocity += Projectile.DirectionTo(target.Center) * speed;
                    Projectile.velocity *= 0.985f;
                    charging = false;
                }
                else
                {
                    charging = true;
                    if (chargeTime <= 0)
                    {
                        float speed = 15f;
                        Projectile.velocity = Projectile.DirectionTo(target.Center) * speed;
                        chargeTime = 20;
                    }
                    else
                    {
                        chargeTime--;
                    }
                }
            }
            else
            {
                charging = false;
            }

            Projectile.friendly = charging;
        }
    }


    class MysteriousSkullBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mysterious Skull");
            // Description.SetDefault("A strange skull shreds apart enemies at your will");
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
                if (Main.projectile[i].type == ModContent.ProjectileType<MysteriousSkullMinion>() && Main.projectile[i].active)
                {
                    player.buffTime[buffIndex] = 60;
                }
            }
        }
    }
}

