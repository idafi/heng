using System;
using System.Collections.Generic;

namespace heng.Video
{
	/// <summary>
	/// Represents an immutable snapshot of the video system's current state.
	/// <para>Each frame, new <see cref="Window"/> instances are constructed, drawn to,
	/// and used to build a new <see cref="VideoState"/>, representing that frame.</para>
	/// The engine will then automatically deal with any OS window management and hardware
	/// rendering necessary to properly reflect the constructed <see cref="VideoState"/>.
	/// </summary>
	public class VideoState
	{
		/// <summary>
		/// All <see cref="Window"/>s currently open.
		/// <para>A <see cref="Window"/>'s <see cref="Window.ID"/> is an index into this collection.</para>
		/// </summary>
		public readonly IReadOnlyList<Window> Windows;
		
		/// <summary>
		/// All sprites that were drawn to the open <see cref="Window"/>s.
		/// </summary>
		public readonly IReadOnlyList<Sprite> Sprites;

		/// <summary>
		/// Constructs a new snapshot of the video system's state.
		/// </summary>
		/// <param name="windows">The fully-constructed <see cref="Window"/>s to be shown this frame.</param>
		/// <param name="sprites">All <see cref="Sprite"/>s to be drawn.</param>
		public VideoState(IEnumerable<Window> windows, IEnumerable<Sprite> sprites)
		{
			Core.Video.GetSnapshot(out Core.Video.State coreState);

			List<Window> wnd = new List<Window>();
			Sprites = new List<Sprite>(sprites);

			foreach(Window w in windows)
			{
				if(w.ID > -1 && w.ID < Core.Video.Windows.Max)
				{
					wnd.Add(w);

					// open the window if it's not yet open
					if(coreState.Windows.WindowInfo[w.ID].ID < 0)
					{ Core.Video.Windows.OpenWindow(w.ID, w.Title, w.Rect, (UInt32)(w.WindowFlags), (UInt32)(w.RendererFlags)); }

					w.Clear(Color.White);

					foreach(Sprite spr in sprites)
					{ w.DrawSprite(spr); }

					Core.Video.Queue.Pump(w.ID);
				}
				else
				{ Log.Error($"can't use window with ID {w.ID}: ID is invalid"); }
			}

			Windows = wnd;
		}
	};
}