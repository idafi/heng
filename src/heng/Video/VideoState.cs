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
		/// Constructs a new snapshot of the video system's state.
		/// </summary>
		/// <param name="windows">The fully-constructed <see cref="Window"/>s to be shown this frame.</param>
		public VideoState(IEnumerable<Window> windows)
		{
			Core.Video.GetSnapshot(out Core.Video.State coreState);

			List<Window> wnd = new List<Window>();

			foreach(Window w in windows)
			{
				if(w.ID > -1 && w.ID < Core.Video.Windows.Max)
				{
					wnd.Add(w);

					if(coreState.Windows.WindowInfo[w.ID].ID < 0)
					{ Core.Video.Windows.OpenWindow(w.Title, w.Rect, (UInt32)(w.WindowFlags), (UInt32)(w.RendererFlags)); }

					Core.Video.Windows.PresentWindow(w.ID);
				}
				else
				{ Log.Error($"can't use window with ID {w.ID}: ID is invalid"); }
			}

			Windows = wnd;
		}
	};
}