using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerrorbornMod.TBUtils;

namespace TerrorbornMod.Items.Equipable.Accessories.Shields
{
    [AutoloadEquip(EquipType.Shield)]
    class Aegis : ModItem
    {
        int cooldown = 10 * 60;
        float knockback = 10f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TBUtils.Accessories.GetParryShieldString(cooldown, knockback) + "\nParrying attacks will cause a holy burst of light that deals 1000 damage to nearby enemies");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.Pink;
            item.defense = 8;
            item.knockBack = knockback;
            item.value = Item.sellPrice(0, 3, 0, 0);
            TerrorbornItem.modItem(item).parryShield = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.parryColor = Color.LightGoldenrodYellow;
            if (modPlayer.JustParried)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<AegisLight>(), 1000, 0f, player.whoAmI);
                TerrorbornMod.ScreenShake(25f);
                Main.PlaySound(SoundID.Item68, player.Center);
            }
            TBUtils.Accessories.UpdateParryShield(cooldown, item, player);
        }
    }

    class AegisLight : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Glow_2";

        int timeLeft = 10;
        const int defaultSize = 1000;
        int currentSize = defaultSize;
        public override void SetDefaults()
        {
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

        public override void AI()
        {
            currentSize -= defaultSize / timeLeft;
        }
    }
}
