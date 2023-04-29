using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic.SpellBooks
{
    class Bombinomicon : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Fires a random explosive" +
                "\n[c/ff0000:These explosives CAN and WILL destroy tiles]" +
                "\n'The last person to use this lost his eye, which haunts him every October'"); */
        }
        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.width = 24;
            Item.height = 26;
            Item.DamageType = DamageClass.Magic; ;
            Item.damage = 60;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.mana = 20;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ProjectileID.Bomb;
            Item.shootSpeed = 10;
            Item.UseSound = SoundID.Item117;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 8;
            Item.autoReuse = true;
        }
        public override bool CanUseItem(Player player)
        {
            int choice = Main.rand.Next(3);
            int subtypeChoice = Main.rand.Next(2);
            if (choice == 0)
            {
                Item.shoot = ProjectileID.Grenade;
                if (Main.rand.NextBool(4))
                {
                    if (subtypeChoice == 0)
                    {
                        Item.shoot = ProjectileID.StickyGrenade;
                    }
                    else
                    {
                        Item.shoot = ProjectileID.BouncyGrenade;
                    }
                }
            }
            if (choice == 1)
            {
                Item.shoot = ProjectileID.Bomb;
                if (Main.rand.NextBool(4))
                {
                    if (subtypeChoice == 0)
                    {
                        Item.shoot = ProjectileID.StickyBomb;
                    }
                    else
                    {
                        Item.shoot = ProjectileID.BouncyBomb;
                    }
                }
            }
            if (choice == 2)
            {
                Item.shoot = ProjectileID.Dynamite;
                if (Main.rand.NextBool(4))
                {
                    if (subtypeChoice == 0)
                    {
                        Item.shoot = ProjectileID.StickyDynamite;
                    }
                    else
                    {
                        Item.shoot = ProjectileID.BouncyDynamite;
                    }
                }
            }
            return base.CanUseItem(player);
        }
    }
}
