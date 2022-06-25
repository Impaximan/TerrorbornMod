using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AzuriteHelmet : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.AzuriteBar>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases max mana by 10" +
                "\n4% increased magic damage" +
                "\n5% increased magic crit");
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 10;
            player.GetDamage(DamageClass.Magic) *= 1.04f;
            player.GetCritChance(DamageClass.Magic) += 5;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 6;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AzuriteChestplate>() && legs.type == ModContent.ItemType<AzuriteLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Every 3rd use of a magic weapon will release a short range aquatic shockwave." +
                            "\nThis can pierce an unlimited amount of enemies and has high knockback, but only" +
                            "\nhas one third of the weapon's base damage." +
                            "\nHitting two or more enemies with this shockwave will give you back 5 mana.";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.AzuriteArmorBonus = true;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class AzuriteChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases max mana by 10" +
                "\n5% increased magic damage" +
                "\nMagic weapons cast faster" +
                "\n2% increased magic crit");
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 10;
            player.GetDamage(DamageClass.Magic) *= 1.07f;
            player.GetCritChance(DamageClass.Magic) += 2;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetAttackSpeed(DamageClass.Magic) += 0.2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.AzuriteBar>(15)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 25;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 7;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class AzuriteLeggings : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Materials.AzuriteBar>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        bool WasInAir = false;
        int TilInAir = 20;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases max mana by 15" +
                "\n3% increased magic damage" +
                "\n2% increased magic crit");
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 15;
            player.GetDamage(DamageClass.Magic) *= 1.03f;
            player.GetCritChance(DamageClass.Magic) += 2;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 12;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }
    }
    
    class azuriteShockwave : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Melee/AzuriteSlash";

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = 300;
        }

        int enemiesHit = 0;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            enemiesHit++;
            if (enemiesHit >= 2)
            {
                Main.player[Projectile.owner].statMana += 5;
                Main.player[Projectile.owner].ManaEffect(5);
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 2;
        }

        int trueTimeLeft = 30;
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.ToRadians(90);
            Projectile.velocity *= 0.95f;
            if (trueTimeLeft <= 0)
            {
                Projectile.alpha += 15;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
            }
            else
            {
                trueTimeLeft--;
            }
        }
        //public override bool PreDraw(ref Color lightColor)
        //{
        //    //Thanks to Seraph for afterimage code.
        //    Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
        //    for (int i = 0; i < Projectile.oldPos.Length; i++)
        //    {
        //        Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
        //        Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
        //        spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
        //    }
        //    return false;
        //}
    }
}