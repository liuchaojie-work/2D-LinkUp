using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject card;
    //牌的行数
    public static int rowNum = 16;
    //牌的列数
    public static int columNum = 16;
    //初始化偶数张牌以及被随机打乱的数组
    public static int[,] temp_map;
    //存储被打乱后的temp_map,以及在其周围加上一圈0
    public static int[,] test_map;
    //贴图数组
    public Sprite[] cards;
    //调整牌间距
    public static float xMove = 0.71f;
    public static float yMove = 0.71f;

    private void Awake()
    {
 
        test_map = new int[rowNum + 2, columNum + 2];
        temp_map = new int[rowNum, columNum];
        for(int i = 0; i < rowNum; i++)
        {
            for(int j = 0; j < columNum; j += 2)
            {
                int temp = Random.Range(0, cards.Length);
                //同时生成2张一样的牌，确保不出现单数牌
                temp_map[i, j] = temp;
                temp_map[i, j + 1] = temp;
            }
        }

        ChangeMap();
        //在周围加上0
        for(int i = 0; i < rowNum + 2; i++)
        {
            for(int j = 0; j < columNum + 2;j++)
            {
                if(0 == i || 0 == j || i == rowNum + 1 || j == columNum + 1)
                {
                    test_map[i, j] = 0;
                }
                else
                {
                    test_map[i, j] = temp_map[i - 1, j - 1];
                }
            }
        }

        BuildMap();
        FindObjectOfType<DrawLine>().CreateLine();
    }

    //将存储ID的数组打乱
    public void ChangeMap()
    {
        for(int i = 0; i < rowNum; i++)
        {
            for(int j = 0; j < columNum; j ++)
            {
                int temp = temp_map[i, j];
                int randomRow = Random.Range(0, rowNum);
                int randomColum = Random.Range(0, columNum);
                temp_map[i, j] = temp_map[randomRow, randomColum];
                temp_map[randomRow, randomColum] = temp;
            }
        }
    }

    //实例化牌
    public void BuildMap()
    {
        //数组的行
        int i = 0;
        //数组的列
        int j = 0;
        GameObject g;
        for(int x = 0; x < rowNum + 2; x++)
        {
            for( int y = 0; y < columNum + 2; y++)
            {
                g = Instantiate(card) as GameObject;
                g.transform.position = new Vector3(x * xMove, -y * yMove, 0);
                Sprite icon = cards[test_map[i, j]];
                g.GetComponent<SpriteRenderer>().sprite = icon;
                //存储牌的属性
                g.GetComponent<Card>().x = x;
                g.GetComponent<Card>().y = y;
                g.GetComponent<Card>().value = test_map[i, j];
                if(x == 0 || y == 0 || x == rowNum + 1 || y == columNum + 1)
                {
                    g.GetComponentInChildren<SpriteRenderer>().enabled = false;
                }
                j++;
            }
            i++;
            j = 0;
        }
    }
}
