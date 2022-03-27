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
    class DreadSoulStaff : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.DreadfulEssence>(), 3);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DreadSoul Staff");
            Tooltip.SetDefault("Summons a dread soul that increases your max HP by 100" +
                "\nRequires 2 minion slots");
        }

        public override void SetDefaults()
        {
            item.width = 64;
            item.height = 60;
            item.mana = 10;
            item.summon = true;
            item.damage = 19;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 0;
            item.rare = 12;
            item.UseSound = SoundID.Item44;
            item.shoot = ModContent.ProjectileType<DreadSoul>();
            item.shootSpeed = 10f;
            item.value = Item.sellPrice(0, 25, 0, 0);
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
                    if (Main.projectile[i].minion && player.slotsMinions > player.maxMinions || Main.projectile[i].type == type)
                    {
                        Main.projectile[i].active = false;
                    }
                }
                Projectile.NewProjectile(new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, type, damage, knockBack, item.owner);
                if (player.slotsMinions <= player.maxMinions)
                {
                    player.AddBuff(ModContent.BuffType<DreadSoulBuff>(), 60);
                }
            }
            return false;
        }
    }

    class DreadSoul : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/TarSwarm";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void SetDefaults()
        {
            projectile.penetrate = -1;
            projectile.width = 30;
            projectile.height = 30;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.ignoreWater = true;
            projectile.minionSlots = 2;
            projectile.timeLeft = 360;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
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
                List<Vector2> positions = bezier.GetPoints(30);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                    Color color = projectile.GetAlpha(Color.Lerp(Color.Goldenrod, Color.LightGoldenrodYellow, mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(30f * mult), color);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 25, 7, DustID.Fire, DustScale: 1f, NoGravity: true);
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
        int fireCounter = 10;
        public override void AI()
        {
            projectile.timeLeft = 500;

            Player player = Main.player[projectile.owner];

            if (!player.HasBuff(ModContent.BuffType<DreadSoulBuff>()))
            {
                projectile.active = false;
            }

            projectile.position = player.Center - new Vector2(0, 100) - projectile.Size / 2;
            projectile.velocity = player.velocity;
        }
    }

    class DreadSoulBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Dread Soul");
            Description.SetDefault("A dreadful soul increases your max life");
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
                if (Main.projectile[i].type == ModContent.ProjectileType<DreadSoul>())
                {
                    player.statLifeMax2 += 100;
                    player.buffTime[buffIndex] = 60;
                }
            }
        }
    }
}