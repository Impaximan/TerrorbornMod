using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using TerrorbornMod.TBUtils;
using Terraria.DataStructures;
using TerrorbornMod.Items.Materials;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class PyroclasticGemStaff : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 12)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ModContent.ItemType<PyroclasticGemstone>(), 12)
                .AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>())
                .Register();
        }
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Tooltip.SetDefault("Fires a pyroclastic bolt that splits at the mouse cursor");
        }
        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.35f;
            Item.damage = 22;
            Item.noMelee = true;
            Item.width = 56;
            Item.height = 52;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item91;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PyroclasticBolt>();
            Item.shootSpeed = 20f;
            Item.mana = 8;
            Item.DamageType = DamageClass.Magic;;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 50);
            return true;
        }
    }

    class PyroclasticBolt : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Glow_2";


        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] != 0 || bursted)
            {
                Projectile.active = false;
            }
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
                List<Vector2> positions = bezier.GetPoints(45);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.Lerp(new Color(255, 194, 177), new Color(255, 194, 177), mult)) * mult;
                    Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        Vector2 target;
        bool start = true;
        bool bursted = false;
        public override void AI()
        {
            if (start)
            {
                start = false;
                target = Main.MouseWorld;
                if (Main.player[Projectile.owner].Distance(Main.MouseWorld) <= 50 && Projectile.ai[0] == 0)
                {
                    bursted = true;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item110, Projectile.Center);
                    for (int i = 0; i < Main.rand.Next(3, 5); i++)
                    {
                        int proj = Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.8f, 1.2f), Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
                        Main.projectile[proj].ai[0] = 1;
                    }
                }
            }

            if (Projectile.ai[0] == 0 && Projectile.Distance(target) <= Projectile.velocity.Length() && !bursted)
            {
                bursted = true;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item110, Projectile.Center);
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.8f, 1.2f), Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Main.projectile[proj].ai[0] = 1;
                }
            }
        }
    }
}
