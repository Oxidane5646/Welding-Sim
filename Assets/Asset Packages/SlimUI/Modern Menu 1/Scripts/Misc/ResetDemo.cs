using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asset_Packages.SlimUI.Modern_Menu_1.Scripts.Misc{
	public class ResetDemo : MonoBehaviour {

		void Update () {
			if(Input.GetKeyDown("r")){
				SceneManager.LoadScene(0);
			}
		}
	}
}