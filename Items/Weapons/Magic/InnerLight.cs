using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using TerrorbornMod.TBUtils;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class InnerLight : ModItem
    {

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddIngredient(ItemID.Lens, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Causes enemies near your cursor to burst into light");
        }
        public override void SetDefaults()
        {
            item.damage = 51;
            item.noMelee = true;
            item.width = 52;
            item.height = 52;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<InnerLightProjectile>();
            item.shootSpeed = 25f;
            item.mana = 5;
            item.magic = true;
        }

        List<NPC> alreadyTargetted = new List<NPC>();
        int usesUntilReset = 0;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            usesUntilReset--;
            if (usesUntilReset >= 4)
            {
                usesUntilReset = 0;
                alreadyTargetted.Clear();
            }

            Vector2 velocity = new Vector2(speedX, speedY);

            float distance = 500;
            bool targetted = false;
            bool firstChoiceOnly = true;
            NPC target = Main.npc[0];
            NPC firstChoice = Main.npc[0];

            foreach (NPC npc in Main.npc)
            {
                if (!npc.friendly && npc.Distance(Main.MouseWorld) <= distance && npc.active && !npc.dontTakeDamage)
                {
                    if (alreadyTargetted.Contains(npc))
                    {
                        distance = npc.Distance(Main.MouseWorld);
                        targetted = true;
                        firstChoice = npc;
                    }
                    else
                    {
                        firstChoiceOnly = false;
                        targetted = true;
                        distance = npc.Distance(Main.MouseWorld);
                        target = npc;
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
                speedX = velocity.X;
                speedY = velocity.Y;
                TerrorbornMod.ScreenShake(2.5f);
                Main.PlaySound(SoundID.Item68, player.Center);
                return true;
            }


            alreadyTargetted.Clear();
            return false;
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
            projectile.magic = true;
            projectile.width = defaultSize;
            projectile.height = defaultSize;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.localNPCHitCooldown = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.timeLeft = timeLeft;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Graphics.DrawGlow_1(spriteBatch, projectile.Center - Main.screenPosition, currentSize, Color.LightYellow);
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

