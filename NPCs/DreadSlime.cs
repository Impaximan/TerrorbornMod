using System.IO;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.NPCs
{
    class DreadSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BlueSlime);
            npc.width = 64;
            npc.height = 52;
            npc.damage = 75;
            npc.defense = 22;
            npc.lifeMax = 3500;
            npc.value *= 50;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.color = Color.White;
            animationType = NPCID.BlueSlime;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Gel, Main.rand.Next(7, 15));
            Item.NewItem(npc.getRect(), ModContent.ItemType<Items.DarkEnergy>());

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
            if (modPlayer.DeimosteelCharm)
            {
                if (Main.rand.NextFloat() <= 0.7f)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                }
            }
            else
            {
                if (Main.rand.NextFloat() <= 0.35f)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!TerrorbornWorld.obtainedShriekOfHorror)
            {
                return 0f;
            }
            if (TerrorbornPlayer.modPlayer(spawnInfo.player).ZoneIncendiary)
            {
                return 0f;
            }
            if (!NPC.downedMoonlord)
            {
                return 0f;
            }
            return SpawnCondition.OverworldDaySlime.Chance * 0.5f;
        }
    }
}