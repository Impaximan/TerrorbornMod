using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class TidalShellChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Tidal shell chestplate");
            // Tooltip.SetDefault("Melee damage increased by 4%");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) *= 1.04f;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class TidalShellLegwear : ModItem
    {
        bool WasInAir = false;
        int TilInAir = 20;
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Melee damage increased by 4%");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 12;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) *= 1.04f;
        }
        
    }

    [AutoloadEquip(EquipType.Head)]
    public class TidalShellHeadplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Melee damage increased by 4%" +
                "\nMelee crit chance increased by 5%"); */
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TidalShellChestplate>() && legs.type == ModContent.ItemType<TidalShellLegwear>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Hitting enemies with melee weapon has a chance to cause geysers to erupt from the" +
                "\nground beneath them" +
                "\nKilling an enemy guarentees this effect";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.TidalShellArmorBonus = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) *= 1.04f;
        }
    }
    class TideFireFriendly : ModProjectile
    {
        public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hide = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 30;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 88, 0, 0, Scale: 2, newColor: Color.SkyBlue);
            Main.dust[dust].noGravity = true;
        }
    }
}
