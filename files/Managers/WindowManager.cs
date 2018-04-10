using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class WindowManager : MonoBehaviour {

	public static Manage m;
	public static Profile p;
	public static Users u;
	public static PermissionsManager pm;

	public static void CloseAllWindows(){
		m = FindObjectOfType<Manage> ();
		p = FindObjectOfType<Profile> ();
		u = FindObjectOfType<Users> ();
		pm = FindObjectOfType<PermissionsManager> ();

		if (m != null) {
			m.HideMenu ();
		}
		if (p != null) {
			p.HideProfile ();
		}
		if (u != null) {
			u.HideMenu ();
		}
		if (pm != null) {
			pm.HidePermissions ();
		}
	}

}
