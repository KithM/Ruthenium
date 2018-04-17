using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class Fonts {

	public static string defaultFont;
	public static int defaultFontSize;

	public static string defaultBoldFont;
	public static int defaultBoldFontSize;

	public static string defaultExtraBoldFont;
	public static int defaultExtraBoldFontSize;

	public static string defaultSystemFont;
	public static int defaultSystemFontSize;

	public static Font GetDefaultFont(){
		string[] systemFonts = Font.GetOSInstalledFontNames ();
		string font = defaultFont;
		Font f = Font.CreateDynamicFontFromOSFont (font, defaultFontSize); // 14

		return f;
	}
	public static Font GetDefaultBoldFont(){
		string[] systemFonts = Font.GetOSInstalledFontNames ();
		string font = defaultBoldFont;
		Font f = Font.CreateDynamicFontFromOSFont (font, defaultBoldFontSize); // 16

		return f;
	}
	public static Font GetDefaultExtraBoldFont(){
		string[] systemFonts = Font.GetOSInstalledFontNames ();
		string font = defaultExtraBoldFont;
		Font f = Font.CreateDynamicFontFromOSFont (font, defaultExtraBoldFontSize); // 16

		return f;
	}
	public static Font GetSystemFont(){
		string[] systemFonts = Font.GetOSInstalledFontNames ();
		string font = defaultSystemFont;
		Font f = Font.CreateDynamicFontFromOSFont (font, defaultSystemFontSize); // 12

		return f;
	}

	public static void LoadFonts(){
		string filePath = System.IO.Path.Combine (SaveLoad.FontsBasePath (), "fonts.ruth");

		if( Directory.Exists(SaveLoad.FontsBasePath() ) == false){
			// NOTE: This can throw an exception if we can't
			// create the folder. But why would this ever happen?
			// We should by definition have the ability to write
			// to our persistent data folder unless something is
			// really broken with the PC / device
			Directory.CreateDirectory (SaveLoad.FontsBasePath());
		}

		if (File.Exists (filePath) == false) {
			// The file doesn't exist, so make a new one
			Debug.LogError ("Fonts::LoadFonts: File doesn't exist! Creating one now.");

			// Create a new file
			StreamWriter writer = new StreamWriter (filePath, true);
			writer.WriteLine ("Roboto Bold");
			writer.WriteLine ("14");
			writer.WriteLine ("Roboto Bold");
			writer.WriteLine ("16");
			writer.WriteLine ("Roboto Bold");
			writer.WriteLine ("16");
			writer.WriteLine ("Roboto");
			writer.WriteLine ("12");
			writer.Close ();
		}

		StreamReader reader = new StreamReader (filePath);
		defaultFont = reader.ReadLine ();
		defaultFontSize = int.Parse(reader.ReadLine ());

		defaultBoldFont = reader.ReadLine ();
		defaultBoldFontSize = int.Parse(reader.ReadLine ());

		defaultExtraBoldFont = reader.ReadLine ();
		defaultExtraBoldFontSize = int.Parse(reader.ReadLine ());

		defaultSystemFont = reader.ReadLine ();
		defaultSystemFontSize = int.Parse(reader.ReadLine ());

		Debug.Log (defaultFont + ", " + defaultBoldFont + ", " + defaultExtraBoldFont + ", " + defaultSystemFont);
		reader.Close ();
	}
}
