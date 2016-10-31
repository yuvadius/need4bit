using UnityEngine;
using System.Collections;

/// <summary>
/// This script will is in charge of deciding when and by how much to pump
/// (increase or decrease) the globe
/// </summary>
public class GlobePump : MonoBehaviour {

	public GlobeSize globe;

	[Tooltip("The ratio of segments to surface size")]
	public float ratio = 3.3f;
	[Tooltip("Each how much time to check for pumping")]
	public float loopTime = 10f;
	[Tooltip("By how much to pump each time")]
	public float pumpAmount = 10f;
	[Tooltip("By how much off we need to be to trigger the pump, meaing that if we require 81 surface but only have 80 currently then nothing will change")]
	public float omnesty = 15;

	/// <summary>
	/// This number needs to hold the amount of segments in existance on the globe by all players
	/// </summary>
	public int amountOfSegments = 0;

	IEnumerator Start() {
		while(true) {
			yield return new WaitForSeconds(loopTime);

			amountOfSegments = 0;
			foreach(var player in PhotonNetwork.playerList) {
				amountOfSegments += player.GetScore();
			}

			if(PhotonNetwork.isMasterClient) {
				if(amountOfSegments * ratio > GlobeSize.instance.surface + omnesty) {
					globe.destinationSurface = Mathf.Clamp(
						globe.destinationSurface + pumpAmount,
						globe.destinationSurface,
						globe.maxSurface
					);
				} else if(amountOfSegments * ratio < GlobeSize.instance.surface - omnesty) {
					globe.destinationSurface = Mathf.Clamp(
						globe.destinationSurface - pumpAmount,
						globe.minSurface,
						globe.destinationSurface
					);
				}
			}
		}
	}
	
}
