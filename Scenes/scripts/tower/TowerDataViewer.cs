using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDataViewer : MonoBehaviour
{
	
	
	//private SystemTextViewer systemTextViewer;

	private Tower currentTower;

	private void Awake()
	{
		OffPanel();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			OffPanel();
		}
	}

	public void OnPanel(Transform tower)
	{
		// 출력해야하는 타워 정보를 받아와서 저장
		currentTower = tower.GetComponent<Tower>();
		// 타워 정보 Panel On
		gameObject.SetActive(true);
		
		
	}

	public void OffPanel()
	{
		// 타워 정보 Panel Off
		gameObject.SetActive(false);
		
	}

	private void UpdateTowerData()
	{
		

		
	}

	public void OnClickEventTowerUpgrade()
	{
		
		
	}

	public void OnClickEventTowerSell()
	{
		// 타워 판매
		currentTower.Sell();
		// 선택한 타워가 사라져서 Panel, 공격범위 Off
		OffPanel();
	}
}

