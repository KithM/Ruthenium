using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Version {

	static string version = "0.1.409";
	static string currentversion;

	public static string GetVersion(){
		return version;
	}
	public static string GetCurrentVersion(){
		return currentversion;
	}
	public static void SetCurrentVersion(string ver){
		currentversion = ver;
	}

}
