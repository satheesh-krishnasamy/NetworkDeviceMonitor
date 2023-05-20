# NetworkDeviceMonitor
This app does the following by periodically monitoring the device. 
1. shows battery status
2. alerts on low battery < 15%
3. alerts when its 100% charged.

## Purpose
1. You can connect charger before the device goes off; this is helpful when you focus on work or meeting without worrying about device's battery status.
2. Avoid overcharging the battery once it reached 100%.


## Note
1. This app works only with JioFI device. This is tested only with Jio M2S wifi device and in Windows OS.
2. This app records the battery status in in-memory light-wight SQLite DB. This data will never be shared/accessed/uploaded outside of you machine. 

## Work in progress
The app only writes the battery status and charger connected information in this SQLite DB. The app will provide you insights [charge speed, usuall charging speed, battery health based on this charging speed, time to replace battery, etc,.] on this data in future.
