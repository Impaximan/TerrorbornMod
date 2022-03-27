//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework.Audio;
//using Terraria;
//using Terraria.ModLoader;

//namespace TerrorbornMod.Sounds.Effects
//{
//	class CoolerMachineGun : ModSound
//	{
//		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
//		{
//			soundInstance.Volume = volume;
//			soundInstance.Pan = pan;
//			soundInstance.Pitch = Main.rand.NextFloat(-0.1f, 0.1f);
//			type = SoundType.Item;
//			return soundInstance;
//		}
//	}
//}