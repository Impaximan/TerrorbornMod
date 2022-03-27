using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class DNMG : ModItem
    {
        int baseReuseDelay = 17;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("D.N.M.G");
            Tooltip.SetDefault("Fires faster the longer you use it" +
                "\nUpon reaching max speed it will fire a mini nuke that does not destroy tiles and reset its speed");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.critDamageMult = 1.3f;
            item.damage = 115;
            item.ranged = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.width = 112;
            item.height = 46;
            item.useTime = 4;
            item.useAnimation = 4;
            item.knockBack = 1f;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Effects/CoolerMachineGun");
            item.shoot = ProjectileID.PurificationPowder;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.sellPrice(0, 25, 0, 0);
            item.rare = 12;
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Bullet;
            item.scale = 1f;
            item.channel = true;
            item.reuseDelay = baseReuseDelay;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, -2);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.DreadfulEssence>(), 3);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateInventory(Player player)
        {
            if (!player.channel)
            {
                item.reuseDelay = baseReuseDelay;
            }
        }

        bool nextShotIsNuke = false;
        public override bool CanUseItem(Player player)
        {
            TerrorbornMod.ScreenShake(0.5f);
            item.reuseDelay -= 2;
            if (item.reuseDelay <= 0)
            {
                item.reuseDelay = baseReuseDelay;
                nextShotIsNuke = true;
                TerrorbornMod.ScreenShake(1.5f);
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (nextShotIsNuke)
            {
                nextShotIsNuke = false;
                Main.PlaySound(SoundID.Item61, player.Center);
                Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<MiniNuke>(), damage * 2, knockBack * 5f, player.whoAmI);
            }
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }

    class MiniNuke : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 34;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 50, 25, DustID.GoldFlame, DustScale: 1.5f, NoGravity: true);
            Main.PlaySound(SoundID.Item14, projectile.Center);
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.friendly && projectile.Distance(npc.Center) <= 200 + ((npc.width + npc.height) / 2) && !npc.dontTakeDamage)
                {
                    npc.StrikeNPC(projectile.damage, 0, 0, Main.rand.Next(101) <= Main.player[projectile.owner].rangedCrit);
                    npc.AddBuff(BuffID.Daybreak, 60 * 10);
                }
            }
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

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction * Main.rand.NextFloat(0.8f, 1.2f), 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }

        public override void AI()
        {
            if (projectile.velocity.X <= 0)
            {
                projectile.spriteDirection = -1;
            }
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(90);
            NPC targetNPC = Main.npc[0];
            float Distance = 500; //max distance away
            bool Targeted = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                //HOME IN
                float speed = 1.2f;
                projectile.velocity += projectile.DirectionTo(targetNPC.Center) * speed;
                projectile.velocity *= 0.95f;
            }
            Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.GoldFlame, Vector2.Zero, 0, Scale: 1f);
            dust.noGravity = true;
        }

        int bouncesLeft = 3;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
            }

            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(SoundID.Dig, projectile.position);
            bouncesLeft--;
            if (bouncesLeft <= 0)
            {
                projectile.timeLeft = 0;
            }
            return false;
        }
    }
}
