using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    //存储牌的数据
    public static GameObject g1, g2;
    public int x1, x2, y1, y2, value1, value2;
    //判定是第几次点选
    public bool select = false;
    public int linkType;
    //存储折点
    public Vector3 z1, z2;
    public GameObject upgradePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)&&isStoped == true)
        {
            IsSelect();
        }
    }

    public void IsSelect()
    {
        //鼠标位置作为射线方向
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //生成射线
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if(select == false)
            {
                //第一个点选的物体为g1
                g1 = hit.transform.gameObject;
                //将g1的颜色改为红色
                g1.GetComponent<SpriteRenderer>().color = Color.red;
                //获取g1在数组中的位置及贴图编号
                x1 = g1.GetComponent<Card>().x;
                y1 = g1.GetComponent<Card>().y;
                value1 = g1.GetComponent<Card>().value;
                select = true;
            }
            else
            {
                //第二个点选的物体为g2
                g2 = hit.transform.gameObject;
                //将g1的颜色改为红色
                g2.GetComponent<SpriteRenderer>().color = Color.white;
                //获取g1在数组中的位置及贴图编号
                x2 = g2.GetComponent<Card>().x;
                y2 = g2.GetComponent<Card>().y;
                value2 = g2.GetComponent<Card>().value;
                select = false;
                IsSame();
            }
        }
    }

    public void IsSame()
    {
        if((value1 == value2)&&(g1.transform.position != g2.transform.position))
        {
            IsLink(x1, y1, x2, y2);
        }
        else
        {
            x1 = x2 = y1 = y2 = value1 = value2 = 0;
        }
    }

    //横向直连
    bool X_Link(int x1, int x2, int y2)
    {
        if(x1 > x2)
        {
            int temp = x1;
            x1 = x2;
            x2 = temp;
        }

        for(int i = x1 + 1;i <= x2; i++)
        {
            //相邻
            if(i == x2)
            {
                return true;
            }

            //间隔不为空，跳出
            if(MapController.test_map[i,y2] != 0)
            {
                break;
            }
            
        }
        return false;
    }

    //纵向直连
    bool Y_Link(int x1, int y1, int y2)
    {
        if(y1 > y2)
        {
            int temp = y1;
            y1 = y2;
            y2 = temp;
        }

        for(int i = y1 + 1; i <= y2; i++)
        {
            //相邻
            if(i == y2)
            {
                return true;
            }

            //间隔不为空，跳出
            if(MapController.test_map[x1, i] != 0)
            {
                break;
            }
        }
        return false;
    }
    bool IsLink(int x1, int y1, int x2, int y2)
    {
        if(x1 == x2)
        {
            if(Y_Link(x1,y1,y2))
            {
                linkType = 0;
                StartCoroutine(CardDestroy(x1, y1, x2, y2));
                return true;
            }
           
        }
        else if(y1 == y2)
        {
            if (X_Link(x1, x2, y1))
            {
                linkType = 0;
                StartCoroutine(CardDestroy(x1, y1, x2, y2));
                return true;
            }
        }

        if(OneCornerLink(x1,y1,x2,y2))
        {
            linkType = 1;
            StartCoroutine(CardDestroy(x1, y1, x2, y2));
            return true;
        }

        if(TwoCornerLink(x1, y1, x2, y2))
        {
            linkType = 2;
            StartCoroutine(CardDestroy(x1, y1, x2, y2));
            return true;
        }
        return false;
    }

    //一折
    bool OneCornerLink(int x1, int y1, int x2, int y2)
    {
        if(0 == MapController.test_map[x1,y2])
        {
            if(X_Link(x1,x2,y2)&&Y_Link(x1,y1,y2))
            {
                z1 = new Vector3(x1 * MapController.xMove, -y2 * MapController.yMove, -1);
                return true;
            }
        }

        if(0 == MapController.test_map[x2,y1])
        {
            if(X_Link(x1,x2,y1)&&Y_Link(x2,y1,y2))
            {
                z1 = new Vector3(x2 * MapController.xMove, -y1 * MapController.yMove, -1);
                return true;
            }
        }
        return false;
    }

    //二折
    bool TwoCornerLink(int x1, int y1, int x2, int y2)
    {
        //右探
        for (int i = x1 + 1; i < MapController.columNum + 2; i++)
        {
            if(0 == MapController.test_map[i,y1])
            {
                if(OneCornerLink(i, y1, x2, y2))
                {
                    z2 = new Vector3(i * MapController.xMove, -y1 * MapController.yMove, -1);
                    return true;
                }
            }
            else
            {
                break;
            }
        }

        //左探
        for(int i = x1 - 1; i > -1; i--)
        {
            if(0 == MapController.test_map[i,y1])
            {
                if(OneCornerLink(i, y1, x2, y2))
                {
                    z2 = new Vector3(i * MapController.xMove, -y1 * MapController.yMove, -1);
                    return true;
                }
            }
            else
            {
                break;
            }
        }

        //下探
        for(int i = y1 + 1; i < MapController.rowNum + 2; i++)
        {
            if(0 == MapController.test_map[x1, i])
            {
                if(OneCornerLink(x1, i, x2, y2))
                {
                    z2 = new Vector3(x1 * MapController.xMove, -i * MapController.yMove, -1);
                    return true;
                }
            }
            else
            {
                break;
            }
        }

        //上探
        for(int i = y1 -1; i > -1; i--)
        {
            if(0 == MapController.test_map[x1, i])
            {
                if(OneCornerLink(x1, i, x2, y2))
                {
                    z2 = new Vector3(x1 * MapController.xMove, -i * MapController.yMove, -1);
                    return true;
                }
            }
            else
            {
                break;
            }
        }

        return false;
    }
    //消除牌
    IEnumerator CardDestroy(int x1, int y1, int x2, int y2)
    {
        FindObjectOfType<DrawLine>().DrawLinkLine(g1, g2, linkType, z1, z2);
        //生成道具
        if (Random.value < 0.10)
        {
            GameObject g;
            g = Instantiate(upgradePrefab, new Vector3(8, -7, -1), Quaternion.identity);
            string name = g.GetComponent<UpGrade>().upgradeName;
            PerformUpgrade(name);
        }
        yield return new WaitForSeconds(0.2f);
        //销毁g1，g2
        Destroy(g1);
        Destroy(g2);
        //刷新数组中g1，g2的位置信息
        MapController.test_map[x1, y1] = 0;
        MapController.test_map[x2, y2] = 0;
        x1 = x2 = y1 = y2 = value1 = value2 = 0;
    }


    void PerformUpgrade(string name)
    {
        name = name.Remove(name.Length - 21);
        switch (name)
        {
            case "plus":
                break;
            case "stop":
                IsStoped();
                break;
            case "clock":
                break;
        }
    }

    bool isStoped = true;
    void IsStoped()
    {
        isStoped = false;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        //禁手持续10s
        yield return new WaitForSeconds(10.0f);
        isStoped = true;
    }
}
