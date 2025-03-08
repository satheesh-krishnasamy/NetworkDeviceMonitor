select 
	SessionId,
	((julianday(max(EventDateTime)) - julianday(min(EventDateTime))) * 24 * 60) as DurationInMins, 
	min(EventDateTime) as Started, 
	max(EventDateTime) as Ended,
	Min(BatteryPercentage) as StartPercentage,
	max(BatteryPercentage) as EndPercentage,
	(max(BatteryPercentage) - Min(BatteryPercentage)) ChargedPercentage,
	 (((julianday(max(EventDateTime)) - julianday(min(EventDateTime))) * 24 * 60) / (max(BatteryPercentage) - Min(BatteryPercentage))) AverageTimeInMinsPerPercentage
from
BatteryData
where 
--EventDateTime >= $StartTime and EventDateTime <= $EndTime 
EventDateTime >= date('2025-03-06') and IsCharging = true and SessionId <> ''
group by SessionId
-----------------


select 
		SessionId,
		min(EventDateTime) as Started, 
		max(EventDateTime) as Ended,
		Min(BatteryPercentage) as StartPercentage,
		max(BatteryPercentage) as EndPercentage
	from
	BatteryData
	where EventDateTime >= date('2025-03-08')
	--and EventDateTime <= $EndTime 
	and IsCharging = true and SessionId <> ''
	group by SessionId;