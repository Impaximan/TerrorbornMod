using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Weapons.Summons.Minions
{
    class DreadSoulStaff : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.DreadfulEssence>(), 3)
                .AddIngredient(ItemID.LunarBar, 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DreadSoul Staff");
            Tooltip.SetDefault("Summons a dread soul that increases your max HP by 100" +
                "\nRequires 2 minion slots");
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 60;
            Item.mana = 10;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 19;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.rare = 12;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<DreadSoul>();
            Item.shootSpeed = 10f;
            Item.value = Item.sellPrice(0, 25, 0, 0);
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
                    if (Main.projectile[i].minion && player.slotsMinions > player.maxMinions || Main.projectile[i].type == type)
                    {
                        Main.projectile[i].active = false;
                    }
                }
                Projectile.NewProjectile(source, new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, type, damage, knockback, player.whoAmI);
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
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 2;
            Projectile.timeLeft = 360;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool PreDraw(ref Color lightColor)
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
                List<Vector2> positions = bezier.GetPoints(30);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.Goldenrod, Color.LightGoldenrodYellow, mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(30f * mult), color);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
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
        int fireCounter = 10;
        public override void AI()
        {
            Projectile.timeLeft = 500;

            Player player = Main.player[Projectile.owner];

            if (!player.HasBuff(ModContent.BuffType<DreadSoulBuff>()))
            {
                Projectile.active = false;
            }

            Projectile.position = player.Center - new Vector2(0, 100) - Projectile.Size / 2;
            Projectile.velocity = player.velocity;
        }
    }

    class DreadSoulBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dread Soul");
            Description.SetDefault("A dreadful soul increases your max life");
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
                if (Main.projectile[i].type == ModContent.ProjectileType<DreadSoul>())
                {
                    player.statLifeMax2 += 100;
                    player.buffTime[buffIndex] = 60;
                }
            }
        }
    }
}