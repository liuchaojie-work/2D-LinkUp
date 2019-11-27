using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpGrade : MonoBehaviour
{
    public Sprite[] upgradeSprites;
    public string upgradeName = "";
    
    private void Awake()
    {
        //随机获取贴图
        Sprite icon = upgradeSprites[Random.Range(0, upgradeSprites.Length)];
        //道具名称的存储
        upgradeName = icon.ToString();
        //贴图
        this.gameObject.GetComponent<SpriteRenderer>().sprite = icon;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //销毁道具
        Destroy(this.gameObject, 0.8f);
    }


}
