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
		// ����ؾ��ϴ� Ÿ�� ������ �޾ƿͼ� ����
		currentTower = tower.GetComponent<Tower>();
		// Ÿ�� ���� Panel On
		gameObject.SetActive(true);
		
		
	}

	public void OffPanel()
	{
		// Ÿ�� ���� Panel Off
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
		// Ÿ�� �Ǹ�
		currentTower.Sell();
		// ������ Ÿ���� ������� Panel, ���ݹ��� Off
		OffPanel();
	}
}

