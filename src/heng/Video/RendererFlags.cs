using System;

namespace heng.Video
{
	/// <summary>
	/// Configuration flags for engine windows' renderers.
	/// <para>This is just a direct translation of SDL2's video flags - don't mess with them.</para>
	/// </summary>
	[Flags]
	public enum RendererFlags : UInt32
	{
		/// <summary>
		/// The renderer is a software fallback.
		/// </summary>
		Software =		0x00000001,

		/// <summary>
		/// The renderer uses hardware acceleration.
		/// </summary>
		Accelerated =	0x00000002,

		/// <summary>
		/// Present is synchronized with the refresh rate.
		/// </summary>
		PresentVSync =	0x00000004,

		/// <summary>
		/// The renderer supports rendering to texture.
		/// </summary>
		TargetTexture =	0x00000008
	};
}