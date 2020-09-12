using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChatPresenter : MonoBehaviour {
    [SerializeField]
    private InputField chatArea = default;
    [SerializeField]
    private Button chatButton = default;

    [SerializeField]
    private TextMesh chat = default;

    [SerializeField]
    private GameObject chatObject = default;

    private void Start () {
        chatButton.onClick.AddListener(OnClickPostMessage);
    }

    /// <summary>
    /// めっせーじの送信
    /// </summary>
    private void OnClickPostMessage () {
        string message = chatArea.text;
        SocketClient.Instance.EmitMessage (message);

        chat.text = message;
        chatObject.SetActive(true);
        DOVirtual.DelayedCall(5.0f,()=>chatObject.SetActive(false));
    }
}