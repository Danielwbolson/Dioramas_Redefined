using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explore : MonoBehaviour
{
	public GameObject BuildingImage;
	public GameObject StepsImage;
	public GameObject PathImage;

	public Button buttonBuilding;
	public Button buttonSteps;
	public Button buttonPath;
	
	public Button buttonBuildingX;
	public Button buttonStepsX;
	public Button buttonPathX;
	
	void Start() {
		
		//buttonFort.gameObject.SetActive(true);
		//buttonBuilding.gameObject.SetActive(true);
		//buttonSteps.gameObject.SetActive(true);
		//buttonPath.gameObject.SetActive(true);
		
		buttonBuilding.onClick.AddListener(() => {
			BuildingImage.SetActive(true);
			//buttonBuildingX.gameObject.SetActive(true);
		});
		
		buttonSteps.onClick.AddListener(() => {
			StepsImage.SetActive(true);
			//buttonStepsX.gameObject.SetActive(true);
		});
		
		buttonPath.onClick.AddListener(() => {
			PathImage.SetActive(true);
			//buttonPathX.gameObject.SetActive(true);
		});
		
		buttonBuildingX.onClick.AddListener(() => {
			BuildingImage.SetActive(false);
			//buttonBuildingX.gameObject.SetActive(false);
		});
		
		buttonStepsX.onClick.AddListener(() => {
			StepsImage.SetActive(false);
			//buttonStepsX.gameObject.SetActive(false);
		});
		
		buttonPathX.onClick.AddListener(() => {
			PathImage.SetActive(false);
			//buttonPathX.gameObject.SetActive(false);
		});

		BuildingImage.SetActive(false);
		////buttonBuildingX.gameObject.SetActive(false);
		StepsImage.SetActive(false);
		////buttonStepsX.gameObject.SetActive(false);
		PathImage.SetActive(false);
		////buttonPathX.gameObject.SetActive(false);
	}
	
}
