using System;
using System.Collections.Generic;

namespace heng.Video
{
	/// <summary>
	/// Represents an immutable snapshot of the video system's current state.
	/// <para>Since the snapshot is immutable, operations that mutate the video system
	/// won't have their effects represented in the <see cref="VideoState"/> instance that called them!</para>
	/// Instead, they will be represented the next time you construct a <see cref="VideoState"/> snapshot.
	/// </summary>
	public class VideoState
	{
		/// <summary>
		/// All <see cref="Window"/>s currently open.
		/// <para>The window's ID (<see cref="Window.ID"/>) is a key into this collection.</para>
		/// </summary>
		public readonly IReadOnlyDictionary<int, Window> Windows;
		
		/// <summary>
		/// Constructs a new snapshot of the video system's state.
		/// </summary>
		public VideoState()
		{
			Core.Video.GetSnapshot(out Core.Video.State coreState);

			Dictionary<int, Window> windows = new Dictionary<int, Window>();
			for(int i = 0; i < coreState.Windows.WindowInfo.Length; i++)
			{
				ref Core.Video.Windows.WindowInfo info = ref coreState.Windows.WindowInfo[i];
				if(info.ID > -1)
				{ windows.Add(info.ID, new Window(info)); }
			}

			Windows = windows;
		}
		
		/// <summary>
		/// Opens a new window.
		/// </summary>
		/// <param name="title">The title of the window.</param>
		/// <param name="rect">The dimensions of the window.</param>
		/// <param name="windowFlags">Configuration flags for the window.</param>
		/// <param name="rendererFlags">Configuration flags for the window's renderer.</param>
		/// <returns>
		/// The window's ID.
		/// <para>Remember: the new window will not be represented in this <see cref="VideoState"/>instance!</para>
		/// Wait until you're ready to get a new snapshot to start operating on the window.
		/// </returns>
		public int OpenWindow(string title, ScreenRect rect, WindowFlags windowFlags, RendererFlags rendererFlags)
		{
			if(title != null)
			{ return Core.Video.Windows.OpenWindow(title, rect, (UInt32)(windowFlags), (UInt32)(rendererFlags)); }
			
			Log.Error("couldn't open window: title is null");
			return -1;
		}
	};
}