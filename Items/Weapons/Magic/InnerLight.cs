using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using TerrorbornMod.Utils;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class InnerLight : ModItem
    {

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddIngredient(ItemID.Lens, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Tooltip.SetDefault("Causes enemies near your cursor to burst into light");
        }
        public override void SetDefaults()
        {
            Item.damage = 51;
            Item.noMelee = true;
            Item.width = 52;
            Item.height = 52;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<InnerLightProjectile>();
            Item.shootSpeed = 25f;
            Item.mana = 5;
            Item.DamageType = DamageClass.Magic;;
        }

        bool targetted = false;
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            usesUntilReset--;
            if (usesUntilReset >= 4)
            {
                usesUntilReset = 0;
                alreadyTargetted.Clear();
            };

            float distance = 500;
            bool firstChoiceOnly = true;
            NPC target = Main.npc[0];
            NPC firstChoice = Main.npc[0];
            targetted = false;

            foreach (NPC NPC in Main.npc)
            {
                if (!NPC.friendly && NPC.Distance(Main.MouseWorld) <= distance && NPC.active && !NPC.dontTakeDamage)
                {
                    if (alreadyTargetted.Contains(NPC))
                    {
                        distance = NPC.Distance(Main.MouseWorld);
                        targetted = true;
                        firstChoice = NPC;
                    }
                    else
                    {
                        firstChoiceOnly = false;
                        targetted = true;
                        distance = NPC.Distance(Main.MouseWorld);
                        target = NPC;
                    }
                }
            }

            if (targetted)
            {
                if (firstChoiceOnly)
                {
                    position = firstChoice.Center;
                    alreadyTargetted.Clear();
                }
                else
                {
                    position = target.Center;
                    alreadyTargetted.Add(target);
                }
                velocity = player.DirectionTo(position) * velocity.Length();
                velocity.X = velocity.X;
                velocity.Y = velocity.Y;
                TerrorbornSystem.ScreenShake(2.5f);
                SoundExtensions.PlaySoundOld(SoundID.Item68, player.Center);
                alreadyTargetted.Clear();
            }
        }

        List<NPC> alreadyTargetted = new List<NPC>();
        int usesUntilReset = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (targetted)
            {
                TerrorbornSystem.ScreenShake(2.5f);
                SoundExtensions.PlaySoundOld(SoundID.Item68, player.Center);
            }
            return targetted;
        }
    }

    class InnerLightProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Glow_2";

        int timeLeft = 10;
        const int defaultSize = 300;
        int currentSize = defaultSize;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.width = defaultSize;
            Projectile.height = defaultSize;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = timeLeft;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, currentSize, Color.LightYellow);
            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Vector2 center = new Vector2(hitbox.X, hitbox.Y) + hitbox.Size() / 2;
            int hitboxSize = (int)(currentSize * 0.75f);
            hitbox.Width = hitboxSize;
            hitbox.Height = hitboxSize;
            hitbox.X = (int)center.X - hitboxSize / 2;
            hitbox.Y = (int)center.Y - hitboxSize / 2;
        }

        public override void AI()
        {
            currentSize -= defaultSize / timeLeft;
        }
    }
}

