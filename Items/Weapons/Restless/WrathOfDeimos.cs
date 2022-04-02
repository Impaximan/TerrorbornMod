using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
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
            Item.staff[Item.type] = true;
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
            Item.damage = 16;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;;
            Item.width = 74;
            Item.mana = 5;
            Item.height = 78;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ModContent.ProjectileType<DeimosFist>();
            Item.shootSpeed = 20;
            Item.crit = 7;
            Item.autoReuse = true;
            modItem.restlessTerrorDrain = 2.5f;
            modItem.restlessChargeUpUses = 5;
        }

        public override bool RestlessCanUseItem(Player player)
        {
            return base.RestlessCanUseItem(player);
        }

        public override bool RestlessShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            if (modItem.RestlessChargedUp())
            {
                damage = (int)(damage * 1.65f);
                int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                Main.projectile[proj].ai[0] = 1;
                Main.projectile[proj].penetrate = 5;
            }
            else
            {
                int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                Main.projectile[proj].ai[0] = 0;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 10)
                .AddIngredient(ModContent.ItemType<Materials.TerrorSample>(), 2)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }
    }

    class DeimosFist : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.width = 26;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            timeUntilReturn = 0;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            if (Projectile.ai[0] == 0)
            {
                return false;
            }
            return true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        int timeUntilReturn = 30;
        float speed = -1;
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] == 0)
            {
                Vector2 originPoint = Main.player[Projectile.owner].Center;
                Vector2 center = Projectile.Center;
                Vector2 distToProj = originPoint - Projectile.Center;
                float projRotation = distToProj.ToRotation() - 1.57f;
                float distance = distToProj.Length();
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Items/Weapons/Restless/DeimosChain");

                while (distance > texture.Height && !float.IsNaN(distance))
                {
                    distToProj.Normalize();
                    distToProj *= texture.Height;
                    center += distToProj;
                    distToProj = originPoint - center;
                    distance = distToProj.Length();


                    //Draw chain
                    Main.spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, texture.Width, texture.Height), Color.White, projRotation,
                        new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
                }
            }

            Texture2D texture2 = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture2, new Rectangle((int)position.X, (int)position.Y, texture2.Width, texture2.Height), new Rectangle(0, 0, texture2.Width, texture2.Height), Projectile.GetAlpha(Color.White), Projectile.rotation - MathHelper.ToRadians(90), new Vector2(texture2.Width / 2, texture2.Height / 2), SpriteEffects.None, 0);

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

            if (Projectile.ai[0] == 1)
            {
                if (modPlayer.deimosChained.Contains(target))
                {
                    foreach (NPC NPC in modPlayer.deimosChained)
                    {
                        NPC.StrikeNPC(Projectile.damage, 0f, 0, crit);
                    }
                }
            }

            if (Projectile.ai[0] == 0)
            {
                if (modPlayer.deimosChained.Count < 5 && !modPlayer.deimosChained.Contains(target))
                {
                    modPlayer.deimosChained.Add(target);
                }
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (speed == -1)
            {
                speed = Projectile.velocity.Length();
            }

            if (Projectile.ai[0] == 0)
            {
                if (timeUntilReturn > 0)
                {
                    timeUntilReturn--;
                }
                else
                {
                    Projectile.tileCollide = false;
                    Projectile.velocity = Projectile.DirectionTo(player.Center) * speed;
                    if (Projectile.Distance(player.Center) <= speed)
                    {
                        Projectile.active = false;
                    }
                }
                Projectile.rotation = Projectile.DirectionFrom(player.Center).ToRotation() + MathHelper.ToRadians(180);
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(180);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.AncientLight, Vector2.Zero);
                dust.noGravity = true;
            }
        }
    }
}

