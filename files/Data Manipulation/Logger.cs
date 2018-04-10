using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public static class Logger {

	public static void WriteLog(string log){
		// TODO: Log user actions or something similar?

		string filePath = System.IO.Path.Combine ( SaveLoad.FileSaveBasePath(), "debug.log" );

		// At this point, filePath should look much like:
		// C:\Users\username\ApplicationData\Orion Games\Space Builder\Saves\SaveGame123.sav

		StreamWriter writer = new StreamWriter(filePath, true);
		writer.WriteLine(log);

		writer.Close();
	}

	public static void WriteSystemInfo(){
		string filePath = System.IO.Path.Combine ( SaveLoad.FileSaveBasePath(), "system.info" );

		StreamWriter writer = new StreamWriter(filePath, false);

		writer.WriteLine ("operatingSystem: " + SystemInfo.operatingSystem.ToString());
		writer.WriteLine ("operatingSystemFamily: " + SystemInfo.operatingSystemFamily);
		writer.WriteLine ("deviceUniqueIdentifier: " + SystemInfo.deviceUniqueIdentifier.ToString() + "\r\n");
		writer.WriteLine ("deviceName: " + SystemInfo.deviceName.ToString());
		writer.WriteLine ("deviceModel: " + SystemInfo.deviceModel.ToString());
		writer.WriteLine ("deviceType: " + SystemInfo.deviceType.ToString() + "\r\n");
		writer.WriteLine ("graphicsDeviceName: " + SystemInfo.graphicsDeviceName.ToString());
		writer.WriteLine ("graphicsDeviceType: " + SystemInfo.graphicsDeviceType.ToString());
		writer.WriteLine ("graphicsDeviceVendor: " + SystemInfo.graphicsDeviceVendor.ToString());
		writer.WriteLine ("graphicsDeviceID: " + SystemInfo.graphicsDeviceID);
		writer.WriteLine ("graphicsMemorySize: " + SystemInfo.graphicsMemorySize + " MB");
		writer.WriteLine ("graphicsMultiThreaded: " + SystemInfo.graphicsMultiThreaded);
		writer.WriteLine ("processorCount: " + SystemInfo.processorCount.ToString());

		writer.Close();
	}

}
