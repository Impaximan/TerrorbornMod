using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class WrathOfDeimos : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/WrathOfDeimos";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {
            DisplayName.SetDefault("Wrath of Deimos");
            Item.staff[item.type] = true;
        }

        public override string defaultTooltip()
        {
            return "Fires a fist that chains up to 5 enemies together";
        }

        public override string altTooltip()
        {
            return "Hitting one chained enemy will damage all chained enemies, hitting the intial enemy twice" +
                "\nIncreased damage";
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            item.damage = 16;
            item.noMelee = true;
            item.magic = true;
            item.width = 74;
            item.mana = 5;
            item.height = 78;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item8;
            item.shoot = ModContent.ProjectileType<DeimosFist>();
            item.shootSpeed = 20;
            item.crit = 7;
            item.autoReuse = true;
            modItem.restlessTerrorDrain = 2.5f;
            modItem.restlessChargeUpUses = 5;
        }

        public override bool RestlessCanUseItem(Player player)
        {
            return base.RestlessCanUseItem(player);
        }

        public override bool RestlessShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            if (modItem.RestlessChargedUp())
            {
                damage = (int)(damage * 1.65f);
                int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
                Main.projectile[proj].ai[0] = 1;
                Main.projectile[proj].penetrate = 5;
            }
            else
            {
                int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
                Main.projectile[proj].ai[0] = 0;
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Materials.TerrorSample>(), 2);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class DeimosFist : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.magic = true;
            projectile.width = 26;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            timeUntilReturn = 0;
            Main.PlaySound(SoundID.Dig, projectile.position);
            if (projectile.ai[0] == 0)
            {
                return false;
            }
            return true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 10;
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        int timeUntilReturn = 30;
        float speed = -1;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.ai[0] == 0)
            {
                Vector2 originPoint = Main.player[projectile.owner].Center;
                Vector2 center = projectile.Center;
                Vector2 distToProj = originPoint - projectile.Center;
                float projRotation = distToProj.ToRotation() - 1.57f;
                float distance = distToProj.Length();
                Texture2D texture = ModContent.GetTexture("TerrorbornMod/Items/Weapons/Restless/DeimosChain");

                while (distance > texture.Height && !float.IsNaN(distance))
                {
                    distToProj.Normalize();
                    distToProj *= texture.Height;
                    center += distToProj;
                    distToProj = originPoint - center;
                    distance = distToProj.Length();


                    //Draw chain
                    spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, texture.Width, texture.Height), Color.White, projRotation,
                        new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
                }
            }

            Texture2D texture2 = Main.projectileTexture[projectile.type];
            Vector2 position = projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture2, new Rectangle((int)position.X, (int)position.Y, texture2.Width, texture2.Height), new Rectangle(0, 0, texture2.Width, texture2.Height), projectile.GetAlpha(Color.White), projectile.rotation - MathHelper.ToRadians(90), new Vector2(texture2.Width / 2, texture2.Height / 2), SpriteEffects.None, 0);

            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (target.type == NPCID.TargetDummy)
            {
                return;
            }

            if (projectile.ai[0] == 1)
            {
                if (modPlayer.deimosChained.Contains(target))
                {
                    foreach (NPC npc in modPlayer.deimosChained)
                    {
                        npc.StrikeNPC(projectile.damage, 0f, 0, crit);
                    }
                }
            }

            if (projectile.ai[0] == 0)
            {
                if (modPlayer.deimosChained.Count < 5 && !modPlayer.deimosChained.Contains(target))
                {
                    modPlayer.deimosChained.Add(target);
                }
            }
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (speed == -1)
            {
                speed = projectile.velocity.Length();
            }

            if (projectile.ai[0] == 0)
            {
                if (timeUntilReturn > 0)
                {
                    timeUntilReturn--;
                }
                else
                {
                    projectile.tileCollide = false;
                    projectile.velocity = projectile.DirectionTo(player.Center) * speed;
                    if (projectile.Distance(player.Center) <= speed)
                    {
                        projectile.active = false;
                    }
                }
                projectile.rotation = projectile.DirectionFrom(player.Center).ToRotation() + MathHelper.ToRadians(180);
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(180);
                Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.AncientLight, Vector2.Zero);
                dust.noGravity = true;
            }
        }
    }
}

