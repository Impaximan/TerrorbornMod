using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class Infectalanche : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Consumes all currently stored terror, requiring at least 15% to be used" +
                "\nThe more terror it consumes the more powerful it will be");
        }
        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.3f;
            Item.damage = 60;
            Item.noMelee = true;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 4, 80, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TerrorBeam>();
            Item.shootSpeed = 5f;
            Item.mana = 20;
            Item.DamageType = DamageClass.Magic;;
        }
        public override bool CanUseItem(Player player)
        {
            if (TerrorbornPlayer.modPlayer(player).TerrorPercent < 15f)
            {
                return false;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            damage = (int)(damage * (float)modPlayer.TerrorPercent / 100f);
            for (int i = 0; i < 8 + (int)(modPlayer.TerrorPercent / 100f * 8); i++)
            {
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y) * Main.rand.NextFloat(0.7f, 1.3f), type, damage, knockback, player.whoAmI, Main.rand.NextFloat(-8f, 8f));
            }
            modPlayer.TerrorPercent = 0f;
            return false;
        }
    }
    class TerrorBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override string Texture => "TerrorbornMod/placeholder";
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.hide = false;
            Projectile.timeLeft = 1000;
        }

        int timeLeft = 260;
        public override bool PreDraw(ref Color lightColor)
        {
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
                List<Vector2> positions = bezier.GetPoints(50);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.Lerp(new Color(44, 65, 59), new Color(94, 142, 127), mult)) * mult;
                    Utils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
                }
            }
            return false;
        }

        public override void AI()
        {
            //int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 44, 0f, 0f, 100, Scale: 1.5f);
            //Main.dust[dust].noGravity = true;
            //Main.dust[dust].velocity = Projectile.velocity;

            timeLeft--;
            if (timeLeft <= 0)
            {
                Projectile.alpha += 15;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
            }
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
        }
    }
}
