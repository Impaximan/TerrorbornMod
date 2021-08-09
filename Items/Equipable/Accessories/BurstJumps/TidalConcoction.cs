using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;

namespace TerrorbornMod.Items.Equipable.Accessories.BurstJumps
{
    class TidalConcoction : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TerrorbornUtils.GetBurstJumpString((int)(60 * 1.5f)) +
                "\nCreates azure geysers below you upon activation");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.AzuriteOre>(), 15);
            recipe.AddIngredient(ModContent.ItemType<Materials.CrackedShell>(), 2);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.damage = 60;
            item.rare = 2;
            item.defense = 5;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.useAnimation = 5;
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.burstJump = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornUtils.UpdateBurstJump((int)(60 * 1.5f), 60 * 1, item, player, new Vector2(20, -15), Color.Azure, SoundID.Item14);
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.BurstJumpTime > 0)
            {
                Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, 88)];
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }

            if (modPlayer.JustBurstJumped)
            {
                for (int i = 0; i < Main.rand.Next(2, 5); i++)
                {
                    Vector2 position = player.Center;
                    if (i != 1)
                    {
                        position.X += Main.rand.Next(-200, 200);
                    }
                    while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                        {
        new Conditions.IsSolid()
                        }), out _))
                    {
                        position.Y++;
                    }
                    int proj = Projectile.NewProjectile(position, new Vector2(0, -20), ModContent.ProjectileType<Items.Equipable.Armor.TideFireFriendly>(), item.damage, 0f, player.whoAmI);
                    Main.projectile[proj].melee = false;
                }
                Main.PlaySound(SoundID.Item88, player.Center);
            }
        }
    }
}

