using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.critDamageMult = 1.3f;
            item.damage = 60;
            item.noMelee = true;
            item.width = 50;
            item.height = 50;
            item.useTime = 50;
            item.useAnimation = 50;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 4, 80, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item43;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<TerrorBeam>();
            item.shootSpeed = 5f;
            item.mana = 20;
            item.magic = true;
        }
        public override bool CanUseItem(Player player)
        {
            if (TerrorbornPlayer.modPlayer(player).TerrorPercent < 15f)
            {
                return false;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            damage = (int)(damage * (float)modPlayer.TerrorPercent / 100f);
            for (int i = 0; i < 8 + (int)(modPlayer.TerrorPercent / 100f * 8); i++)
            {
                Projectile.NewProjectile(position, new Vector2(speedX, speedY) * Main.rand.NextFloat(0.7f, 1.3f), type, damage, knockBack, player.whoAmI, Main.rand.NextFloat(-8f, 8f));
            }
            modPlayer.TerrorPercent = 0f;
            return false;
        }
    }
    class TerrorBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.penetrate = 10;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.hide = false;
            projectile.timeLeft = 1000;
        }

        int timeLeft = 260;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
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
                List<Vector2> positions = bezier.GetPoints(50);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                    Color color = projectile.GetAlpha(Color.Lerp(new Color(44, 65, 59), new Color(94, 142, 127), mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(25f * mult), color);
                }
            }
            return false;
        }

        public override void AI()
        {
            //int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 44, 0f, 0f, 100, Scale: 1.5f);
            //Main.dust[dust].noGravity = true;
            //Main.dust[dust].velocity = projectile.velocity;

            timeLeft--;
            if (timeLeft <= 0)
            {
                projectile.alpha += 15;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
            projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(projectile.ai[0]));
        }
    }
}
