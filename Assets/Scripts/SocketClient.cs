using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;

public class SocketClient : SingletonMonoBehaviourFast<SocketClient> {
    SocketIOComponent socket;
    [SerializeField]
    GameObject playerObject = default;
    private Dictionary<string, CharaController> playerTable = new Dictionary<string, CharaController> ();

    public void Start () {
        GameObject go = GameObject.Find ("SocketIO");
        socket = go.GetComponent<SocketIOComponent> ();

        socket.On ("online", OnOnline);
        socket.On ("offline", OnOffline);
        socket.On ("login_users", OnLoginUsers);

        socket.On ("message", OnMessage);
        socket.On ("move", OnMove);

        //StartCoroutine ("BeepBoop");

    }

    private IEnumerator BeepBoop () {
        // wait 1 seconds and continue
        yield return new WaitForSeconds (1);

        //socket.Emit("message");
        EmitMessage ("text");

        // wait 3 seconds and continue
        yield return new WaitForSeconds (3);

        //socket.Emit("message");
        EmitMessage ("text");

        // wait 2 seconds and continue
        yield return new WaitForSeconds (2);

        //socket.Emit("message");
        EmitMessage ("text");

        // wait ONE FRAME and continue
        yield return null;

        //socket.Emit("message");
        //socket.Emit("message");
        EmitMessage ("text");
        EmitMessage ("text");
    }

    public void OnOnline (SocketIOEvent e) {
        string id = e.data.GetField ("id").str;
        // ユーザ生成
        // 配列へ追加
        GameObject obj = Instantiate (playerObject, new Vector3 (0, playerObject.transform.position.y, 0), playerObject.transform.rotation);
        // Dictionaryに追加
        playerTable.Add (id, obj.GetComponent<CharaController>());

        Debug.Log ("online id: " + id);
    }

    public void OnOffline (SocketIOEvent e) {
        string id = e.data.GetField ("id").str;
        // 対象ユーザをDestroy
        if (playerTable.ContainsKey (id)) {
            //Destroy (playerTable[id]);
            playerTable[id].DestroyObject();
            playerTable.Remove (id);
        }
        Debug.Log ("offline id: " + id);
    }

    public void OnLoginUsers (SocketIOEvent e) {
        string id = e.data.GetField ("id").str;
        string message = e.data.GetField ("message").str;
        // ユーザを生成
        message = message.Replace ("\\", string.Empty);
        Users users = JsonUtility.FromJson<Users> (message);
        foreach (var user in users.users) {
            GameObject obj = Instantiate (playerObject, new Vector3 (user.pos_x, playerObject.transform.position.y, user.pos_z), playerObject.transform.rotation);
            // Dictionaryに追加
            playerTable.Add (user.id, obj.GetComponent<CharaController>());
        }

        Debug.Log ("message id: " + id + " message: " + message);
    }

    public void OnMessage (SocketIOEvent e) {
        JSONObject obj = e.data;

        string id = obj.GetField ("id").str;
        string message = obj.GetField ("message").str;
        foreach (KeyValuePair<string, CharaController> kvp in playerTable) {
            if (kvp.Key == id) {
                kvp.Value.Remark(message);
                break;
            }
        }

        Debug.Log ("message id: " + id + " message: " + message);
    }
    public void OnMove (SocketIOEvent e) {
        JSONObject obj = e.data;

        string id = obj.GetField ("id").str;
        float pos_x = obj.GetField ("x").n;
        float pos_z = obj.GetField ("z").n;

        // 対象ユーザを探索し位置更新
        foreach (KeyValuePair<string, CharaController> kvp in playerTable) {
            if (kvp.Key == id) {
                // Vector3 pos = kvp.Value.transform.position;
                // pos.x = pos_x;
                // pos.z = pos_z;
                // kvp.Value.transform.position = pos;
                kvp.Value.Move(pos_x,pos_z);
                break;
            }
        }

        Debug.Log ("message id: " + id + "pos_x " + pos_x + " pos_z " + pos_z);
    }

    public void EmitMessage (string message) {
        JSONObject jsonObject = new JSONObject (JSONObject.Type.OBJECT);
        jsonObject.AddField ("message", message);

        socket.Emit ("message", jsonObject);
    }

    public void EmitMove (Vector3 pos) {
        JSONObject jsonObject = new JSONObject (JSONObject.Type.OBJECT);
        jsonObject.AddField ("x", pos.x);
        jsonObject.AddField ("z", pos.z);

        socket.Emit ("move", jsonObject);
    }
}

[System.Serializable]
public class Users {
    [System.Serializable]
    public class Potisions {
        public string id;
        public int pos_x;
        public int pos_z;
    }
    public Potisions[] users;
}