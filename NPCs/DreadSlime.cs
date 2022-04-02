using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs
{
    class DreadSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BlueSlime);
            NPC.width = 64;
            NPC.height = 52;
            NPC.damage = 75;
            NPC.defense = 22;
            NPC.lifeMax = 3500;
            NPC.value *= 50;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            NPC.color = Color.White;
            animationType = NPCID.BlueSlime;
        }

        public override void NPCLoot()
        {
            Item.NewItem(NPC.getRect(), ItemID.Gel, Main.rand.Next(7, 15));
            Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.DarkEnergy>());

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
            if (modPlayer.DeimosteelCharm)
            {
                if (Main.rand.NextFloat() <= 0.7f)
                {
                    Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                }
            }
            else
            {
                if (Main.rand.NextFloat() <= 0.35f)
                {
                    Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!TerrorbornSystem.obtainedShriekOfHorror)
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