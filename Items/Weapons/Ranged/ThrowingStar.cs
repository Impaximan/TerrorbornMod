using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.TBUtils;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class ThrowingStar : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe(75)
                .AddIngredient(ModContent.ItemType<Materials.NovagoldBar>())
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Implodes on impact with tiles, pulling enemies closer to itself");
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Ranged;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 0f;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 15;
            Item.shoot = ModContent.ProjectileType<ThrowingStarProjectile>();
            Item.width = 30;
            Item.height = 30;
        }
    }

    class ThrowingStarProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/ThrowingStar";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60 * 10;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            if (timeUntilFall > 0)
            {
                Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.LightSkyBlue) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                    Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(45f * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)), color * 0.5f);
                }
            }
            return true;
        }

        int timeUntilFall = 60 * 2;
        public override void AI()
        {
            int direction = 1;
            if (Projectile.velocity.X <= 0)
            {
                direction = -1;
            }
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length() * 2 * direction);

            if (timeUntilFall > 0)
            {
                timeUntilFall--;
            }
            else
            {
                Projectile.velocity.Y += 0.2f;
            }
        }

        public override void Kill(int timeLeft)
        {

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (timeUntilFall > 0)
            {
                bool actuallyHappened = false;
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                    Projectile.velocity.X = -oldVelocity.X * 0.5f;
                    actuallyHappened = true;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                    Projectile.velocity.Y = -oldVelocity.Y * 0.5f;
                    actuallyHappened = true;
                }

                if (actuallyHappened)
                {
                    Projectile.penetrate = 1;
                    if (Projectile.velocity.Y <= 1f && Projectile.velocity.Y >= -5f)
                    {
                        Projectile.velocity.Y = -5f;
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                    TerrorbornSystem.ScreenShake(1.5f);
                    timeUntilFall = 0;
                    DustExplosion(Projectile.Center, 20, 50f, 100f);
                    foreach (NPC NPC in Main.npc)
                    {
                        if (!NPC.dontTakeDamage && !NPC.friendly && NPC.active && NPC.knockBackResist != 0f && NPC.Distance(Projectile.Center) <= 300)
                        {
                            NPC.velocity = Projectile.DirectionFrom(NPC.Center) * 10f * NPC.knockBackResist;
                        }
                    }
                    return false;
                }
            }
            return true;
        }

        public void DustExplosion(Vector2 position, int dustAmount, float minDistance, float maxDistance)
        {
            Vector2 direction = new Vector2(0, 1);
            for (int i = 0; i < dustAmount; i++)
            {
                Vector2 newPos = position + direction.RotatedBy(MathHelper.ToRadians((360f / dustAmount) * i)) * Main.rand.NextFloat(minDistance, maxDistance);
                Dust dust = Dust.NewDustPerfect(newPos, 15);
                dust.scale = 1f;
                dust.velocity = (position - newPos) / 10;
                dust.noGravity = true;
            }
        }
    }
}
