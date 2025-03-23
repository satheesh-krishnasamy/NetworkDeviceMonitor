# Workstation and network device battery level monitor
This app does the following by periodically monitoring the devices (laptop and jio network device). 
1. Shows battery level and charging status
2. Alerts on low battery level < 20%
3. Warns to disconect charger when the battery level is >= 80
4. Alerts when its 100% charged.
5. Shows the battery draining/charging trend as a chart.
6. Shows the no.of time the battery has charged on each day for the past 7 days.

## Purpose
1. You can connect charger before the device goes off; this is helpful when you focus on work or meeting without worrying about device's battery status.
2. Avoid overcharging the battery once it reached 100%.


## Note
1. This app works only with JioFI device. This is tested only with Jio M2S wifi device and in Windows OS.
2. This app records the battery status in in-memory light-wight SQLite DB. This data will never be shared/accessed/uploaded outside of you machine. 

## Work in progress
The app only writes the battery status and charger connected information in this SQLite DB. The app will provide you insights [charge speed, usuall charging speed, battery health based on this charging speed, draining speed, time to replace battery, etc,.] on this data in future.
