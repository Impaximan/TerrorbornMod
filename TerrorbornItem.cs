using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Terraria.UI;
using TerrorbornMod;
using Terraria.Map;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;

namespace TerrorbornMod
{
    class TerrorbornItem : GlobalItem
    {
        public static bool[] NecromancerItem;

        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;


        int azureCounter = 2;
        public override bool CanUseItem(Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            bool darkblood = player.HasBuff(ModContent.BuffType<Buffs.Darkblood>());
            if (player.HasBuff(ModContent.BuffType<Buffs.GraniteSpark>()) || (modPlayer.MidShriek && !darkblood) || !modPlayer.canUseItems)
            {
                return false;
            }
            if (modPlayer.AzuriteArmorBonus && item.magic && base.CanUseItem(item, player) && player.statMana >= item.mana)
            {
                if (azureCounter <= 0)
                {
                    azureCounter = 2;
                    float speed = 22.5f;
                    Vector2 velocity = player.DirectionTo(Main.MouseWorld) * speed;

                    Projectile.NewProjectile(player.Center, velocity, ModContent.ProjectileType<Items.Equipable.Armor.azuriteShockwave>(), item.damage / 3, 30, player.whoAmI);
                }
                else
                {
                    azureCounter--;
                }
            }
            return base.CanUseItem(item, player);
        }
        public override bool UseItem(Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return base.UseItem(item, player);
        }

        public override void SetDefaults(Item item)
        {
        }

        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (crit && modPlayer.SangoonBand && target.type != NPCID.TargetDummy)
            {
                if (modPlayer.SangoonBandCooldown <= 0)
                {
                    player.HealEffect(1);
                    player.statLife += 1;
                    modPlayer.SangoonBandCooldown = 20;
                }
            }
        }
    }
}
