using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ItemDataManager;

public class ItemCSVConnecter
{
    //유니티내에 Tools메뉴추가 > .asset 파일을 쉽게 만들기위함
    [MenuItem("Tools/Import Items From CSV")] 

    
    public static void ImportItems()
    {
        //csv파일경로
        string csvPath = "Assets/WorkSpace/KBK/CSV/Itemdata - Data.csv";
        //csv파일 읽어서 배열에 저장
        string[] lines = File.ReadAllLines(csvPath);

        //헤더는 건너뛰고 2번째 줄부터 파싱
        for (int i = 1; i < lines.Length; i++) 
        {
            //현재줄을 ,로나눠서 셀을 배열로 저장
            string[] values = lines[i].Split(','); 
            //scriptableobject 인스턴스 생성
            Item item = ScriptableObject.CreateInstance<Item>();
            //아이템 이름 0에 저장
            item.itemName = values[0];
            //아이템 타입 enum으로 변환
            item.itemType = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), values[1]);
            //Resources 폴더 내에서 Icons폴더 내의 Sprite 호출
            item.sprite = Resources.Load<Sprite>("Icons/" + values[2]);
            //아이템 설명 텍스트
            item.Description = values[3];

            //hp회복값
            int.TryParse(values[4], out item.hpRestore);
            //배고픔 회복값
            int.TryParse(values[5], out item.hungerRestore);
            //목마름 회복값
            int.TryParse(values[6], out item.thirstRestore);
            //추가공격력
            int.TryParse(values[7], out item.addAttackPower);
            //추가방어력
            int.TryParse(values[8], out item.addDefensePower);
            //내구도
            int.TryParse(values[9], out item.durability);
            //아이템기본갯수
            int.TryParse(values[10], out item.itemCount);
            //item.freshness;
            //item.cureStatusEffect;
            

            


            string assetPath = $"Assets/WorkSpace/KBK/Items/{item.itemName}.asset"; // .asset 만들 파일 경로
            AssetDatabase.CreateAsset(item, assetPath); //.asset 제작
        }

        AssetDatabase.SaveAssets(); //.asset 저장
        AssetDatabase.Refresh(); //.asset 갱신
        //Debug.Log("아이템 데이터 생성 완료");
    }
}
