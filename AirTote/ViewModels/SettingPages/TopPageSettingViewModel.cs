using CommunityToolkit.Mvvm.ComponentModel;
using AirTote.Services;

namespace AirTote.ViewModels.SettingPages;

public partial class TopPageSettingViewModel : ObservableObject
{
	static readonly TimeSpan IntervalDefaultValue = new(0, 0, 2);

	static List<TimeSpan> _IntervalList { get; } = new List<TimeSpan>
	{
		new(0, 0, 0, 0, 100),
		new(0, 0, 0, 0, 200),
		new(0, 0, 0, 0, 500),
		new(0, 0, 1),
		new(0, 0, 2),
		new(0, 0, 5),
		new(0, 0, 10),
		new(0, 0, 30),
		new(0, 1, 0),
	};

	public static System.Collections.IList IntervalList => _IntervalList;
	public static System.Collections.IList IntervalStrList => _IntervalList.Select(v => v.ToString(@"m\分s\.f\秒")).ToList();

	public TopPageSettingViewModel()
		=> LoadValues();

	public void LoadValues()
	{
		PreferenceManager.TryGet<bool>(PreferenceManager.Keys.TopPage_EnableLocationService, ref _IsLocationEnabled);
		PreferenceManager.TryGet<bool>(PreferenceManager.Keys.TopPage_EnableLocationFollowAnimation, ref _IsLocationFollowAnimationEnabled);
		if (PreferenceManager.TryGet<long>(PreferenceManager.Keys.TopPage_LocationRefleshInterval, ref _LocationRefleshInterval_ms))
			_LocationRefleshInterval = TimeSpan.FromMilliseconds(LocationRefleshInterval_ms);

		LocationRefleshIntervalIndex = _IntervalList.IndexOf(LocationRefleshInterval);
	}

	[ObservableProperty]
	private bool _IsLocationEnabled = true;
	[ObservableProperty]
	private bool _IsLocationFollowAnimationEnabled = true;
	[ObservableProperty]
	private TimeSpan _LocationRefleshInterval = IntervalDefaultValue;
	[ObservableProperty]
	private int _LocationRefleshIntervalIndex;

	[ObservableProperty]
	private long _LocationRefleshInterval_ms = (long)IntervalDefaultValue.TotalMilliseconds;

	partial void OnLocationRefleshIntervalIndexChanged(int value)
		=> LocationRefleshInterval = _IntervalList[value];
	partial void OnLocationRefleshIntervalChanged(TimeSpan value)
		=> LocationRefleshInterval_ms = (long)value.TotalMilliseconds;

	partial void OnIsLocationEnabledChanged(bool value)
		=> PreferenceManager.Set(PreferenceManager.Keys.TopPage_EnableLocationService, value);
	partial void OnIsLocationFollowAnimationEnabledChanged(bool value)
		=> PreferenceManager.Set(PreferenceManager.Keys.TopPage_EnableLocationFollowAnimation, value);
	partial void OnLocationRefleshInterval_msChanged(long value)
		=> PreferenceManager.Set(PreferenceManager.Keys.TopPage_LocationRefleshInterval, value);
}
