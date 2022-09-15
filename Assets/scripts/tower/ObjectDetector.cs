using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
	[SerializeField]
	private TowerSpawner towerSpawner;
	[SerializeField]
	private TowerDataViewer towerDataViewer;

	private Camera mainCamera;
	private Ray ray;
	private RaycastHit hit;
	private Transform hitTransform = null;          // ���콺 ��ŷ���� ������ ������Ʈ �ӽ� ����
	private Transform previousHitTransform = null;  // ���콺�� ������ �ӹ����� Ÿ�� ���� �����

	private void Awake()
	{
		// "MainCamera" �±׸� ������ �ִ� ������Ʈ Ž�� �� Camera ������Ʈ ���� ����
		// GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); �� ����
		mainCamera = Camera.main;
	}

	private void Update()
	{
		// ���콺�� UI�� �ӹ��� ���� ���� �Ʒ� �ڵ尡 ������� �ʵ��� ��
		if (EventSystem.current.IsPointerOverGameObject() == true)
		{
			return;
		}
			

		// ���콺 ���� ��ư�� ������ ��
		if (Input.GetMouseButtonDown(0))
		{
			// ī�޶� ��ġ���� ȭ���� ���콺 ��ġ�� �����ϴ� ���� ����
			// ray.origin : ������ ������ġ(=ī�޶� ��ġ)
			// ray.direction : ������ �������
			ray = mainCamera.ScreenPointToRay(Input.mousePosition);

			// 2D ����͸� ���� 3D ������ ������Ʈ�� ���콺�� �����ϴ� ���
			// ������ �ε����� ������Ʈ�� �����ؼ� hit�� ����
			if (Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
				hitTransform = hit.transform;

				/*
				// ������ �ε��� ������Ʈ�� �±װ� "Tile"�̸�
				if (hit.transform.CompareTag("Tile"))
				{
					Debug.Log(hit.transform);
					// Ÿ���� �����ϴ� SpawnTower() ȣ��
					towerSpawner.SpawnTower(hit.transform);
					// Ÿ���� ������ ���� �������� ����
				//	hit.transform.GetComponent<Tile>().OnColorReset();
				}
				// Ÿ���� �����ϸ� �ش� Ÿ�� ������ ����ϴ� Ÿ�� ����â On
				
				if (hit.transform.CompareTag("Tower"))
				{
					Debug.Log(hit.transform.position);
					towerDataViewer.OnPanel(hit.transform);
				}
				*/
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			// ���콺�� ������ �� ������ ������Ʈ�� ���ų� ������ ������Ʈ�� Ÿ���� �ƴϸ�
			if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
			{
				// Ÿ�� ���� �г��� ��Ȱ��ȭ �Ѵ�
				towerDataViewer.OffPanel();
			}

			hitTransform = null;
		}
	}

	private void OnChangePreviousTileColor()
	{
		// ���콺�� �ٷ� ������ ��ġ�ϰ� �ִ� Ÿ�� (���� ����)
		if (previousHitTransform != null)
		{
		//	previousHitTransform.GetComponent<Tile>().OnColorReset();
		}
	}
}

