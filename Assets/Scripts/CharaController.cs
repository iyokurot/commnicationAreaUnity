using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharaController : MonoBehaviour
{
    [SerializeField]
    private TextMesh chat = default;

    [SerializeField]
    private GameObject chatObject = default;

    public void Move(float x, float z){
        Vector3 pos = this.gameObject.transform.position;
                pos.x = x;
                pos.z = z;
                this.gameObject.transform.position = pos;
    }


    public void Remark(string message)
    {
        Debug.Log("onmessage");
        chat.text = message;
        chatObject.SetActive(true);
        DOVirtual.DelayedCall(5.0f,()=>chatObject.SetActive(false));
    }


    public void DestroyObject(){
        Destroy(this.gameObject);
    }
}
