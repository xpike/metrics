# metrics
Supports observability  best-practices

# TODO:
- Metrics Tag Context
  - Implemented like Request Context
  - Should support "global tags" (also like Request Context)
- HTTP package
  - HTTP Tax Context Provider
  - HTTP Controller Wrapper for timings?
- Config Enhancements
  - Default tags (which feed into Tag Context Global Tags)
  - Metric name prefix
- Utilize metrics from DataStores/Drivers/Caching
  - For Drivers, it'll be indirect, via http client middleware
- Base Engine that simplifies using Logging and Metrics
  - Just so that engines aren't a special case - Drivers/DataStores/Caching will do it for you
  - (unrelated project, but inspired by / depends on Metrics)
- OperationTracker
	- Should be configurable
		- Ability to enable/disable recording of Count and/or Result metrics 
		  since DataDog tracks a count for timers, and since tagging will have the result