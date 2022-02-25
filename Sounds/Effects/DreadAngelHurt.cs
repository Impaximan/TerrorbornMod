using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Sounds.Effects
{
    class DreadAngelHurt : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
		{
			//if (soundInstance.State == SoundState.Playing)
			//{
			//	return null;
			//}

			soundInstance.Volume = volume;
			soundInstance.Pan = pan;
			soundInstance.Pitch = Main.rand.NextFloat(-0.1f, 0.1f);
			return soundInstance;
		}
	}
}
