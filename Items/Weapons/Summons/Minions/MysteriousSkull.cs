using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Weapons.Summons.Minions
{
    class MysteriousSkull : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 12);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a skull to assault your foes");
        }
        public override void SetDefaults()
        {
            item.mana = 10;
            item.summon = true;
            item.damage = 87;
            item.width = 26;
            item.height = 28;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 0;
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item44;
            item.shoot = ModContent.ProjectileType<MysteriousSkullMinion>();
            item.shootSpeed = 10f;
            item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
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
                Projectile.NewProjectile(new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, type, damage, knockBack, item.owner);
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
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            Main.projFrames[projectile.type] = 8;
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            projectile.penetrate = -1;
            projectile.width = 28;
            projectile.height = 352 / 8;
            projectile.tileCollide = false;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 360;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.minion = true;
            projectile.minionSlots = 1;
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
                projectile.frame = frame + 4;
            }
            else
            {
                projectile.frame = frame;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (charging)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                BezierCurve bezier = new BezierCurve();
                bezier.Controls.Clear();
                foreach (Vector2 pos in projectile.oldPos)
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
                        Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                        Color color = projectile.GetAlpha(Color.Lerp(new Color(255, 194, 177), new Color(255, 194, 177), mult)) * mult;
                        TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(25f * mult), color);
                    }
                }

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            }
            return base.PreDraw(spriteBatch, lightColor);
        }


        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 25, 7, 6, DustScale: 1f, NoGravity: true);
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
            projectile.timeLeft = 500;
            if (projectile.velocity.X > 0)
            {
                projectile.spriteDirection = -1;
            }
            else
            {
                projectile.spriteDirection = 1;
            }

            FindFrame(projectile.height);

            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(ModContent.BuffType<MysteriousSkullBuff>()))
            {
                projectile.active = false;
            }

            bool Targeted = false;
            NPC target = Main.npc[0];

            float Distance = 1000;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    target = Main.npc[i];
                    Distance = Main.npc[i].Distance(projectile.Center);
                    Targeted = true;
                }
            }

            if (player.HasMinionAttackTargetNPC)
            {
                target = Main.npc[player.MinionAttackTargetNPC];
            }

            if (!projectile.CanHit(player) || !Targeted || !projectile.CanHit(target))
            {
                mode = 0;
            }
            else
            {
                mode = 1;
            }

            if (mode == 0)
            {
                if (projectile.Distance(player.Center) > 100)
                {
                    float speed = 0.6f;
                    projectile.velocity += projectile.DirectionTo(player.Center) * speed;
                    projectile.velocity *= 0.98f;
                }
                if (projectile.Distance(player.Center) > 5000)
                {
                    projectile.position = player.Center - new Vector2(projectile.width / 2, projectile.height / 2);
                }
            }

            if (mode == 1)
            {
                if (projectile.Distance(target.Center) > 200)
                {
                    float speed = 0.6f;
                    projectile.velocity += projectile.DirectionTo(target.Center) * speed;
                    projectile.velocity *= 0.985f;
                    charging = false;
                }
                else
                {
                    charging = true;
                    if (chargeTime <= 0)
                    {
                        float speed = 15f;
                        projectile.velocity = projectile.DirectionTo(target.Center) * speed;
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

            projectile.friendly = charging;
        }
    }


    class MysteriousSkullBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Mysterious Skull");
            Description.SetDefault("A strange skull shreds apart enemies at your will");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            longerExpertDebuff = false;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<MysteriousSkullMinion>())
                {
                    player.buffTime[buffIndex] = 60;
                }
            }
        }
    }
}

