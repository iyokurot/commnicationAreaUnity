using System.Collections;
using SocketIO;
using UnityEngine;

public class SocketClient : SingletonMonoBehaviourFast<SocketClient> {
    SocketIOComponent socket;

    public void Start () {
        GameObject go = GameObject.Find ("SocketIO");
        socket = go.GetComponent<SocketIOComponent> ();

        socket.On ("online", OnOnline);
        socket.On ("offline", OnOffline);

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

        Debug.Log ("online id: " + id);
    }

    public void OnOffline (SocketIOEvent e) {
        string id = e.data.GetField ("id").str;

        Debug.Log ("offline id: " + id);
    }

    public void OnMessage (SocketIOEvent e) {
        JSONObject obj = e.data;

        string id = obj.GetField ("id").str;
        string message = obj.GetField ("message").str;

        Debug.Log ("message id: " + id + " message: " + message);
    }
    public void OnMove (SocketIOEvent e) {
        JSONObject obj = e.data;

        string id = obj.GetField ("id").str;
        float pos_x = obj.GetField ("x").n;
        float pos_z = obj.GetField ("z").n;

        Debug.Log ("message id: " + id + "pos_x " + pos_x + " pos_z " + pos_z);
    }

    public void EmitMessage (string message) {
        JSONObject jsonObject = new JSONObject (JSONObject.Type.OBJECT);
        jsonObject.AddField ("message", message);
        jsonObject.AddField ("x", this.transform.position.x);
        jsonObject.AddField ("y", this.transform.position.y);

        socket.Emit ("message", jsonObject);
    }

    public void EmitMove (Vector3 pos) {
        JSONObject jsonObject = new JSONObject (JSONObject.Type.OBJECT);
        jsonObject.AddField ("x", pos.x);
        jsonObject.AddField ("z", pos.z);

        socket.Emit ("move", jsonObject);
    }
}