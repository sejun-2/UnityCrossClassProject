using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ItemDataManager;

public class ItemCSVConnecter
{
    //����Ƽ���� Tools�޴��߰� > .asset ������ ���� ���������
    [MenuItem("Tools/Import Items From CSV")] 

    
    public static void ImportItems()
    {
        //csv���ϰ��
        string csvPath = "Assets/WorkSpace/KBK/CSV/Itemdata - Data.csv";
        //csv���� �о �迭�� ����
        string[] lines = File.ReadAllLines(csvPath);

        //����� �ǳʶٰ� 2��° �ٺ��� �Ľ�
        for (int i = 1; i < lines.Length; i++) 
        {
            //�������� ,�γ����� ���� �迭�� ����
            string[] values = lines[i].Split(','); 
            //scriptableobject �ν��Ͻ� ����
            Item item = ScriptableObject.CreateInstance<Item>();
            //������ �̸� 0�� ����
            item.itemName = values[0];
            //������ Ÿ�� enum���� ��ȯ
            item.itemType = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), values[1]);
            //Resources ���� ������ Icons���� ���� Sprite ȣ��
            item.sprite = Resources.Load<Sprite>("Icons/" + values[2]);
            //������ ���� �ؽ�Ʈ
            item.Description = values[3];

            //hpȸ����
            int.TryParse(values[4], out item.hpRestore);
            //����� ȸ����
            int.TryParse(values[5], out item.hungerRestore);
            //�񸶸� ȸ����
            int.TryParse(values[6], out item.thirstRestore);
            //�߰����ݷ�
            int.TryParse(values[7], out item.addAttackPower);
            //�߰�����
            int.TryParse(values[8], out item.addDefensePower);
            //������
            int.TryParse(values[9], out item.durability);
            //�����۱⺻����
            int.TryParse(values[10], out item.itemCount);
            //item.freshness;
            //item.cureStatusEffect;
            

            


            string assetPath = $"Assets/WorkSpace/KBK/Items/{item.itemName}.asset"; // .asset ���� ���� ���
            AssetDatabase.CreateAsset(item, assetPath); //.asset ����
        }

        AssetDatabase.SaveAssets(); //.asset ����
        AssetDatabase.Refresh(); //.asset ����
        //Debug.Log("������ ������ ���� �Ϸ�");
    }
}
