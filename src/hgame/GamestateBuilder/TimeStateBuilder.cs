using heng.Time;

namespace hgame
{
	public partial class GamestateBuilder
	{
		public class TimeStateBuilder
		{
			public TimeState Build(TimeState old)
			{
				return old?.Update(0) ?? new TimeState();
			}
		};
	};
}